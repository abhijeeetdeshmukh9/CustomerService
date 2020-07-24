using CustomerService.Controllers;
using CustomerService.DAL;
using CustomerService.Interfaces;
using CustomerService.Models;
using CustomerService.UnitTest;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace CustomerService.UnitTest
{
    [TestClass]
    public class CustomerServiceControllerTest
    {
                
        [TestMethod]
        public void Test1()
        {
            var customerDetails = new CustomerDetails()
            {
                CustomerName = "Abhijeet",
                TotalSalesAmount = 200,
                CustomerType = 1,
                Filename = "abc.txt",
                TimeStamp = Convert.ToDateTime("2020-07-21"),
                UniqueId = "401"
            };
            var mock = new Mock<ICustomerDetails>();
            mock.Setup(p => p.AddEditCustomerDetails(customerDetails)).Returns(true);
        }
    }
}
