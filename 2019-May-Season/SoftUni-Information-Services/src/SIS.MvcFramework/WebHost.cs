using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using SIS.HTTP.Enums;
using SIS.HTTP.Responses;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Action;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using SIS.MvcFramework;
using SIS.MvcFramework.Routing;
using SIS.MvcFramework.Sessions;

namespace SIS.MvcFramework
{
    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            IServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            IHttpSessionStorage httpSessionStorage = new HttpSessionStorage();

            AutoRegisterRoutes(application, serverRoutingTable);
            application.ConfigureServices();
            application.Configure(serverRoutingTable);
            var server = new Server(8000, serverRoutingTable, httpSessionStorage);
            server.Run();
        }

        private static IEnumerable<Type> GetApplicationControlles(IMvcApplication application)
        {
            return application.GetType().Assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract
                    && typeof(Controller).IsAssignableFrom(type));
        }

        private static IEnumerable<MethodInfo> GetApplicationControlerActions(Type controller)
        {
            return controller
                .GetMethods(BindingFlags.DeclaredOnly
                | BindingFlags.Public
                | BindingFlags.Instance)
                .Where(x => !x.IsSpecialName && x.DeclaringType == controller)
                .Where(x => x.GetCustomAttributes().All(a => a.GetType() != typeof(NonActionAttribute)));
        }

        private static string GetApplicationContorolerActionPath(Type controller, string actionName)
        {
            return $"/{controller.Name.Replace("Controller", string.Empty)}/{actionName}";
        }

        private static TAttribute GetApplicationControllerActionAtribute<TAttribute>(MethodInfo action)
           where TAttribute : Attribute
        {
            return action.GetCustomAttributes().LastOrDefault(
                attribute =>
                    attribute.GetType() == typeof(AuthorizeAttribute)
                 || attribute.GetType().IsSubclassOf(typeof(TAttribute))) as TAttribute;
        }

        private static TAttribute GetApplicationControllerAtribute<TAttribute>(Type controller)
            where TAttribute : Attribute
        {
            return controller.GetCustomAttributes().LastOrDefault(
                attribute =>
                    attribute.GetType() == typeof(AuthorizeAttribute)) as TAttribute;
        }

        private static bool IsPrincipalAuthorized(Principal controllerPrincipal, Type controller, MethodInfo action)
        {
            var authorizeActionAttribute = GetApplicationControllerActionAtribute<AuthorizeAttribute>(action);
            var authorizeConttrolerAttribute = GetApplicationControllerAtribute<AuthorizeAttribute>(controller);

            if (authorizeActionAttribute != null
                && (!authorizeActionAttribute.IsInAuthority(controllerPrincipal)
                    || !authorizeConttrolerAttribute.IsInAuthority(controllerPrincipal))
                )
            {

                return false;
            }

            return true;
        }

        private static void AutoRegisterRoutes(
            IMvcApplication application, IServerRoutingTable serverRoutingTable)
        {
            var controllers = GetApplicationControlles(application);
            // TODO: RemoveToString from InfoController
            foreach (var controller in controllers)
            {
                var actions = GetApplicationControlerActions(controller);

                foreach (var action in actions)
                {
                    var path = GetApplicationContorolerActionPath(controller, action.Name);
                    var attribute = GetApplicationControllerActionAtribute<BaseHttpAttribute>(action);
                    var httpMethod = HttpRequestMethod.Get;
                    if (attribute != null)
                    {
                        httpMethod = attribute.Method;
                    }

                    if (attribute?.Url != null)
                    {
                        path = attribute.Url;
                    }

                    if (attribute?.ActionName != null)
                    {
                        path = GetApplicationContorolerActionPath(controller, attribute.ActionName);
                    }

                    serverRoutingTable.Add(httpMethod, path, request =>
                    {
                        // request => new UsersController().Login(request)
                        var controllerInstance = Activator.CreateInstance(controller);
                        ((Controller)controllerInstance).Request = request;

                        // Security Authorization
                        var controllerPrincipal = ((Controller)controllerInstance).User;
                        
                        if (!IsPrincipalAuthorized(controllerPrincipal, controller, action))
                        {
                            // TODO: Redirect to configured URL
                            return new HttpResponse(HttpResponseStatusCode.Forbidden);
                        }

                        var response = action.Invoke(controllerInstance, new object[0]) as ActionResult;
                        return response;
                    });

                    Console.WriteLine(httpMethod + " " + path);
                }
            }
            // Reflection
            // Assembly
            // typeof(Server).GetMethods()
            // sb.GetType().GetMethods();
            // Activator.CreateInstance(typeof(Server))
            var sb = DateTime.UtcNow;

        }
    }
}
