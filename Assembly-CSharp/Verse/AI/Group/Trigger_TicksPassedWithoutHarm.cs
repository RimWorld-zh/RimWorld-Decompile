using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A13 RID: 2579
	public class Trigger_TicksPassedWithoutHarm : Trigger_TicksPassed
	{
		// Token: 0x060039A3 RID: 14755 RVA: 0x001E8333 File Offset: 0x001E6733
		public Trigger_TicksPassedWithoutHarm(int tickLimit) : base(tickLimit)
		{
		}

		// Token: 0x060039A4 RID: 14756 RVA: 0x001E8340 File Offset: 0x001E6740
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
