using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A13 RID: 2579
	public class TriggerData_TicksPassed : TriggerData
	{
		// Token: 0x060039A2 RID: 14754 RVA: 0x001E7F5E File Offset: 0x001E635E
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}

		// Token: 0x040024A8 RID: 9384
		public int ticksPassed;
	}
}
