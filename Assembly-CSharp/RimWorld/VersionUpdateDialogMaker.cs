using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D3 RID: 2515
	public static class VersionUpdateDialogMaker
	{
		// Token: 0x0600385E RID: 14430 RVA: 0x001E0508 File Offset: 0x001DE908
		public static void CreateVersionUpdateDialogIfNecessary()
		{
			if (!VersionUpdateDialogMaker.dialogDone && LastPlayedVersion.Version != null && (VersionControl.CurrentMajor != LastPlayedVersion.Version.Major || VersionControl.CurrentMinor != LastPlayedVersion.Version.Minor))
			{
				VersionUpdateDialogMaker.CreateNewVersionDialog();
			}
		}

		// Token: 0x0600385F RID: 14431 RVA: 0x001E0560 File Offset: 0x001DE960
		private static void CreateNewVersionDialog()
		{
			string text = LastPlayedVersion.Version.Major + "." + LastPlayedVersion.Version.Minor;
			string text2 = VersionControl.CurrentMajor + "." + VersionControl.CurrentMinor;
			string text3 = "GameUpdatedToNewVersionInitial".Translate(new object[]
			{
				text,
				text2
			});
			text3 += "\n\n";
			if (BackCompatibility.IsSaveCompatibleWith(LastPlayedVersion.Version.ToString()))
			{
				text3 += "GameUpdatedToNewVersionSavesCompatible".Translate();
			}
			else
			{
				text3 += "GameUpdatedToNewVersionSavesIncompatible".Translate();
			}
			text3 += "\n\n";
			text3 += "GameUpdatedToNewVersionSteam".Translate();
			Find.WindowStack.Add(new Dialog_MessageBox(text3, null, null, null, null, null, false, null, null));
			VersionUpdateDialogMaker.dialogDone = true;
		}

		// Token: 0x0400240B RID: 9227
		private static bool dialogDone = false;
	}
}
