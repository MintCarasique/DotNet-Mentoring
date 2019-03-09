using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSV.Library
{
    public class FileSystemVisitor
    {
        public IEnumerable<string> PerformProcess(string startPath)
        {
            string[] entries = Directory.GetFileSystemEntries(startPath, "*", SearchOption.AllDirectories);

            return entries;
        }
        
    }
}
