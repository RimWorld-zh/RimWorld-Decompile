using System;
using System.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E37 RID: 3639
	public class Dialog_PackageSelector : Window
	{
		// Token: 0x040038E7 RID: 14567
		private Action<DefPackage> setPackageCallback;

		// Token: 0x040038E8 RID: 14568
		private ModContentPack mod;

		// Token: 0x040038E9 RID: 14569
		private string relFolder;

		// Token: 0x0600561D RID: 22045 RVA: 0x002C6642 File Offset: 0x002C4A42
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
		// (get) Token: 0x0600561E RID: 22046 RVA: 0x002C6678 File Offset: 0x002C4A78
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1000f, 700f);
			}
		}

		// Token: 0x17000D75 RID: 3445
		// (get) Token: 0x0600561F RID: 22047 RVA: 0x002C669C File Offset: 0x002C4A9C
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005620 RID: 22048 RVA: 0x002C66B4 File Offset: 0x002C4AB4
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
