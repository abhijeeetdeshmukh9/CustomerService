using CustomerService.Models;
using CustomerService.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;

namespace CustomerService.DAL
{
    public class UserDAL : IUsers
    {
        private readonly CustomerServiceContext _customerServiceContext;        

        public UserDAL(CustomerServiceContext customerServiceContext)
        {
            _customerServiceContext = customerServiceContext;
        }

        public bool AddEditUser(Users user)
        {            
            try
            {
                if(user.ID == 0)
                {
                    _customerServiceContext.Add(user);
                    _customerServiceContext.SaveChanges();
                }
                else
                {
                    var users = _customerServiceContext.Users.FirstOrDefault(x => x.ID == user.ID);

                    if (users != null)
                    {
                        users.Username = user.Username;
                        users.Password = user.Password;
                        _customerServiceContext.SaveChanges();
                    }
                }                

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool DeleteUser(int Id)
        {
            try
            {
                var users = _customerServiceContext.Users.FirstOrDefault(x => x.ID == Id);
                _customerServiceContext.Remove(users);
                _customerServiceContext.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }      
         

        public Users GetUserByUsernamePassword(string username, string password)
        {
            try
            {
                return _customerServiceContext.Users.FirstOrDefault(x=> x.Username == username && x.Password == password);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        IEnumerable<Users> IUsers.GetAllUsers()
        {
            try
            {
                return _customerServiceContext.Users.ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
