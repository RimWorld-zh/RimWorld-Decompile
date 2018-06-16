using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A14 RID: 2580
	public class Trigger_TicksPassedAndNoRecentHarm : Trigger_TicksPassed
	{
		// Token: 0x060039A1 RID: 14753 RVA: 0x001E7E9F File Offset: 0x001E629F
		public Trigger_TicksPassedAndNoRecentHarm(int tickLimit) : base(tickLimit)
		{
		}

		// Token: 0x060039A2 RID: 14754 RVA: 0x001E7EAC File Offset: 0x001E62AC
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
