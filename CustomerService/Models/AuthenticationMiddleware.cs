using CustomerService.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerService.Models
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUsers user, ILogger<AuthenticationMiddleware> logger)
        {
            logger.LogInformation("Authentication Process Started!");
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                //Extract credentials
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));


                string username = usernamePassword.Substring(0, usernamePassword.IndexOf(':'));
                string password = usernamePassword.Substring(usernamePassword.IndexOf(':') + 1);

                //string username = "abhijeet";
                //string password = "Secure*12";

                if (user.GetUserByUsernamePassword(username, password) != null)
                {
                    logger.LogInformation("Authentication Successful!");
                    await _next.Invoke(context);
                }
                else
                {
                    logger.LogError("Authentication Failed!");
                    context.Response.StatusCode = 401; //Unauthorized
                    return;
                }
            }
            else
            {                
                // no authorization header
                context.Response.StatusCode = 401; //Unauthorized
                return;
            }
        }
    }
}
