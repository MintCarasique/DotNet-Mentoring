using FSV.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSV.Console
{
    class Program
    {
        private const string testPath = "D:\\Documents\\DotNet-Mentoring\\";

        static void Main(string[] args)
        {
            var fileProc = new FileSystemVisitor();

            var fileEntries = fileProc.PerformProcess(testPath);
            
            foreach(var entry in fileEntries)
            {
                System.Console.WriteLine(entry);
            }
        }
    }
}
