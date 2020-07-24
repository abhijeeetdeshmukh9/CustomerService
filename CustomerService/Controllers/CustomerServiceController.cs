using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CustomerService.Interfaces;
using CustomerService.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Moq;

namespace CustomerService.Controllers
{
    /// <summary>
    /// Provides all functionality to the /CustomerService/ route.
    /// </summary>

    [ApiController]
    [Route("[controller]")]
    public class CustomerServiceController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICustomerDetails _customerDetails;

        public CustomerServiceController(ICustomerDetails customerDetails, ILogger<CustomerServiceController> logger)
        {
            _customerDetails = customerDetails;
            _logger = logger;
        }

        // Returns All Customers from CustomerDetails table 
        [HttpGet]
        [Route("GetAllCustomerDetails")]
        public IActionResult GetAllCustomerDetails()
        {
            try
            {
                _logger.LogInformation("Request for GetAllCustomerDetails Resource");

                var customersDetails = _customerDetails.GetAllCustomerDetails();

                if (customersDetails.Count() == 0)
                {
                    _logger.LogInformation("Details Not Found");
                    return NotFound("No data found!");
                }

                return Ok(customersDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetCustomerDetails - {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        // Returns Customer for particular Id from CustomerDetails table
        [HttpGet]
        [Route("GetCustomerDetailsById/{id}")]
        public IActionResult GetCustomerDetailsById(string id)
        {
            try
            {
                _logger.LogInformation($"Request for GetCustomerDetailsById Resource for Id : {id}");

                var customersDetails = _customerDetails.GetCustomerDetailsById(id);

                if (customersDetails == null)
                {
                    _logger.LogDebug("Details Not Found");
                    return NotFound("No data found!");
                }

                return Ok(customersDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetCustomerDetailsById - {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        //Action Method to add list of Customer Details in CustomerDetails table
        [HttpPost]
        [Route("AddEditCustomerDetails")]
        public IActionResult AddEditCustomerDetails(List<CustomerDetails> lstCustomerDetails)
        {
            var lstCustomerRecords = new List<CustomerDetails>();

            try
            {
                _logger.LogInformation($"Request for AddEditCustomerDetails Resource");

                if (lstCustomerDetails.Count == 0)
                {
                    return BadRequest("No Data received!");
                }

                foreach (var customerDetails in lstCustomerDetails.OrderBy(x => x.CustomerName).ThenBy(x => x.UniqueId))
                {
                    if (string.IsNullOrEmpty(customerDetails.UniqueId) == true
                       && Helper.HasSpecialChar(customerDetails.UniqueId) == true)
                    {
                        _logger.LogError(Helper.LogDetails(customerDetails, "Unique Id value is not correct"));
                        continue;
                    }

                    //if (string.IsNullOrEmpty(Convert.ToString(customerDetails.TimeStamp)) == false)
                    //{
                    //    var timestamp = Convert.ToDateTime(customerDetails.TimeStamp).ToString("yyyy-MM-dd");
                    //    var currentDate = DateTime.Now.ToString("yyyy-MM-dd");

                    //    if (timestamp == currentDate)
                    //    {
                    //        _logger.LogError(Helper.LogDetails(customerDetails, "Timestamp should be less than Current date!"));
                    //        continue;
                    //    }
                    //}
                    if (!string.IsNullOrEmpty(Convert.ToString(customerDetails.TimeStamp)))
                    {
                        var isDateEqual = Helper.DateCompare(DateTime.Now, customerDetails.TimeStamp);

                        if (isDateEqual)
                        {
                            _logger.LogError(Helper.LogDetails(customerDetails, "Timestamp should be less than Current date!"));
                            continue;
                        }
                    }
                    else
                    {
                        _logger.LogError(Helper.LogDetails(customerDetails, "Timestamp value is empty!"));
                        continue;
                    }

                    lstCustomerRecords.Add(customerDetails);                    
                }               

                var status = _customerDetails.AddEditCustomerDetails(lstCustomerRecords);

                if (status == false)
                {
                    _logger.LogError($"Error While inserting data to CustomerDetails table");
                    return BadRequest($"Error While inserting data to CustomerDetails table");
                }
                else
                {                    

                    _logger.LogInformation(JsonConvert.SerializeObject(lstCustomerRecords));
                    return Ok(JsonConvert.SerializeObject(lstCustomerRecords));
                }                                
            }
            catch (Exception ex)
            {
                _logger.LogError($"AddEditCustomerDetails - {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}
