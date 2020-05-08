using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantRepository.Models
{
    public class Restaurants
    {
        public string id { get; set; }
        public string Restaurant_Name { get; set; }
        public string Restaurant_Address { get; set; }
        public string City { get; set; }
        public decimal Distance { get; set; }
        public decimal Budget { get; set; }
        public string Cousine { get; set; }
        public int Rating { get; set; }

        public IList<MenuModel> Menu { get; set; }
    }
}
