using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Models
{
    public class CustomerServiceContext : DbContext
    {
        public CustomerServiceContext(DbContextOptions<CustomerServiceContext> options) : base(options)
        {

        }
        public DbSet<CustomerDetails> CustomerDetails { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<CustomerType> CustomerType { get; set; }
    }
}
