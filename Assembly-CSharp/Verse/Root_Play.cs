using RimWorld;
using RimWorld.Planet;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	public class Root_Play : Root
	{
		public MusicManagerPlay musicManagerPlay;

		[CompilerGenerated]
		private static Action<Exception> _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static Action<Exception> _003C_003Ef__mg_0024cache1;

		public override void Start()
		{
			base.Start();
			this.musicManagerPlay = new MusicManagerPlay();
			if (Find.GameInitData != null && !Find.GameInitData.gameToLoad.NullOrEmpty())
			{
				LongEventHandler.QueueLongEvent(delegate
				{
					SavedGameLoader.LoadGameFromSaveFile(Find.GameInitData.gameToLoad);
				}, "LoadingLongEvent", true, GameAndMapInitExceptionHandlers.ErrorWhileLoadingGame);
			}
			else
			{
				LongEventHandler.QueueLongEvent(delegate
				{
					if (Current.Game == null)
					{
						Root_Play.SetupForQuickTestPlay();
					}
					Current.Game.InitNewGame();
				}, "GeneratingMap", true, GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap);
			}
			LongEventHandler.QueueLongEvent(delegate
			{
				ScreenFader.SetColor(Color.black);
				ScreenFader.StartFade(Color.clear, 0.5f);
			}, null, false, null);
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
