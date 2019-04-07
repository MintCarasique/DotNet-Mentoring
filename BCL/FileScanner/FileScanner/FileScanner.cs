using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Directory = System.IO.Directory;

namespace FileScanner
{
    public class FileScanner
    {
        public FileScanner(IEnumerable<string> directories)
        {
            foreach (var directory in directories)
            {
                AddDirectoryToScan(directory);
            }
        }

        private void AddDirectoryToScan(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            FileSystemWatcher watcher = new FileSystemWatcher(directory);

            watcher.Created += (sender, eventArgs) =>
            {
                this.OnFileCreated(eventArgs.Name, eventArgs.FullPath);
            };

            watcher.EnableRaisingEvents = true;
        }

        private void OnFileCreated(string fileName, string filePath)
        {

        }
    }
}
