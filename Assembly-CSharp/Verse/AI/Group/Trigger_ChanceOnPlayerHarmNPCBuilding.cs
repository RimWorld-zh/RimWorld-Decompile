using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000A1F RID: 2591
	public class Trigger_ChanceOnPlayerHarmNPCBuilding : Trigger
	{
		// Token: 0x060039BB RID: 14779 RVA: 0x001E848F File Offset: 0x001E688F
		public Trigger_ChanceOnPlayerHarmNPCBuilding(float chance)
		{
			this.chance = chance;
		}

		// Token: 0x060039BC RID: 14780 RVA: 0x001E84AC File Offset: 0x001E68AC
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			return signal.type == TriggerSignalType.BuildingDamaged && signal.dinfo.Def.externalViolence && signal.thing.def.category == ThingCategory.Building && signal.dinfo.Instigator != null && signal.dinfo.Instigator.Faction == Faction.OfPlayer && signal.thing.Faction != Faction.OfPlayer && Rand.Value < this.chance;
		}

		// Token: 0x040024B7 RID: 9399
		private float chance = 1f;
	}
}
