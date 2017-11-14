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
			using (IEnumerator<XElement> enumerator = doc.Root.Elements().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					XElement element = enumerator.Current;
					string key = element.Name.ToString();
					string value2 = element.Value;
					value2 = value2.Replace("\\n", "\n");
					yield return new KeyValuePair<string, string>(key, value2);
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			yield break;
			IL_0121:
			/*Error near IL_0122: Unexpected return in MoveNext()*/;
		}
	}
}
