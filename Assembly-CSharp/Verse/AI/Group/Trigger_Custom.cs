using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A15 RID: 2581
	public class Trigger_Custom : Trigger
	{
		// Token: 0x040024A7 RID: 9383
		private Func<TriggerSignal, bool> condition;

		// Token: 0x060039A7 RID: 14759 RVA: 0x001E83D7 File Offset: 0x001E67D7
		public Trigger_Custom(Func<TriggerSignal, bool> condition)
		{
			this.condition = condition;
		}

		// Token: 0x060039A8 RID: 14760 RVA: 0x001E83E8 File Offset: 0x001E67E8
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return this.condition(signal);
		}
	}
}
