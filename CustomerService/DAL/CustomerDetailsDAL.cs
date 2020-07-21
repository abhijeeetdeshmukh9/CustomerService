using CustomerService.Models;
using CustomerService.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace CustomerService.DAL
{
    public class CustomerDetailsDAL : ICustomerDetails
    {
        private readonly CustomerServiceContext _customerServiceContext;
        private readonly ILogger _logger;

        public CustomerDetailsDAL(CustomerServiceContext customerServiceContext,ILogger<CustomerDetailsDAL> logger)
        {
            _customerServiceContext = customerServiceContext;
            _logger = logger;
        }

        public bool AddEditCustomerDetails(CustomerDetails customerDetails)
        {
            try
            {

                var customerDetailsData = _customerServiceContext.CustomerDetails.FirstOrDefault(x => x.UniqueId == customerDetails.UniqueId.Trim());

                if (customerDetailsData != null)
                {
                    customerDetailsData.TotalSalesAmount = customerDetails.TotalSalesAmount;
                    customerDetailsData.CustomerName = customerDetails.CustomerName;
                    customerDetailsData.CustomerType = customerDetails.CustomerType;
                    customerDetailsData.TimeStamp = customerDetails.TimeStamp;
                    customerDetailsData.Filename = customerDetails.Filename;
                }
                else
                {
                    _customerServiceContext.Add(customerDetails);

                }
                _customerServiceContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }     

        public CustomerDetails GetCustomerDetailsById(string Id)
        {
            try
            {
                return _customerServiceContext.CustomerDetails.FirstOrDefault(x => x.UniqueId == Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        IEnumerable<CustomerDetails> ICustomerDetails.GetAllCustomerDetails()
        {
            try
            {
                return _customerServiceContext.CustomerDetails.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
