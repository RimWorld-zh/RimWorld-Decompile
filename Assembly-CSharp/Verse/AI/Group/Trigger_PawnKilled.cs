using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A20 RID: 2592
	public class Trigger_PawnKilled : Trigger
	{
		// Token: 0x060039C0 RID: 14784 RVA: 0x001E8968 File Offset: 0x001E6D68
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && signal.condition == PawnLostCondition.IncappedOrKilled && signal.Pawn.Dead;
		}
	}
}
