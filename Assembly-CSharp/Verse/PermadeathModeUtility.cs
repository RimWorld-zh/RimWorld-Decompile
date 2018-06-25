using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000FA6 RID: 4006
	public static class PermadeathModeUtility
	{
		// Token: 0x060060C3 RID: 24771 RVA: 0x0031018C File Offset: 0x0030E58C
		public static string GeneratePermadeathSaveName()
		{
			string text = NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, null, false, null, null);
			text = GenFile.SanitizedFileName(text);
			return PermadeathModeUtility.NewPermadeathSaveNameWithAppendedNumberIfNecessary(text, null);
		}

		// Token: 0x060060C4 RID: 24772 RVA: 0x003101C8 File Offset: 0x0030E5C8
		public static string GeneratePermadeathSaveNameBasedOnPlayerInput(string factionName, string acceptedNameEvenIfTaken = null)
		{
			string name = GenFile.SanitizedFileName(factionName);
			return PermadeathModeUtility.NewPermadeathSaveNameWithAppendedNumberIfNecessary(name, acceptedNameEvenIfTaken);
		}

		// Token: 0x060060C5 RID: 24773 RVA: 0x003101EC File Offset: 0x0030E5EC
		public static void CheckUpdatePermadeathModeUniqueNameOnGameLoad(string filename)
		{
			if (Current.Game.Info.permadeathMode && Current.Game.Info.permadeathModeUniqueName != filename)
			{
				Log.Warning("Savefile's name has changed and doesn't match permadeath mode's unique name. Fixing...", false);
				Current.Game.Info.permadeathModeUniqueName = filename;
			}
		}

		// Token: 0x060060C6 RID: 24774 RVA: 0x00310248 File Offset: 0x0030E648
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

		// Token: 0x060060C7 RID: 24775 RVA: 0x0031029C File Offset: 0x0030E69C
		private static string AppendedPermadeathModeSuffix(string str)
		{
			return str + " " + "PermadeathModeSaveSuffix".Translate();
		}
	}
}
