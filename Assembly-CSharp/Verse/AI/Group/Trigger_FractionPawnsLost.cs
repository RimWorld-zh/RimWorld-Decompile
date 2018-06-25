using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A17 RID: 2583
	public class Trigger_FractionPawnsLost : Trigger
	{
		// Token: 0x040024AA RID: 9386
		private float fraction = 0.5f;

		// Token: 0x060039AB RID: 14763 RVA: 0x001E8473 File Offset: 0x001E6873
		public Trigger_FractionPawnsLost(float fraction)
		{
			this.fraction = fraction;
		}

		// Token: 0x060039AC RID: 14764 RVA: 0x001E8490 File Offset: 0x001E6890
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.PawnLost && (float)lord.numPawnsLostViolently >= (float)lord.numPawnsEverGained * this.fraction;
		}
	}
}
