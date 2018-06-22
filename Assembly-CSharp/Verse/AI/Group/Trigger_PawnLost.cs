using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A1E RID: 2590
	public class Trigger_PawnLost : Trigger
	{
		// Token: 0x060039BC RID: 14780 RVA: 0x001E88F4 File Offset: 0x001E6CF4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost;
		}
	}
}
