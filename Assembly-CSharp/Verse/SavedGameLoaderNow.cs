﻿using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Verse
{
	public class SavedGameLoaderNow
	{
		[CompilerGenerated]
		private static Func<ModContentPack, string> <>f__am$cache0;

		public SavedGameLoaderNow()
		{
		}

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
			try
			{
				ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.Map, true);
				if (!Scribe.EnterNode("game"))
				{
					Log.Error("Could not find game XML node.", false);
					Scribe.ForceStop();
					return;
				}
				Current.Game = new Game();
				Current.Game.LoadGame();
			}
			catch (Exception)
			{
				Scribe.ForceStop();
				throw;
			}
			PermadeathModeUtility.CheckUpdatePermadeathModeUniqueNameOnGameLoad(fileName);
			DeepProfiler.End();
		}

		[CompilerGenerated]
		private static string <LoadGameFromSaveFileNow>m__0(ModContentPack mod)
		{
			return mod.ToString();
		}
	}
}
