using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A15 RID: 2581
	public class Trigger_TicksPassedWithoutHarm : Trigger_TicksPassed
	{
		// Token: 0x060039A3 RID: 14755 RVA: 0x001E7EF3 File Offset: 0x001E62F3
		public Trigger_TicksPassedWithoutHarm(int tickLimit) : base(tickLimit)
		{
		}

		// Token: 0x060039A4 RID: 14756 RVA: 0x001E7F00 File Offset: 0x001E6300
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
