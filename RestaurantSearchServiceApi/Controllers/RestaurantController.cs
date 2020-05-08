using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantService.Interface;

namespace RestaurantSearchServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _service;

        public RestaurantController(IRestaurantService service)
        {
            _service = service;
        }

        // GET: api/Restaurant
        [HttpGet]
        public async Task<IActionResult> Get(string name, string location, decimal distance,
            string cousine, decimal budget, int rating)
        {
            var result = await _service.GetRestaurants(name, location, distance, cousine, budget, rating);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }
        
    }
}
