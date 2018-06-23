using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A10 RID: 2576
	public class Trigger_TicksPassedAndNoRecentHarm : Trigger_TicksPassed
	{
		// Token: 0x040024A4 RID: 9380
		private const int MinTicksSinceDamage = 300;

		// Token: 0x0600399D RID: 14749 RVA: 0x001E81B3 File Offset: 0x001E65B3
		public Trigger_TicksPassedAndNoRecentHarm(int tickLimit) : base(tickLimit)
		{
		}

		// Token: 0x0600399E RID: 14750 RVA: 0x001E81C0 File Offset: 0x001E65C0
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
