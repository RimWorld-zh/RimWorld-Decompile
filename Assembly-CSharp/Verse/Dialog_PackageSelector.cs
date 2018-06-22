using System;
using System.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E34 RID: 3636
	public class Dialog_PackageSelector : Window
	{
		// Token: 0x06005619 RID: 22041 RVA: 0x002C632A File Offset: 0x002C472A
		public Dialog_PackageSelector(Action<DefPackage> setPackageCallback, ModContentPack mod, string relFolder)
		{
			this.setPackageCallback = setPackageCallback;
			this.mod = mod;
			this.relFolder = relFolder;
			this.doCloseX = true;
			this.onlyOneOfTypeAllowed = true;
			this.draggable = true;
		}

		// Token: 0x17000D75 RID: 3445
		// (get) Token: 0x0600561A RID: 22042 RVA: 0x002C6360 File Offset: 0x002C4760
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1000f, 700f);
			}
		}

		// Token: 0x17000D76 RID: 3446
		// (get) Token: 0x0600561B RID: 22043 RVA: 0x002C6384 File Offset: 0x002C4784
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600561C RID: 22044 RVA: 0x002C639C File Offset: 0x002C479C
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

		// Token: 0x040038DF RID: 14559
		private Action<DefPackage> setPackageCallback;

		// Token: 0x040038E0 RID: 14560
		private ModContentPack mod;

		// Token: 0x040038E1 RID: 14561
		private string relFolder;
	}
}
