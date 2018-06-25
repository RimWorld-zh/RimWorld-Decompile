using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A1E RID: 2590
	public class Trigger_ChanceOnPlayerHarmNPCBuilding : Trigger
	{
		// Token: 0x040024C3 RID: 9411
		private float chance = 1f;

		// Token: 0x060039BA RID: 14778 RVA: 0x001E8B27 File Offset: 0x001E6F27
		public Trigger_ChanceOnPlayerHarmNPCBuilding(float chance)
		{
			this.chance = chance;
		}

		// Token: 0x060039BB RID: 14779 RVA: 0x001E8B44 File Offset: 0x001E6F44
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.BuildingDamaged && signal.dinfo.Def.externalViolence && signal.thing.def.category == ThingCategory.Building && signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction == Faction.OfPlayer && signal.thing.Faction != Faction.OfPlayer && Rand.Value < this.chance;
		}
	}
}
