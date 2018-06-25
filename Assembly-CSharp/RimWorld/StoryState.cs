using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200035B RID: 859
	public class StoryState : IExposable
	{
		// Token: 0x04000925 RID: 2341
		private IIncidentTarget target;

		// Token: 0x04000926 RID: 2342
		private int lastThreatBigTick = -1;

		// Token: 0x04000927 RID: 2343
		public Dictionary<IncidentDef, int> lastFireTicks = new Dictionary<IncidentDef, int>();

		// Token: 0x06000EE4 RID: 3812 RVA: 0x0007DBF5 File Offset: 0x0007BFF5
		public StoryState(IIncidentTarget target)
		{
			this.target = target;
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x0007DC18 File Offset: 0x0007C018
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

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0007DCAB File Offset: 0x0007C0AB
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastThreatBigTick, "lastThreatBigTick", 0, true);
			Scribe_Collections.Look<IncidentDef, int>(ref this.lastFireTicks, "lastFireTicks", LookMode.Def, LookMode.Value);
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x0007DCD4 File Offset: 0x0007C0D4
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

		// Token: 0x06000EE8 RID: 3816 RVA: 0x0007DDC8 File Offset: 0x0007C1C8
		public void CopyTo(StoryState other)
		{
			other.lastThreatBigTick = this.lastThreatBigTick;
			other.lastFireTicks.Clear();
			foreach (KeyValuePair<IncidentDef, int> keyValuePair in this.lastFireTicks)
			{
				other.lastFireTicks.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}
	}
}
