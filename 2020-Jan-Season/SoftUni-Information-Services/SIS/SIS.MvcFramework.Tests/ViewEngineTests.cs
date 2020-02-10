using System.Collections.Generic;
using System.IO;
using Xunit;

namespace SIS.MvcFramework.Tests
{
    public class ViewEngineTests
    {
        [Theory]
        [InlineData("OnlyHtmlView")]
        [InlineData("ForForeachIfView")]
        [InlineData("ViewModelView")]
        public void TestGetHtml(string testName)
        {
            var viewModel = new TestViewModel()
            {
                Name = "Niki",
                Year = 2020,
                Numbers = new List<int> { 1, 10, 100, 1000, 10000 },
            };

            var viewContent = File.ReadAllText($"ViewTests/{testName}.html");
            var expectedResultContent = File.ReadAllText($"ViewTests/{testName}.Expected.html");

            IViewEngine viewEngine = new ViewEngine();
            var actualResult = viewEngine.GetHtml(viewContent, viewModel, "123");

            Assert.Equal(expectedResultContent, actualResult);
        }

        [Fact]
        public void TestGetHtmlWithTemplateModel()
        {
            var viewModel = new List<int> { 1, 2, 3 };

            var viewContent = @"
@foreach (var num in Model)
{
<p>@num</p>
}";
            var expectedResultContent = @"
<p>1</p>
<p>2</p>
<p>3</p>
";

            IViewEngine viewEngine = new ViewEngine();
            var actualResult = viewEngine.GetHtml(viewContent, viewModel, null);

            Assert.Equal(expectedResultContent, actualResult);
        }

        [Fact]
        public void TestGetHtmlWithGenericTemplateModel()
        {
            var viewModel = new Dictionary<string, Dictionary<string, int>>()
                                {
                                    {
                                        "Pesho",
                                        new Dictionary<string, int>
                                            {
                                                { "C# Basics", 100 }, { "C# Web", 75 },
                                            }
                                    },
                                    {
                                        "Gosho",
                                        new Dictionary<string, int>
                                            {
                                                { "JavaScript Advanced", 30 }, { "PHP Web", 50 },
                                            }
                                    }
                                };

            var viewContent = @"
@foreach (var student in Model)
{
<p> User: @student.Key </p>
<ul>
@foreach (var course in student.Value)
{
<li> @course.Key -> @course.Value </li>
}
</ul>
}";
            var expectedResultContent = @"
<p> User: Pesho </p>
<ul>
<li> C# Basics -> 100 </li>
<li> C# Web -> 75 </li>
</ul>
<p> User: Gosho </p>
<ul>
<li> JavaScript Advanced -> 30 </li>
<li> PHP Web -> 50 </li>
</ul>
";
            IViewEngine viewEngine = new ViewEngine();
            var actualResult = viewEngine.GetHtml(viewContent, viewModel, null);

            Assert.Equal(expectedResultContent, actualResult);
        }
    }
}
