using System;
using System.IO;
using System.Xml;

namespace Verse
{
	// Token: 0x02000D8A RID: 3466
	public class LoadableXmlAsset
	{
		// Token: 0x040033BD RID: 13245
		public string name;

		// Token: 0x040033BE RID: 13246
		public string fullFolderPath;

		// Token: 0x040033BF RID: 13247
		public XmlDocument xmlDoc;

		// Token: 0x040033C0 RID: 13248
		public ModContentPack mod;

		// Token: 0x040033C1 RID: 13249
		public DefPackage defPackage;

		// Token: 0x06004D86 RID: 19846 RVA: 0x002878AC File Offset: 0x00285CAC
		public LoadableXmlAsset(string name, string fullFolderPath, string contents)
		{
			this.name = name;
			this.fullFolderPath = fullFolderPath;
			try
			{
				this.xmlDoc = new XmlDocument();
				this.xmlDoc.LoadXml(contents);
			}
			catch (Exception ex)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Exception reading ",
					name,
					" as XML: ",
					ex
				}), false);
				this.xmlDoc = null;
			}
		}

		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x06004D87 RID: 19847 RVA: 0x00287934 File Offset: 0x00285D34
		public string FullFilePath
		{
			get
			{
				return this.fullFolderPath + Path.DirectorySeparatorChar + this.name;
			}
		}

		// Token: 0x06004D88 RID: 19848 RVA: 0x00287964 File Offset: 0x00285D64
		public override string ToString()
		{
			return this.name;
		}
	}
}
