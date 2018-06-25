using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A22 RID: 2594
	public class Trigger_PawnLostViolently : Trigger
	{
		// Token: 0x060039C3 RID: 14787 RVA: 0x001E8D74 File Offset: 0x001E7174
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && (signal.condition == PawnLostCondition.MadePrisoner || signal.condition == PawnLostCondition.IncappedOrKilled);
		}
	}
}
