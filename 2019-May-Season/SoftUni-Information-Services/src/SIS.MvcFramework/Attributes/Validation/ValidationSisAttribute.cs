using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework.Attributes.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ValidationSisAttribute : Attribute
    {
        protected ValidationSisAttribute(string errorMessage = "Error Message")
        {
            this.ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }

        public abstract bool IsValid(object value);
    }
}
