using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MachineLearing.Models
{
    public class QueryRequest
    {
        public string[] ColumnNames { get; set; }
        public string[,] Values { get; set; }
    }
}