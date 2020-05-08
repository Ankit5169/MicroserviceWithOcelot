using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderRepository.Model;
using OrderService.Interface;

namespace OrderManagementServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }
        // GET: api/Order
        [HttpGet]
        public async Task<IEnumerable<Orders>> Get()
        {
            //Get All Orders -- pass 0
            return await _service.ViewOrders(0);
        }

        // GET: api/Order/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IEnumerable<Orders>> Get(int id)
        {
            return await _service.ViewOrders(id);
        }

        // POST: api/Order
        [HttpPost]
        public async Task<TotalFoodPrice> Post([FromBody] Order obj)
        {
            return await _service.PlacingOrder(obj);
        }

        // POST: api/Order
        [HttpPut]
        [Route("CreateOrder")]
        public async Task<string> CreateOrder([FromBody] CreateOrder obj)
        {
            return await _service.createOrder(obj);
        }


        // PUT: api/Order/5
        [HttpPut("{id}")]
        public async Task<string> UpdateOrder(int id, [FromBody] Order obj)
        {
            return await _service.updateOrder(id, obj);
        }

        // DELETE: api/order/5
        [HttpDelete("{id}")]
        public async Task<string> Delete(int id)
        {
            return await _service.CancelOrder(id);
        }
    }
}
