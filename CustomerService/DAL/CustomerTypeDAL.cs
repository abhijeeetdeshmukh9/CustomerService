using CustomerService.Models;
using CustomerService.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CustomerService.DAL
{
    /// <summary>
    /// This class is used for performing all the CRUD operations related to Customer Type
    /// </summary>
    public class CustomerTypeDAL : ICustomerType
    {
        private readonly CustomerServiceContext _customerServiceContext;
        public CustomerTypeDAL(CustomerServiceContext customerServiceContext)
        {
            _customerServiceContext = customerServiceContext;
        }

        // Add or Update the CustomerType
        public bool AddEditCustomerType(CustomerType customerType)
        {
            try
            {

                if (customerType.ID == 0)
                {
                    _customerServiceContext.Add(customerType);
                    _customerServiceContext.SaveChanges();
                }
                else
                {
                    var customerTypeData = _customerServiceContext.CustomerType.FirstOrDefault(x => x.ID == customerType.ID);

                    if (customerTypeData != null)
                    {
                        customerTypeData.Description = customerType.Description;
                        _customerServiceContext.SaveChanges();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Delete customer Type
        public bool DeleteCustomerType(int Id)
        {
            try
            {
                var customerType = _customerServiceContext.CustomerType.FirstOrDefault(x => x.ID == Id);
                _customerServiceContext.Remove(customerType);
                _customerServiceContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Returns list of All Customer Types
        public IEnumerable<CustomerType> GetAllCustomerType()
        {
            try
            {
                return _customerServiceContext.CustomerType.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // Returns Customer Type by Customer Id
        public CustomerType GetCustomerTypeById(int Id)
        {
            try
            {
                return _customerServiceContext.CustomerType.FirstOrDefault(x => x.ID == Id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
       
    }
}
