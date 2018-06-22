using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A16 RID: 2582
	public class Trigger_PawnsLost : Trigger
	{
		// Token: 0x060039A9 RID: 14761 RVA: 0x001E83A7 File Offset: 0x001E67A7
		public Trigger_PawnsLost(int count)
		{
			this.count = count;
		}

		// Token: 0x060039AA RID: 14762 RVA: 0x001E83C0 File Offset: 0x001E67C0
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && lord.numPawnsLostViolently >= this.count;
		}

		// Token: 0x040024AA RID: 9386
		private int count = 1;
	}
}
