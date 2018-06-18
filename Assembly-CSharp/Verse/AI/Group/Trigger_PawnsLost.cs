using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A1A RID: 2586
	public class Trigger_PawnsLost : Trigger
	{
		// Token: 0x060039AF RID: 14767 RVA: 0x001E8167 File Offset: 0x001E6567
		public Trigger_PawnsLost(int count)
		{
			this.count = count;
		}

		// Token: 0x060039B0 RID: 14768 RVA: 0x001E8180 File Offset: 0x001E6580
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && lord.numPawnsLostViolently >= this.count;
		}

		// Token: 0x040024AF RID: 9391
		private int count = 1;
	}
}
