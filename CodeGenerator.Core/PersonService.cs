//this one
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenerator.Core
{
    public partial class PersonService
    {
        public void DoSomething([GeneratePartialClass] string param1, string param2, [GeneratePartialClass] string param3)
        {

            throw new ArgumentNullException();
        }
    }
}
