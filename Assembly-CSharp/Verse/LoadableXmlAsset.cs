using System;
using System.IO;
using System.Xml;

namespace Verse
{
	// Token: 0x02000D87 RID: 3463
	public class LoadableXmlAsset
	{
		// Token: 0x040033B6 RID: 13238
		public string name;

		// Token: 0x040033B7 RID: 13239
		public string fullFolderPath;

		// Token: 0x040033B8 RID: 13240
		public XmlDocument xmlDoc;

		// Token: 0x040033B9 RID: 13241
		public ModContentPack mod;

		// Token: 0x040033BA RID: 13242
		public DefPackage defPackage;

		// Token: 0x06004D82 RID: 19842 RVA: 0x002874A0 File Offset: 0x002858A0
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

		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x06004D83 RID: 19843 RVA: 0x00287528 File Offset: 0x00285928
		public string FullFilePath
		{
			get
			{
				return this.fullFolderPath + Path.DirectorySeparatorChar + this.name;
			}
		}

		// Token: 0x06004D84 RID: 19844 RVA: 0x00287558 File Offset: 0x00285958
		public override string ToString()
		{
			return this.name;
		}
	}
}
