using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.Models
{
    public class CustomerType
    {
        [Key]
        public int ID { get; set; }
        public int Description { get; set; }
    }
}
