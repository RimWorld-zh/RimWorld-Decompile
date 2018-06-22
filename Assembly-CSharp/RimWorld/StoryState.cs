using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000359 RID: 857
	public class StoryState : IExposable
	{
		// Token: 0x06000EE1 RID: 3809 RVA: 0x0007DA95 File Offset: 0x0007BE95
		public StoryState(IIncidentTarget target)
		{
			this.target = target;
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000EE2 RID: 3810 RVA: 0x0007DAB8 File Offset: 0x0007BEB8
		public int LastThreatBigTick
		{
			get
			{
				if (this.lastThreatBigTick > Find.TickManager.TicksGame + 1000)
				{
					Log.Error(string.Concat(new object[]
					{
						"Latest big threat queue time was ",
						this.lastThreatBigTick,
						" at tick ",
						Find.TickManager.TicksGame,
						". This is too far in the future. Resetting."
					}), false);
					this.lastThreatBigTick = Find.TickManager.TicksGame - 1;
				}
				return this.lastThreatBigTick;
			}
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x0007DB4B File Offset: 0x0007BF4B
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastThreatBigTick, "lastThreatBigTick", 0, true);
			Scribe_Collections.Look<IncidentDef, int>(ref this.lastFireTicks, "lastFireTicks", LookMode.Def, LookMode.Value);
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x0007DB74 File Offset: 0x0007BF74
		public void Notify_IncidentFired(FiringIncident qi)
		{
			if (!qi.parms.forced && qi.parms.target == this.target)
			{
				int ticksGame = Find.TickManager.TicksGame;
				if (qi.def.category == IncidentCategoryDefOf.ThreatBig || qi.def.category == IncidentCategoryDefOf.RaidBeacon)
				{
					if (this.lastThreatBigTick <= ticksGame)
					{
						this.lastThreatBigTick = ticksGame;
					}
					else
					{
						Log.Error("Queueing threats backwards in time (" + qi + ")", false);
					}
					Find.StoryWatcher.statsRecord.numThreatBigs++;
				}
				if (this.lastFireTicks.ContainsKey(qi.def))
				{
					this.lastFireTicks[qi.def] = ticksGame;
				}
				else
				{
					this.lastFireTicks.Add(qi.def, ticksGame);
				}
			}
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x0007DC68 File Offset: 0x0007C068
		public void CopyTo(StoryState other)
		{
			other.lastThreatBigTick = this.lastThreatBigTick;
			other.lastFireTicks.Clear();
			foreach (KeyValuePair<IncidentDef, int> keyValuePair in this.lastFireTicks)
			{
				other.lastFireTicks.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x04000922 RID: 2338
		private IIncidentTarget target;

		// Token: 0x04000923 RID: 2339
		private int lastThreatBigTick = -1;

		// Token: 0x04000924 RID: 2340
		public Dictionary<IncidentDef, int> lastFireTicks = new Dictionary<IncidentDef, int>();
	}
}
