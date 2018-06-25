using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000209 RID: 521
	public class ThoughtWorker_CreepyBreathing : ThoughtWorker
	{
		// Token: 0x060009DD RID: 2525 RVA: 0x00058734 File Offset: 0x00056B34
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			ThoughtState result;
			if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				result = false;
			}
			else if (!other.story.traits.HasTrait(TraitDefOf.CreepyBreathing))
			{
				result = false;
			}
			else if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Hearing))
			{
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}
	}
}
