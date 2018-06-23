using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A18 RID: 2584
	public class Trigger_ChanceOnTickInteval : Trigger
	{
		// Token: 0x040024AD RID: 9389
		private float chancePerInterval;

		// Token: 0x040024AE RID: 9390
		private int interval;

		// Token: 0x060039AD RID: 14765 RVA: 0x001E844F File Offset: 0x001E684F
		public Trigger_ChanceOnTickInteval(int interval, float chancePerInterval)
		{
			this.chancePerInterval = chancePerInterval;
			this.interval = interval;
		}

		// Token: 0x060039AE RID: 14766 RVA: 0x001E8468 File Offset: 0x001E6868
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.interval == 0 && Rand.Value < this.chancePerInterval;
		}
	}
}
