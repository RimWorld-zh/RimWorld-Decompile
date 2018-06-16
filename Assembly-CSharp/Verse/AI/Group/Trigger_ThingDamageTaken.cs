using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A2C RID: 2604
	public class Trigger_ThingDamageTaken : Trigger
	{
		// Token: 0x060039D3 RID: 14803 RVA: 0x001E89A7 File Offset: 0x001E6DA7
		public Trigger_ThingDamageTaken(Thing thing, float damageFraction)
		{
			this.thing = thing;
			this.damageFraction = damageFraction;
		}

		// Token: 0x060039D4 RID: 14804 RVA: 0x001E89CC File Offset: 0x001E6DCC
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
