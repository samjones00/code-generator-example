using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace CodeGenerator.Console
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
                    var splitClass = typeDeclaration.ToString().Split('{', 2);

                    sourceBuilder.Append($@"
namespace GeneratedMappers
{{
public class {generatedClassName}
{{
");

                    sourceBuilder.AppendLine(splitClass[1].Replace(className, generatedClassName));
                    sourceBuilder.AppendLine("}");
                    context.AddSource($"MapperGenerator {generatedClassName}", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));

                }
            }
        }

        public void Initialize(GeneratorInitializationContext context)
        {
//#if DEBUG
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
        }
//#endif
    }
}