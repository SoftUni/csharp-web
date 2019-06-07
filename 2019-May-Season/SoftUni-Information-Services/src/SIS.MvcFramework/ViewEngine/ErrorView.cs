using System;
using System.Collections.Generic;
using System.Text;
using SIS.MvcFramework.Identity;

namespace SIS.MvcFramework.ViewEngine
{
    using Validation;

    public class ErrorView : IView
    {
        private readonly string errors;

        public ErrorView(string errors)
        {
            this.errors = errors;
        }

        public string GetHtml(object model, ModelStateDictionary modelState, Principal user)
        {
            return errors;
        }
    }
}
