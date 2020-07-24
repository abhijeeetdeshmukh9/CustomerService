using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.Models
{
    public class CustomerDetails
    {
        [Key]
        public string UniqueId { get; set; }
        public int CustomerType { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalSalesAmount { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Filename { get; set; }
    }        
}
