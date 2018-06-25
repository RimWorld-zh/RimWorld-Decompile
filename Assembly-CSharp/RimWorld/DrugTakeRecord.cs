using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020004E9 RID: 1257
	public class DrugTakeRecord : IExposable
	{
		// Token: 0x04000D1A RID: 3354
		public ThingDef drug;

		// Token: 0x04000D1B RID: 3355
		public int lastTakenTicks;

		// Token: 0x04000D1C RID: 3356
		private int timesTakenThisDayInt;

		// Token: 0x04000D1D RID: 3357
		private int thisDay;

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06001673 RID: 5747 RVA: 0x000C7270 File Offset: 0x000C5670
		public int LastTakenDays
		{
			get
			{
				return GenDate.DaysPassedAt(this.lastTakenTicks);
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06001674 RID: 5748 RVA: 0x000C7290 File Offset: 0x000C5690
		// (set) Token: 0x06001675 RID: 5749 RVA: 0x000C72C2 File Offset: 0x000C56C2
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

		// Token: 0x06001676 RID: 5750 RVA: 0x000C72D8 File Offset: 0x000C56D8
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.drug, "drug");
			Scribe_Values.Look<int>(ref this.lastTakenTicks, "lastTakenTicks", 0, false);
			Scribe_Values.Look<int>(ref this.timesTakenThisDayInt, "timesTakenThisDay", 0, false);
			Scribe_Values.Look<int>(ref this.thisDay, "thisDay", 0, false);
		}
	}
}
