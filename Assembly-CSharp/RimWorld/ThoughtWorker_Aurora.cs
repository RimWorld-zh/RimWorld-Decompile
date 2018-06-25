using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200021A RID: 538
	public class ThoughtWorker_Aurora : ThoughtWorker_GameCondition
	{
		// Token: 0x060009FF RID: 2559 RVA: 0x000591D8 File Offset: 0x000575D8
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return base.CurrentStateInternal(p).Active && p.SpawnedOrAnyParentSpawned && !p.PositionHeld.Roofed(p.MapHeld) && p.health.capacities.CapableOf(PawnCapacityDefOf.Sight);
		}
	}
}
