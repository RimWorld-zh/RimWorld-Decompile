using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000FA2 RID: 4002
	public static class PermadeathModeUtility
	{
		// Token: 0x06006090 RID: 24720 RVA: 0x0030DA68 File Offset: 0x0030BE68
		public static string GeneratePermadeathSaveName()
		{
			string text = NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, null, false, null, null);
			text = GenFile.SanitizedFileName(text);
			return PermadeathModeUtility.NewPermadeathSaveNameWithAppendedNumberIfNecessary(text, null);
		}

		// Token: 0x06006091 RID: 24721 RVA: 0x0030DAA4 File Offset: 0x0030BEA4
		public static string GeneratePermadeathSaveNameBasedOnPlayerInput(string factionName, string acceptedNameEvenIfTaken = null)
		{
			string name = GenFile.SanitizedFileName(factionName);
			return PermadeathModeUtility.NewPermadeathSaveNameWithAppendedNumberIfNecessary(name, acceptedNameEvenIfTaken);
		}

		// Token: 0x06006092 RID: 24722 RVA: 0x0030DAC8 File Offset: 0x0030BEC8
		public static void CheckUpdatePermadeathModeUniqueNameOnGameLoad(string filename)
		{
			if (Current.Game.Info.permadeathMode && Current.Game.Info.permadeathModeUniqueName != filename)
			{
				Log.Warning("Savefile's name has changed and doesn't match permadeath mode's unique name. Fixing...", false);
				Current.Game.Info.permadeathModeUniqueName = filename;
			}
		}

		// Token: 0x06006093 RID: 24723 RVA: 0x0030DB24 File Offset: 0x0030BF24
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

		// Token: 0x06006094 RID: 24724 RVA: 0x0030DB78 File Offset: 0x0030BF78
		private static string AppendedPermadeathModeSuffix(string str)
		{
			return str + " " + "PermadeathModeSaveSuffix".Translate();
		}
	}
}
