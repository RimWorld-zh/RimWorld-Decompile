using System;
using System.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000E38 RID: 3640
	public class Dialog_PackageSelector : Window
	{
		// Token: 0x060055FF RID: 22015 RVA: 0x002C476E File Offset: 0x002C2B6E
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
		// (get) Token: 0x06005600 RID: 22016 RVA: 0x002C47A4 File Offset: 0x002C2BA4
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1000f, 700f);
			}
		}

		// Token: 0x17000D75 RID: 3445
		// (get) Token: 0x06005601 RID: 22017 RVA: 0x002C47C8 File Offset: 0x002C2BC8
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005602 RID: 22018 RVA: 0x002C47E0 File Offset: 0x002C2BE0
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

		// Token: 0x040038D3 RID: 14547
		private Action<DefPackage> setPackageCallback;

		// Token: 0x040038D4 RID: 14548
		private ModContentPack mod;

		// Token: 0x040038D5 RID: 14549
		private string relFolder;
	}
}
