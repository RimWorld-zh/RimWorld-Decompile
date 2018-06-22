using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A15 RID: 2581
	public class Trigger_FractionPawnsLost : Trigger
	{
		// Token: 0x060039A7 RID: 14759 RVA: 0x001E8347 File Offset: 0x001E6747
		public Trigger_FractionPawnsLost(float fraction)
		{
			this.fraction = fraction;
		}

		// Token: 0x060039A8 RID: 14760 RVA: 0x001E8364 File Offset: 0x001E6764
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && (float)lord.numPawnsLostViolently >= (float)lord.numPawnsEverGained * this.fraction;
		}

		// Token: 0x040024A9 RID: 9385
		private float fraction = 0.5f;
	}
}
