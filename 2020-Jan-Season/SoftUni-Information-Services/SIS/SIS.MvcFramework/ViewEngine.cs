using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SIS.MvcFramework
{
    public class ViewEngine : IViewEngine
    {
        public string GetHtml(string templateHtml, object model, string user)
        {
            var methodCode = PrepareCSharpCode(templateHtml);

            var modelType = model?.GetType() ?? typeof(object);
            var typeName = modelType.FullName;

            if (modelType.IsGenericType)
            {
                typeName = GetGenericTypeFullName(modelType);
            }

            var code = @$"using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using SIS.MvcFramework;
namespace AppViewNamespace
{{
    public class AppViewCode : IView
    {{
        public string GetHtml(object model, string user)
        {{
            var Model = model as {typeName};
            var User = user;
            var html = new StringBuilder();

{methodCode}

            return html.ToString();
        }}
    }}
}}";

            IView view = GetInstanceFromCode(code, model);
            string html = view.GetHtml(model, user);
            return html;
        }

        private string GetGenericTypeFullName(Type modelType)
        {
            var argumentCountBeginning = modelType.Name.LastIndexOf('`');
            var genericModelTypeName = modelType.Name.Substring(0, argumentCountBeginning);
            var genericTypeFullName = $"{modelType.Namespace}.{genericModelTypeName}";
            var genericTypeArguments = modelType.GenericTypeArguments.Select(GetGenericTypeArgumentFullName);
            var modelTypeName = $"{genericTypeFullName}<{string.Join(", ", genericTypeArguments)}>";
            return modelTypeName;
        }

        private string GetGenericTypeArgumentFullName(Type genericTypeArgument)
        {
            if (genericTypeArgument.IsGenericType)
            {
                return GetGenericTypeFullName(genericTypeArgument);
            }

            return genericTypeArgument.FullName;
        }

        private IView GetInstanceFromCode(string code, object model)
        {
            var compilation = CSharpCompilation.Create("AppViewAssembly")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
            if (model != null)
            {
                compilation = compilation.AddReferences(MetadataReference.CreateFromFile(model.GetType().Assembly.Location));
            }

            var libraries = Assembly.Load(new AssemblyName("netstandard")).GetReferencedAssemblies();
            foreach (var library in libraries)
            {
                compilation = compilation.AddReferences(
                    MetadataReference.CreateFromFile(Assembly.Load(library).Location));
            }

            compilation = compilation.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(code));

            using var memoryStream = new MemoryStream();

            var compilationResult = compilation.Emit(memoryStream);
            if (!compilationResult.Success)
            {
                return new ErrorView(
                    compilationResult.Diagnostics
                    .Where(x => x.Severity == DiagnosticSeverity.Error)
                    .Select(x => x.GetMessage()));
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            var assemblyByteArray = memoryStream.ToArray();
            var assembly = Assembly.Load(assemblyByteArray);
            var type = assembly.GetType("AppViewNamespace.AppViewCode");
            var instance = Activator.CreateInstance(type) as IView;
            return instance;
        }

        private string PrepareCSharpCode(string templateHtml)
        {
            var cSharpExpressionRegex = new Regex(@"[^\<\""\s&]+", RegexOptions.Compiled);
            var supportedOpperators = new[] { "if", "for", "foreach", "else" };
            StringBuilder cSharpCode = new StringBuilder();
            StringReader reader = new StringReader(templateHtml);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (line.TrimStart().StartsWith("{")
                    || line.TrimStart().StartsWith("}"))
                {
                    cSharpCode.AppendLine(line);
                }
                else if (supportedOpperators.Any(x => line.TrimStart().StartsWith("@" + x)))
                {
                    var indexOfAt = line.IndexOf("@");
                    line = line.Remove(indexOfAt, 1);
                    cSharpCode.AppendLine(line);
                }
                else
                {
                    var currentCSharpLine = new StringBuilder("html.AppendLine(@\"");
                    while (line.Contains("@"))
                    {
                        var atSignLocation = line.IndexOf("@");
                        var before = line.Substring(0, atSignLocation);
                        currentCSharpLine.Append(before.Replace("\"", "\"\"") + "\" + ");
                        var cSharpAndEndOfLine = line.Substring(atSignLocation + 1);
                        var cSharpExpression = cSharpExpressionRegex.Match(cSharpAndEndOfLine);
                        currentCSharpLine.Append(cSharpExpression.Value + " + @\"");
                        var after = cSharpAndEndOfLine.Substring(cSharpExpression.Length);
                        line = after;
                    }

                    currentCSharpLine.Append(line.Replace("\"", "\"\"") + "\");");
                    cSharpCode.AppendLine(currentCSharpLine.ToString());
                }
            }

            return cSharpCode.ToString();
        }
    }
}
