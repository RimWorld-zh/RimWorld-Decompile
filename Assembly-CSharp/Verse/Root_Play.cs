using RimWorld;
using RimWorld.Planet;
using System;
using UnityEngine;

namespace Verse
{
	public class Root_Play : Root
	{
		public MusicManagerPlay musicManagerPlay;

		public override void Start()
		{
			base.Start();
			this.musicManagerPlay = new MusicManagerPlay();
			if (Find.GameInitData != null && !Find.GameInitData.gameToLoad.NullOrEmpty())
			{
				LongEventHandler.QueueLongEvent((Action)delegate
				{
					SavedGameLoader.LoadGameFromSaveFile(Find.GameInitData.gameToLoad);
				}, "LoadingLongEvent", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileLoadingGame));
			}
			else
			{
				LongEventHandler.QueueLongEvent((Action)delegate
				{
					if (Current.Game == null)
					{
						Root_Play.SetupForQuickTestPlay();
					}
					Current.Game.InitNewGame();
				}, "GeneratingMap", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap));
			}
			LongEventHandler.QueueLongEvent((Action)delegate
			{
				ScreenFader.SetColor(Color.black);
				ScreenFader.StartFade(Color.clear, 0.5f);
			}, (string)null, false, null);
		}

		public override void Update()
		{
			base.Update();
			if (!LongEventHandler.ShouldWaitForEvent && !base.destroyed)
			{
				try
				{
					ShipCountdown.ShipCountdownUpdate();
					Current.Game.UpdatePlay();
					this.musicManagerPlay.MusicUpdate();
				}
				catch (Exception e)
				{
					Log.Notify_Exception(e);
					throw;
					IL_0044:;
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
			Current.Game.storyteller = new Storyteller(StorytellerDefOf.Cassandra, DifficultyDefOf.Hard);
			Current.Game.World = WorldGenerator.GenerateWorld(0.05f, GenText.RandomSeedString(), OverallRainfall.Normal, OverallTemperature.Normal);
			Rand.RandomizeStateFromTime();
			Find.GameInitData.ChooseRandomStartingTile();
			Find.GameInitData.mapSize = 150;
			Find.GameInitData.PrepForMapGen();
			Find.Scenario.PreMapGenerate();
		}
	}
}
