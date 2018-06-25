using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A21 RID: 2593
	public class Trigger_PawnLostViolently : Trigger
	{
		// Token: 0x060039C2 RID: 14786 RVA: 0x001E8A48 File Offset: 0x001E6E48
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && (signal.condition == PawnLostCondition.MadePrisoner || signal.condition == PawnLostCondition.IncappedOrKilled);
		}
	}
}
