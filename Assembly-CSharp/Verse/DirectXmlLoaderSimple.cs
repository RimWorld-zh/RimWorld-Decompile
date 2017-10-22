using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Verse
{
	public static class DirectXmlLoaderSimple
	{
		public static IEnumerable<KeyValuePair<string, string>> ValuesFromXmlFile(FileInfo file)
		{
			XDocument doc = XDocument.Load(file.FullName);
			foreach (XElement item in doc.Root.Elements())
			{
				string key = item.Name.ToString();
				string value2 = item.Value;
				value2 = value2.Replace("\\n", "\n");
				yield return new KeyValuePair<string, string>(key, value2);
			}
		}
	}
}
