using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A12 RID: 2578
	public class Trigger_TicksPassedAndNoRecentHarm : Trigger_TicksPassed
	{
		// Token: 0x040024A5 RID: 9381
		private const int MinTicksSinceDamage = 300;

		// Token: 0x060039A1 RID: 14753 RVA: 0x001E82DF File Offset: 0x001E66DF
		public Trigger_TicksPassedAndNoRecentHarm(int tickLimit) : base(tickLimit)
		{
		}

		// Token: 0x060039A2 RID: 14754 RVA: 0x001E82EC File Offset: 0x001E66EC
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
