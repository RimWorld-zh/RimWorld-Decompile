using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A13 RID: 2579
	public class Trigger_TicksPassedAndNoRecentHarm : Trigger_TicksPassed
	{
		// Token: 0x040024B5 RID: 9397
		private const int MinTicksSinceDamage = 300;

		// Token: 0x060039A2 RID: 14754 RVA: 0x001E860B File Offset: 0x001E6A0B
		public Trigger_TicksPassedAndNoRecentHarm(int tickLimit) : base(tickLimit)
		{
		}

		// Token: 0x060039A3 RID: 14755 RVA: 0x001E8618 File Offset: 0x001E6A18
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (base.ActivateOn(lord, signal))
			{
				if (Find.TickManager.TicksGame - lord.lastPawnHarmTick >= 300)
				{
					return true;
				}
			}
			return false;
		}
	}
}
