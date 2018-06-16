using System;
using System.IO;
using RimWorld;
using Verse.Profile;

namespace Verse
{
	// Token: 0x02000D8E RID: 3470
	public static class GameDataSaveLoader
	{
		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x06004D81 RID: 19841 RVA: 0x00286D50 File Offset: 0x00285150
		public static bool CurrentGameStateIsValuable
		{
			get
			{
				return Find.TickManager.TicksGame > GameDataSaveLoader.lastSaveTick + 60;
			}
		}

		// Token: 0x06004D82 RID: 19842 RVA: 0x00286D7C File Offset: 0x0028517C
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

		// Token: 0x06004D83 RID: 19843 RVA: 0x00286DF4 File Offset: 0x002851F4
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

		// Token: 0x06004D84 RID: 19844 RVA: 0x00286EBC File Offset: 0x002852BC
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

		// Token: 0x06004D85 RID: 19845 RVA: 0x00286F38 File Offset: 0x00285338
		public static void CheckVersionAndLoadGame(string saveFileName)
		{
			PreLoadUtility.CheckVersionAndLoad(GenFilePaths.FilePathForSavedGame(saveFileName), ScribeMetaHeaderUtility.ScribeHeaderMode.Map, delegate
			{
				GameDataSaveLoader.LoadGame(saveFileName);
			});
		}

		// Token: 0x06004D86 RID: 19846 RVA: 0x00286F70 File Offset: 0x00285370
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

		// Token: 0x06004D87 RID: 19847 RVA: 0x00286FAA File Offset: 0x002853AA
		public static void LoadGame(FileInfo saveFile)
		{
			GameDataSaveLoader.LoadGame(Path.GetFileNameWithoutExtension(saveFile.Name));
		}

		// Token: 0x040033BF RID: 13247
		private static int lastSaveTick = -9999;

		// Token: 0x040033C0 RID: 13248
		public const string SavedScenarioParentNodeName = "savedscenario";

		// Token: 0x040033C1 RID: 13249
		public const string SavedWorldParentNodeName = "savedworld";

		// Token: 0x040033C2 RID: 13250
		public const string SavedGameParentNodeName = "savegame";

		// Token: 0x040033C3 RID: 13251
		public const string GameNodeName = "game";

		// Token: 0x040033C4 RID: 13252
		public const string WorldNodeName = "world";

		// Token: 0x040033C5 RID: 13253
		public const string ScenarioNodeName = "scenario";

		// Token: 0x040033C6 RID: 13254
		public const string AutosavePrefix = "Autosave";

		// Token: 0x040033C7 RID: 13255
		public const string AutostartSaveName = "autostart";
	}
}
