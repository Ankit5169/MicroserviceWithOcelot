using CustomerRepository.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CustomerService.Interface
{
    public interface ICustomerService
    {
        string GetAuthCodeURL();
        Task<string> GetUserDetails(string code);
        Task<string> updateUserDetails(int id, User user);
        Task<string> unRegisterUser(int id);
    }
}
