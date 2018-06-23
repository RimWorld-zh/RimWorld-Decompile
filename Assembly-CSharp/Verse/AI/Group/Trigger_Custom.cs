using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A13 RID: 2579
	public class Trigger_Custom : Trigger
	{
		// Token: 0x040024A6 RID: 9382
		private Func<TriggerSignal, bool> condition;

		// Token: 0x060039A3 RID: 14755 RVA: 0x001E82AB File Offset: 0x001E66AB
		public Trigger_Custom(Func<TriggerSignal, bool> condition)
		{
			this.condition = condition;
		}

		// Token: 0x060039A4 RID: 14756 RVA: 0x001E82BC File Offset: 0x001E66BC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return this.condition(signal);
		}
	}
}
