1. Project Name - CustomerService

2. Agenda - This project is used for inserting Customer data from a text file using Windows Forms and Web API.

3. Technologies Used
	- Framework Used : .Net Core 3.1
	- Persistance Storage : Entity Framework Core
	- Language : C#
	- Client Application type : Windows Forms
	- Server Application type : WebAPI
	- Logging Framework : Serilog
	- Unit Testing Framework : Moq
	- Database : SSMS
	- Editing Tool : Visual Studio 2019
	- API Testing Tool : Swagger

4. Input File - testfile.txt file is used for inserting customer data to Database.

5. Projects in the Solution 
	- CustomerService : WebAPI project
	- CustomerServiceClient : Windows Forms Application

6. Steps to Run application 
	- Go to "..\CustomerService\CustomerServiceClient\bin\Debug\netcoreapp3.1" and find CustomerServiceClient.exe file.
	- Double click on this exe and one form will open. This forms contains Minimum Sales amount and Filename fields.
	- Then click on Browse button and upload the file "testfile.txt" in this solution.
	- Once the procedure is complete you will get one Messagebox saying File "File Processed Succesfully!".
	- To verify the result login to ssms database in azure and check CustomerDetails table.
	- Other tables used are Users to verify user for Basic Authentication and CustomerType.
	- Also, the logs for CustomerServiceClient are present in "..\CustomerService\CustomerServiceClient\bin\Debug\netcoreapp3.1" with the filenam
	  "CustomerClientLog.txt"

7. Azure Database Credentials
	- Server Name : customerservice.database.windows.net
	- Username : abhijeet
	- Password : Secure*12
	- Database : CustomerService
	- Tables : CustomerDetails, Users, CustomerType

8. Azure Web API Url used in ClientApplication - https://customerservice20200723121321.azurewebsites.net/CustomerService/

