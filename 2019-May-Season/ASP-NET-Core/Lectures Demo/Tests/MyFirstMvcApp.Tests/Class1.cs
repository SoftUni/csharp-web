using System;
using Xunit;

namespace MyFirstMvcApp.Tests
{
    public class TestExamples
    {
        [Theory]
        [InlineData(2, 10, 1024)]
        [InlineData(3, 3, 27)]
        [InlineData(4, 4, 256)]
        public void TestMathPowShouldReturn1024WhenGiven2And10(int num, int exp, int expectedResult)
        {
            // Arrange
            // Moq, new, InMemoryDb
            // Act
            var result = Math.Pow(num, exp);
            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void StringEqualsTest()
        {
            // Assert.True("Hello, SoftUni".Contains("SoftUni"));
            Assert.Contains("SoftUni", "Hello, SoftUni");
        }
    }
}
