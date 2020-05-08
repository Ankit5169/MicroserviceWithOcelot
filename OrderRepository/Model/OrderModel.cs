using System;
using System.Collections.Generic;
using System.Text;

namespace OrderRepository.Model
{
    public class OrderModel
    {
        public string id { get; set; }
        public string rest_id { get; set; }
        public string Restaurant_Name { get; set; }
        public string PaymentStatus { get; set; }
        public string OrderStatus { get; set; }
        public string Total_Amount { get; set; }
        public string FoodOrdered { get; set; }
    }
}
