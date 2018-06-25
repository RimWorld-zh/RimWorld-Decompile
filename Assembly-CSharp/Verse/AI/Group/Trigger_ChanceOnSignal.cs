using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A19 RID: 2585
	public class Trigger_ChanceOnSignal : Trigger
	{
		// Token: 0x040024AC RID: 9388
		private TriggerSignalType signalType;

		// Token: 0x040024AD RID: 9389
		private float chance;

		// Token: 0x060039AF RID: 14767 RVA: 0x001E8526 File Offset: 0x001E6926
		public Trigger_ChanceOnSignal(TriggerSignalType signalType, float chance)
		{
			this.signalType = signalType;
			this.chance = chance;
		}

		// Token: 0x060039B0 RID: 14768 RVA: 0x001E8540 File Offset: 0x001E6940
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == this.signalType && Rand.Value < this.chance;
		}
	}
}
