using System;
using System.Collections.Generic;
using FileScanner;
using FileScanner.Configuration;
using FileScanner.Resources;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Directory = FileScanner.Configuration.Directory;

namespace FileScannerConsole
{
    class Program
    {
        private static List<string> _directories;
        private static List<Rule> _rules;
        static void Main(string[] args)
        {
            if (!(ConfigurationManager
                .GetSection("fileScannerSection") is FileScannerSection configuration))
            {
                Console.WriteLine(LocalizationResources.ConfigurationSectionWasNotFound);

                return;
            }

            GetConfigurationInfo(configuration);

            FileScanner.FileScanner watcher = new FileScanner.FileScanner(
                _directories,
                new Logger(),
                configuration.Rules.DefaultDirectory,
                _rules);

            Console.WriteLine(LocalizationResources.ExitMessage);

            while (true) ;
        }

        private static void GetConfigurationInfo(FileScannerSection config)
        {
            _directories = new List<string>();
            _rules = new List<Rule>();

            foreach (Directory directory in config.Directories)
            {
                _directories.Add(directory.Path);
            }

            foreach (Rule rule in config.Rules)
            {
                _rules.Add(rule);
            }

            CultureInfo.DefaultThreadCurrentCulture = config.Culture;
            CultureInfo.DefaultThreadCurrentUICulture = config.Culture;
        }
    }
}
