using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000FA3 RID: 4003
	public static class PermadeathModeUtility
	{
		// Token: 0x06006092 RID: 24722 RVA: 0x0030D98C File Offset: 0x0030BD8C
		public static string GeneratePermadeathSaveName()
		{
			string text = NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, null, false, null);
			text = GenFile.SanitizedFileName(text);
			return PermadeathModeUtility.NewPermadeathSaveNameWithAppendedNumberIfNecessary(text, null);
		}

		// Token: 0x06006093 RID: 24723 RVA: 0x0030D9C8 File Offset: 0x0030BDC8
		public static string GeneratePermadeathSaveNameBasedOnPlayerInput(string factionName, string acceptedNameEvenIfTaken = null)
		{
			string name = GenFile.SanitizedFileName(factionName);
			return PermadeathModeUtility.NewPermadeathSaveNameWithAppendedNumberIfNecessary(name, acceptedNameEvenIfTaken);
		}

		// Token: 0x06006094 RID: 24724 RVA: 0x0030D9EC File Offset: 0x0030BDEC
		public static void CheckUpdatePermadeathModeUniqueNameOnGameLoad(string filename)
		{
			if (Current.Game.Info.permadeathMode && Current.Game.Info.permadeathModeUniqueName != filename)
			{
				Log.Warning("Savefile's name has changed and doesn't match permadeath mode's unique name. Fixing...", false);
				Current.Game.Info.permadeathModeUniqueName = filename;
			}
		}

		// Token: 0x06006095 RID: 24725 RVA: 0x0030DA48 File Offset: 0x0030BE48
		private static string NewPermadeathSaveNameWithAppendedNumberIfNecessary(string name, string acceptedNameEvenIfTaken = null)
		{
			int num = 0;
			string text;
			do
			{
				num++;
				text = name;
				if (num != 1)
				{
					text += num;
				}
				text = PermadeathModeUtility.AppendedPermadeathModeSuffix(text);
			}
			while (SaveGameFilesUtility.SavedGameNamedExists(text) && text != acceptedNameEvenIfTaken);
			return text;
		}

		// Token: 0x06006096 RID: 24726 RVA: 0x0030DA9C File Offset: 0x0030BE9C
		private static string AppendedPermadeathModeSuffix(string str)
		{
			return str + " " + "PermadeathModeSaveSuffix".Translate();
		}
	}
}
