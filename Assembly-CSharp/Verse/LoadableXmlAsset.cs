using System;
using System.IO;
using System.Xml;

namespace Verse
{
	public class LoadableXmlAsset
	{
		public string name;

		public string fullFolderPath;

		public XmlDocument xmlDoc;

		public ModContentPack mod;

		public DefPackage defPackage;

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

		public string FullFilePath
		{
			get
			{
				return this.fullFolderPath + Path.DirectorySeparatorChar + this.name;
			}
		}

		public override string ToString()
		{
			return this.name;
		}
	}
}
