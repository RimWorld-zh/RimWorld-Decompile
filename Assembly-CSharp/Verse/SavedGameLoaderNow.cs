using System;
using System.Linq;

namespace Verse
{
	// Token: 0x02000C67 RID: 3175
	public class SavedGameLoaderNow
	{
		// Token: 0x060045BD RID: 17853 RVA: 0x0024C2B0 File Offset: 0x0024A6B0
		public static void LoadGameFromSaveFileNow(string fileName)
		{
			string str = (from mod in LoadedModManager.RunningMods
			select mod.ToString()).ToCommaList(false);
			Log.Message("Loading game from file " + fileName + " with mods " + str, false);
			DeepProfiler.Start("Loading game from file " + fileName);
			Current.Game = new Game();
			DeepProfiler.Start("InitLoading (read file)");
			Scribe.loader.InitLoading(GenFilePaths.FilePathForSavedGame(fileName));
			DeepProfiler.End();
			ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.Map, true);
			if (Scribe.EnterNode("game"))
			{
				Current.Game = new Game();
				Current.Game.LoadGame();
				PermadeathModeUtility.CheckUpdatePermadeathModeUniqueNameOnGameLoad(fileName);
				DeepProfiler.End();
			}
			else
			{
				Log.Error("Could not find game XML node.", false);
				Scribe.ForceStop();
			}
		}
	}
}
