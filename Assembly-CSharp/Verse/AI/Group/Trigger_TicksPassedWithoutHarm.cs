using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A14 RID: 2580
	public class Trigger_TicksPassedWithoutHarm : Trigger_TicksPassed
	{
		// Token: 0x060039A4 RID: 14756 RVA: 0x001E865F File Offset: 0x001E6A5F
		public Trigger_TicksPassedWithoutHarm(int tickLimit) : base(tickLimit)
		{
		}

		// Token: 0x060039A5 RID: 14757 RVA: 0x001E866C File Offset: 0x001E6A6C
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
