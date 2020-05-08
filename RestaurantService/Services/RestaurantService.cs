using RestaurantRepository.Interface;
using RestaurantRepository.Models;
using RestaurantService.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantService.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _repository;

        public RestaurantService(IRestaurantRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<Restaurants>> GetRestaurants(string name, string location, decimal distance, string cousine, decimal budget, int rating)
        {
            return await _repository.GetRestaurants(name, location, distance, cousine, budget, rating);
        }
    }
}
