using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A2B RID: 2603
	public class Trigger_ThingDamageTaken : Trigger
	{
		// Token: 0x040024C6 RID: 9414
		private Thing thing;

		// Token: 0x040024C7 RID: 9415
		private float damageFraction = 0.5f;

		// Token: 0x060039D4 RID: 14804 RVA: 0x001E9113 File Offset: 0x001E7513
		public Trigger_ThingDamageTaken(Thing thing, float damageFraction)
		{
			this.thing = thing;
			this.damageFraction = damageFraction;
		}

		// Token: 0x060039D5 RID: 14805 RVA: 0x001E9138 File Offset: 0x001E7538
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && (this.thing.DestroyedOrNull() || (float)this.thing.HitPoints < (1f - this.damageFraction) * (float)this.thing.MaxHitPoints);
		}
	}
}
