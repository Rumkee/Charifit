using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calorie.Models.Social
{
    public class OpenGraphVM
    {
        
        public string type { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public string url { get; set; }
        public string site_name { get; set; }
        public string app_id { get; set; }

    }
}