using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A21 RID: 2593
	public class Trigger_PawnLost : Trigger
	{
		// Token: 0x060039C1 RID: 14785 RVA: 0x001E8D4C File Offset: 0x001E714C
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost;
		}
	}
}
