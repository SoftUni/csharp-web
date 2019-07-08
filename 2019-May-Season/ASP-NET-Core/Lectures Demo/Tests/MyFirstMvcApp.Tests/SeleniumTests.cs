using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MyFirstMvcApp.Tests
{
    public class SeleniumTests
    {
        [Fact]
        public void TestJs()
        {
            var browser = this.GetPage("/");
            var count = browser.FindElementsByCssSelector(".findMe").Count;
            Assert.Equal(0, count);
        }

        private RemoteWebDriver GetPage(string url)
        {
            var server = new SeleniumServerFactory<Startup>();
            server.CreateClient();

            var opts = new ChromeOptions();
            opts.AddArgument("--headless"); // Optional, comment this out if you want to SEE the browser window
            opts.AddArgument("no-sandbox");

            var browser = new RemoteWebDriver(opts);
            browser.Navigate().GoToUrl(server.RootUri + url);
            return browser;
        }
    }
}
