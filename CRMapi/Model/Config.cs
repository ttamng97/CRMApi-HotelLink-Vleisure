using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMapi.Model
{
    public class Config
    {
        public Dictionary<string,string> HotelLinkUrls { get; set; }
        public Dictionary<string,string> ServiceUrl { get; set; }

        public Logging Logs { get; set; }

    }
    public class Logging { 
        public bool GetRatePlans { get; set; }
    }
}
