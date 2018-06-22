using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009CF RID: 2511
	public static class VersionUpdateDialogMaker
	{
		// Token: 0x0600385A RID: 14426 RVA: 0x001E07B4 File Offset: 0x001DEBB4
		public static void CreateVersionUpdateDialogIfNecessary()
		{
			if (!VersionUpdateDialogMaker.dialogDone && LastPlayedVersion.Version != null && (VersionControl.CurrentMajor != LastPlayedVersion.Version.Major || VersionControl.CurrentMinor != LastPlayedVersion.Version.Minor))
			{
				VersionUpdateDialogMaker.CreateNewVersionDialog();
			}
		}

		// Token: 0x0600385B RID: 14427 RVA: 0x001E080C File Offset: 0x001DEC0C
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

		// Token: 0x04002406 RID: 9222
		private static bool dialogDone = false;
	}
}
