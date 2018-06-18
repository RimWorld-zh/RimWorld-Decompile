using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A17 RID: 2583
	public class Trigger_Custom : Trigger
	{
		// Token: 0x060039A9 RID: 14761 RVA: 0x001E806B File Offset: 0x001E646B
		public Trigger_Custom(Func<TriggerSignal, bool> condition)
		{
			this.condition = condition;
		}

		// Token: 0x060039AA RID: 14762 RVA: 0x001E807C File Offset: 0x001E647C
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return this.condition(signal);
		}

		// Token: 0x040024AB RID: 9387
		private Func<TriggerSignal, bool> condition;
	}
}
