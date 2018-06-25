using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A20 RID: 2592
	public class Trigger_PawnLost : Trigger
	{
		// Token: 0x060039C0 RID: 14784 RVA: 0x001E8A20 File Offset: 0x001E6E20
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost;
		}
	}
}
