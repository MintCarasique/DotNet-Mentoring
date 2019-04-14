using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FileScanner.Interfaces;
using Directory = System.IO.Directory;

namespace FileScanner
{
    public class FileScanner
    {
        private readonly IConsoleAdapter _console;
        private readonly string _defaultDirectory;

        public FileScanner(IEnumerable<string> directories, IConsoleAdapter console, string defaultDirectory)
        {
            _console = console;
            _defaultDirectory = defaultDirectory;

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

        private void MoveFile(string name, string path)
        {

        }
    }
}
