using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A16 RID: 2582
	public class Trigger_TickCondition : Trigger
	{
		// Token: 0x040024A8 RID: 9384
		private Func<bool> condition;

		// Token: 0x040024A9 RID: 9385
		private int checkEveryTicks = 1;

		// Token: 0x060039A9 RID: 14761 RVA: 0x001E8409 File Offset: 0x001E6809
		public Trigger_TickCondition(Func<bool> condition, int checkEveryTicks = 1)
		{
			this.condition = condition;
			this.checkEveryTicks = checkEveryTicks;
		}

		// Token: 0x060039AA RID: 14762 RVA: 0x001E8428 File Offset: 0x001E6828
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.checkEveryTicks == 0 && this.condition();
		}
	}
}
