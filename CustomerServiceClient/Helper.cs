using System;

namespace CustomerServiceClient
{
    public class Helper
    {
        static string specialChars = @"#:;";
        public static bool HasSpecialChar(string input)
        {
            
            foreach (var item in specialChars)
            {
                if (input.Contains(item)) return true;
            }

            return false;
        }
        public static string LogDetails(CustomerDetails customer, string message)
        {
            return LogDetails(customer.UniqueId, customer.CustomerName, customer.CustomerType.ToString(), customer.Filename, customer.TimeStamp.ToString(), customer.TotalSalesAmount.ToString(), message);
        }

        public static string LogDetails(string UniqueId, string CustomerName, string CustomerType, string Filename, string TimeStamp, string TotalSalesAmount, string message)
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
