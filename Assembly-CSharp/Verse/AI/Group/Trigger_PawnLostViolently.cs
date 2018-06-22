using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A1F RID: 2591
	public class Trigger_PawnLostViolently : Trigger
	{
		// Token: 0x060039BE RID: 14782 RVA: 0x001E891C File Offset: 0x001E6D1C
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && (signal.condition == PawnLostCondition.MadePrisoner || signal.condition == PawnLostCondition.IncappedOrKilled);
		}
	}
}
