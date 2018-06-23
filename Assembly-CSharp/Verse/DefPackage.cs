using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000AF7 RID: 2807
	public class DefPackage
	{
		// Token: 0x04002756 RID: 10070
		public string fileName = "NamelessPackage";

		// Token: 0x04002757 RID: 10071
		public string relFolder = "";

		// Token: 0x04002758 RID: 10072
		public List<Def> defs = new List<Def>();

		// Token: 0x06003E39 RID: 15929 RVA: 0x0020CC16 File Offset: 0x0020B016
		public DefPackage(string name, string relFolder)
		{
			this.fileName = name;
			this.relFolder = relFolder;
		}

		// Token: 0x06003E3A RID: 15930 RVA: 0x0020CC50 File Offset: 0x0020B050
		public List<Def>.Enumerator GetEnumerator()
		{
			return this.defs.GetEnumerator();
		}

		// Token: 0x06003E3B RID: 15931 RVA: 0x0020CC70 File Offset: 0x0020B070
		public void AddDef(Def def)
		{
			this.defs.Add(def);
		}

		// Token: 0x06003E3C RID: 15932 RVA: 0x0020CC80 File Offset: 0x0020B080
		public void RemoveDef(Def def)
		{
			if (def == null)
			{
				throw new ArgumentNullException("def");
			}
			if (!this.defs.Contains(def))
			{
				throw new InvalidOperationException(string.Concat(new object[]
				{
					"Package ",
					this,
					" cannot remove ",
					def,
					" because it doesn't contain it."
				}));
			}
			this.defs.Remove(def);
		}

		// Token: 0x06003E3D RID: 15933 RVA: 0x0020CCF0 File Offset: 0x0020B0F0
		public void SaveIn(ModContentPack mod)
		{
			string fullFolderPath = this.GetFullFolderPath(mod);
			string text = Path.Combine(fullFolderPath, this.fileName);
			XDocument xdocument = new XDocument();
			XElement xelement = new XElement("DefPackage");
			xdocument.Add(xelement);
			try
			{
				foreach (Def def in this.defs)
				{
					XElement content = DirectXmlSaver.XElementFromObject(def, def.GetType());
					xelement.Add(content);
				}
				DirectXmlSaveFormatter.AddWhitespaceFromRoot(xelement);
				SaveOptions options = SaveOptions.DisableFormatting;
				xdocument.Save(text, options);
				Messages.Message("Saved in " + text, MessageTypeDefOf.PositiveEvent, false);
			}
			catch (Exception ex)
			{
				Messages.Message("Exception saving XML: " + ex.ToString(), MessageTypeDefOf.NegativeEvent, false);
				throw;
			}
		}

		// Token: 0x06003E3E RID: 15934 RVA: 0x0020CDF4 File Offset: 0x0020B1F4
		public override string ToString()
		{
			return this.relFolder + "/" + this.fileName;
		}

		// Token: 0x06003E3F RID: 15935 RVA: 0x0020CE20 File Offset: 0x0020B220
		public string GetFullFolderPath(ModContentPack mod)
		{
			return Path.GetFullPath(Path.Combine(Path.Combine(mod.RootDir, "Defs/"), this.relFolder));
		}

		// Token: 0x06003E40 RID: 15936 RVA: 0x0020CE58 File Offset: 0x0020B258
		public static string UnusedPackageName(string relFolder, ModContentPack mod)
		{
			string fullPath = Path.GetFullPath(Path.Combine(Path.Combine(mod.RootDir, "Defs/"), relFolder));
			int num = 1;
			string text;
			do
			{
				text = "NewPackage" + num.ToString() + ".xml";
				num++;
			}
			while (File.Exists(Path.Combine(fullPath, text)));
			return text;
		}
	}
}
