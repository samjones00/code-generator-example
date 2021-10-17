using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CodeGenerator.NetStandard
{
    [Generator]
    public class PartialClassGenerator : ISourceGenerator
    {

        public void Execute(GeneratorExecutionContext context)
        {
            var syntaxTrees = context.Compilation.SyntaxTrees;

            foreach (var syntaxTree in syntaxTrees)
            {
                var typeDeclarations = syntaxTree.GetRoot().DescendantNodes()
                   .OfType<TypeDeclarationSyntax>()
                   .Where(x => x.Modifiers.Any(y => y.ValueText.Contains("partial")))
                   //.Where(x => x.AttributeLists.Any(xx => xx.ToString().StartsWith("[ThrowIfNull")))
                   .ToList();

                foreach (var typeDeclaration in typeDeclarations)
                {
                    var parameterDeclarations = syntaxTree.GetRoot().DescendantNodes()
                      .OfType<ParameterListSyntax>()
                      .Where(x => x.Parameters.Any(xx => xx.ToString().StartsWith("[GeneratePartial")))
                      .ToList();

                    var attributeNames = new List<string>();

                    foreach (var parameterDeclaration in parameterDeclarations)
                    {
                        foreach (var item in parameterDeclaration.Parameters.Where(x => x.AttributeLists.Any()))
                        {
                            attributeNames.Add(item.Identifier.ValueText);
                        }
                    }

                    var namespaceName = syntaxTree.GetRoot().DescendantNodes().OfType<NamespaceDeclarationSyntax>();

                    var usingDirectives = syntaxTree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>();
                    var directivesAsText = string.Join("\r\n", usingDirectives);
                    var sourceBuilder = new StringBuilder(directivesAsText);

                    var className = typeDeclaration.Identifier.ToString();
                    var generatedClassName = $"{className}_ThrowIfNull";
                    var splitClass = typeDeclaration.ToString().Split(new[] { '{' }, 2);

                    sourceBuilder.AppendLine(Environment.NewLine);
                    sourceBuilder.AppendLine("namespace CodeGenerator.Core");
                    sourceBuilder.Append($@"{{
                    public partial class {className}
                    {{");

                    sourceBuilder.AppendLine("//hi");
                    sourceBuilder.AppendLine("}}");
                    //sourceBuilder.AppendLine("}}");
                    context.AddSource($"ThrowIfNull.Generated_{className}", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
                }
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
//#if DEBUG
//            if (!Debugger.IsAttached)
//            {
//                Debugger.Launch();
//            }
//#endif
        }
    }
}
