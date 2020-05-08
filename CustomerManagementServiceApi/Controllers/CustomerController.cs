using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CustomerRepository.Model;
using CustomerService.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CustomerManagementServiceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ICustomerService _service;

        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        /// <summary>
        /// /This Get fetches Auth Token form google to get details
        /// </summary>
        [HttpGet]
        [Route("register")]
        public void Register()
        {
            var url = _service.GetAuthCodeURL();
            Response.Redirect(url);
        }

        /// <summary>
        /// This Get fetches user details and saves it in DB
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("gettoken")]
        public async Task<string> Get(string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new Exception("Failed to Authenticate!");
            else
                return await _service.GetUserDetails(code);
        }

        //Update Customer Details
        // PUT: api/Customer/5
        [HttpPut("{id}")]
        public async Task<string> Put(int id, [FromBody] User user)
        {
            return await _service.updateUserDetails(id, user);
        }


        [HttpPut]
        [Route("unregister/{id}")]
        public async Task<string> UnRegister(int id)
        {
            return await _service.unRegisterUser(id);
        }
    }


}
