using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerServiceClient
{
    public class CustomerDetails
    {
        public string UniqueId { get; set; }
        public int CustomerType { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalSalesAmount { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string Filename { get; set; }
    }
}
