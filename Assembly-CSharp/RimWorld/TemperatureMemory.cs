using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000448 RID: 1096
	public class TemperatureMemory : IExposable
	{
		// Token: 0x04000B91 RID: 2961
		private Map map;

		// Token: 0x04000B92 RID: 2962
		private int growthSeasonUntilTick = -1;

		// Token: 0x04000B93 RID: 2963
		private int noSowUntilTick = -1;

		// Token: 0x04000B94 RID: 2964
		private const int TicksBuffer = 30000;

		// Token: 0x06001301 RID: 4865 RVA: 0x000A43DB File Offset: 0x000A27DB
		public TemperatureMemory(Map map)
		{
			this.map = map;
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06001302 RID: 4866 RVA: 0x000A43FC File Offset: 0x000A27FC
		public bool GrowthSeasonOutdoorsNow
		{
			get
			{
				return Find.TickManager.TicksGame < this.growthSeasonUntilTick;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06001303 RID: 4867 RVA: 0x000A4424 File Offset: 0x000A2824
		public bool GrowthSeasonOutdoorsNowForSowing
		{
			get
			{
				return (this.noSowUntilTick <= 0 || Find.TickManager.TicksGame >= this.noSowUntilTick) && this.GrowthSeasonOutdoorsNow;
			}
		}

		// Token: 0x06001304 RID: 4868 RVA: 0x000A4468 File Offset: 0x000A2868
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

		// Token: 0x06001305 RID: 4869 RVA: 0x000A4500 File Offset: 0x000A2900
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.growthSeasonUntilTick, "growthSeasonUntilTick", 0, true);
			Scribe_Values.Look<int>(ref this.noSowUntilTick, "noSowUntilTick", 0, true);
		}
	}
}
