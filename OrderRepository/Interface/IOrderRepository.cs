using OrderRepository.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrderRepository.Interface
{
    public interface IOrderRepository
    {
        Task<TotalFoodPrice> PlacingOrder(Order order);
        Task<bool> createOrder(CreateOrder order);
        Task<bool> updateOrder(int id, Order order);
        Task<bool> CancelOrder(int id);
        Task<IEnumerable<Orders>> ViewOrders(int id);
    }
}
