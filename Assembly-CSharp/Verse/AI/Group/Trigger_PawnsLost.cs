using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A1A RID: 2586
	public class Trigger_PawnsLost : Trigger
	{
		// Token: 0x060039AD RID: 14765 RVA: 0x001E8093 File Offset: 0x001E6493
		public Trigger_PawnsLost(int count)
		{
			this.count = count;
		}

		// Token: 0x060039AE RID: 14766 RVA: 0x001E80AC File Offset: 0x001E64AC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && lord.numPawnsLostViolently >= this.count;
		}

		// Token: 0x040024AF RID: 9391
		private int count = 1;
	}
}
