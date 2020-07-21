using CustomerService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.Interfaces
{
    public interface IUsers
    {
        IEnumerable<Users> GetAllUsers();
        Users GetUserByUsernamePassword(string username, string password);
        bool AddEditUser(Users user);
        bool DeleteUser(int Id);
    }
}
