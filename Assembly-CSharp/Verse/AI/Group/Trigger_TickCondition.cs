using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A14 RID: 2580
	public class Trigger_TickCondition : Trigger
	{
		// Token: 0x040024A7 RID: 9383
		private Func<bool> condition;

		// Token: 0x040024A8 RID: 9384
		private int checkEveryTicks = 1;

		// Token: 0x060039A5 RID: 14757 RVA: 0x001E82DD File Offset: 0x001E66DD
		public Trigger_TickCondition(Func<bool> condition, int checkEveryTicks = 1)
		{
			this.condition = condition;
			this.checkEveryTicks = checkEveryTicks;
		}

		// Token: 0x060039A6 RID: 14758 RVA: 0x001E82FC File Offset: 0x001E66FC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % this.checkEveryTicks == 0 && this.condition();
		}
	}
}
