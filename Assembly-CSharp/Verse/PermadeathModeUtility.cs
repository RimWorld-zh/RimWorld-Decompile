using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000FA7 RID: 4007
	public static class PermadeathModeUtility
	{
		// Token: 0x060060C3 RID: 24771 RVA: 0x003103D0 File Offset: 0x0030E7D0
		public static string GeneratePermadeathSaveName()
		{
			string text = NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, null, false, null, null);
			text = GenFile.SanitizedFileName(text);
			return PermadeathModeUtility.NewPermadeathSaveNameWithAppendedNumberIfNecessary(text, null);
		}

		// Token: 0x060060C4 RID: 24772 RVA: 0x0031040C File Offset: 0x0030E80C
		public static string GeneratePermadeathSaveNameBasedOnPlayerInput(string factionName, string acceptedNameEvenIfTaken = null)
		{
			string name = GenFile.SanitizedFileName(factionName);
			return PermadeathModeUtility.NewPermadeathSaveNameWithAppendedNumberIfNecessary(name, acceptedNameEvenIfTaken);
		}

		// Token: 0x060060C5 RID: 24773 RVA: 0x00310430 File Offset: 0x0030E830
		public static void CheckUpdatePermadeathModeUniqueNameOnGameLoad(string filename)
		{
			if (Current.Game.Info.permadeathMode && Current.Game.Info.permadeathModeUniqueName != filename)
			{
				Log.Warning("Savefile's name has changed and doesn't match permadeath mode's unique name. Fixing...", false);
				Current.Game.Info.permadeathModeUniqueName = filename;
			}
		}

		// Token: 0x060060C6 RID: 24774 RVA: 0x0031048C File Offset: 0x0030E88C
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

		// Token: 0x060060C7 RID: 24775 RVA: 0x003104E0 File Offset: 0x0030E8E0
		private static string AppendedPermadeathModeSuffix(string str)
		{
			return str + " " + "PermadeathModeSaveSuffix".Translate();
		}
	}
}
