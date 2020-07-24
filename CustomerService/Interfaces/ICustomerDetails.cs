using CustomerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.Interfaces
{
    public interface ICustomerDetails
    {
        IEnumerable<CustomerDetails> GetAllCustomerDetails();
        CustomerDetails GetCustomerDetailsById(string Id);
        bool AddEditCustomerDetails(CustomerDetails customerDetails);
        bool AddEditCustomerDetails(List<CustomerDetails> customerDetails);
    }
}
