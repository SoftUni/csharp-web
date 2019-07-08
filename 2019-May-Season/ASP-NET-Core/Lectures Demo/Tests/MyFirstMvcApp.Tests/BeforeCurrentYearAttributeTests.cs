using MyFirstMvcApp.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MyFirstMvcApp.Tests
{
    public class BeforeCurrentYearAttributeTests
    {
        [Theory]
        [InlineData(1900, 1900, true)]
        [InlineData(1900, 1899, false)]
        [InlineData(1900, 1901, true)]
        [InlineData(1900, 2019, true)]
        [InlineData(1900, 0, false)]
        [InlineData(1900, int.MinValue, false)]
        [InlineData(1900, int.MaxValue, false)]
        [InlineData(1900, -1, false)]
        [InlineData(-2019, -2018, true)]
        [InlineData(1900, 1, false)]
        [InlineData(0, 0, true)]
        [InlineData(-2019, -2019, true)]
        public void IsValidShouldWorkCorrectly(int afterYear, int year, bool expected)
        {
            // Arrange
            var attribute = new BeforeCurrentYearAttribute(afterYear);

            // Act
            var result = attribute.IsValid(year);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void IsValidWithCurrentYearShouldReturnTrue()
        {
            // Arrange
            const int afterYear = 1900;
            var attribute = new BeforeCurrentYearAttribute(afterYear);

            // Act
            var result = attribute.IsValid(DateTime.UtcNow.Year);

            // Assert
            Assert.True(result);
        }
    }
}
