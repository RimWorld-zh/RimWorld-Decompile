using System;
using Verse;

namespace RimWorld
{
	public static class VersionUpdateDialogMaker
	{
		private static bool dialogDone = false;

		public static void CreateVersionUpdateDialogIfNecessary()
		{
			if (!VersionUpdateDialogMaker.dialogDone && LastPlayedVersion.Version != (Version)null)
			{
				if (VersionControl.CurrentMajor == LastPlayedVersion.Version.Major && VersionControl.CurrentMinor == LastPlayedVersion.Version.Minor)
					return;
				VersionUpdateDialogMaker.CreateNewVersionDialog();
			}
		}

		private static void CreateNewVersionDialog()
		{
			string text = LastPlayedVersion.Version.Major + "." + LastPlayedVersion.Version.Minor;
			string text2 = VersionControl.CurrentMajor + "." + VersionControl.CurrentMinor;
			string str = "GameUpdatedToNewVersionInitial".Translate(text, text2);
			str += "\n\n";
			str = ((!BackCompatibility.IsSaveCompatibleWith(LastPlayedVersion.Version.ToString())) ? (str + "GameUpdatedToNewVersionSavesIncompatible".Translate()) : (str + "GameUpdatedToNewVersionSavesCompatible".Translate()));
			str += "\n\n";
			str += "GameUpdatedToNewVersionSteam".Translate();
			Find.WindowStack.Add(new Dialog_MessageBox(str, (string)null, null, (string)null, null, (string)null, false));
			VersionUpdateDialogMaker.dialogDone = true;
		}
	}
}
