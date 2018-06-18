using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200044A RID: 1098
	public class TemperatureMemory : IExposable
	{
		// Token: 0x06001307 RID: 4871 RVA: 0x000A407B File Offset: 0x000A247B
		public TemperatureMemory(Map map)
		{
			this.map = map;
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06001308 RID: 4872 RVA: 0x000A409C File Offset: 0x000A249C
		public bool GrowthSeasonOutdoorsNow
		{
			get
			{
				return Find.TickManager.TicksGame < this.growthSeasonUntilTick;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06001309 RID: 4873 RVA: 0x000A40C4 File Offset: 0x000A24C4
		public bool GrowthSeasonOutdoorsNowForSowing
		{
			get
			{
				return (this.noSowUntilTick <= 0 || Find.TickManager.TicksGame >= this.noSowUntilTick) && this.GrowthSeasonOutdoorsNow;
			}
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x000A4108 File Offset: 0x000A2508
		public void GrowthSeasonMemoryTick()
		{
			if (this.map.mapTemperature.OutdoorTemp > 0f && this.map.mapTemperature.OutdoorTemp < 58f)
			{
				this.growthSeasonUntilTick = Find.TickManager.TicksGame + 30000;
			}
			else if (this.map.mapTemperature.OutdoorTemp < -2f)
			{
				this.growthSeasonUntilTick = -1;
				this.noSowUntilTick = Find.TickManager.TicksGame + 30000;
			}
		}

		// Token: 0x0600130B RID: 4875 RVA: 0x000A41A0 File Offset: 0x000A25A0
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.growthSeasonUntilTick, "growthSeasonUntilTick", 0, true);
			Scribe_Values.Look<int>(ref this.noSowUntilTick, "noSowUntilTick", 0, true);
		}

		// Token: 0x04000B91 RID: 2961
		private Map map;

		// Token: 0x04000B92 RID: 2962
		private int growthSeasonUntilTick = -1;

		// Token: 0x04000B93 RID: 2963
		private int noSowUntilTick = -1;

		// Token: 0x04000B94 RID: 2964
		private const int TicksBuffer = 30000;
	}
}
