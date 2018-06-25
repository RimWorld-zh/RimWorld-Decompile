using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A1D RID: 2589
	public class Trigger_ChanceOnPlayerHarmNPCBuilding : Trigger
	{
		// Token: 0x040024B3 RID: 9395
		private float chance = 1f;

		// Token: 0x060039B9 RID: 14777 RVA: 0x001E87FB File Offset: 0x001E6BFB
		public Trigger_ChanceOnPlayerHarmNPCBuilding(float chance)
		{
			this.chance = chance;
		}

		// Token: 0x060039BA RID: 14778 RVA: 0x001E8818 File Offset: 0x001E6C18
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.BuildingDamaged && signal.dinfo.Def.externalViolence && signal.thing.def.category == ThingCategory.Building && signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction == Faction.OfPlayer && signal.thing.Faction != Faction.OfPlayer && Rand.Value < this.chance;
		}
	}
}
