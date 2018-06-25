using System;

namespace Verse.AI.Group
{
	// Token: 0x02000A2A RID: 2602
	public class Trigger_ThingDamageTaken : Trigger
	{
		// Token: 0x040024B6 RID: 9398
		private Thing thing;

		// Token: 0x040024B7 RID: 9399
		private float damageFraction = 0.5f;

		// Token: 0x060039D3 RID: 14803 RVA: 0x001E8DE7 File Offset: 0x001E71E7
		public Trigger_ThingDamageTaken(Thing thing, float damageFraction)
		{
			this.thing = thing;
			this.damageFraction = damageFraction;
		}

		// Token: 0x060039D4 RID: 14804 RVA: 0x001E8E0C File Offset: 0x001E720C
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.Tick && (this.thing.DestroyedOrNull() || (float)this.thing.HitPoints < (1f - this.damageFraction) * (float)this.thing.MaxHitPoints);
		}
	}
}
