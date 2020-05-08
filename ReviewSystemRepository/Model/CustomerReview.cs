using System;
using System.Collections.Generic;
using System.Text;

namespace ReviewSystemRepository.Model
{
    public class CustomerReview
    {
        public int cust_id { get; set; }
        public int rest_id { get; set; }
        public int rating { get; set; }
        public string comment { get; set; }
    }
}
