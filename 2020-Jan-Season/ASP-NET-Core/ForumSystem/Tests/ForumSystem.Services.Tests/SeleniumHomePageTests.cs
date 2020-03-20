using ForumSystem.Web;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ForumSystem.Services.Tests
{
    public class SeleniumHomePageTests
    {
        private RemoteWebDriver browser;
        private SeleniumServerFactory<Startup> serverFactory;

        public SeleniumHomePageTests()
        {
            this.serverFactory = new SeleniumServerFactory<Startup>();
            serverFactory.CreateClient();
            var options = new ChromeOptions();
            options.AddArguments("--headless", "--no-sandbox", "--ignore-certificate-errors");
            this.browser = new RemoteWebDriver(options);
        }

        [Fact]
        public void HomePageShouldHaveH1Tag()
        {
            this.browser.Navigate().GoToUrl(this.serverFactory.RootUri + "/Home/Index");
            Assert.Contains("Welcome to", this.browser.FindElementByCssSelector("h1").Text);
        }
    }
}
