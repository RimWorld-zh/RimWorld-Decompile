using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A16 RID: 2582
	public class Trigger_Custom : Trigger
	{
		// Token: 0x040024B7 RID: 9399
		private Func<TriggerSignal, bool> condition;

		// Token: 0x060039A8 RID: 14760 RVA: 0x001E8703 File Offset: 0x001E6B03
		public Trigger_Custom(Func<TriggerSignal, bool> condition)
		{
			this.condition = condition;
		}

		// Token: 0x060039A9 RID: 14761 RVA: 0x001E8714 File Offset: 0x001E6B14
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return this.condition(signal);
		}
	}
}
