using Microsoft.Extensions.Configuration;
using OrderRepository.Interface;
using OrderRepository.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Linq;

namespace OrderRepository.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IConfiguration _config;
        public OrderRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<Orders>> ViewOrders(int id)
        {
            var orderList = new List<Orders>();
            try
            {
                var connectionString = _config.GetConnectionString("DbConnection");

                string query = "Select o.id,o.rest_id, r.Restaurant_Name, p.textValue PaymentStatus, " +
                                "s.textValue OrderStatus, o.Total_Amount,dbo.GetFoodOrdered(o.id) FoodOrdered " +
                                "from tbl_Orders o " +
                                "left join tbl_Restaurant r on o.rest_id = r.id " +
                                "left join tbl_PaymentStatus p on o.PaymnetStatus = p.id " +
                                "left Join tbl_Status s on o.Order_Status = s.id ";
                if (id > 0)
                    query += "where o.id = " + id;

                using (var connection = new SqlConnection(connectionString))
                {
                    var result = await connection.QueryAsync<OrderModel>(query);
                    if (result != null)
                    {
                        foreach (var row in result)
                        {
                            var orderData = new Orders();

                            orderData.id = row.id;
                            orderData.rest_id = row.rest_id;
                            orderData.Restaurant_Name = row.Restaurant_Name;
                            orderData.PaymentStatus = row.PaymentStatus;
                            orderData.OrderStatus = row.OrderStatus;
                            orderData.Total_Amount = row.Total_Amount;
                            orderData.foodOrdered = ConvertToFoodOrderedModel(row.FoodOrdered);
                            
                            orderList.Add(orderData);
                        }
                    }
                }
                return orderList;
            }
            catch (Exception ex)
            { return null; }
        }

        private IList<FoodOrdered> ConvertToFoodOrderedModel(string value)
        {
            var rows = value.Split(',');
            List<FoodOrdered> menuList = new List<FoodOrdered>();

            foreach (var val in rows)
            {
                FoodOrdered menu = new FoodOrdered();
                var arr = val.Split('_');
                menu.id = arr[0];
                menu.Item = arr[1];
                
                menuList.Add(menu);
            }

            return menuList;
        }

        public async Task<bool> CancelOrder(int id)
        {
            bool orderCancelled = false;
            try
            {
                var connectionString = _config.GetConnectionString("DbConnection");
                string query = "delete from tbl_Orders where id = " + id + ";" +
                            "delete from tbl_OrderedItems where Order_ID = " + id + ";";

                using (var connection = new SqlConnection(connectionString))
                {
                    var affectedRows = await connection.ExecuteAsync(query);
                    if (affectedRows > 0)
                    {
                        orderCancelled = true;
                    }
                }
            }
            catch (Exception ex)
            { }
            return orderCancelled;
        }

        public async Task<bool> updateOrder(int id, Order order)
        {
            bool orderUpdated = false;
            try
            {
                var connectionString = _config.GetConnectionString("DbConnection");
                string query = "update tbl_Orders set rest_id = " + order.rest_id + " , customer_id = 1,Total_Amount = 0,PaymnetStatus = 2, Order_Status = 6 where id = " + id + ";" +
                            "delete from tbl_OrderedItems where Order_ID = " + id + ";";

                foreach (var val in order.itemSelected.Split(','))
                {
                    query += "INSERT INTO tbl_OrderedItems VALUES(" + id + "," + val + ");";
                }

                using (var connection = new SqlConnection(connectionString))
                {
                    var affectedRows = await connection.ExecuteAsync(query);
                    if (affectedRows > 0)
                    {
                        orderUpdated = true;
                    }
                }
            }
            catch (Exception ex)
            { }
            return orderUpdated;
        }
        public async Task<bool> createOrder(CreateOrder order)
        {
            bool orderPlaced = false;
            try
            {
                var connectionString = _config.GetConnectionString("DbConnection");
                string query = string.Empty;
                if (Convert.ToBoolean(order.payment_success))
                {
                    //update order table with status order paied/ and update the Stock items
                    query = "update tbl_Orders set Total_Amount = " + order.total_amount + " , PaymnetStatus = 1, Order_Status = 1 where id = " + order.order_id + ";" +
                        "update tbl_menu_Items set Stock = Stock-1 where id in (select menuItem_ID from tbl_OrderedItems where  Order_ID = " + order.order_id + ")";
                }
                else
                {
                    //update order table with status order cancelled/failed
                    query = "update tbl_Orders set Total_Amount = 0 , PaymnetStatus = 2, Order_Status = 4 where id = " + order.order_id + ";";
                }

                using (var connection = new SqlConnection(connectionString))
                {
                    var affectedRows = await connection.ExecuteAsync(query);
                    if (affectedRows > 0)
                    {
                        orderPlaced = true;
                    }
                }

                orderPlaced = true;
            }
            catch (Exception ex)
            {

            }
            return orderPlaced;
        }

        public async Task<TotalFoodPrice> PlacingOrder(Order order)
        {
            try
            {
                var connectionString = _config.GetConnectionString("DbConnection");
                var totalFoodPrices = new TotalFoodPrice();

                string query = "INSERT INTO tbl_Orders VALUES (" + order.rest_id + ", 1, 0,2,6);" +
                    "DECLARE @id int;" +
                    "SELECT @id = IDENT_CURRENT('tbl_Orders');";

                foreach (var val in order.itemSelected.Split(','))
                {
                    query += "INSERT INTO tbl_OrderedItems VALUES(@id," + val + ");";
                }


                using (var connection = new SqlConnection(connectionString))
                {
                    var affectedRows = await connection.ExecuteAsync(query);
                    if (affectedRows > 0)
                    {
                        var getTotalVal = "Select i.Order_ID, SUM(m.Price) total_price from tbl_Orders o left join tbl_OrderedItems i on o.id = i.Order_ID " +
                                " left join tbl_menu_Items m on i.menuItem_ID = m.id where o.id = IDENT_CURRENT('tbl_Orders') Group by i.Order_ID;";
                        var result = await connection.QueryAsync<TotalFoodPrice>(getTotalVal);
                        if (result != null)
                        {
                            totalFoodPrices.order_id = result.ToList()[0].order_id;
                            totalFoodPrices.total_price = result.ToList()[0].total_price;
                        }
                    }
                }
                return totalFoodPrices;
            }
            catch (Exception ex)
            { return null; }
        }
    }
}
