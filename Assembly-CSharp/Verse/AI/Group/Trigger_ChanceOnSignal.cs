using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A1A RID: 2586
	public class Trigger_ChanceOnSignal : Trigger
	{
		// Token: 0x040024BC RID: 9404
		private TriggerSignalType signalType;

		// Token: 0x040024BD RID: 9405
		private float chance;

		// Token: 0x060039B0 RID: 14768 RVA: 0x001E8852 File Offset: 0x001E6C52
		public Trigger_ChanceOnSignal(TriggerSignalType signalType, float chance)
		{
			this.signalType = signalType;
			this.chance = chance;
		}

		// Token: 0x060039B1 RID: 14769 RVA: 0x001E886C File Offset: 0x001E6C6C
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == this.signalType && Rand.Value < this.chance;
		}
	}
}
