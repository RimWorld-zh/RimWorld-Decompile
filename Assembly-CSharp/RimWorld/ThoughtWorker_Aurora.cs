using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200021A RID: 538
	public class ThoughtWorker_Aurora : ThoughtWorker_GameCondition
	{
		// Token: 0x06000A00 RID: 2560 RVA: 0x000591DC File Offset: 0x000575DC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return base.CurrentStateInternal(p).Active && p.SpawnedOrAnyParentSpawned && !p.PositionHeld.Roofed(p.MapHeld) && p.health.capacities.CapableOf(PawnCapacityDefOf.Sight);
		}
	}
}
