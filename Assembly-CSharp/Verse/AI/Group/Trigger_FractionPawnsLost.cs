using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A18 RID: 2584
	public class Trigger_FractionPawnsLost : Trigger
	{
		// Token: 0x040024BA RID: 9402
		private float fraction = 0.5f;

		// Token: 0x060039AC RID: 14764 RVA: 0x001E879F File Offset: 0x001E6B9F
		public Trigger_FractionPawnsLost(float fraction)
		{
			this.fraction = fraction;
		}

		// Token: 0x060039AD RID: 14765 RVA: 0x001E87BC File Offset: 0x001E6BBC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && (float)lord.numPawnsLostViolently >= (float)lord.numPawnsEverGained * this.fraction;
		}
	}
}
