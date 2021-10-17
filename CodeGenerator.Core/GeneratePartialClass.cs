using System;

namespace CodeGenerator.Core
{
    [AttributeUsage(AttributeTargets.Parameter)]
    internal class GeneratePartialClass : Attribute
    {
    }
}