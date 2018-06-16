using System;
using System.IO;
using System.Xml;

namespace Verse
{
	// Token: 0x02000D8B RID: 3467
	public class LoadableXmlAsset
	{
		// Token: 0x06004D6F RID: 19823 RVA: 0x00285F10 File Offset: 0x00284310
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
		// (get) Token: 0x06004D70 RID: 19824 RVA: 0x00285F98 File Offset: 0x00284398
		public string FullFilePath
		{
			get
			{
				return this.fullFolderPath + Path.DirectorySeparatorChar + this.name;
			}
		}

		// Token: 0x06004D71 RID: 19825 RVA: 0x00285FC8 File Offset: 0x002843C8
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x040033AD RID: 13229
		public string name;

		// Token: 0x040033AE RID: 13230
		public string fullFolderPath;

		// Token: 0x040033AF RID: 13231
		public XmlDocument xmlDoc;

		// Token: 0x040033B0 RID: 13232
		public ModContentPack mod;

		// Token: 0x040033B1 RID: 13233
		public DefPackage defPackage;
	}
}
