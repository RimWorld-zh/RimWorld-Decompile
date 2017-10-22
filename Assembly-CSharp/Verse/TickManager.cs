#define ENABLE_PROFILER
using RimWorld;
using System;
using System.Collections.Generic;
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

		private TickList tickListNormal = new TickList(TickerType.Normal);

		private TickList tickListRare = new TickList(TickerType.Rare);

		private TickList tickListLong = new TickList(TickerType.Long);

		public TimeSlower slower = new TimeSlower();

		private int lastNothingHappeningCheckTick = -1;

		private bool nothingHappeningCached = false;

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
					Log.ErrorOnce("Accessing TicksAbs but gameStartAbsTick is not set yet.", 1049580013);
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
					result = (float)((this.curTimeSpeed != 0) ? 1.0 : 0.0);
				}
				else
				{
					switch (this.curTimeSpeed)
					{
					case TimeSpeed.Paused:
					{
						result = 0f;
						break;
					}
					case TimeSpeed.Normal:
					{
						result = 1f;
						break;
					}
					case TimeSpeed.Fast:
					{
						result = 3f;
						break;
					}
					case TimeSpeed.Superfast:
					{
						result = (float)((Find.VisibleMap != null) ? ((!this.NothingHappeningInGame()) ? 6.0 : 12.0) : 150.0);
						break;
					}
					case TimeSpeed.Ultrafast:
					{
						result = (float)((Find.VisibleMap != null) ? 15.0 : 250.0);
						break;
					}
					default:
					{
						result = -1f;
						break;
					}
					}
				}
				return result;
			}
		}

		private float CurTimePerTick
		{
			get
			{
				return (float)((this.TickRateMultiplier != 0.0) ? (1.0 / (60.0 * this.TickRateMultiplier)) : 0.0);
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
				return (byte)((Find.MainTabsRoot.OpenTab == MainButtonDefOf.Menu) ? 1 : 0) != 0;
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
			if (this.curTimeSpeed != 0)
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
				int num = 0;
				while (num < maps.Count)
				{
					List<Pawn> list = maps[num].mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer);
					for (int i = 0; i < list.Count; i++)
					{
						Pawn pawn = list[i];
						if (pawn.HostFaction == null && pawn.RaceProps.Humanlike && pawn.Awake())
						{
							this.nothingHappeningCached = false;
							break;
						}
					}
					if (this.nothingHappeningCached)
					{
						num++;
						continue;
					}
					break;
				}
				if (this.nothingHappeningCached)
				{
					int num2 = 0;
					while (num2 < maps.Count)
					{
						if (!maps[num2].IsPlayerHome || (int)maps[num2].dangerWatcher.DangerRating < 1)
						{
							num2++;
							continue;
						}
						this.nothingHappeningCached = false;
						break;
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
			{
				result = null;
				break;
			}
			case TickerType.Normal:
			{
				result = this.tickListNormal;
				break;
			}
			case TickerType.Rare:
			{
				result = this.tickListRare;
				break;
			}
			case TickerType.Long:
			{
				result = this.tickListLong;
				break;
			}
			default:
			{
				throw new InvalidOperationException();
			}
			}
			return result;
		}

		public void TickManagerUpdate()
		{
			if (!this.Paused)
			{
				if (Mathf.Abs(Time.deltaTime - this.CurTimePerTick) < this.CurTimePerTick * 0.20000000298023224)
				{
					this.realTimeToTickThrough += this.CurTimePerTick;
				}
				else
				{
					this.realTimeToTickThrough += Time.deltaTime;
				}
				int num = 0;
				while (this.realTimeToTickThrough > 0.0 && (float)num < this.TickRateMultiplier * 2.0)
				{
					this.DoSingleTick();
					this.realTimeToTickThrough -= this.CurTimePerTick;
					num++;
				}
				if (this.realTimeToTickThrough > 0.0)
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
				this.ticksGameInt += 250;
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
				Log.Error(ex.ToString());
			}
			Profiler.EndSample();
			Profiler.BeginSample("Scenario.TickScenario()");
			try
			{
				Find.Scenario.TickScenario();
			}
			catch (Exception ex2)
			{
				Log.Error(ex2.ToString());
			}
			Profiler.EndSample();
			Profiler.BeginSample("WorldTick");
			try
			{
				Find.World.WorldTick();
			}
			catch (Exception ex3)
			{
				Log.Error(ex3.ToString());
			}
			Profiler.EndSample();
			Profiler.BeginSample("StoryWatcherTick");
			try
			{
				Find.StoryWatcher.StoryWatcherTick();
			}
			catch (Exception ex4)
			{
				Log.Error(ex4.ToString());
			}
			Profiler.EndSample();
			Profiler.BeginSample("GameEnder.GameEndTick()");
			try
			{
				Find.GameEnder.GameEndTick();
			}
			catch (Exception ex5)
			{
				Log.Error(ex5.ToString());
			}
			Profiler.EndSample();
			Profiler.BeginSample("Storyteller.StorytellerTick()");
			try
			{
				Find.Storyteller.StorytellerTick();
			}
			catch (Exception ex6)
			{
				Log.Error(ex6.ToString());
			}
			Profiler.EndSample();
			Profiler.BeginSample("taleManager.TaleManagerTick()");
			try
			{
				Current.Game.taleManager.TaleManagerTick();
			}
			catch (Exception ex7)
			{
				Log.Error(ex7.ToString());
			}
			Profiler.EndSample();
			Profiler.BeginSample("WorldPostTick");
			try
			{
				Find.World.WorldPostTick();
			}
			catch (Exception ex8)
			{
				Log.Error(ex8.ToString());
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
				Log.Error(ex9.ToString());
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
				Log.Error(ex10.ToString());
			}
			Profiler.EndSample();
			Profiler.BeginSample("Autosaver.AutosaverTick()");
			try
			{
				Find.Autosaver.AutosaverTick();
			}
			catch (Exception ex11)
			{
				Log.Error(ex11.ToString());
			}
			Profiler.EndSample();
			Debug.developerConsoleVisible = false;
		}

		public void RemoveAllFromMap(Map map)
		{
			this.tickListNormal.RemoveWhere((Predicate<Thing>)((Thing x) => x.Map == map));
			this.tickListRare.RemoveWhere((Predicate<Thing>)((Thing x) => x.Map == map));
			this.tickListLong.RemoveWhere((Predicate<Thing>)((Thing x) => x.Map == map));
		}

		public void DebugSetTicksGame(int newTicksGame)
		{
			this.ticksGameInt = newTicksGame;
		}
	}
}
