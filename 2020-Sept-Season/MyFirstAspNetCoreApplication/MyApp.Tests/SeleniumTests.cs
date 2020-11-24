using MyFirstAspNetCoreApplication;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MyApp.Tests
{
    public class SeleniumTests
    {
        [Fact]
        public void H1ElementIsRemovedFromPrivacyPage()
        {
            var serverFactory = new SeleniumServerFactory<Startup>();

            var options = new ChromeOptions();
            options.AddArguments("--headless");
            options.AcceptInsecureCertificates = true;
            var webDriver = new ChromeDriver(options);

            webDriver.Navigate().GoToUrl(serverFactory.RootUri + "/Home/Privacy");
            Assert.Throws<NoSuchElementException>(() => webDriver.FindElementByTagName("h1"));
        }
    }
}
