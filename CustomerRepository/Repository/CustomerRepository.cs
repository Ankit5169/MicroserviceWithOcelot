using CustomerRepository.Interface;
using CustomerRepository.Model;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRepository.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IConfiguration _config;
        public CustomerRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> SaveUserDetails(UserDetails user)
        {
            try
            {
                var connectionString = _config.GetConnectionString("DbConnection");

                string query = "INSERT INTO tbl_Customer VALUES ('" + user.id + "','" + user.name + "','" + user.given_name + "','" + user.family_name + "','" +
                    user.gender + "', null,null,null,null,null,1);";


                using (var connection = new SqlConnection(connectionString))
                {
                    var affectedRows = await connection.ExecuteAsync(query);
                    if (affectedRows > 0)
                        return user.given_name + " " + user.family_name;
                    else
                        return string.Empty;
                }

            }
            catch (Exception ex)
            { return string.Empty; }
        }

        public async Task<bool> unRegisterUser(int id)
        {
            bool result = false;
            try
            {
                var connectionString = _config.GetConnectionString("DbConnection");
                string query = "update tbl_Customer set is_Active = 0 where id = " + id + ";";

                using (var connection = new SqlConnection(connectionString))
                {
                    var affectedRows = await connection.ExecuteAsync(query);
                    if (affectedRows > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            { }
            return result;
        }

        public async Task<bool> UpdateUserDetails(int id, User user)
        {
            bool userUpdated = false;
            try
            {
                var connectionString = _config.GetConnectionString("DbConnection");
                string query = "update tbl_Customer set first_name = '" + user.first_name + "', last_name = '" + user.last_name + "'," +
                    " gender = '" + user.gender + "', DOB = null, mobile_no = '" + user.mobile_no + "', Email = '" + user.Email + "'," +
                        " Delivery_Address ='" + user.Delivery_Address + "', City_id =" + user.City_id + " where id = " + id + ";";

                using (var connection = new SqlConnection(connectionString))
                {
                    var affectedRows = await connection.ExecuteAsync(query);
                    if (affectedRows > 0)
                    {
                        userUpdated = true;
                    }
                }
            }
            catch (Exception ex)
            { }
            return userUpdated;
        }
    }

}
