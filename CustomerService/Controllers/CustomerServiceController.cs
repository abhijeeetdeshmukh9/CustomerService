using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CustomerService.Interfaces;
using CustomerService.Models;
using Microsoft.AspNetCore.Authorization;
using CustomerService.DAL;
using Microsoft.Extensions.Logging;

namespace CustomerService.Controllers
{


    [ApiController]
    [Route("[controller]")]  
    public class CustomerServiceController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICustomerDetails _customerDetails;

        public CustomerServiceController(ICustomerDetails customerDetails,ILogger<CustomerServiceController> logger)
        {
            _customerDetails = customerDetails;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetCustomerDetails")]
        public IActionResult GetAllCustomerDetails()
        {
            try 
            {
                _logger.LogInformation("Request for GetAllCustomerDetails Resource");
                var customersDetails = _customerDetails.GetAllCustomerDetails();

                if(customersDetails.Count() == 0)
                {
                    _logger.LogDebug("Details Not Found");
                    return NotFound("No data found!");
                }

                return Ok(customersDetails);
            }
            catch(Exception ex)
            {
                _logger.LogError($"GetCustomerDetails - {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

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

        [HttpPost]
        [Route("AddEditCustomerDetails")]
        public IActionResult AddEditCustomerDetails(CustomerDetails customerDetails)
        {
            try
            {
                _logger.LogInformation($"Request for AddEditCustomerDetails Resource");

                if (string.IsNullOrEmpty(customerDetails.UniqueId) == true
                   || Helper.hasSpecialChar(customerDetails.UniqueId))
                    return BadRequest("Please correct UniqueId value!");                

                if(string.IsNullOrEmpty(Convert.ToString(customerDetails.TimeStamp)) == false)
                {
                    var timestamp = Convert.ToDateTime(customerDetails.TimeStamp).ToString("yyyy-MM-dd");
                    var currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                    if(timestamp == currentDate)
                    {
                        _logger.LogError(Helper.LogDetails(customerDetails, "Timestamp should be less than Current date!"));

                        return BadRequest("Timestamp should be less than Current date!");
                    }
                }

                _logger.LogInformation(Helper.LogDetails(customerDetails,"Object Received for Insertion Process"));
                var status = _customerDetails.AddEditCustomerDetails(customerDetails);

                if (status == false)
                {
                    _logger.LogError(Helper.LogDetails(customerDetails, "Error While inserting data to CustomerDetails table"));
                    return BadRequest();
                }

                _logger.LogInformation(Helper.LogDetails(customerDetails, "Record Inserted Suceessfully to CustomerDetails table"));
                return Ok("Record Inserted Successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"AddEditCustomerDetails - {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}
