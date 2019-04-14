using FileScanner.Configuration;
using System.Configuration;

namespace FileScanner.Configuration
{
	public class DirectoriesCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement() => new Directory();

		protected override object GetElementKey(ConfigurationElement element) =>
			((Directory)element).Path;
	}
}
