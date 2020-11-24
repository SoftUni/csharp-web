using Microsoft.AspNetCore.Mvc.Testing;
using MyFirstAspNetCoreApplication;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyApp.Tests
{
    public class WebTests
    {
        [Fact]
        public async Task HomePageShouldContainDevelopmentHeading()
        {
            var webApplicationFactory = new WebApplicationFactory<Startup>();
            HttpClient client = webApplicationFactory.CreateClient();

            var response = await client.GetAsync("/");

            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains("<h1>Development</h1>", html);

            Assert.True(response.Headers.Contains("x-info-action-name"));
        }
    }
}
