using CustomerRepository.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRepository.Interface
{
    public interface ICustomerRepository
    {
        Task<string> SaveUserDetails(UserDetails user);
        Task<bool> UpdateUserDetails(int id, User user);
        Task<bool> unRegisterUser(int id);
    }
}
