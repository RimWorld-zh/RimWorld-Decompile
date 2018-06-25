using System;
using System.IO;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse.Profile;

namespace Verse
{
	public static class GameDataSaveLoader
	{
		private static int lastSaveTick = -9999;

		public const string SavedScenarioParentNodeName = "savedscenario";

		public const string SavedWorldParentNodeName = "savedworld";

		public const string SavedGameParentNodeName = "savegame";

		public const string GameNodeName = "game";

		public const string WorldNodeName = "world";

		public const string ScenarioNodeName = "scenario";

		public const string AutosavePrefix = "Autosave";

		public const string AutostartSaveName = "autostart";

		[CompilerGenerated]
		private static Action <>f__am$cache0;

		public static bool CurrentGameStateIsValuable
		{
			get
			{
				return Find.TickManager.TicksGame > GameDataSaveLoader.lastSaveTick + 60;
			}
		}

		public static void SaveScenario(Scenario scen, string absFilePath)
		{
			try
			{
				scen.fileName = Path.GetFileNameWithoutExtension(absFilePath);
				SafeSaver.Save(absFilePath, "savedscenario", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Scribe_Deep.Look<Scenario>(ref scen, "scenario", new object[0]);
				});
			}
			catch (Exception ex)
			{
				Log.Error("Exception while saving world: " + ex.ToString(), false);
			}
		}

		public static bool TryLoadScenario(string absPath, ScenarioCategory category, out Scenario scen)
		{
			scen = null;
			try
			{
				Scribe.loader.InitLoading(absPath);
				try
				{
					ScribeMetaHeaderUtility.LoadGameDataHeader(ScribeMetaHeaderUtility.ScribeHeaderMode.Scenario, true);
					Scribe_Deep.Look<Scenario>(ref scen, "scenario", new object[0]);
					Scribe.loader.FinalizeLoading();
				}
				catch
				{
					Scribe.ForceStop();
					throw;
				}
				scen.fileName = Path.GetFileNameWithoutExtension(new FileInfo(absPath).Name);
				scen.Category = category;
			}
			catch (Exception ex)
			{
				Log.Error("Exception loading scenario: " + ex.ToString(), false);
				scen = null;
				Scribe.ForceStop();
			}
			return scen != null;
		}

		public static void SaveGame(string fileName)
		{
			try
			{
				string path = GenFilePaths.FilePathForSavedGame(fileName);
				SafeSaver.Save(path, "savegame", delegate
				{
					ScribeMetaHeaderUtility.WriteMetaHeader();
					Game game = Current.Game;
					Scribe_Deep.Look<Game>(ref game, "game", new object[0]);
				});
				GameDataSaveLoader.lastSaveTick = Find.TickManager.TicksGame;
			}
			catch (Exception arg)
			{
				Log.Error("Exception while saving game: " + arg, false);
			}
		}

		public static void CheckVersionAndLoadGame(string saveFileName)
		{
			PreLoadUtility.CheckVersionAndLoad(GenFilePaths.FilePathForSavedGame(saveFileName), ScribeMetaHeaderUtility.ScribeHeaderMode.Map, delegate
			{
				GameDataSaveLoader.LoadGame(saveFileName);
			});
		}

		public static void LoadGame(string saveFileName)
		{
			Action preLoadLevelAction = delegate()
			{
				MemoryUtility.ClearAllMapsAndWorld();
				Current.Game = new Game();
				Current.Game.InitData = new GameInitData();
				Current.Game.InitData.gameToLoad = saveFileName;
			};
			LongEventHandler.QueueLongEvent(preLoadLevelAction, "Play", "LoadingLongEvent", true, null);
		}

		public static void LoadGame(FileInfo saveFile)
		{
			GameDataSaveLoader.LoadGame(Path.GetFileNameWithoutExtension(saveFile.Name));
		}

		// Note: this type is marked as 'beforefieldinit'.
		static GameDataSaveLoader()
		{
		}

		[CompilerGenerated]
		private static void <SaveGame>m__0()
		{
			ScribeMetaHeaderUtility.WriteMetaHeader();
			Game game = Current.Game;
			Scribe_Deep.Look<Game>(ref game, "game", new object[0]);
		}

		[CompilerGenerated]
		private sealed class <SaveScenario>c__AnonStorey0
		{
			internal Scenario scen;

			public <SaveScenario>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				ScribeMetaHeaderUtility.WriteMetaHeader();
				Scribe_Deep.Look<Scenario>(ref this.scen, "scenario", new object[0]);
			}
		}

		[CompilerGenerated]
		private sealed class <CheckVersionAndLoadGame>c__AnonStorey1
		{
			internal string saveFileName;

			public <CheckVersionAndLoadGame>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				GameDataSaveLoader.LoadGame(this.saveFileName);
			}
		}

		[CompilerGenerated]
		private sealed class <LoadGame>c__AnonStorey2
		{
			internal string saveFileName;

			public <LoadGame>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				MemoryUtility.ClearAllMapsAndWorld();
				Current.Game = new Game();
				Current.Game.InitData = new GameInitData();
				Current.Game.InitData.gameToLoad = this.saveFileName;
			}
		}
	}
}
