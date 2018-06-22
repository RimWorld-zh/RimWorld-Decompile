using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000218 RID: 536
	public class ThoughtWorker_Aurora : ThoughtWorker_GameCondition
	{
		// Token: 0x060009FC RID: 2556 RVA: 0x0005905C File Offset: 0x0005745C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return base.CurrentStateInternal(p).Active && p.SpawnedOrAnyParentSpawned && !p.PositionHeld.Roofed(p.MapHeld) && p.health.capacities.CapableOf(PawnCapacityDefOf.Sight);
		}
	}
}
