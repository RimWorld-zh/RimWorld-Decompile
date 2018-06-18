using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A1C RID: 2588
	public class Trigger_ChanceOnTickInteval : Trigger
	{
		// Token: 0x060039B3 RID: 14771 RVA: 0x001E820F File Offset: 0x001E660F
		public Trigger_ChanceOnTickInteval(int interval, float chancePerInterval)
		{
			this.chancePerInterval = chancePerInterval;
			this.interval = interval;
		}

		// Token: 0x060039B4 RID: 14772 RVA: 0x001E8228 File Offset: 0x001E6628
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.interval == 0 && Rand.Value < this.chancePerInterval;
		}

		// Token: 0x040024B2 RID: 9394
		private float chancePerInterval;

		// Token: 0x040024B3 RID: 9395
		private int interval;
	}
}
