using CustomerService.Models;
using CustomerService.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using EFCore.BulkExtensions;

namespace CustomerService.DAL
{
    /// <summary>
    /// This class is used for performing all the CRUD operations related to Customer Details
    /// </summary>
    public class CustomerDetailsDAL : ICustomerDetails
    {
        private readonly CustomerServiceContext _customerServiceContext;
        private readonly ILogger _logger;

        public CustomerDetailsDAL(CustomerServiceContext customerServiceContext, ILogger<CustomerDetailsDAL> logger)
        {
            _customerServiceContext = customerServiceContext;
            _logger = logger;
        }

        // Add or Update the Customer Details  
        public bool AddEditCustomerDetails(CustomerDetails customerDetails)
        {
            try
            {
                var customerDetailsData = _customerServiceContext.CustomerDetails.FirstOrDefault(x => x.UniqueId == customerDetails.UniqueId.Trim());

                if (customerDetailsData != null)
                {
                    _logger.LogInformation(Helper.LogDetails(customerDetailsData,$"Updating the Customer Details for UniqueId{customerDetailsData.UniqueId}"));
                    customerDetailsData.TotalSalesAmount = customerDetails.TotalSalesAmount;
                    customerDetailsData.CustomerName = customerDetails.CustomerName;
                    customerDetailsData.CustomerType = customerDetails.CustomerType;
                    customerDetailsData.TimeStamp = customerDetails.TimeStamp;
                    customerDetailsData.Filename = customerDetails.Filename;
                    _logger.LogInformation(Helper.LogDetails(customerDetailsData, $"Updated the Customer Details for UniqueId{customerDetailsData.UniqueId}"));
                }
                else
                {
                    _logger.LogInformation(Helper.LogDetails(customerDetails, $"Adding the Customer Details for UniqueId{customerDetails.UniqueId}"));
                    _customerServiceContext.Add(customerDetails);
                    _logger.LogInformation(Helper.LogDetails(customerDetails, $"Added the Customer Details for UniqueId{customerDetails.UniqueId}"));
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

        public bool AddEditCustomerDetails(List<CustomerDetails> lstCustomerDetails)
        {
            try
            {
                _customerServiceContext.BulkInsert(lstCustomerDetails);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        // Returns Customer Details based on Id of the customer
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

        // Returns List of All Customers        
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
