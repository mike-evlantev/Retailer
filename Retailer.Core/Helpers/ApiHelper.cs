using Retailer.Core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Retailer.Core.Helpers
{
    public class ApiHelper : IApiHelper
    {
        private HttpClient _apiClient;
        private IUserModel _loggedInUser;

        public ApiHelper(IUserModel loggedInUser)
        {
            InitializeClient();
            _loggedInUser = loggedInUser;
        }

        public HttpClient ApiClient
        {
            get { return _apiClient; }
        }

        private void InitializeClient()
        {
            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["apiBaseUrl"]);
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AuthenticatedUser> AuthenticateAsync(string username, string password)
        {
            var data = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("grant_type", "password"),
                new KeyValuePair<string,string>("username", username),
                new KeyValuePair<string,string>("password", password)
            });

            using (HttpResponseMessage response = await _apiClient.PostAsync("/Token", data))
            {
                if (response.IsSuccessStatusCode)
                    return (await response.Content.ReadAsAsync<AuthenticatedUser>());
                else
                    throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task GetLoggedInUserInfo(string token)
        {
            UserModel user;

            _apiClient.DefaultRequestHeaders.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            using (HttpResponseMessage response = await _apiClient.GetAsync("/api/Account/GetLoggedInUser"))
            {
                if (response.IsSuccessStatusCode) 
                {
                    user = await response.Content.ReadAsAsync<UserModel>();
                    _loggedInUser.Id = user.Id;
                    _loggedInUser.Token = token;
                    _loggedInUser.FirstName = user.FirstName;
                    _loggedInUser.LastName = user.LastName;
                    _loggedInUser.Email = user.Email;
                    _loggedInUser.CreatedDate = user.CreatedDate;
                }
                else
                    throw new Exception(response.ReasonPhrase);
            }
        }

        public void ClearClient() => _apiClient = new HttpClient();
    }
}
