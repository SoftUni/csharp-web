using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.MvcFramework
{
    public class ErrorView : IView
    {
        private readonly IEnumerable<string> errors;

        public ErrorView(IEnumerable<string> errors)
        {
            this.errors = errors;
        }

        public string GetHtml(object model, string user)
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<h1>View compilation errors:</h1>");
            html.AppendLine("<ul>");
            foreach (var error in this.errors)
            {
                html.AppendLine($"<li>{error}</li>");
            }

            html.AppendLine("</ul>");
            return html.ToString();
        }
    }
}
