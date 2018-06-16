using System;
using System.IO;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000BE0 RID: 3040
	public class Root_Play : Root
	{
		// Token: 0x0600424E RID: 16974 RVA: 0x0022DE68 File Offset: 0x0022C268
		public override void Start()
		{
			Log.ResetMessageCount();
			base.Start();
			try
			{
				this.musicManagerPlay = new MusicManagerPlay();
				FileInfo autostart = (!Root.checkedAutostartSaveFile) ? SaveGameFilesUtility.GetAutostartSaveFile() : null;
				Root.checkedAutostartSaveFile = true;
				if (autostart != null)
				{
					Action action = delegate()
					{
						SavedGameLoaderNow.LoadGameFromSaveFileNow(Path.GetFileNameWithoutExtension(autostart.Name));
					};
					string textKey = "LoadingLongEvent";
					bool doAsynchronously = true;
					if (Root_Play.<>f__mg$cache0 == null)
					{
						Root_Play.<>f__mg$cache0 = new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileLoadingGame);
					}
					LongEventHandler.QueueLongEvent(action, textKey, doAsynchronously, Root_Play.<>f__mg$cache0);
				}
				else if (Find.GameInitData != null && !Find.GameInitData.gameToLoad.NullOrEmpty())
				{
					Action action2 = delegate()
					{
						SavedGameLoaderNow.LoadGameFromSaveFileNow(Find.GameInitData.gameToLoad);
					};
					string textKey2 = "LoadingLongEvent";
					bool doAsynchronously2 = true;
					if (Root_Play.<>f__mg$cache1 == null)
					{
						Root_Play.<>f__mg$cache1 = new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileLoadingGame);
					}
					LongEventHandler.QueueLongEvent(action2, textKey2, doAsynchronously2, Root_Play.<>f__mg$cache1);
				}
				else
				{
					Action action3 = delegate()
					{
						if (Current.Game == null)
						{
							Root_Play.SetupForQuickTestPlay();
						}
						Current.Game.InitNewGame();
					};
					string textKey3 = "GeneratingMap";
					bool doAsynchronously3 = true;
					if (Root_Play.<>f__mg$cache2 == null)
					{
						Root_Play.<>f__mg$cache2 = new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap);
					}
					LongEventHandler.QueueLongEvent(action3, textKey3, doAsynchronously3, Root_Play.<>f__mg$cache2);
				}
				LongEventHandler.QueueLongEvent(delegate()
				{
					ScreenFader.SetColor(Color.black);
					ScreenFader.StartFade(Color.clear, 0.5f);
				}, null, false, null);
			}
			catch (Exception arg)
			{
				Log.Error("Critical error in root Start(): " + arg, false);
			}
		}

		// Token: 0x0600424F RID: 16975 RVA: 0x0022E00C File Offset: 0x0022C40C
		public override void Update()
		{
			base.Update();
			if (!LongEventHandler.ShouldWaitForEvent && !this.destroyed)
			{
				try
				{
					Profiler.BeginSample("ShipCountdownUpdate()");
					ShipCountdown.ShipCountdownUpdate();
					Profiler.EndSample();
					Profiler.BeginSample("TargetHighlighterUpdate()");
					TargetHighlighter.TargetHighlighterUpdate();
					Profiler.EndSample();
					Profiler.BeginSample("Game.Update()");
					Current.Game.UpdatePlay();
					Profiler.EndSample();
					Profiler.BeginSample("MusicUpdate()");
					this.musicManagerPlay.MusicUpdate();
					Profiler.EndSample();
				}
				catch (Exception arg)
				{
					Log.Error("Root level exception in Update(): " + arg, false);
				}
			}
		}

		// Token: 0x06004250 RID: 16976 RVA: 0x0022E0C8 File Offset: 0x0022C4C8
		private static void SetupForQuickTestPlay()
		{
			Current.ProgramState = ProgramState.Entry;
			Current.Game = new Game();
			Current.Game.InitData = new GameInitData();
			Current.Game.Scenario = ScenarioDefOf.Crashlanded.scenario;
			Find.Scenario.PreConfigure();
			Current.Game.storyteller = new Storyteller(StorytellerDefOf.Cassandra, DifficultyDefOf.Hard);
			Current.Game.World = WorldGenerator.GenerateWorld(0.05f, GenText.RandomSeedString(), OverallRainfall.Normal, OverallTemperature.Normal);
			Find.GameInitData.ChooseRandomStartingTile();
			Find.GameInitData.mapSize = 150;
			Find.GameInitData.PrepForMapGen();
			Find.Scenario.PreMapGenerate();
		}

		// Token: 0x04002D53 RID: 11603
		public MusicManagerPlay musicManagerPlay;

		// Token: 0x04002D54 RID: 11604
		[CompilerGenerated]
		private static Action<Exception> <>f__mg$cache0;

		// Token: 0x04002D55 RID: 11605
		[CompilerGenerated]
		private static Action<Exception> <>f__mg$cache1;

		// Token: 0x04002D56 RID: 11606
		[CompilerGenerated]
		private static Action<Exception> <>f__mg$cache2;
	}
}
