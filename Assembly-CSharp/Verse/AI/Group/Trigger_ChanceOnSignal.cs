using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A17 RID: 2583
	public class Trigger_ChanceOnSignal : Trigger
	{
		// Token: 0x060039AB RID: 14763 RVA: 0x001E83FA File Offset: 0x001E67FA
		public Trigger_ChanceOnSignal(TriggerSignalType signalType, float chance)
		{
			this.signalType = signalType;
			this.chance = chance;
		}

		// Token: 0x060039AC RID: 14764 RVA: 0x001E8414 File Offset: 0x001E6814
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == this.signalType && Rand.Value < this.chance;
		}

		// Token: 0x040024AB RID: 9387
		private TriggerSignalType signalType;

		// Token: 0x040024AC RID: 9388
		private float chance;
	}
}
