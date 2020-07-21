using CustomerService;
using CustomerService.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Forms;

namespace CustomerServiceClient
{
    public partial class Form1 : Form
    {
        private readonly ILogger _logger;

        public Form1(ILogger<Form1> logger)
        {
            _logger = logger;
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            _logger.LogInformation("Set the details");
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            //dlg.ShowDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string line;
                string fileName;
                fileName = dlg.FileName;
                try
                {
                    _logger.LogInformation($"Database Insertion process started for file {fileName}");
                    //MessageBox.Show(fileName);
                    using (StreamReader file = new StreamReader(fileName))
                    {
                        while ((line = file.ReadLine()) != null)
                        {
                            decimal minimumSalesValue = 0;
                            string[] fields = line.Split(',');
                            string day = string.Empty;
                            string month = string.Empty;
                            string year = string.Empty;

                            string[] dateFields = fields[4].Split('-');

                            day = dateFields[2].Replace("[", "").Replace("]", "");
                            month = dateFields[1].Replace("(", "").Replace(")", "");
                            year = dateFields[0].Replace("[", "").Replace("]", "");

                            if (year.Length > 4)
                                year = year.Substring(0, 4);

                            var currentDate = DateTime.Now.ToString("yyyy-MM-dd");
                            var timestamp = year + "-" + month + "-" + day;

                            if (string.IsNullOrEmpty(fields[0]) == false && Helper.hasSpecialChar(fields[0]) == false)
                            {

                                minimumSalesValue = string.IsNullOrEmpty(textBox1.Text) ? minimumSalesValue : Convert.ToDecimal(textBox1.Text.Trim());

                                if (Convert.ToDecimal(fields[3]) <= minimumSalesValue)
                                {
                                    _logger.LogError($"Minimun Sales amount value is greater than total sales amount for UniqueId {fields[0]}");
                                    continue;
                                }

                                if (currentDate == timestamp)
                                {
                                    _logger.LogError($"Timestamp value is greater or equal to Current Date for UniqueId {fields[0]}!");
                                    continue;
                                }

                                string apiUrl = "http://localhost:59138/CustomerService/AddEditCustomerDetails";

                                var customer = new CustomerDetails()
                                {
                                    UniqueId = fields[0],
                                    CustomerName = fields[2],
                                    CustomerType = Convert.ToInt32(fields[1]),
                                    Filename = string.IsNullOrEmpty(textBox2.Text) ? fileName : textBox1.Text.Trim(),
                                    TimeStamp = Convert.ToDateTime(timestamp),
                                    TotalSalesAmount = minimumSalesValue
                                };

                                
                                _logger.LogInformation(Helper.LogDetails(customer,"Received data for insertion!"));
                                //string username = Convert.ToBase64String(Encoding.UTF8.GetBytes("abhijeet"));
                                //string password = Convert.ToBase64String(Encoding.UTF8.GetBytes("Secure*12"));

                                string username = "abhijeet";
                                string password = "Secure*12";

                                HttpClient client = new HttpClient();
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", username + ":" + password);
                                var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                                var result = await client.PostAsync(apiUrl, content);

                                //var response = result;
                                //var json = client.UploadString(apiUrl + "/AddEditCustomerDetails", inputJson);
                            }
                            else
                            {
                                _logger.LogError($"UniqueId {fields[0]} value is not Correct. Please Remove special character if it has any!");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error Occured while processing the file{fileName}. " + ex.Message);
                }
            }
        }
    }
}
