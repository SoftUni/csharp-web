using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using MyFirstMvcApp.Filters;
using MyFirstMvcApp.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using Xunit;

namespace MyFirstMvcApp.Tests
{
    public class MyAuthorizeFilterAttributeTests
    {
        [Fact]
        public void OnAuthorizationShouldCallGetUsernamesIfThereAreUsers()
        {
            var serviceMock = new Mock<IUsersService>();
            serviceMock.Setup(x => x.GetCount()).Returns(1);
            var attribute = new MyAuthorizeFilterAttribute(serviceMock.Object);
            var httpContextMock = new Mock<HttpContext>();
            attribute.OnAuthorization(new AuthorizationFilterContext(
                new ActionContext()
                {
                    HttpContext = httpContextMock.Object,
                    RouteData = new RouteData(),
                    ActionDescriptor = new ActionDescriptor()
                },
                new List<IFilterMetadata>()));

            serviceMock.Verify(x => x.GetUsernames(), Times.Once);
        }
    }
}
