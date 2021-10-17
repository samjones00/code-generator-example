using CodeGenerator.Core;
using CodeGenerator.Generated;

namespace CodeGenerator.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var person = new Person();

            //
            var per = new PersonDto();

            var service = new PersonService();

            service.DoSomething(null,null,null);
        }
    }
}
