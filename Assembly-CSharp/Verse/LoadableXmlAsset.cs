using System;
using System.IO;
using System.Xml;

namespace Verse
{
	// Token: 0x02000D8A RID: 3466
	public class LoadableXmlAsset
	{
		// Token: 0x06004D6D RID: 19821 RVA: 0x00285EF0 File Offset: 0x002842F0
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

		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x06004D6E RID: 19822 RVA: 0x00285F78 File Offset: 0x00284378
		public string FullFilePath
		{
			get
			{
				return this.fullFolderPath + Path.DirectorySeparatorChar + this.name;
			}
		}

		// Token: 0x06004D6F RID: 19823 RVA: 0x00285FA8 File Offset: 0x002843A8
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x040033AB RID: 13227
		public string name;

		// Token: 0x040033AC RID: 13228
		public string fullFolderPath;

		// Token: 0x040033AD RID: 13229
		public XmlDocument xmlDoc;

		// Token: 0x040033AE RID: 13230
		public ModContentPack mod;

		// Token: 0x040033AF RID: 13231
		public DefPackage defPackage;
	}
}
