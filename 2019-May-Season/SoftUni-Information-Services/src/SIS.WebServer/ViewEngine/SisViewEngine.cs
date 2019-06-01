﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SIS.MvcFramework.ViewEngine
{
    public class SisViewEngine : IViewEngine
    {
        public string GetHtml<T>(string viewContent, T model)
        {
            string csharpHtmlCode = this.GetCSharpCode(viewContent);
            string code = $@"
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using SIS.MvcFramework.ViewEngine;
namespace AppViewCodeNamespace
{{
    public class AppViewCode : IView
    {{
        public string GetHtml(object model)
        {{
            var Model = model as {model.GetType().FullName};
	        var html = new StringBuilder();

            {csharpHtmlCode}
            
	        return html.ToString();
        }}
    }}
}}";
            var view = this.CompileAndInstance(code, model.GetType().Assembly);
            var htmlResult = view?.GetHtml(model);
            return htmlResult;
        }

        private string GetCSharpCode(string viewContent)
        {
            // TODO: { var a = "Niki"; }
            var lines = viewContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            var csharpCode = new StringBuilder();
            var supportedOperators = new[] { "for", "if", "else" };
            var csharpCodeRegex = new Regex(@"[^\s<""]+", RegexOptions.Compiled);
            foreach (var line in lines)
            {
                if (line.TrimStart().StartsWith("{") || line.TrimStart().StartsWith("}"))
                {
                    // { / }
                    csharpCode.AppendLine(line);
                }
                else if (supportedOperators.Any(x => line.TrimStart().StartsWith("@" + x)))
                {
                    // @C#
                    var atSignLocation = line.IndexOf("@");
                    var csharpLine = line.Remove(atSignLocation, 1);
                    csharpCode.AppendLine(csharpLine);
                }
                else
                {
                    // HTML
                    if (!line.Contains("@"))
                    {
                        var csharpLine = $"html.AppendLine(@\"{line.Replace("\"", "\"\"")}\");";
                        csharpCode.AppendLine(csharpLine);
                    }
                    else
                    {
                        var csharpStringToAppend = "html.AppendLine(@\"";
                        var restOfLine = line;
                        while (restOfLine.Contains("@"))
                        {
                            var atSignLocation = restOfLine.IndexOf("@");
                            var plainText = restOfLine.Substring(0, atSignLocation).Replace("\"", "\"\"");
                            var csharpExpression = csharpCodeRegex.Match(restOfLine.Substring(atSignLocation + 1))?.Value;
                            csharpStringToAppend += plainText + "\" + " + csharpExpression + " + @\"";

                            if (restOfLine.Length <= atSignLocation + csharpExpression.Length + 1)
                            {
                                restOfLine = string.Empty;
                            }
                            else
                            {
                                restOfLine = restOfLine.Substring(atSignLocation + csharpExpression.Length + 1);
                            }
                        }

                        csharpStringToAppend += $"{restOfLine.Replace("\"", "\"\"")}\");";
                        csharpCode.AppendLine(csharpStringToAppend);
                    }
                }
            }

            return csharpCode.ToString();
        }

        private IView CompileAndInstance(string code, Assembly modelAssembly)
        {
            var compilation = CSharpCompilation.Create("AppViewAssembly")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(modelAssembly.Location));

            var netStandardAssembly = Assembly.Load(new AssemblyName("netstandard")).GetReferencedAssemblies();
            foreach (var assembly in netStandardAssembly)
            {
                compilation = compilation.AddReferences(
                    MetadataReference.CreateFromFile(Assembly.Load(assembly).Location));
            }

            compilation = compilation.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(code));

            using (var memoryStream = new MemoryStream())
            {
                var compilationResult = compilation.Emit(memoryStream);
                if (!compilationResult.Success)
                {
                    foreach (var error in compilationResult.Diagnostics.Where(x => x.Severity == DiagnosticSeverity.Error))
                    {
                        Console.WriteLine(error.GetMessage());
                    }

                    return null;
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
                var assemblyBytes = memoryStream.ToArray();
                var assembly = Assembly.Load(assemblyBytes);

                var type = assembly.GetType("AppViewCodeNamespace.AppViewCode");
                if (type == null)
                {
                    Console.WriteLine("AppViewCode not found.");
                    return null;
                }

                var instance = Activator.CreateInstance(type);
                return instance as IView;
            }
        }
    }
}
