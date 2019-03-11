using System;
using HelloWorldStandard;

namespace HelloWorldConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var userName = Console.ReadLine();
            var hello = new HelloWorld();
            Console.WriteLine(hello.ReturnHelloMessage(userName));
        }
    }
}
