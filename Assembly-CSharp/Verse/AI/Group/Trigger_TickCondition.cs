using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A18 RID: 2584
	public class Trigger_TickCondition : Trigger
	{
		// Token: 0x060039AB RID: 14763 RVA: 0x001E809D File Offset: 0x001E649D
		public Trigger_TickCondition(Func<bool> condition, int checkEveryTicks = 1)
		{
			this.condition = condition;
			this.checkEveryTicks = checkEveryTicks;
		}

		// Token: 0x060039AC RID: 14764 RVA: 0x001E80BC File Offset: 0x001E64BC
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
