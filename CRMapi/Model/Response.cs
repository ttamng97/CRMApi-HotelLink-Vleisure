using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMapi.Model
{
    public class Response
    {
        public bool result { get; set; }
        public dynamic data { get; set; }
        public string message { get; set; }
    }
}
