using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020001FD RID: 509
	public class ThoughtWorker_Disfigured : ThoughtWorker
	{
		// Token: 0x060009C8 RID: 2504 RVA: 0x00057F1C File Offset: 0x0005631C
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
