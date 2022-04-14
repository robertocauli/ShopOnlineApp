using ShopOnline.Model;
using System;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;

namespace ShopOnlineApp.Proxy
{
    public class UserRESTProxy : IUserProxy
    {
        public LoginResult Login(User user)
        {
            HttpClient client = IniziatizeClient();
            var response = client.PostAsJsonAsync("User/Login", user).Result;
            var loginResult = GetRequestResult<LoginResult>(response);
            return loginResult;
        }

        public RegistrationResult Registration(User user)
        {
            HttpClient client = IniziatizeClient();
            var response = client.PostAsJsonAsync("User/Registration", user).Result;
            var registrationResult = GetRequestResult<RegistrationResult>(response);
            return registrationResult;
        }

        private static T GetRequestResult<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var content = response.Content.ReadAsStringAsync().Result;
                var control = JsonConvert.DeserializeObject<T>(content);

                return control;
            }
            else
            {
                throw new WebException(((int)response.StatusCode).ToString());
            }
        }

        private HttpClient IniziatizeClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost/ShopOnlineApp.RestService/api/");
            return client;
        }
    }
}
