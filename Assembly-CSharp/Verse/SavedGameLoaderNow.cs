using System;
using System.Linq;

namespace Verse
{
	// Token: 0x02000C66 RID: 3174
	public class SavedGameLoaderNow
	{
		// Token: 0x060045C7 RID: 17863 RVA: 0x0024DA14 File Offset: 0x0024BE14
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
