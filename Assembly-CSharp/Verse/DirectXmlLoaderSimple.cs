using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Verse
{
	// Token: 0x02000D83 RID: 3459
	public static class DirectXmlLoaderSimple
	{
		// Token: 0x06004D5A RID: 19802 RVA: 0x002845DC File Offset: 0x002829DC
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

		// Token: 0x02000D84 RID: 3460
		public struct XmlKeyValuePair
		{
			// Token: 0x0400339C RID: 13212
			public string key;

			// Token: 0x0400339D RID: 13213
			public string value;

			// Token: 0x0400339E RID: 13214
			public int lineNumber;
		}
	}
}
