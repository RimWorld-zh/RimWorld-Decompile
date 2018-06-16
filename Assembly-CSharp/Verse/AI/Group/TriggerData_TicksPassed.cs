using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A13 RID: 2579
	public class TriggerData_TicksPassed : TriggerData
	{
		// Token: 0x060039A0 RID: 14752 RVA: 0x001E7E8A File Offset: 0x001E628A
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}

		// Token: 0x040024A8 RID: 9384
		public int ticksPassed;
	}
}
