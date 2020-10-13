using SUS.HTTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SUS.MvcFramework
{
    public static class Host
    {
        public static async Task CreateHostAsync(IMvcApplication application, int port = 80)
        {
            // TODO: {controller}/{action}/{id}
            List<Route> routeTable = new List<Route>();
            IServiceCollection serviceCollection = new ServiceCollection();

            application.ConfigureServices(serviceCollection);
            application.Configure(routeTable);

            AutoRegisterStaticFile(routeTable);
            AutoRegisterRoutes(routeTable, application, serviceCollection);

            Console.WriteLine("All registered routes:");
            foreach (var route in routeTable)
            {
                Console.WriteLine($"{route.Method} {route.Path}");
            }

            IHttpServer server = new HttpServer(routeTable);

            // Process.Start(@"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe", "http://localhost/");
            await server.StartAsync(port);
        }

        private static void AutoRegisterRoutes(List<Route> routeTable, IMvcApplication application, IServiceCollection serviceCollection)
        {
            var controllerTypes = application.GetType().Assembly.GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(Controller)));
            foreach (var controllerType in controllerTypes)
            {
                var methods = controllerType.GetMethods()
                    .Where(x => x.IsPublic && !x.IsStatic && x.DeclaringType == controllerType
                    && !x.IsAbstract && !x.IsConstructor && !x.IsSpecialName);
                foreach (var method in methods)
                {
                    var url = "/" + controllerType.Name.Replace("Controller", string.Empty)
                        + "/" + method.Name;

                    var attribute = method.GetCustomAttributes(false)
                        .Where(x => x.GetType().IsSubclassOf(typeof(BaseHttpAttribute)))
                        .FirstOrDefault() as BaseHttpAttribute;

                    var httpMethod = HttpMethod.Get;

                    if (attribute != null)
                    {
                        httpMethod = attribute.Method;
                    }

                    if (!string.IsNullOrEmpty(attribute?.Url))
                    {
                        url = attribute.Url;
                    }

                    routeTable.Add(new Route(url, httpMethod, request => ExecuteAction(request, controllerType, method, serviceCollection)));
                }
            }
        }

        private static HttpResponse ExecuteAction(HttpRequest request, Type controllerType, MethodInfo action, IServiceCollection serviceCollection)
        {
            var instance = serviceCollection.CreateInstance(controllerType) as Controller;
            instance.Request = request;
            var arguments = new List<object>();
            var parameters = action.GetParameters();
            foreach (var parameter in parameters)
            {
                var httpParamerValue = GetParameterFromRequest(request, parameter.Name);
                var parameterValue = Convert.ChangeType(httpParamerValue, parameter.ParameterType);
                if (parameterValue == null && 
                    parameter.ParameterType != typeof(string)
                    && parameter.ParameterType != typeof(int?))
                {
                    // complex type
                    parameterValue = Activator.CreateInstance(parameter.ParameterType);
                    var properties = parameter.ParameterType.GetProperties();
                    foreach (var property in properties)
                    {
                        var propertyHttpParamerValue = GetParameterFromRequest(request, property.Name);
                        var propertyParameterValue = Convert.ChangeType(propertyHttpParamerValue, property.PropertyType);
                        property.SetValue(parameterValue, propertyParameterValue);
                    }
                }
                
                arguments.Add(parameterValue);
            }

            var response = action.Invoke(instance, arguments.ToArray()) as HttpResponse;
            return response;
        }

        private static string GetParameterFromRequest(HttpRequest request, string parameterName)
        {
            parameterName = parameterName.ToLower();
            if (request.FormData.Any(x => x.Key.ToLower() == parameterName))
            {
                return request.FormData
                    .FirstOrDefault(x => x.Key.ToLower() == parameterName).Value;
            }

            if (request.QueryData.Any(x => x.Key.ToLower() == parameterName))
            {
                return request.QueryData
                    .FirstOrDefault(x => x.Key.ToLower() == parameterName).Value;
            }

            return null;
        }

        private static void AutoRegisterStaticFile(List<Route> routeTable)
        {
            var staticFiles = Directory.GetFiles("wwwroot", "*", SearchOption.AllDirectories);
            foreach (var staticFile in staticFiles)
            {
                var url = staticFile.Replace("wwwroot", string.Empty)
                    .Replace("\\", "/");
                routeTable.Add(new Route(url, HttpMethod.Get, (request) =>
                {
                    var fileContent = File.ReadAllBytes(staticFile);
                    var fileExt = new FileInfo(staticFile).Extension;
                    var contentType = fileExt switch
                    {
                        ".txt" => "text/plain",
                        ".js" => "text/javascript",
                        ".css" => "text/css",
                        ".jpg" => "image/jpg",
                        ".jpeg" => "image/jpg",
                        ".png" => "image/png",
                        ".gif" => "image/gif",
                        ".ico" => "image/vnd.microsoft.icon",
                        ".html" => "text/html",
                        _ => "text/plain",
                    };

                    return new HttpResponse(contentType, fileContent, HttpStatusCode.Ok);
                }));
            }
        }
    }
}
