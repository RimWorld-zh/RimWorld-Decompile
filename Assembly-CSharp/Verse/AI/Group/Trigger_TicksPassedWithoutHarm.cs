using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A15 RID: 2581
	public class Trigger_TicksPassedWithoutHarm : Trigger_TicksPassed
	{
		// Token: 0x060039A5 RID: 14757 RVA: 0x001E7FC7 File Offset: 0x001E63C7
		public Trigger_TicksPassedWithoutHarm(int tickLimit) : base(tickLimit)
		{
		}

		// Token: 0x060039A6 RID: 14758 RVA: 0x001E7FD4 File Offset: 0x001E63D4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (Trigger_PawnHarmed.SignalIsHarm(signal))
			{
				base.Data.ticksPassed = 0;
			}
			return base.ActivateOn(lord, signal);
		}
	}
}
