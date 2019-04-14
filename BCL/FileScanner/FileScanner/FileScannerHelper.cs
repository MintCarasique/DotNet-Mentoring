using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FileScanner.Configuration;
using FileScanner.Interfaces;
using FileScanner.Resources;
using Dir = System.IO.Directory;

namespace FileScanner
{
    class FileScannerHelper
    {
        private readonly IEnumerable<Rule> _rules;
        private readonly IConsoleAdapter _logger;
        private readonly string _defaultDirectory;
        private int _currentIndexNumber = 1;

        public FileScannerHelper(IEnumerable<Rule> rules, string defaultDirectory, IConsoleAdapter logger)
        {
            _logger = logger;
            _rules = rules;
            _defaultDirectory = defaultDirectory;
        }

        public void ShiftFile(string fileName, string filePath)
        {
            foreach (Rule rule in _rules)
            {
                Regex template = new Regex(rule.Template);
                if (!template.IsMatch(fileName))
                {
                    continue;
                }

                _logger?.Write(Resources.LocalizationResources.FoundMatchingRule);
                string dest = this.BuildNewFilePath(fileName, rule);
                this.TransferFile(filePath, dest);
                _logger?.Write(Resources.LocalizationResources.FileTransfered);

                return;
            }

            string defaultDest = Path.Combine(_defaultDirectory, fileName);
            _logger?.Write(Resources.LocalizationResources.MatchingRuleNotFound);
            this.TransferFile(filePath, defaultDest);
            _logger?.Write(Resources.LocalizationResources.FileTransfered);
        }

        private string BuildNewFilePath(string fileName, Rule rule)
        {
            StringBuilder result = new StringBuilder();

            if (rule.IsShiftDateRequired)
            {
                result.Append($"{DateTime.Now.ToShortDateString()}_");
            }

            if (rule.IsIndexNumberRequired)
            {
                result.Append($"{_currentIndexNumber}_");
            }

            _currentIndexNumber++;
            result.Append(fileName);
            return Path.Combine(rule.DestinationDirectory, result.ToString());
        }

        private void TransferFile(string from, string to)
        {
            string directoryName = Path.GetDirectoryName(to);
            if (!Dir.Exists(directoryName))
            {
                Dir.CreateDirectory(directoryName);
            }

            if (File.Exists(to))
            {
                File.Delete(to);
            }

            File.Move(from, to);
        }
    }
}
