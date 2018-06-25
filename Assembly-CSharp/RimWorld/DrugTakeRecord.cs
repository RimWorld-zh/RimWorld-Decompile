using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004E9 RID: 1257
	public class DrugTakeRecord : IExposable
	{
		// Token: 0x04000D1D RID: 3357
		public ThingDef drug;

		// Token: 0x04000D1E RID: 3358
		public int lastTakenTicks;

		// Token: 0x04000D1F RID: 3359
		private int timesTakenThisDayInt;

		// Token: 0x04000D20 RID: 3360
		private int thisDay;

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06001672 RID: 5746 RVA: 0x000C7470 File Offset: 0x000C5870
		public int LastTakenDays
		{
			get
			{
				return GenDate.DaysPassedAt(this.lastTakenTicks);
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06001673 RID: 5747 RVA: 0x000C7490 File Offset: 0x000C5890
		// (set) Token: 0x06001674 RID: 5748 RVA: 0x000C74C2 File Offset: 0x000C58C2
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

		// Token: 0x06001675 RID: 5749 RVA: 0x000C74D8 File Offset: 0x000C58D8
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.drug, "drug");
			Scribe_Values.Look<int>(ref this.lastTakenTicks, "lastTakenTicks", 0, false);
			Scribe_Values.Look<int>(ref this.timesTakenThisDayInt, "timesTakenThisDay", 0, false);
			Scribe_Values.Look<int>(ref this.thisDay, "thisDay", 0, false);
		}
	}
}
