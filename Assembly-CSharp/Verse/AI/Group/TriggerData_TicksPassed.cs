using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A12 RID: 2578
	public class TriggerData_TicksPassed : TriggerData
	{
		// Token: 0x040024B4 RID: 9396
		public int ticksPassed;

		// Token: 0x060039A1 RID: 14753 RVA: 0x001E85F6 File Offset: 0x001E69F6
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}
	}
}
