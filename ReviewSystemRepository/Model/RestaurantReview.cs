using System;
using System.Collections.Generic;
using System.Text;

namespace ReviewSystemRepository.Model
{
    public class RestaurantReview : RestaurantModel
    {
       public IList<ReviewModel> reviews { get; set; }
    }
}
