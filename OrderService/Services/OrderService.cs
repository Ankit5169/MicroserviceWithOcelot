using OrderRepository.Interface;
using OrderRepository.Model;
using OrderService.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Services
{
    public class OrderService : IOrderService
    {
        private IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> createOrder(CreateOrder order)
        {
            var result = await _repository.createOrder(order);
            if(result)
            {
                //********************SEND NOTIFICATION TO DELIVERY TEAM********************
                return "Order Placed Successfully!!";
            }
            else
            { return "Order coud not be Placed!"; }
        }

        public async Task<TotalFoodPrice> PlacingOrder(Order order)
        {
            return await _repository.PlacingOrder(order);
        }

        public async Task<string> updateOrder(int id, Order order)
        {
            var result = await _repository.updateOrder(id,order);
            if (result)
            {return "Order Updated Successfully!!";}
            else
            { return "Order coud not be updated!"; }
        }
        public async Task<string> CancelOrder(int id)
        {
            var result = await _repository.CancelOrder(id);
            if (result)
            { return "Order Cancelled Successfully!!"; }
            else
            { return "Order coud not be Cancelled!"; }
        }
        
        public async Task<IEnumerable<Orders>> ViewOrders(int id)
        {
            return await _repository.ViewOrders(id);
        }
    }
}
