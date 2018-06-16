using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A1C RID: 2588
	public class Trigger_ChanceOnTickInteval : Trigger
	{
		// Token: 0x060039B1 RID: 14769 RVA: 0x001E813B File Offset: 0x001E653B
		public Trigger_ChanceOnTickInteval(int interval, float chancePerInterval)
		{
			this.chancePerInterval = chancePerInterval;
			this.interval = interval;
		}

		// Token: 0x060039B2 RID: 14770 RVA: 0x001E8154 File Offset: 0x001E6554
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
