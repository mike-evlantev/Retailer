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

        public ApiHelper()
        {
            InitializeClient();
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
    }
}
