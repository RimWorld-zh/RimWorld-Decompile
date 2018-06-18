using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A23 RID: 2595
	public class Trigger_PawnLostViolently : Trigger
	{
		// Token: 0x060039C4 RID: 14788 RVA: 0x001E86DC File Offset: 0x001E6ADC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && (signal.condition == PawnLostCondition.MadePrisoner || signal.condition == PawnLostCondition.IncappedOrKilled);
		}
	}
}
