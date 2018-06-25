using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A17 RID: 2583
	public class Trigger_TickCondition : Trigger
	{
		// Token: 0x040024B8 RID: 9400
		private Func<bool> condition;

		// Token: 0x040024B9 RID: 9401
		private int checkEveryTicks = 1;

		// Token: 0x060039AA RID: 14762 RVA: 0x001E8735 File Offset: 0x001E6B35
		public Trigger_TickCondition(Func<bool> condition, int checkEveryTicks = 1)
		{
			this.condition = condition;
			this.checkEveryTicks = checkEveryTicks;
		}

		// Token: 0x060039AB RID: 14763 RVA: 0x001E8754 File Offset: 0x001E6B54
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.checkEveryTicks == 0 && this.condition();
		}
	}
}
