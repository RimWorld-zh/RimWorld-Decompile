using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000FA2 RID: 4002
	public static class PermadeathModeUtility
	{
		// Token: 0x060060B9 RID: 24761 RVA: 0x0030FB0C File Offset: 0x0030DF0C
		public static string GeneratePermadeathSaveName()
		{
			string text = NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, null, false, null, null);
			text = GenFile.SanitizedFileName(text);
			return PermadeathModeUtility.NewPermadeathSaveNameWithAppendedNumberIfNecessary(text, null);
		}

		// Token: 0x060060BA RID: 24762 RVA: 0x0030FB48 File Offset: 0x0030DF48
		public static string GeneratePermadeathSaveNameBasedOnPlayerInput(string factionName, string acceptedNameEvenIfTaken = null)
		{
			string name = GenFile.SanitizedFileName(factionName);
			return PermadeathModeUtility.NewPermadeathSaveNameWithAppendedNumberIfNecessary(name, acceptedNameEvenIfTaken);
		}

		// Token: 0x060060BB RID: 24763 RVA: 0x0030FB6C File Offset: 0x0030DF6C
		public static void CheckUpdatePermadeathModeUniqueNameOnGameLoad(string filename)
		{
			if (Current.Game.Info.permadeathMode && Current.Game.Info.permadeathModeUniqueName != filename)
			{
				Log.Warning("Savefile's name has changed and doesn't match permadeath mode's unique name. Fixing...", false);
				Current.Game.Info.permadeathModeUniqueName = filename;
			}
		}

		// Token: 0x060060BC RID: 24764 RVA: 0x0030FBC8 File Offset: 0x0030DFC8
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

		// Token: 0x060060BD RID: 24765 RVA: 0x0030FC1C File Offset: 0x0030E01C
		private static string AppendedPermadeathModeSuffix(string str)
		{
			return str + " " + "PermadeathModeSaveSuffix".Translate();
		}
	}
}
