using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A11 RID: 2577
	public class Trigger_TicksPassedWithoutHarm : Trigger_TicksPassed
	{
		// Token: 0x0600399F RID: 14751 RVA: 0x001E8207 File Offset: 0x001E6607
		public Trigger_TicksPassedWithoutHarm(int tickLimit) : base(tickLimit)
		{
		}

		// Token: 0x060039A0 RID: 14752 RVA: 0x001E8214 File Offset: 0x001E6614
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
