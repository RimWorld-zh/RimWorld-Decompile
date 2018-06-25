using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009D1 RID: 2513
	public static class VersionUpdateDialogMaker
	{
		// Token: 0x04002407 RID: 9223
		private static bool dialogDone = false;

		// Token: 0x0600385E RID: 14430 RVA: 0x001E08F8 File Offset: 0x001DECF8
		public static void CreateVersionUpdateDialogIfNecessary()
		{
			if (!VersionUpdateDialogMaker.dialogDone && LastPlayedVersion.Version != null && (VersionControl.CurrentMajor != LastPlayedVersion.Version.Major || VersionControl.CurrentMinor != LastPlayedVersion.Version.Minor))
			{
				VersionUpdateDialogMaker.CreateNewVersionDialog();
			}
		}

		// Token: 0x0600385F RID: 14431 RVA: 0x001E0950 File Offset: 0x001DED50
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
	}
}
