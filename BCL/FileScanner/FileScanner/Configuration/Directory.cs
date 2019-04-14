using System.Configuration;

namespace FileScanner.Configuration
{
	public class Directory : ConfigurationElement
	{
		[ConfigurationProperty("path", IsKey = true)]
		public string Path => (string)base["path"];
	}
}
