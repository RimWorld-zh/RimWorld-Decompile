using System;
using System.IO;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000BDC RID: 3036
	public class Root_Play : Root
	{
		// Token: 0x06004252 RID: 16978 RVA: 0x0022E5FC File Offset: 0x0022C9FC
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

		// Token: 0x06004253 RID: 16979 RVA: 0x0022E7A0 File Offset: 0x0022CBA0
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

		// Token: 0x06004254 RID: 16980 RVA: 0x0022E85C File Offset: 0x0022CC5C
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

		// Token: 0x04002D58 RID: 11608
		public MusicManagerPlay musicManagerPlay;

		// Token: 0x04002D59 RID: 11609
		[CompilerGenerated]
		private static Action<Exception> <>f__mg$cache0;

		// Token: 0x04002D5A RID: 11610
		[CompilerGenerated]
		private static Action<Exception> <>f__mg$cache1;

		// Token: 0x04002D5B RID: 11611
		[CompilerGenerated]
		private static Action<Exception> <>f__mg$cache2;
	}
}
