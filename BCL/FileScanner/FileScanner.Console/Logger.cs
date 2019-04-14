using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileScanner.Interfaces;

namespace FileScannerConsole
{
    public class Logger : IConsoleAdapter
    {

        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }
}
