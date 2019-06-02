using SIS.MvcFramework.Identity;
using SIS.MvcFramework.ViewEngine;

using System.Collections.Generic;
using System.IO;

using Xunit;

namespace SIS.MvcFramework.Tests
{
    public class TestSisViewEngine
    {
        [Theory]
        [InlineData("TestWithoutCSharpCode")]
        [InlineData("UseForForeachAndIf")]
        [InlineData("UseModelData")]
        public void TestGetHtml(string testFileName)
        {
            IViewEngine viewEngine = new SisViewEngine();
            var viewFileName = $"ViewTests/{testFileName}.html";
            var expectedResultFileName = $"ViewTests/{testFileName}.Result.html";

            var viewContent = File.ReadAllText(viewFileName);
            var expectedResult = File.ReadAllText(expectedResultFileName);
            Principal user = new Principal
            {
                Username = "Pesho"

            };
            var model = new TestViewModel()
            {
                StringValue = "str",
                ListValues = new List<string> { "123", "val1", string.Empty },
            };
            var actualResult = viewEngine.GetHtml<object>(viewContent, model, user );
            Assert.Equal(expectedResult.TrimEnd(), actualResult.TrimEnd());
        }

        [Theory]
        [InlineData("TestWithoutCSharpCode")]
        [InlineData("UseForForeachAndIf")]
        [InlineData("TestEscapeStyleAndScript")]
        [InlineData("TestCsharpCodeBlock")]
        [InlineData("TestCsharpCodeBlockWhitHtml")]
        public void TestGetHtmlWhitOutModel(string testFileName)
        {
            IViewEngine viewEngine = new SisViewEngine();
            var viewFileName = $"ViewTests/{testFileName}.html";
            var expectedResultFileName = $"ViewTests/{testFileName}.Result.html";

            var viewContent = File.ReadAllText(viewFileName);
            var expectedResult = File.ReadAllText(expectedResultFileName);
            Principal user = new Principal
            {
                Username = "Jon"

            };

            var actualResult = viewEngine.GetHtml<object>(viewContent, new object(), user);
            Assert.Equal(expectedResult.TrimEnd(), actualResult.TrimEnd());
        }
    }
}
