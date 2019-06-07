using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework.Attributes.Validation
{
    public class StringLengthSisAttribute : ValidationSisAttribute
    {
        private readonly int minLength;
        private readonly int maxLength;

        public StringLengthSisAttribute(int minLength, int maxLength, string errorMessage)
            : base(errorMessage)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        public override bool IsValid(object value)
        {
            string valueAsString = (string)Convert.ChangeType(value, typeof(string));

            if (valueAsString.Length >= minLength && valueAsString.Length <= maxLength)
            {
                return true;
            }

            return false;
        }
    }
}
