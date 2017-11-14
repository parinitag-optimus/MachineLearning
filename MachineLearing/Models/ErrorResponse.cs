using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MachineLearing.Models
{
    public class ErrorResponse
    {
        public class RootObject
        {
            public Error error { get; set; }
        }

        public class Error
        {
            public string code { get; set; }
            public string message { get; set; }
            public List<Detail> details { get; set; }
        }

        public class Detail
        {
            public string code { get; set; }
            public string message { get; set; }
        }


    }
}