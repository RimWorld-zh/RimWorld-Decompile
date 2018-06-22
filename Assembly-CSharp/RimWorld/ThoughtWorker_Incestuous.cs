using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020001FB RID: 507
	public class ThoughtWorker_Incestuous : ThoughtWorker
	{
		// Token: 0x060009C2 RID: 2498 RVA: 0x00057DC8 File Offset: 0x000561C8
		protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
		{
			ThoughtState result;
			if (!other.RaceProps.Humanlike)
			{
				result = false;
			}
			else if (!RelationsUtility.PawnsKnowEachOther(pawn, other))
			{
				result = false;
			}
			else if (LovePartnerRelationUtility.IncestOpinionOffsetFor(other, pawn) == 0f)
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
