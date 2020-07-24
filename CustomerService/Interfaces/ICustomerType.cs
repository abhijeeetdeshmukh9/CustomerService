using CustomerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.Interfaces
{
    public interface ICustomerType
    {
        IEnumerable<CustomerType> GetAllCustomerType();
        CustomerType GetCustomerTypeById(int Id);
        bool AddEditCustomerType(CustomerType customerType);
        bool DeleteCustomerType(int Id);
    }
}
