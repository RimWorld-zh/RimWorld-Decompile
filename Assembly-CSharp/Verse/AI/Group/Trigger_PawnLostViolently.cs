using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A23 RID: 2595
	public class Trigger_PawnLostViolently : Trigger
	{
		// Token: 0x060039C2 RID: 14786 RVA: 0x001E8608 File Offset: 0x001E6A08
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && (signal.condition == PawnLostCondition.MadePrisoner || signal.condition == PawnLostCondition.IncappedOrKilled);
		}
	}
}
