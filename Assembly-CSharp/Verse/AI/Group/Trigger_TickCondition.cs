using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A18 RID: 2584
	public class Trigger_TickCondition : Trigger
	{
		// Token: 0x060039A9 RID: 14761 RVA: 0x001E7FC9 File Offset: 0x001E63C9
		public Trigger_TickCondition(Func<bool> condition, int checkEveryTicks = 1)
		{
			this.condition = condition;
			this.checkEveryTicks = checkEveryTicks;
		}

		// Token: 0x060039AA RID: 14762 RVA: 0x001E7FE8 File Offset: 0x001E63E8
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.checkEveryTicks == 0 && this.condition();
		}

		// Token: 0x040024AC RID: 9388
		private Func<bool> condition;

		// Token: 0x040024AD RID: 9389
		private int checkEveryTicks = 1;
	}
}
