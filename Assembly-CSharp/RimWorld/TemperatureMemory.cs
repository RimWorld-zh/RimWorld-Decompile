using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200044A RID: 1098
	public class TemperatureMemory : IExposable
	{
		// Token: 0x06001307 RID: 4871 RVA: 0x000A406F File Offset: 0x000A246F
		public TemperatureMemory(Map map)
		{
			this.map = map;
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06001308 RID: 4872 RVA: 0x000A4090 File Offset: 0x000A2490
		public bool GrowthSeasonOutdoorsNow
		{
			get
			{
				return Find.TickManager.TicksGame < this.growthSeasonUntilTick;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06001309 RID: 4873 RVA: 0x000A40B8 File Offset: 0x000A24B8
		public bool GrowthSeasonOutdoorsNowForSowing
		{
			get
			{
				return (this.noSowUntilTick <= 0 || Find.TickManager.TicksGame >= this.noSowUntilTick) && this.GrowthSeasonOutdoorsNow;
			}
		}

		// Token: 0x0600130A RID: 4874 RVA: 0x000A40FC File Offset: 0x000A24FC
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

		// Token: 0x0600130B RID: 4875 RVA: 0x000A4194 File Offset: 0x000A2594
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
