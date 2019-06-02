using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using SIS.MvcFramework.Identity;
using System.Collections.Generic;

namespace SIS.MvcFramework.ViewEngine
{
    public class SisViewEngine : IViewEngine
    {
        private bool IsHtmlEscapeTag = false;
        private bool IsHtmlTag = false;
        private bool IsCsharpCodeBlock = false;

        private Stack<string> StackCodeBlock;
        private Stack<string> StackHtmlBlock;

        private string GetModelType<T>(T model)
        {
            if (model is IEnumerable)
            {
                return $"IEnumerable<{model.GetType().GetGenericArguments()[0].FullName}>";
            }

            return model.GetType().FullName;
        }

        public string GetHtml<T>(string viewContent, T model, Principal user = null)
        {
            string csharpHtmlCode = this.GetCSharpCode(viewContent);
            string code = $@"
using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using SIS.MvcFramework.ViewEngine;
using SIS.MvcFramework.Identity;
namespace AppViewCodeNamespace
{{
    public class AppViewCode : IView
    {{
        public string GetHtml(object model, Principal user)
        {{
            var Model = {(model == null ? "new {}" : "model as " + GetModelType(model))};
            var User = user;            

	        var html = new StringBuilder();

            {csharpHtmlCode}
            
	        return html.ToString();
        }}
    }}
}}";
            var view = this.CompileAndInstance(code, model?.GetType().Assembly);
            var htmlResult = view?.GetHtml(model, user);
            return htmlResult;
        }

        private void TrakingHtmlEscapeTag(string line)
        {
            if (line.TrimStart().StartsWith("<style>") || line.TrimStart().StartsWith("<script>"))
            {
                this.IsHtmlEscapeTag = true;
            }

            if (line.TrimStart().StartsWith("</style>") || line.TrimStart().StartsWith("</script>")
                || line.TrimEnd().EndsWith("</style>") || line.TrimEnd().EndsWith("</script>"))
            {
                this.IsHtmlEscapeTag = false;
            }
        }

        private void TrakingHtmlTags(string line)
        {
            if (line.Contains("<"))
            {
                this.IsHtmlTag = true;
                this.StackHtmlBlock.Push(line);
            }

            if (this.StackHtmlBlock.Count == 0)
            {
                this.IsHtmlTag = false;
            }

            if (this.StackHtmlBlock.Count != 0 && line.Contains("</"))
            {
                this.StackHtmlBlock.Pop();
            }

        }

        private void TrakingCsharpCodeBlock(string line)
        {
            if (line.TrimStart().StartsWith("@{"))
            {
                this.IsCsharpCodeBlock = true;
                this.StackCodeBlock.Push(line);
            }

            if (this.StackCodeBlock.Count == 0)
            {
                this.IsCsharpCodeBlock = false;
            }

            if (this.IsCsharpCodeBlock)
            {
                if (line.TrimStart().StartsWith("{"))
                {
                    this.StackCodeBlock.Push(line);
                }

                if (this.StackCodeBlock.Count != 0 &&
                    line.TrimStart().StartsWith("}") || line.TrimEnd().EndsWith("}"))
                {
                    this.StackCodeBlock.Pop();
                }
            }
        }

        private string GetCsharpStringFromHtmlLine(string line)
        {
            var csharpCodeRegex = new Regex(@"[^\s<""]+", RegexOptions.Compiled);

            var csharpStringToAppend = "html.AppendLine(@\"";
            var restOfLine = line;
            while (restOfLine.Contains("@"))
            {
                var atSignLocation = restOfLine.IndexOf("@");
                var plainText = restOfLine.Substring(0, atSignLocation).Replace("\"", "\"\"");
                var csharpExpression = csharpCodeRegex.Match(restOfLine.Substring(atSignLocation + 1))?.Value;

                if (csharpExpression.Contains("{") && csharpExpression.Contains("}"))
                {
                    var csharpInlineExpression =
                        csharpExpression.Substring(1, csharpExpression.IndexOf("}") - 1);

                    csharpStringToAppend += plainText + "\" + " + csharpInlineExpression + " + @\"";

                    csharpExpression = csharpExpression.Substring(0, csharpExpression.IndexOf("}") + 1);
                }
                else
                {
                    csharpStringToAppend += plainText + "\" + " + csharpExpression + " + @\"";
                }

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
            return csharpStringToAppend;
        }

        private string GetCSharpCode(string viewContent)
        {
            // TODO: { var a = "Niki"; }
            var lines = viewContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            var csharpCode = new StringBuilder();
            var supportedOperators = new[] { "for", "if", "else" };
            
            this.StackCodeBlock = new Stack<string>();
            this.StackHtmlBlock = new Stack<string>();

            foreach (var line in lines)
            {

                this.TrakingHtmlEscapeTag(line);
                this.TrakingCsharpCodeBlock(line);

                if (!this.IsHtmlEscapeTag && !this.IsCsharpCodeBlock
                    && (line.TrimStart().StartsWith("{") || line.TrimStart().StartsWith("}")))
                {
                    // { / }
                    csharpCode.AppendLine(line);   
                }
                else if (this.IsCsharpCodeBlock)
                {
                    this.TrakingHtmlTags(line);

                    if (line.TrimStart().StartsWith("@{") && line.TrimEnd().EndsWith("}"))
                    {
                        var atSignLocation = line.IndexOf("@");
                        var csharpLine = line.Remove(atSignLocation, 2);
                        csharpLine = csharpLine.Remove(csharpLine.Length - 1, 1);
                        csharpCode.AppendLine(csharpLine);
                    }

                    if (this.StackCodeBlock.Count != 0 && !line.TrimStart().StartsWith("@{") && !this.IsHtmlTag)
                    {
                        csharpCode.AppendLine(line);
                    }

                    if(this.IsHtmlTag)
                    {
                        string csharpStringToAppend = GetCsharpStringFromHtmlLine(line);
                        csharpCode.AppendLine(csharpStringToAppend);
                    }

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
                    // HTML TODO: Improve escape @RenderBody()
                    if (!line.Contains("@") || line.Contains("@RenderBody()"))
                    {
                        var csharpLine = $"html.AppendLine(@\"{line.Replace("\"", "\"\"")}\");";
                        csharpCode.AppendLine(csharpLine);
                    }
                    else
                    {
                        string csharpStringToAppend = GetCsharpStringFromHtmlLine(line);
                        csharpCode.AppendLine(csharpStringToAppend);
                    }
                }
            }

            return csharpCode.ToString();
        }



        private IView CompileAndInstance(string code, Assembly modelAssembly)
        {
            modelAssembly = modelAssembly ?? Assembly.GetEntryAssembly();

            var compilation = CSharpCompilation.Create("AppViewAssembly")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(Object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(Assembly.GetEntryAssembly().Location))
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
