using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020001FB RID: 507
	public class ThoughtWorker_Incestuous : ThoughtWorker
	{
		// Token: 0x060009C4 RID: 2500 RVA: 0x00057D84 File Offset: 0x00056184
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
