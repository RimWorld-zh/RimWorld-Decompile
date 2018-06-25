using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	public sealed class TickManager : IExposable
	{
		private int ticksGameInt = 0;

		public int gameStartAbsTick = 0;

		private float realTimeToTickThrough = 0f;

		private TimeSpeed curTimeSpeed = TimeSpeed.Normal;

		public TimeSpeed prePauseTimeSpeed = TimeSpeed.Paused;

		private int startingYearInt = 5500;

		private Stopwatch clock = new Stopwatch();

		private TickList tickListNormal = new TickList(TickerType.Normal);

		private TickList tickListRare = new TickList(TickerType.Rare);

		private TickList tickListLong = new TickList(TickerType.Long);

		public TimeSlower slower = new TimeSlower();

		private int lastAutoScreenshot = 0;

		private float WorstAllowedFPS = 22f;

		private int lastNothingHappeningCheckTick = -1;

		private bool nothingHappeningCached = false;

		public TickManager()
		{
		}

		public int TicksGame
		{
			get
			{
				return this.ticksGameInt;
			}
		}

		public int TicksAbs
		{
			get
			{
				int result;
				if (this.gameStartAbsTick == 0)
				{
					Log.ErrorOnce("Accessing TicksAbs but gameStartAbsTick is not set yet (you most likely want to use GenTicks.TicksAbs instead).", 1049580013, false);
					result = this.ticksGameInt;
				}
				else
				{
					result = this.ticksGameInt + this.gameStartAbsTick;
				}
				return result;
			}
		}

		public int StartingYear
		{
			get
			{
				return this.startingYearInt;
			}
		}

		public float TickRateMultiplier
		{
			get
			{
				float result;
				if (this.slower.ForcedNormalSpeed)
				{
					if (this.curTimeSpeed == TimeSpeed.Paused)
					{
						result = 0f;
					}
					else
					{
						result = 1f;
					}
				}
				else
				{
					switch (this.curTimeSpeed)
					{
					case TimeSpeed.Paused:
						result = 0f;
						break;
					case TimeSpeed.Normal:
						result = 1f;
						break;
					case TimeSpeed.Fast:
						result = 3f;
						break;
					case TimeSpeed.Superfast:
						if (Find.Maps.Count == 0)
						{
							result = 120f;
						}
						else if (this.NothingHappeningInGame())
						{
							result = 12f;
						}
						else
						{
							result = 6f;
						}
						break;
					case TimeSpeed.Ultrafast:
						if (Find.Maps.Count == 0)
						{
							result = 150f;
						}
						else
						{
							result = 15f;
						}
						break;
					default:
						result = -1f;
						break;
					}
				}
				return result;
			}
		}

		private float CurTimePerTick
		{
			get
			{
				float result;
				if (this.TickRateMultiplier == 0f)
				{
					result = 0f;
				}
				else
				{
					result = 1f / (60f * this.TickRateMultiplier);
				}
				return result;
			}
		}

		public bool Paused
		{
			get
			{
				return this.curTimeSpeed == TimeSpeed.Paused || Find.WindowStack.WindowsForcePause || LongEventHandler.ForcePause;
			}
		}

		public bool NotPlaying
		{
			get
			{
				return Find.MainTabsRoot.OpenTab == MainButtonDefOf.Menu;
			}
		}

		public TimeSpeed CurTimeSpeed
		{
			get
			{
				return this.curTimeSpeed;
			}
			set
			{
				this.curTimeSpeed = value;
			}
		}

		public void TogglePaused()
		{
			if (this.curTimeSpeed != TimeSpeed.Paused)
			{
				this.prePauseTimeSpeed = this.curTimeSpeed;
				this.curTimeSpeed = TimeSpeed.Paused;
			}
			else if (this.prePauseTimeSpeed != this.curTimeSpeed)
			{
				this.curTimeSpeed = this.prePauseTimeSpeed;
			}
			else
			{
				this.curTimeSpeed = TimeSpeed.Normal;
			}
		}

		private bool NothingHappeningInGame()
		{
			if (this.lastNothingHappeningCheckTick != this.TicksGame)
			{
				this.nothingHappeningCached = true;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Pawn> list = maps[i].mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
					for (int j = 0; j < list.Count; j++)
					{
						Pawn pawn = list[j];
						if (pawn.HostFaction == null && pawn.RaceProps.Humanlike && pawn.Awake())
						{
							this.nothingHappeningCached = false;
							break;
						}
					}
					if (!this.nothingHappeningCached)
					{
						break;
					}
				}
				if (this.nothingHappeningCached)
				{
					for (int k = 0; k < maps.Count; k++)
					{
						if (maps[k].IsPlayerHome && maps[k].dangerWatcher.DangerRating >= StoryDanger.Low)
						{
							this.nothingHappeningCached = false;
							break;
						}
					}
				}
				this.lastNothingHappeningCheckTick = this.TicksGame;
			}
			return this.nothingHappeningCached;
		}

		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksGameInt, "ticksGame", 0, false);
			Scribe_Values.Look<int>(ref this.gameStartAbsTick, "gameStartAbsTick", 0, false);
			Scribe_Values.Look<int>(ref this.startingYearInt, "startingYear", 0, false);
		}

		public void RegisterAllTickabilityFor(Thing t)
		{
			TickList tickList = this.TickListFor(t);
			if (tickList != null)
			{
				tickList.RegisterThing(t);
			}
		}

		public void DeRegisterAllTickabilityFor(Thing t)
		{
			TickList tickList = this.TickListFor(t);
			if (tickList != null)
			{
				tickList.DeregisterThing(t);
			}
		}

		private TickList TickListFor(Thing t)
		{
			TickList result;
			switch (t.def.tickerType)
			{
			case TickerType.Never:
				result = null;
				break;
			case TickerType.Normal:
				result = this.tickListNormal;
				break;
			case TickerType.Rare:
				result = this.tickListRare;
				break;
			case TickerType.Long:
				result = this.tickListLong;
				break;
			default:
				throw new InvalidOperationException();
			}
			return result;
		}

		public void TickManagerUpdate()
		{
			if (!this.Paused)
			{
				float curTimePerTick = this.CurTimePerTick;
				if (Mathf.Abs(Time.deltaTime - curTimePerTick) < curTimePerTick * 0.1f)
				{
					this.realTimeToTickThrough += curTimePerTick;
				}
				else
				{
					this.realTimeToTickThrough += Time.deltaTime;
				}
				int num = 0;
				float tickRateMultiplier = this.TickRateMultiplier;
				this.clock.Reset();
				this.clock.Start();
				while (this.realTimeToTickThrough > 0f && (float)num < tickRateMultiplier * 2f)
				{
					this.DoSingleTick();
					this.realTimeToTickThrough -= curTimePerTick;
					num++;
					if (this.Paused || (float)this.clock.ElapsedMilliseconds > 1000f / this.WorstAllowedFPS)
					{
						break;
					}
				}
				if (this.realTimeToTickThrough > 0f)
				{
					this.realTimeToTickThrough = 0f;
				}
			}
		}

		public void DoSingleTick()
		{
			List<Map> maps = Find.Maps;
			Profiler.BeginSample("MapPreTick()");
			for (int i = 0; i < maps.Count; i++)
			{
				Profiler.BeginSample("Map " + i);
				maps[i].MapPreTick();
				Profiler.EndSample();
			}
			Profiler.EndSample();
			if (!DebugSettings.fastEcology)
			{
				this.ticksGameInt++;
			}
			else
			{
				this.ticksGameInt += 2000;
			}
			Shader.SetGlobalFloat(ShaderPropertyIDs.GameSeconds, this.TicksGame.TicksToSeconds());
			Profiler.BeginSample("tickListNormal");
			this.tickListNormal.Tick();
			Profiler.EndSample();
			Profiler.BeginSample("tickListRare");
			this.tickListRare.Tick();
			Profiler.EndSample();
			Profiler.BeginSample("tickListLong");
			this.tickListLong.Tick();
			Profiler.EndSample();
			Profiler.BeginSample("DateNotifierTick()");
			try
			{
				Find.DateNotifier.DateNotifierTick();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("Scenario.TickScenario()");
			try
			{
				Find.Scenario.TickScenario();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("WorldTick");
			try
			{
				Find.World.WorldTick();
			}
			catch (Exception ex3)
			{
				Log.Error(ex3.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("StoryWatcherTick");
			try
			{
				Find.StoryWatcher.StoryWatcherTick();
			}
			catch (Exception ex4)
			{
				Log.Error(ex4.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("GameEnder.GameEndTick()");
			try
			{
				Find.GameEnder.GameEndTick();
			}
			catch (Exception ex5)
			{
				Log.Error(ex5.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("Storyteller.StorytellerTick()");
			try
			{
				Find.Storyteller.StorytellerTick();
			}
			catch (Exception ex6)
			{
				Log.Error(ex6.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("taleManager.TaleManagerTick()");
			try
			{
				Current.Game.taleManager.TaleManagerTick();
			}
			catch (Exception ex7)
			{
				Log.Error(ex7.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("WorldPostTick");
			try
			{
				Find.World.WorldPostTick();
			}
			catch (Exception ex8)
			{
				Log.Error(ex8.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("MapPostTick()");
			for (int j = 0; j < maps.Count; j++)
			{
				Profiler.BeginSample("Map " + j);
				maps[j].MapPostTick();
				Profiler.EndSample();
			}
			Profiler.EndSample();
			Profiler.BeginSample("History.HistoryTick()");
			try
			{
				Find.History.HistoryTick();
			}
			catch (Exception ex9)
			{
				Log.Error(ex9.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("GameComponentTick()");
			GameComponentUtility.GameComponentTick();
			Profiler.EndSample();
			Profiler.BeginSample("LetterStack.LetterStackTick()");
			try
			{
				Find.LetterStack.LetterStackTick();
			}
			catch (Exception ex10)
			{
				Log.Error(ex10.ToString(), false);
			}
			Profiler.EndSample();
			Profiler.BeginSample("Autosaver.AutosaverTick()");
			try
			{
				Find.Autosaver.AutosaverTick();
			}
			catch (Exception ex11)
			{
				Log.Error(ex11.ToString(), false);
			}
			Profiler.EndSample();
			if (DebugViewSettings.logHourlyScreenshot && Find.TickManager.TicksGame >= this.lastAutoScreenshot + 2500)
			{
				ScreenshotTaker.QueueSilentScreenshot();
				this.lastAutoScreenshot = Find.TickManager.TicksGame / 2500 * 2500;
			}
			Profiler.BeginSample("FilthMonitor.FilthMonitorTick()");
			try
			{
				FilthMonitor.FilthMonitorTick();
			}
			catch (Exception ex12)
			{
				Log.Error(ex12.ToString(), false);
			}
			Profiler.EndSample();
			UnityEngine.Debug.developerConsoleVisible = false;
		}

		public void RemoveAllFromMap(Map map)
		{
			this.tickListNormal.RemoveWhere((Thing x) => x.Map == map);
			this.tickListRare.RemoveWhere((Thing x) => x.Map == map);
			this.tickListLong.RemoveWhere((Thing x) => x.Map == map);
		}

		public void DebugSetTicksGame(int newTicksGame)
		{
			this.ticksGameInt = newTicksGame;
		}

		[CompilerGenerated]
		private sealed class <RemoveAllFromMap>c__AnonStorey0
		{
			internal Map map;

			public <RemoveAllFromMap>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing x)
			{
				return x.Map == this.map;
			}

			internal bool <>m__1(Thing x)
			{
				return x.Map == this.map;
			}

			internal bool <>m__2(Thing x)
			{
				return x.Map == this.map;
			}
		}
	}
}
