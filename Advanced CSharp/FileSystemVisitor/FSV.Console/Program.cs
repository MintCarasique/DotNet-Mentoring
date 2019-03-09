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

            fileProc.Start += (object sender, EventArgs e) => System.Console.WriteLine("Process started");
            fileProc.FileFound += (object sender, VisitorEventArgs e) => System.Console.WriteLine($"File found: {e.EntryName}");
            fileProc.DirectoryFound += (object sender, VisitorEventArgs e) => System.Console.WriteLine($"Directory found: {e.EntryName}");
            fileProc.FilteredFileFound += (object sender, VisitorEventArgs e) =>
            {
                if(e.EntryName.Length >= 7)
                {
                    System.Console.WriteLine($"Excluding {e.EntryName} Directory from search");
                    e.Action = ProcessAction.Exclude;
                }
            };
            fileProc.FilteredDirectoryFound += (object sender, VisitorEventArgs e) =>
            {
                if (e.EntryName == "bin")
                {
                    System.Console.WriteLine("Bin directory found!");
                    e.Action = ProcessAction.Interrupt;
                }
            };


            foreach (var entry in fileProc.PerformProcess(testPath))
            {
               // System.Console.WriteLine(entry);
            }
        }
    }
}
