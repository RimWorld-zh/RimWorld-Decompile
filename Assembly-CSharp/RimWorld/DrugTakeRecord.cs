using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004EB RID: 1259
	public class DrugTakeRecord : IExposable
	{
		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06001678 RID: 5752 RVA: 0x000C712C File Offset: 0x000C552C
		public int LastTakenDays
		{
			get
			{
				return GenDate.DaysPassedAt(this.lastTakenTicks);
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06001679 RID: 5753 RVA: 0x000C714C File Offset: 0x000C554C
		// (set) Token: 0x0600167A RID: 5754 RVA: 0x000C717E File Offset: 0x000C557E
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

		// Token: 0x0600167B RID: 5755 RVA: 0x000C7194 File Offset: 0x000C5594
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.drug, "drug");
			Scribe_Values.Look<int>(ref this.lastTakenTicks, "lastTakenTicks", 0, false);
			Scribe_Values.Look<int>(ref this.timesTakenThisDayInt, "timesTakenThisDay", 0, false);
			Scribe_Values.Look<int>(ref this.thisDay, "thisDay", 0, false);
		}

		// Token: 0x04000D1D RID: 3357
		public ThingDef drug;

		// Token: 0x04000D1E RID: 3358
		public int lastTakenTicks;

		// Token: 0x04000D1F RID: 3359
		private int timesTakenThisDayInt;

		// Token: 0x04000D20 RID: 3360
		private int thisDay;
	}
}
