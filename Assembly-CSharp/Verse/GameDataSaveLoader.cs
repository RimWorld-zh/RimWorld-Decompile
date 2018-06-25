using System;
using System.IO;
using RimWorld;
using Verse.Profile;

namespace Verse
{
	// Token: 0x02000D8C RID: 3468
	public static class GameDataSaveLoader
	{
		// Token: 0x040033C8 RID: 13256
		private static int lastSaveTick = -9999;

		// Token: 0x040033C9 RID: 13257
		public const string SavedScenarioParentNodeName = "savedscenario";

		// Token: 0x040033CA RID: 13258
		public const string SavedWorldParentNodeName = "savedworld";

		// Token: 0x040033CB RID: 13259
		public const string SavedGameParentNodeName = "savegame";

		// Token: 0x040033CC RID: 13260
		public const string GameNodeName = "game";

		// Token: 0x040033CD RID: 13261
		public const string WorldNodeName = "world";

		// Token: 0x040033CE RID: 13262
		public const string ScenarioNodeName = "scenario";

		// Token: 0x040033CF RID: 13263
		public const string AutosavePrefix = "Autosave";

		// Token: 0x040033D0 RID: 13264
		public const string AutostartSaveName = "autostart";

		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x06004D98 RID: 19864 RVA: 0x0028840C File Offset: 0x0028680C
		public static bool CurrentGameStateIsValuable
		{
			get
			{
				return Find.TickManager.TicksGame > GameDataSaveLoader.lastSaveTick + 60;
			}
		}

		// Token: 0x06004D99 RID: 19865 RVA: 0x00288438 File Offset: 0x00286838
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

		// Token: 0x06004D9A RID: 19866 RVA: 0x002884B0 File Offset: 0x002868B0
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

		// Token: 0x06004D9B RID: 19867 RVA: 0x00288578 File Offset: 0x00286978
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

		// Token: 0x06004D9C RID: 19868 RVA: 0x002885F4 File Offset: 0x002869F4
		public static void CheckVersionAndLoadGame(string saveFileName)
		{
			PreLoadUtility.CheckVersionAndLoad(GenFilePaths.FilePathForSavedGame(saveFileName), ScribeMetaHeaderUtility.ScribeHeaderMode.Map, delegate
			{
				GameDataSaveLoader.LoadGame(saveFileName);
			});
		}

		// Token: 0x06004D9D RID: 19869 RVA: 0x0028862C File Offset: 0x00286A2C
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

		// Token: 0x06004D9E RID: 19870 RVA: 0x00288666 File Offset: 0x00286A66
		public static void LoadGame(FileInfo saveFile)
		{
			GameDataSaveLoader.LoadGame(Path.GetFileNameWithoutExtension(saveFile.Name));
		}
	}
}
