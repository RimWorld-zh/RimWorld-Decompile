using System;
using System.IO;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	public class Root_Play : Root
	{
		public MusicManagerPlay musicManagerPlay;

		[CompilerGenerated]
		private static Action<Exception> <>f__mg$cache0;

		[CompilerGenerated]
		private static Action<Exception> <>f__mg$cache1;

		[CompilerGenerated]
		private static Action<Exception> <>f__mg$cache2;

		[CompilerGenerated]
		private static Action <>f__am$cache0;

		[CompilerGenerated]
		private static Action <>f__am$cache1;

		[CompilerGenerated]
		private static Action <>f__am$cache2;

		public Root_Play()
		{
		}

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

		private static void SetupForQuickTestPlay()
		{
			Current.ProgramState = ProgramState.Entry;
			Current.Game = new Game();
			Current.Game.InitData = new GameInitData();
			Current.Game.Scenario = ScenarioDefOf.Crashlanded.scenario;
			Find.Scenario.PreConfigure();
			Current.Game.storyteller = new Storyteller(StorytellerDefOf.Cassandra, DifficultyDefOf.ExtraHard);
			Current.Game.World = WorldGenerator.GenerateWorld(0.05f, GenText.RandomSeedString(), OverallRainfall.Normal, OverallTemperature.Normal);
			Find.GameInitData.ChooseRandomStartingTile();
			Find.GameInitData.mapSize = 150;
			Find.GameInitData.PrepForMapGen();
			Find.Scenario.PreMapGenerate();
		}

		[CompilerGenerated]
		private static void <Start>m__0()
		{
			SavedGameLoaderNow.LoadGameFromSaveFileNow(Find.GameInitData.gameToLoad);
		}

		[CompilerGenerated]
		private static void <Start>m__1()
		{
			if (Current.Game == null)
			{
				Root_Play.SetupForQuickTestPlay();
			}
			Current.Game.InitNewGame();
		}

		[CompilerGenerated]
		private static void <Start>m__2()
		{
			ScreenFader.SetColor(Color.black);
			ScreenFader.StartFade(Color.clear, 0.5f);
		}

		[CompilerGenerated]
		private sealed class <Start>c__AnonStorey0
		{
			internal FileInfo autostart;

			public <Start>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				SavedGameLoaderNow.LoadGameFromSaveFileNow(Path.GetFileNameWithoutExtension(this.autostart.Name));
			}
		}
	}
}
