using CustomerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService
{
    public class Helper
    {
        public static bool hasSpecialChar(string input)
        {
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
            foreach (var item in specialChar)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }
        public static string LogDetails(CustomerDetails customer, string message)
        {
            return LogDetails(customer.UniqueId, customer.CustomerName, customer.CustomerType.ToString(), customer.Filename, customer.TimeStamp.ToString(),customer.TotalSalesAmount.ToString(), message);
        }

        public static string LogDetails(string UniqueId, string CustomerName, string CustomerType, string Filename, string TimeStamp, string TotalSalesAmount,string message)
        {
            return String.Format("{0} {1} {2} {3} {4} {5}",
                (string.IsNullOrEmpty(UniqueId) ? "" : "UniqueId : " + UniqueId),
                (string.IsNullOrEmpty(CustomerName) ? "" : "CustomerName : " + CustomerName),
                (string.IsNullOrEmpty(Filename) ? "" : "Filename : " + Filename),
                (string.IsNullOrEmpty(TimeStamp) ? "" : "TimeStamp : " + TimeStamp),
                (string.IsNullOrEmpty(TotalSalesAmount) ? "" : "AgentId :" + TotalSalesAmount),
                message
                );
        }
    }
}
