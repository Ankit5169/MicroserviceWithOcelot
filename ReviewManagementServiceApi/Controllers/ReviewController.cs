using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewSystemRepository.Model;
using ReviewSystemService.Interface;

namespace ReviewManagementServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _service;

        public ReviewController(IReviewService service)
        {
            _service = service;
        }
        // GET: api/Review
        [HttpGet]
        public async Task<IEnumerable<RestaurantReview>> Get()
        {
            return await _service.ViewComments(0);
        }

        // GET: api/Review/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IEnumerable<RestaurantReview>> Get(int id)
        {
            return await _service.ViewComments(id);
        }

        // POST: api/Review
        [HttpPost]
        public async Task<string> Post([FromBody] CustomerReview review)
        {
            return await _service.AddReviewComment(review);
        }

        // PUT: api/Review/5
        [HttpPut("{id}")]
        public async Task<string> Put(int id, [FromBody] UpdateComment obj)
        {
            return await _service.UpdateReviewComment(id, obj);
        }

    }
}
