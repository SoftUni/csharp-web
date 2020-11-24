using MyFirstAspNetCoreApplication.ValidationAttributes;
using System;
using Xunit;

namespace MyApp.Tests
{
    public class CurrentYearMaxValueAttributeTests
    {
        [Fact]
        public void IsValidReturnsFalseForDateTimeAfterCurrentYear()
        {
            // Arange
            var attribute = new CurrentYearMaxValueAttribute(1990);

            // Act
            var isValid = attribute.IsValid(DateTime.UtcNow.AddYears(1));

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void IsValidReturnsFalseForYearAfterCurrentYear()
        {
            // Arange
            var attribute = new CurrentYearMaxValueAttribute(1990);

            // Act
            var isValid = attribute.IsValid(DateTime.UtcNow.AddYears(1).Year);

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void IsValidReturnsTrueForYearBeforeCurrentYear()
        {
            // Arange
            var attribute = new CurrentYearMaxValueAttribute(1990);

            // Act
            var isValid = attribute.IsValid(DateTime.UtcNow.AddYears(-1).Year);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void IsValidReturnsTrueForDateTimeBeforeCurrentYear()
        {
            // Arange
            var attribute = new CurrentYearMaxValueAttribute(1990);

            // Act
            var isValid = attribute.IsValid(DateTime.UtcNow.AddYears(-1));

            // Assert
            Assert.True(isValid);
        }
    }
}
