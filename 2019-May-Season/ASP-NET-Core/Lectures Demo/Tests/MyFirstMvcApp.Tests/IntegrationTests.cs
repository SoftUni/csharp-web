using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MyFirstMvcApp.Tests
{
    public class IntegrationTests
    {
        [Fact]
        public async Task IndexPageShouldReturn200OK()
        {
            var server = new WebApplicationFactory<Startup>();
            var client = server.CreateClient();
            var response = await client.GetAsync("/Home/Index");
            var html = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            Assert.Contains("<div>1</div>", html);
        }
    }
}
