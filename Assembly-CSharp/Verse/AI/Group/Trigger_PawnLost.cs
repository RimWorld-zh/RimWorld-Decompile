using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A22 RID: 2594
	public class Trigger_PawnLost : Trigger
	{
		// Token: 0x060039C2 RID: 14786 RVA: 0x001E86B4 File Offset: 0x001E6AB4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost;
		}
	}
}
