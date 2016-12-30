using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SalesReport.Models
{
    public class ReportModel
    {
        public string[] States { get; set; }
        public string SelectedStateProvince { get; set; }
        public SalesRow[] TotalSales { get; set; }
        //public int StateAndProvince { get; set; }
         
    }

    public class SalesRow
    {
        public string Product { get; set; }
        public decimal Amount { get; set; }
    }
}