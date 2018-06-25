using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A1A RID: 2586
	public class Trigger_ChanceOnTickInteval : Trigger
	{
		// Token: 0x040024AE RID: 9390
		private float chancePerInterval;

		// Token: 0x040024AF RID: 9391
		private int interval;

		// Token: 0x060039B1 RID: 14769 RVA: 0x001E857B File Offset: 0x001E697B
		public Trigger_ChanceOnTickInteval(int interval, float chancePerInterval)
		{
			this.chancePerInterval = chancePerInterval;
			this.interval = interval;
		}

		// Token: 0x060039B2 RID: 14770 RVA: 0x001E8594 File Offset: 0x001E6994
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.interval == 0 && Rand.Value < this.chancePerInterval;
		}
	}
}
