using OrderRepository.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Interface
{
    public interface IOrderService
    {
        Task<TotalFoodPrice> PlacingOrder(Order order);
        Task<string> createOrder(CreateOrder order);
        Task<string> updateOrder(int id, Order order);
        Task<string> CancelOrder(int id);
        Task<IEnumerable<Orders>> ViewOrders(int id);
    }
}
