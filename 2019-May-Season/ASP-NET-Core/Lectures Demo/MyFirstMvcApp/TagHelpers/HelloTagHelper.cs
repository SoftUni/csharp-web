using Microsoft.AspNetCore.Razor.TagHelpers;
using MyFirstMvcApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstMvcApp.TagHelpers
{
    [HtmlTargetElement("h1", Attributes = "greeting-name")]
    public class HelloTagHelper : TagHelper
    {
        private readonly IUsersService usersService;

        public HelloTagHelper(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public string GreetingName { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.SetAttribute("Niki", this.GreetingName);
            output.Content.SetContent(this.GreetingName);
            output.PreElement.SetContent(this.GreetingName);
            output.PostElement.SetContent(this.usersService.GetCount().ToString());
        }
    }
}
