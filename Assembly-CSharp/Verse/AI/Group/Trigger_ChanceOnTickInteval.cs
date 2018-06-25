using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A1B RID: 2587
	public class Trigger_ChanceOnTickInteval : Trigger
	{
		// Token: 0x040024BE RID: 9406
		private float chancePerInterval;

		// Token: 0x040024BF RID: 9407
		private int interval;

		// Token: 0x060039B2 RID: 14770 RVA: 0x001E88A7 File Offset: 0x001E6CA7
		public Trigger_ChanceOnTickInteval(int interval, float chancePerInterval)
		{
			this.chancePerInterval = chancePerInterval;
			this.interval = interval;
		}

		// Token: 0x060039B3 RID: 14771 RVA: 0x001E88C0 File Offset: 0x001E6CC0
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.interval == 0 && Rand.Value < this.chancePerInterval;
		}
	}
}
