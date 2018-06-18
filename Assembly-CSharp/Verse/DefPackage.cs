using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000AFB RID: 2811
	public class DefPackage
	{
		// Token: 0x06003E3E RID: 15934 RVA: 0x0020C8F2 File Offset: 0x0020ACF2
		public DefPackage(string name, string relFolder)
		{
			this.fileName = name;
			this.relFolder = relFolder;
		}

		// Token: 0x06003E3F RID: 15935 RVA: 0x0020C92C File Offset: 0x0020AD2C
		public List<Def>.Enumerator GetEnumerator()
		{
			return this.defs.GetEnumerator();
		}

		// Token: 0x06003E40 RID: 15936 RVA: 0x0020C94C File Offset: 0x0020AD4C
		public void AddDef(Def def)
		{
			this.defs.Add(def);
		}

		// Token: 0x06003E41 RID: 15937 RVA: 0x0020C95C File Offset: 0x0020AD5C
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

		// Token: 0x06003E42 RID: 15938 RVA: 0x0020C9CC File Offset: 0x0020ADCC
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

		// Token: 0x06003E43 RID: 15939 RVA: 0x0020CAD0 File Offset: 0x0020AED0
		public override string ToString()
		{
			return this.relFolder + "/" + this.fileName;
		}

		// Token: 0x06003E44 RID: 15940 RVA: 0x0020CAFC File Offset: 0x0020AEFC
		public string GetFullFolderPath(ModContentPack mod)
		{
			return Path.GetFullPath(Path.Combine(Path.Combine(mod.RootDir, "Defs/"), this.relFolder));
		}

		// Token: 0x06003E45 RID: 15941 RVA: 0x0020CB34 File Offset: 0x0020AF34
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

		// Token: 0x0400275B RID: 10075
		public string fileName = "NamelessPackage";

		// Token: 0x0400275C RID: 10076
		public string relFolder = "";

		// Token: 0x0400275D RID: 10077
		public List<Def> defs = new List<Def>();
	}
}
