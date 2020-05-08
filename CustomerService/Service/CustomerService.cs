using CustomerRepository.Interface;
using CustomerRepository.Model;
using CustomerService.Interface;
using CustomerService.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CustomerService.Service
{
    public class CustomerService : ICustomerService
    {
        private ICustomerRepository _repository;
        private readonly IConfiguration _config;

        public CustomerService(ICustomerRepository repository, IConfiguration config)
        {
            _repository = repository;
            _config = config;
        }

        public string GetAuthCodeURL()
        {
            string clientid = _config.GetSection("GoogleAuthentication").GetSection("ClinetID").Value;
            string clientsecret = _config.GetSection("GoogleAuthentication").GetSection("ClientSecret").Value;
            string redirection_url = _config.GetSection("GoogleAuthentication").GetSection("Redirect_URL").Value;

            string url = "https://accounts.google.com/o/oauth2/v2/auth?scope=profile&include_granted_scopes=true&redirect_uri=" +
                redirection_url + "&response_type=code&client_id=" + clientid + "";
            return url;
        }

        public async Task<string> GetUserDetails(string code)
        {
            var obj = GetAccesToken(code);
            if (obj != null)
            {
                var userDetails = GetUserProfile(obj.access_token);
                if (userDetails != null)
                {
                    var userName = await _repository.SaveUserDetails(userDetails);
                    return "Welcome " + userName;
                }
                else
                    return "Not able to fetch user details";
            }
            else
                return "Not able to fetch user details";
        }

        //Get token from google OAUTH to get the user details
        private TokenClass GetAccesToken(string code)
        {
            string clientid = _config.GetSection("GoogleAuthentication").GetSection("ClinetID").Value;
            string clientsecret = _config.GetSection("GoogleAuthentication").GetSection("ClientSecret").Value;
            string redirection_url = _config.GetSection("GoogleAuthentication").GetSection("Redirect_URL").Value;
            string url = "https://accounts.google.com/o/oauth2/token";

            string poststring = "grant_type=authorization_code&code=" + code + "&client_id=" + clientid + "&client_secret=" +
                clientsecret + "&redirect_uri=" + redirection_url + "";
            var request = (System.Net.HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";
            UTF8Encoding utfenc = new UTF8Encoding();
            byte[] bytes = utfenc.GetBytes(poststring);
            Stream outputstream = null;
            try
            {
                request.ContentLength = bytes.Length;
                outputstream = request.GetRequestStream();
                outputstream.Write(bytes, 0, bytes.Length);
            }
            catch { }
            var response = (HttpWebResponse)request.GetResponse();
            var streamReader = new StreamReader(response.GetResponseStream());
            string responseFromServer = streamReader.ReadToEnd();
            TokenClass obj = JsonConvert.DeserializeObject<TokenClass>(responseFromServer);
            return obj;
        }
        private UserDetails GetUserProfile(string accesstoken)
        {
            string url = "https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token=" + accesstoken + "";
            WebRequest request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            response.Close();

            var userinfo = JsonConvert.DeserializeObject<UserDetails>(responseFromServer);

            if (userinfo != null)
                return userinfo;
            else
                return null;

        }

        public async Task<string> updateUserDetails(int id, User user)
        {
            var result = await _repository.UpdateUserDetails(id, user);
            if (result)
                return "Customer Details Updated Successfully.";
            else
                return "Customer Details Updated Failed.";
        }

        public async Task<string> unRegisterUser(int id)
        {
            var result = await _repository.unRegisterUser(id);
            if (result)
                return "Customer Un-Registered Successfully.";
            else
                return "Failed.";
        }
    }
}

