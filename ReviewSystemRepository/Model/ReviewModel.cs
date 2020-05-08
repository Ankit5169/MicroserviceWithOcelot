using System;
using System.Collections.Generic;
using System.Text;

namespace ReviewSystemRepository.Model
{
    public class ReviewModel
    {
        public string id { get; set; }
        public string rating { get; set; }
        public string comment { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }
}
