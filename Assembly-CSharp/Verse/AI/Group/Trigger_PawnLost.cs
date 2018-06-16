using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A22 RID: 2594
	public class Trigger_PawnLost : Trigger
	{
		// Token: 0x060039C0 RID: 14784 RVA: 0x001E85E0 File Offset: 0x001E69E0
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost;
		}
	}
}
