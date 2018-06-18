using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A2C RID: 2604
	public class Trigger_ThingDamageTaken : Trigger
	{
		// Token: 0x060039D5 RID: 14805 RVA: 0x001E8A7B File Offset: 0x001E6E7B
		public Trigger_ThingDamageTaken(Thing thing, float damageFraction)
		{
			this.thing = thing;
			this.damageFraction = damageFraction;
		}

		// Token: 0x060039D6 RID: 14806 RVA: 0x001E8AA0 File Offset: 0x001E6EA0
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && (this.thing.DestroyedOrNull() || (float)this.thing.HitPoints < (1f - this.damageFraction) * (float)this.thing.MaxHitPoints);
		}

		// Token: 0x040024BA RID: 9402
		private Thing thing;

		// Token: 0x040024BB RID: 9403
		private float damageFraction = 0.5f;
	}
}
