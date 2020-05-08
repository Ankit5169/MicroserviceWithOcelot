using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantRepository.Models
{
    public class RestaurantModel
    {
        public int Id { get; set; }
        public string Restaurant_Name { get; set; }
        public string Restaurant_Address { get; set; }
        public string City { get; set; }
        public decimal Distance { get; set; }
        public decimal Budget { get; set; }
        public string Cousine { get; set; }
        public string Menu { get; set; }
        public int Rating { get; set; }

    }
}
