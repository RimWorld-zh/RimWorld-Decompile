using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020001FD RID: 509
	public class ThoughtWorker_Disfigured : ThoughtWorker
	{
		// Token: 0x060009C5 RID: 2501 RVA: 0x00057F5C File Offset: 0x0005635C
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			ThoughtState result;
			if (!other.RaceProps.Humanlike || other.Dead)
			{
				result = false;
			}
			else if (!RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				result = false;
			}
			else if (!RelationsUtility.IsDisfigured(other))
			{
				result = false;
			}
			else if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
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
