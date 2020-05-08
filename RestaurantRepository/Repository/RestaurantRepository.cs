using Dapper;
using Microsoft.Extensions.Configuration;
using RestaurantRepository.Interface;
using RestaurantRepository.Models;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;


namespace RestaurantRepository.Repository
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly IConfiguration _config;

        public RestaurantRepository(IConfiguration config)
        {
            _config = config;
        }
        public async Task<IEnumerable<Restaurants>> GetRestaurants(string name, string location, decimal distance, string cousine, decimal budget, int rating)
        {
            var restList = new List<Restaurants>();
            var connectionString = _config.GetConnectionString("DbConnection");

            var query = "select r.id, r.Restaurant_Name, r.Restaurant_Address,";
            query += "c.City,r.Distance, r.Budget, cc.Cousine, dbo.GetMenu(r.id) Menu, dbo.GetRating(r.id) rating ";
            query += "from tbl_Restaurant r ";
            query += "left join tbl_City_State c on r.City_id = c.id ";
            query += "left join tbl_Cousine cc on r.Cusine_id = cc.id ";
            query += "where 1=1 ";

            if (!string.IsNullOrEmpty(name))
                query += "and LOWER(r.Restaurant_Name) like '" + name.ToLower() + "%'";
            if (!string.IsNullOrEmpty(location))
                query += "and LOWER(c.City) like '" + location.ToLower() + "%'";
            if (distance > 0)
                query += "and r.Distance between 0 and" + distance;
            if (budget > 0)
                query += "and r.Budget between 0 and" + budget;
            if (!string.IsNullOrEmpty(cousine))
                query += "and LOWER(cc.Cousine) like '" + cousine.ToLower() + "%'";
            if (rating > 0)
                query += "and rating between 0 and" + rating;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var eventName = await connection.QueryAsync<RestaurantModel>(query);

                if (eventName != null)
                {
                    foreach (var row in eventName)
                    {
                        var restData = new Restaurants();

                        restData.id = row.Id.ToString();
                        restData.Restaurant_Name = row.Restaurant_Name;
                        restData.Restaurant_Address = row.Restaurant_Address;
                        restData.City = row.City;
                        restData.Cousine = row.Cousine;
                        restData.Budget = row.Budget;
                        restData.Distance = row.Distance;
                        restData.Menu = ConvertToMenuModel(row.Menu);
                        restData.Rating = row.Rating;

                        restList.Add(restData);
                    }
                }
                return restList;
            }

        }
        private IList<MenuModel> ConvertToMenuModel(string value)
        {
            var rows = value.Split(',');
            List<MenuModel> menuList = new List<MenuModel>();
            
            foreach(var val in rows)
            {
                MenuModel menu = new MenuModel();
                var arr = val.Split('_');
                menu.id = arr[0];
                menu.Item = arr[1];
                menu.Price = arr[2];

                menuList.Add(menu);
            }
            
            return menuList;
        }
    }
}
