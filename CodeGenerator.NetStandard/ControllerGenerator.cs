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
    public class ControllerGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var ns = "Controllers.Generated";
            var controllerName = "Generated";
            var message = DateTime.Now;

            var sourceBuilder = new StringBuilder();

            sourceBuilder.Append(
                $@"using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//yup
namespace {ns}");
            sourceBuilder.AppendLine("{");
            sourceBuilder.AppendLine("  [ApiController]");
            sourceBuilder.AppendLine("  [Route(\"[controller]\")]");
            sourceBuilder.AppendLine($" public class {controllerName} : ControllerBase");
            sourceBuilder.AppendLine("  {");
            sourceBuilder.AppendLine("    [HttpGet]");
            sourceBuilder.AppendLine("    public JsonResult Get()");
            sourceBuilder.AppendLine("    {");
            sourceBuilder.AppendLine($"      return new JsonResult(\"Hi Clara, This controller was generated at {message}\");");
            sourceBuilder.AppendLine("    }");
            sourceBuilder.AppendLine("  }");
            sourceBuilder.AppendLine("}");

            context.AddSource($"{ns}", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
        }

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG
            //if (!Debugger.IsAttached)
            //{
            //    Debugger.Launch();
            //}
#endif        
        }
    }
}
