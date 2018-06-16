using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A24 RID: 2596
	public class Trigger_PawnKilled : Trigger
	{
		// Token: 0x060039C4 RID: 14788 RVA: 0x001E8654 File Offset: 0x001E6A54
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && signal.condition == PawnLostCondition.IncappedOrKilled && signal.Pawn.Dead;
		}
	}
}
