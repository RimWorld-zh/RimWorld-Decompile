using System;
using System.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E36 RID: 3638
	public class Dialog_PackageSelector : Window
	{
		// Token: 0x040038DF RID: 14559
		private Action<DefPackage> setPackageCallback;

		// Token: 0x040038E0 RID: 14560
		private ModContentPack mod;

		// Token: 0x040038E1 RID: 14561
		private string relFolder;

		// Token: 0x0600561D RID: 22045 RVA: 0x002C6456 File Offset: 0x002C4856
		public Dialog_PackageSelector(Action<DefPackage> setPackageCallback, ModContentPack mod, string relFolder)
		{
			this.setPackageCallback = setPackageCallback;
			this.mod = mod;
			this.relFolder = relFolder;
			this.doCloseX = true;
			this.onlyOneOfTypeAllowed = true;
			this.draggable = true;
		}

		// Token: 0x17000D74 RID: 3444
		// (get) Token: 0x0600561E RID: 22046 RVA: 0x002C648C File Offset: 0x002C488C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1000f, 700f);
			}
		}

		// Token: 0x17000D75 RID: 3445
		// (get) Token: 0x0600561F RID: 22047 RVA: 0x002C64B0 File Offset: 0x002C48B0
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005620 RID: 22048 RVA: 0x002C64C8 File Offset: 0x002C48C8
		public override void DoWindowContents(Rect inRect)
		{
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(inRect.AtZero());
			listing_Standard.ColumnWidth = 240f;
			foreach (DefPackage defPackage in this.mod.GetDefPackagesInFolder(this.relFolder))
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(defPackage.fileName);
				if (listing_Standard.ButtonText(fileNameWithoutExtension, null))
				{
					this.setPackageCallback(defPackage);
					this.Close(true);
				}
			}
			listing_Standard.End();
		}
	}
}
