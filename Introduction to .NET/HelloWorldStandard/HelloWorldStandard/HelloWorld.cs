using System;

namespace HelloWorldStandard
{
    public class HelloWorld
    {
        public string ReturnHelloMessage(string name)
        {
            return $"{DateTime.Now} Hello, {name}";
        }
    }
}
