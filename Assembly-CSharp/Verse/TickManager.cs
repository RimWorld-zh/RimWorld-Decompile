using System;
using System.Collections.Generic;
using System.Diagnostics;
using RimWorld;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000BD1 RID: 3025
	public sealed class TickManager : IExposable
	{
		// Token: 0x04002D18 RID: 11544
		private int ticksGameInt = 0;

		// Token: 0x04002D19 RID: 11545
		public int gameStartAbsTick = 0;

		// Token: 0x04002D1A RID: 11546
		private float realTimeToTickThrough = 0f;

		// Token: 0x04002D1B RID: 11547
		private TimeSpeed curTimeSpeed = TimeSpeed.Normal;

		// Token: 0x04002D1C RID: 11548
		public TimeSpeed prePauseTimeSpeed = TimeSpeed.Paused;

		// Token: 0x04002D1D RID: 11549
		private int startingYearInt = 5500;

		// Token: 0x04002D1E RID: 11550
		private Stopwatch clock = new Stopwatch();

		// Token: 0x04002D1F RID: 11551
		private TickList tickListNormal = new TickList(TickerType.Normal);

		// Token: 0x04002D20 RID: 11552
		private TickList tickListRare = new TickList(TickerType.Rare);

		// Token: 0x04002D21 RID: 11553
		private TickList tickListLong = new TickList(TickerType.Long);

		// Token: 0x04002D22 RID: 11554
		public TimeSlower slower = new TimeSlower();

		// Token: 0x04002D23 RID: 11555
		private int lastAutoScreenshot = 0;

		// Token: 0x04002D24 RID: 11556
		private float WorstAllowedFPS = 22f;

		// Token: 0x04002D25 RID: 11557
		private int lastNothingHappeningCheckTick = -1;

		// Token: 0x04002D26 RID: 11558
		private bool nothingHappeningCached = false;

		// Token: 0x17000A4D RID: 2637
		// (get) Token: 0x060041F4 RID: 16884 RVA: 0x0022C5B8 File Offset: 0x0022A9B8
		public int TicksGame
		{
			get
			{
				return this.ticksGameInt;
			}
		}

		// Token: 0x17000A4E RID: 2638
		// (get) Token: 0x060041F5 RID: 16885 RVA: 0x0022C5D4 File Offset: 0x0022A9D4
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

		// Token: 0x17000A4F RID: 2639
		// (get) Token: 0x060041F6 RID: 16886 RVA: 0x0022C620 File Offset: 0x0022AA20
		public int StartingYear
		{
			get
			{
				return this.startingYearInt;
			}
		}

		// Token: 0x17000A50 RID: 2640
		// (get) Token: 0x060041F7 RID: 16887 RVA: 0x0022C63C File Offset: 0x0022AA3C
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

		// Token: 0x17000A51 RID: 2641
		// (get) Token: 0x060041F8 RID: 16888 RVA: 0x0022C730 File Offset: 0x0022AB30
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

		// Token: 0x17000A52 RID: 2642
		// (get) Token: 0x060041F9 RID: 16889 RVA: 0x0022C774 File Offset: 0x0022AB74
		public bool Paused
		{
			get
			{
				return this.curTimeSpeed == TimeSpeed.Paused || Find.WindowStack.WindowsForcePause || LongEventHandler.ForcePause;
			}
		}

		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x060041FA RID: 16890 RVA: 0x0022C7AC File Offset: 0x0022ABAC
		public bool NotPlaying
		{
			get
			{
				return Find.MainTabsRoot.OpenTab == MainButtonDefOf.Menu;
			}
		}

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x060041FB RID: 16891 RVA: 0x0022C7E0 File Offset: 0x0022ABE0
		// (set) Token: 0x060041FC RID: 16892 RVA: 0x0022C7FB File Offset: 0x0022ABFB
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

		// Token: 0x060041FD RID: 16893 RVA: 0x0022C808 File Offset: 0x0022AC08
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

		// Token: 0x060041FE RID: 16894 RVA: 0x0022C868 File Offset: 0x0022AC68
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

		// Token: 0x060041FF RID: 16895 RVA: 0x0022C9AA File Offset: 0x0022ADAA
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksGameInt, "ticksGame", 0, false);
			Scribe_Values.Look<int>(ref this.gameStartAbsTick, "gameStartAbsTick", 0, false);
			Scribe_Values.Look<int>(ref this.startingYearInt, "startingYear", 0, false);
		}

		// Token: 0x06004200 RID: 16896 RVA: 0x0022C9E4 File Offset: 0x0022ADE4
		public void RegisterAllTickabilityFor(Thing t)
		{
			TickList tickList = this.TickListFor(t);
			if (tickList != null)
			{
				tickList.RegisterThing(t);
			}
		}

		// Token: 0x06004201 RID: 16897 RVA: 0x0022CA08 File Offset: 0x0022AE08
		public void DeRegisterAllTickabilityFor(Thing t)
		{
			TickList tickList = this.TickListFor(t);
			if (tickList != null)
			{
				tickList.DeregisterThing(t);
			}
		}

		// Token: 0x06004202 RID: 16898 RVA: 0x0022CA2C File Offset: 0x0022AE2C
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

		// Token: 0x06004203 RID: 16899 RVA: 0x0022CA94 File Offset: 0x0022AE94
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

		// Token: 0x06004204 RID: 16900 RVA: 0x0022CB98 File Offset: 0x0022AF98
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

		// Token: 0x06004205 RID: 16901 RVA: 0x0022D05C File Offset: 0x0022B45C
		public void RemoveAllFromMap(Map map)
		{
			this.tickListNormal.RemoveWhere((Thing x) => x.Map == map);
			this.tickListRare.RemoveWhere((Thing x) => x.Map == map);
			this.tickListLong.RemoveWhere((Thing x) => x.Map == map);
		}

		// Token: 0x06004206 RID: 16902 RVA: 0x0022D0BC File Offset: 0x0022B4BC
		public void DebugSetTicksGame(int newTicksGame)
		{
			this.ticksGameInt = newTicksGame;
		}
	}
}
