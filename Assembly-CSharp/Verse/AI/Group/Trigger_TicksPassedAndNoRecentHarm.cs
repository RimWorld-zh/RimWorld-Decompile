using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A14 RID: 2580
	public class Trigger_TicksPassedAndNoRecentHarm : Trigger_TicksPassed
	{
		// Token: 0x060039A3 RID: 14755 RVA: 0x001E7F73 File Offset: 0x001E6373
		public Trigger_TicksPassedAndNoRecentHarm(int tickLimit) : base(tickLimit)
		{
		}

		// Token: 0x060039A4 RID: 14756 RVA: 0x001E7F80 File Offset: 0x001E6380
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

		// Token: 0x040024A9 RID: 9385
		private const int MinTicksSinceDamage = 300;
	}
}
