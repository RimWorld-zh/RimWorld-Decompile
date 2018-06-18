using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Verse
{
	// Token: 0x02000D82 RID: 3458
	public static class DirectXmlLoaderSimple
	{
		// Token: 0x06004D58 RID: 19800 RVA: 0x002845BC File Offset: 0x002829BC
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

		// Token: 0x02000D83 RID: 3459
		public struct XmlKeyValuePair
		{
			// Token: 0x0400339A RID: 13210
			public string key;

			// Token: 0x0400339B RID: 13211
			public string value;

			// Token: 0x0400339C RID: 13212
			public int lineNumber;
		}
	}
}
