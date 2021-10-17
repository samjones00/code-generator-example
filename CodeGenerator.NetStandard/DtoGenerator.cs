using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CodeGenerator
{
    [Generator]
    public class DtoGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var syntaxTrees = context.Compilation.SyntaxTrees;

            foreach (var syntaxTree in syntaxTrees)
            {
                var typeDeclarations = syntaxTree.GetRoot().DescendantNodes()
                    .OfType<TypeDeclarationSyntax>()
                    .Where(x => x.AttributeLists.Any(xx => xx.ToString().StartsWith("[GenerateDto")))
                    .ToList();

                foreach (var typeDeclaration in typeDeclarations)
                {
                    var usingDirectives = syntaxTree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>();
                    var directivesAsText = string.Join("\r\n", usingDirectives);
                    var sourceBuilder = new StringBuilder(directivesAsText);

                    var className = typeDeclaration.Identifier.ToString();
                    var generatedClassName = $"{className}Dto";
                    var splitClass = typeDeclaration.ToString().Split(new[] { '{' }, 2);

                    sourceBuilder.AppendLine(Environment.NewLine);
                    sourceBuilder.AppendLine("namespace CodeGenerator.Generated");
                    sourceBuilder.Append($@"{{
    public class {generatedClassName}
    {{");

                    sourceBuilder.AppendLine(splitClass[1].Replace(className, generatedClassName));
                    sourceBuilder.AppendLine("}");
                    context.AddSource($"CodeGenerator_{generatedClassName}", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
                }
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
//            #if DEBUG
//            if (!Debugger.IsAttached)
//            {
//                Debugger.Launch();
//            }
//#endif
        }
    }
}