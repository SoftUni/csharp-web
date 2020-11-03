using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAspNetCoreApplication.TagHelpers
{
    [HtmlTargetElement("table", Attributes = "bootstrap-table")]
    public class BootstrapTableTagHelper : TagHelper
    {
        public string TableName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.PreElement.AppendHtml($"<h2>{TableName}</h2>");
            output.Attributes.RemoveAll("bootstrap-table");
            output.Attributes.Add(new TagHelperAttribute("class", "table table-striped table-hover table-sm"));
        }
    }
}
