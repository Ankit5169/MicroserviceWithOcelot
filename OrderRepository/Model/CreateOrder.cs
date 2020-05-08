using System;
using System.Collections.Generic;
using System.Text;

namespace OrderRepository.Model
{
    public class CreateOrder
    {
        public string order_id { get; set; }
        public string total_amount { get; set; }
        public string payment_success { get; set; }
        public string payment_mode { get; set; }
    }
}
