using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A0F RID: 2575
	public class TriggerData_TicksPassed : TriggerData
	{
		// Token: 0x0600399C RID: 14748 RVA: 0x001E819E File Offset: 0x001E659E
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksPassed, "ticksPassed", 0, false);
		}

		// Token: 0x040024A3 RID: 9379
		public int ticksPassed;
	}
}
