using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A1B RID: 2587
	public class Trigger_ChanceOnPlayerHarmNPCBuilding : Trigger
	{
		// Token: 0x060039B5 RID: 14773 RVA: 0x001E86CF File Offset: 0x001E6ACF
		public Trigger_ChanceOnPlayerHarmNPCBuilding(float chance)
		{
			this.chance = chance;
		}

		// Token: 0x060039B6 RID: 14774 RVA: 0x001E86EC File Offset: 0x001E6AEC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.BuildingDamaged && signal.dinfo.Def.externalViolence && signal.thing.def.category == ThingCategory.Building && signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction == Faction.OfPlayer && signal.thing.Faction != Faction.OfPlayer && Rand.Value < this.chance;
		}

		// Token: 0x040024B2 RID: 9394
		private float chance = 1f;
	}
}
