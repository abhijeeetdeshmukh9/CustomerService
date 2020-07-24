using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Forms;

namespace CustomerServiceClient
{
    public partial class Form1 : Form
    {
        #region Private Variables

        private readonly ILogger _logger;
        private string username = ConfigurationManager.AppSettings["Username"];
        private string password = ConfigurationManager.AppSettings["Password"];
        private string apiurl = ConfigurationManager.AppSettings["ApiUrl"];
        private string DATETIME_ERROR_MESSAGE = "Date is not valid. Please correct it for UniqueId";
        private string MINSALES_ERROR_MESSAGE = "Minimun Sales amount value is greater than total sales amount for UniqueId";
        private string DATECOMPARE_ERROR_MESSAGE = "Timestamp value is greater or equal to Current Date for UniqueId";

        #endregion

        public Form1(ILogger<Form1> logger)
        {
            _logger = logger;
            Form form1 = new Form();
            form1.StartPosition = FormStartPosition.CenterScreen;

            //EnableDisableControls(true);

            InitializeComponent();
        }


        #region Private Methods
        // Button Click Event of the Form
        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMinSalesAmt.Text) && string.IsNullOrEmpty(txtMinSalesAmt.Text))
            {
                MessageBox.Show("Please type Minimum Sales Amount and Filename!");
            }
            else
            {

                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string line;
                    string fileName;
                    fileName = dlg.FileName;
                    try
                    {
                        using (StreamReader file = new StreamReader(fileName))
                        {
                            if (new FileInfo(fileName).Length != 0)
                            {
                                EnableDisableControls(false);
                                HttpClient client = null;
                                _logger.LogInformation($"Database Insertion process started for file {fileName}");
                                try
                                {

                                    var lstCustomerDetails = new List<CustomerDetails>();

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

                                        //var currentDate = DateTime.Now.ToString("yyyy-MM-dd");                                        
                                        var currentDate = DateTime.Now;

                                        var status = Helper.ValidateDate((year + "-" + month + "-" + day),out DateTime timestamp);

                                        if(status != Helper.Status.ValidDate)
                                        {
                                            //_logger.LogError($"{DATETIME_ERROR_MESSAGE} - {fields[0]}");
                                            _logger.LogError(string.Format("{0} - {1}",(status == Helper.Status.DateEqual)?DATECOMPARE_ERROR_MESSAGE:DATETIME_ERROR_MESSAGE,fields[0]));
                                            continue;
                                        }

                                        minimumSalesValue = (string.IsNullOrEmpty(txtMinSalesAmt.Text)) ? minimumSalesValue : Convert.ToDecimal(txtMinSalesAmt.Text.Trim());

                                        if (Convert.ToDecimal(fields[3]) <= minimumSalesValue)
                                        {
                                            _logger.LogError($"{MINSALES_ERROR_MESSAGE} - {fields[0]}");
                                            continue;
                                        }

                                        var customer = new CustomerDetails()
                                        {
                                            UniqueId = fields[0],
                                            CustomerName = fields[2],
                                            CustomerType = Convert.ToInt32(fields[1]),
                                            Filename = txtFilename.Text.Trim(),
                                            TimeStamp = Convert.ToDateTime(timestamp),
                                            TotalSalesAmount = Convert.ToDecimal(fields[3])
                                        };

                                        lstCustomerDetails.Add(customer);

                                    }

                                    string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes($"{username}:{password}"));

                                    client = new HttpClient();
                                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encoded);
                                    var content = new StringContent(JsonConvert.SerializeObject(lstCustomerDetails), Encoding.UTF8, "application/json");
                                    var result = await client.PostAsync(apiurl, content);

                                    client.Dispose();
                                    
                                    EnableDisableControls();

                                    MessageBox.Show("File Processed Succesfully!");
                                }
                                catch (Exception ex)
                                {                                    
                                    client.Dispose();
                                    Reset();
                                    EnableDisableControls();
                                    _logger.LogError($"Error Occured while processing the file{fileName}. " + ex.Message);
                                }
                            }
                            else
                            {
                                MessageBox.Show("File is empty");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Reset();
                        EnableDisableControls();
                        _logger.LogError($"Error Occured while processing the file {fileName}. " + ex.Message);
                        MessageBox.Show("Error Occured while processing the file");
                    }
                }
            }
        }

        // Allow only Numerical value with decimal point
        private void txtMinSalesAmt_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        // Handles enable disable event of the form controls
        private void EnableDisableControls(bool status = true)
        {
            txtFilename.Enabled = status;
            txtMinSalesAmt.Enabled = status;
            button1.Enabled = status;
            button1.Text = (status == true) ? "Browse" : "Processing...";
        }

        // Clears the values of textboxes and resets the button text to Browse
        private void Reset()
        {
            txtMinSalesAmt.Clear();
            txtFilename.Clear();
            EnableDisableControls();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Reset();
        }

        #endregion
    }
}
