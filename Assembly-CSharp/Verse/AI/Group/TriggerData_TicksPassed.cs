using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A11 RID: 2577
	public class TriggerData_TicksPassed : TriggerData
	{
		// Token: 0x040024A4 RID: 9380
		public int ticksPassed;

		// Token: 0x060039A0 RID: 14752 RVA: 0x001E82CA File Offset: 0x001E66CA
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}
	}
}
