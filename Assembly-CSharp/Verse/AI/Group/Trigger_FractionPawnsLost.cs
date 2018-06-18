using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A19 RID: 2585
	public class Trigger_FractionPawnsLost : Trigger
	{
		// Token: 0x060039AD RID: 14765 RVA: 0x001E8107 File Offset: 0x001E6507
		public Trigger_FractionPawnsLost(float fraction)
		{
			this.fraction = fraction;
		}

		// Token: 0x060039AE RID: 14766 RVA: 0x001E8124 File Offset: 0x001E6524
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && (float)lord.numPawnsLostViolently >= (float)lord.numPawnsEverGained * this.fraction;
		}

		// Token: 0x040024AE RID: 9390
		private float fraction = 0.5f;
	}
}
