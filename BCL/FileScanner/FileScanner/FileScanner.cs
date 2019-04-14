using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FileScanner.Interfaces;
using FileScanner.Configuration;
using Directory = System.IO.Directory;
using Rule = FileScanner.Configuration.Rule;

namespace FileScanner
{
    public class FileScanner
    {
        private readonly IConsoleAdapter _console;
        private readonly string _defaultDirectory;
        private readonly FileScannerHelper _helper;

        public FileScanner(IEnumerable<string> directories, IConsoleAdapter console, string defaultDirectory, IEnumerable<Rule> rules)
        {
            _console = console;
            _defaultDirectory = defaultDirectory;
            _helper = new FileScannerHelper(rules, defaultDirectory, console);

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
            var creationDate = File.GetCreationTime(filePath);
            _console?.Write(string.Format(
                Resources.LocalizationResources.CreatedFileFound,
                fileName,
                creationDate.ToShortDateString()));
            _helper.ShiftFile(fileName, filePath);
        }
    }
}
