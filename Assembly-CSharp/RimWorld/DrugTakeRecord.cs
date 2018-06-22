using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004E7 RID: 1255
	public class DrugTakeRecord : IExposable
	{
		// Token: 0x170002EF RID: 751
		// (get) Token: 0x0600166F RID: 5743 RVA: 0x000C7120 File Offset: 0x000C5520
		public int LastTakenDays
		{
			get
			{
				return GenDate.DaysPassedAt(this.lastTakenTicks);
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06001670 RID: 5744 RVA: 0x000C7140 File Offset: 0x000C5540
		// (set) Token: 0x06001671 RID: 5745 RVA: 0x000C7172 File Offset: 0x000C5572
		public int TimesTakenThisDay
		{
			get
			{
				int result;
				if (this.thisDay != GenDate.DaysPassed)
				{
					result = 0;
				}
				else
				{
					result = this.timesTakenThisDayInt;
				}
				return result;
			}
			set
			{
				this.timesTakenThisDayInt = value;
				this.thisDay = GenDate.DaysPassed;
			}
		}

		// Token: 0x06001672 RID: 5746 RVA: 0x000C7188 File Offset: 0x000C5588
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.drug, "drug");
			Scribe_Values.Look<int>(ref this.lastTakenTicks, "lastTakenTicks", 0, false);
			Scribe_Values.Look<int>(ref this.timesTakenThisDayInt, "timesTakenThisDay", 0, false);
			Scribe_Values.Look<int>(ref this.thisDay, "thisDay", 0, false);
		}

		// Token: 0x04000D1A RID: 3354
		public ThingDef drug;

		// Token: 0x04000D1B RID: 3355
		public int lastTakenTicks;

		// Token: 0x04000D1C RID: 3356
		private int timesTakenThisDayInt;

		// Token: 0x04000D1D RID: 3357
		private int thisDay;
	}
}
