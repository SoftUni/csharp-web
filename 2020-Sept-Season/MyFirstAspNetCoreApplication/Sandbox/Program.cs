using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sandbox
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient httpClient = new HttpClient();
            var listOfParameters = new List<KeyValuePair<string, string>>();
            listOfParameters.Add(new KeyValuePair<string, string>("Input.Email", "admin2@nikolay.it"));
            listOfParameters.Add(new KeyValuePair<string, string>("Input.Password", "123456"));
            var response = await httpClient.PostAsync("https://presscenters.com/Identity/Account/Login",
                new FormUrlEncodedContent(listOfParameters));
        }
    }
}
