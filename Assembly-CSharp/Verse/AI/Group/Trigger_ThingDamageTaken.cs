using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A28 RID: 2600
	public class Trigger_ThingDamageTaken : Trigger
	{
		// Token: 0x060039CF RID: 14799 RVA: 0x001E8CBB File Offset: 0x001E70BB
		public Trigger_ThingDamageTaken(Thing thing, float damageFraction)
		{
			this.thing = thing;
			this.damageFraction = damageFraction;
		}

		// Token: 0x060039D0 RID: 14800 RVA: 0x001E8CE0 File Offset: 0x001E70E0
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && (this.thing.DestroyedOrNull() || (float)this.thing.HitPoints < (1f - this.damageFraction) * (float)this.thing.MaxHitPoints);
		}

		// Token: 0x040024B5 RID: 9397
		private Thing thing;

		// Token: 0x040024B6 RID: 9398
		private float damageFraction = 0.5f;
	}
}
