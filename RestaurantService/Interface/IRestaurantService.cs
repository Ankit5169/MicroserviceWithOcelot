using RestaurantRepository.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantService.Interface
{
    public interface IRestaurantService
    {
        Task<IEnumerable<Restaurants>> GetRestaurants(string name, string location, decimal distance,
            string cousine, decimal budget, int rating);
    }
}
