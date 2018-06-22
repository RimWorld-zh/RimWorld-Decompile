using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Verse
{
	// Token: 0x02000D7F RID: 3455
	public static class DirectXmlLoaderSimple
	{
		// Token: 0x06004D6D RID: 19821 RVA: 0x00285B6C File Offset: 0x00283F6C
		public static IEnumerable<DirectXmlLoaderSimple.XmlKeyValuePair> ValuesFromXmlFile(FileInfo file)
		{
			XDocument doc = XDocument.Load(file.FullName, LoadOptions.SetLineInfo);
			foreach (XElement element in doc.Root.Elements())
			{
				string key = element.Name.ToString();
				string value = element.Value;
				value = value.Replace("\\n", "\n");
				yield return new DirectXmlLoaderSimple.XmlKeyValuePair
				{
					key = key,
					value = value,
					lineNumber = ((IXmlLineInfo)element).LineNumber
				};
			}
			yield break;
		}

		// Token: 0x02000D80 RID: 3456
		public struct XmlKeyValuePair
		{
			// Token: 0x040033A5 RID: 13221
			public string key;

			// Token: 0x040033A6 RID: 13222
			public string value;

			// Token: 0x040033A7 RID: 13223
			public int lineNumber;
		}
	}
}
