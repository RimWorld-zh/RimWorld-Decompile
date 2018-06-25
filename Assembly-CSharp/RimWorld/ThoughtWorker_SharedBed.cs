using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200021E RID: 542
	public class ThoughtWorker_SharedBed : ThoughtWorker
	{
		// Token: 0x06000A07 RID: 2567 RVA: 0x00059374 File Offset: 0x00057774
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			ThoughtState result;
			if (!p.RaceProps.Humanlike)
			{
				result = false;
			}
			else
			{
				result = (LovePartnerRelationUtility.GetMostDislikedNonPartnerBedOwner(p) != null);
			}
			return result;
		}
	}
}
