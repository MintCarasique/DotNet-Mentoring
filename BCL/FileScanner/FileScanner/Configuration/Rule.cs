using System.Configuration;

namespace FileScanner.Configuration
{
	public class Rule : ConfigurationElement
	{
		[ConfigurationProperty("template", IsKey = true)]
		public string Template => (string)this["template"];

		[ConfigurationProperty("destinationDirectory")]
		public string DestinationDirectory => (string)this["destinationDirectory"];

		[ConfigurationProperty("isIndexNumberRequired")]
		public bool IsIndexNumberRequired => (bool)this["isIndexNumberRequired"];

		[ConfigurationProperty("isShiftDateRequired")]
		public bool IsShiftDateRequired => (bool)this["isShiftDateRequired"];
	}
}
