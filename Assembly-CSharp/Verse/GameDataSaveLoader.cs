using System;
using System.IO;
using RimWorld;
using Verse.Profile;

namespace Verse
{
	// Token: 0x02000D8D RID: 3469
	public static class GameDataSaveLoader
	{
		// Token: 0x040033CF RID: 13263
		private static int lastSaveTick = -9999;

		// Token: 0x040033D0 RID: 13264
		public const string SavedScenarioParentNodeName = "savedscenario";

		// Token: 0x040033D1 RID: 13265
		public const string SavedWorldParentNodeName = "savedworld";

		// Token: 0x040033D2 RID: 13266
		public const string SavedGameParentNodeName = "savegame";

		// Token: 0x040033D3 RID: 13267
		public const string GameNodeName = "game";

		// Token: 0x040033D4 RID: 13268
		public const string WorldNodeName = "world";

		// Token: 0x040033D5 RID: 13269
		public const string ScenarioNodeName = "scenario";

		// Token: 0x040033D6 RID: 13270
		public const string AutosavePrefix = "Autosave";

		// Token: 0x040033D7 RID: 13271
		public const string AutostartSaveName = "autostart";

		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x06004D98 RID: 19864 RVA: 0x002886EC File Offset: 0x00286AEC
		public static bool CurrentGameStateIsValuable
		{
			get
			{
				return Find.TickManager.TicksGame > GameDataSaveLoader.lastSaveTick + 60;
			}
		}

		// Token: 0x06004D99 RID: 19865 RVA: 0x00288718 File Offset: 0x00286B18
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

		// Token: 0x06004D9A RID: 19866 RVA: 0x00288790 File Offset: 0x00286B90
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

		// Token: 0x06004D9B RID: 19867 RVA: 0x00288858 File Offset: 0x00286C58
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

		// Token: 0x06004D9C RID: 19868 RVA: 0x002888D4 File Offset: 0x00286CD4
		public static void CheckVersionAndLoadGame(string saveFileName)
		{
			PreLoadUtility.CheckVersionAndLoad(GenFilePaths.FilePathForSavedGame(saveFileName), ScribeMetaHeaderUtility.ScribeHeaderMode.Map, delegate
			{
				GameDataSaveLoader.LoadGame(saveFileName);
			});
		}

		// Token: 0x06004D9D RID: 19869 RVA: 0x0028890C File Offset: 0x00286D0C
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

		// Token: 0x06004D9E RID: 19870 RVA: 0x00288946 File Offset: 0x00286D46
		public static void LoadGame(FileInfo saveFile)
		{
			GameDataSaveLoader.LoadGame(Path.GetFileNameWithoutExtension(saveFile.Name));
		}
	}
}
