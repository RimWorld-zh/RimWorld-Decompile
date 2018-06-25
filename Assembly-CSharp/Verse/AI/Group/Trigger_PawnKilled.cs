using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A23 RID: 2595
	public class Trigger_PawnKilled : Trigger
	{
		// Token: 0x060039C5 RID: 14789 RVA: 0x001E8DC0 File Offset: 0x001E71C0
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && signal.condition == PawnLostCondition.IncappedOrKilled && signal.Pawn.Dead;
		}
	}
}
