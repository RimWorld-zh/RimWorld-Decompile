using System;
using System.IO;
using System.Xml;

namespace Verse
{
	// Token: 0x02000D89 RID: 3465
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

		// Token: 0x06004D86 RID: 19846 RVA: 0x002875CC File Offset: 0x002859CC
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
		// (get) Token: 0x06004D87 RID: 19847 RVA: 0x00287654 File Offset: 0x00285A54
		public string FullFilePath
		{
			get
			{
				return this.fullFolderPath + Path.DirectorySeparatorChar + this.name;
			}
		}

		// Token: 0x06004D88 RID: 19848 RVA: 0x00287684 File Offset: 0x00285A84
		public override string ToString()
		{
			return this.name;
		}
	}
}
