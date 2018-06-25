using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A19 RID: 2585
	public class Trigger_PawnsLost : Trigger
	{
		// Token: 0x040024BB RID: 9403
		private int count = 1;

		// Token: 0x060039AE RID: 14766 RVA: 0x001E87FF File Offset: 0x001E6BFF
		public Trigger_PawnsLost(int count)
		{
			this.count = count;
		}

		// Token: 0x060039AF RID: 14767 RVA: 0x001E8818 File Offset: 0x001E6C18
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && lord.numPawnsLostViolently >= this.count;
		}
	}
}
