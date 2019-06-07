using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework.Attributes.Validation
{
   public class PasswordSisAttribute : ValidationSisAttribute
    {
        public PasswordSisAttribute(string errorMessage) 
            : base(errorMessage)
        {
        }

        public override bool IsValid(object value)
        {
            string valueAsString = (string)Convert.ChangeType(value, typeof(string));

            if (valueAsString.Length < 3)
            {
                return false;
            }

            return true;
        }
    }
}
