using RimWorld;
using System;

namespace Verse
{
	public static class PermadeathModeUtility
	{
		public static string GeneratePermadeathSaveName()
		{
			string text = NameGenerator.GenerateName(RulePackDefOf.NamerFactionPlayerRandomized, (Predicate<string>)null, false);
			int num = 0;
			string text2;
			while (true)
			{
				num++;
				text2 = text;
				if (num != 1)
				{
					text2 += num;
				}
				text2 = PermadeathModeUtility.AppendPermadeathModeSuffix(text2);
				if (!SaveGameFilesUtility.SavedGameNamedExists(text2))
					break;
			}
			return text2;
		}

		public static void CheckUpdatePermadeathModeUniqueNameOnGameLoad(string filename)
		{
			if (Current.Game.Info.permadeathMode && Current.Game.Info.permadeathModeUniqueName != filename)
			{
				Log.Warning("Savefile's name has changed and doesn't match permadeath mode's unique name. Fixing...");
				Current.Game.Info.permadeathModeUniqueName = filename;
			}
		}

		private static string AppendPermadeathModeSuffix(string str)
		{
			return str + " " + "PermadeathModeSaveSuffix".Translate();
		}
	}
}
