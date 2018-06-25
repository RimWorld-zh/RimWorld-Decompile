using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A18 RID: 2584
	public class Trigger_PawnsLost : Trigger
	{
		// Token: 0x040024AB RID: 9387
		private int count = 1;

		// Token: 0x060039AD RID: 14765 RVA: 0x001E84D3 File Offset: 0x001E68D3
		public Trigger_PawnsLost(int count)
		{
			this.count = count;
		}

		// Token: 0x060039AE RID: 14766 RVA: 0x001E84EC File Offset: 0x001E68EC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && lord.numPawnsLostViolently >= this.count;
		}
	}
}
