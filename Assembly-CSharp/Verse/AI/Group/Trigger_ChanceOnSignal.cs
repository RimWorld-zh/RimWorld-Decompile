using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A1B RID: 2587
	public class Trigger_ChanceOnSignal : Trigger
	{
		// Token: 0x060039B1 RID: 14769 RVA: 0x001E81BA File Offset: 0x001E65BA
		public Trigger_ChanceOnSignal(TriggerSignalType signalType, float chance)
		{
			this.signalType = signalType;
			this.chance = chance;
		}

		// Token: 0x060039B2 RID: 14770 RVA: 0x001E81D4 File Offset: 0x001E65D4
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == this.signalType && Rand.Value < this.chance;
		}

		// Token: 0x040024B0 RID: 9392
		private TriggerSignalType signalType;

		// Token: 0x040024B1 RID: 9393
		private float chance;
	}
}
