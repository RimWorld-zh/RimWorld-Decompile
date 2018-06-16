using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A17 RID: 2583
	public class Trigger_Custom : Trigger
	{
		// Token: 0x060039A7 RID: 14759 RVA: 0x001E7F97 File Offset: 0x001E6397
		public Trigger_Custom(Func<TriggerSignal, bool> condition)
		{
			this.condition = condition;
		}

		// Token: 0x060039A8 RID: 14760 RVA: 0x001E7FA8 File Offset: 0x001E63A8
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return this.condition(signal);
		}

		// Token: 0x040024AB RID: 9387
		private Func<TriggerSignal, bool> condition;
	}
}
