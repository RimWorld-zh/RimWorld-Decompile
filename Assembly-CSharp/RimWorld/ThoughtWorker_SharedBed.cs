using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200021C RID: 540
	public class ThoughtWorker_SharedBed : ThoughtWorker
	{
		// Token: 0x06000A06 RID: 2566 RVA: 0x000591B4 File Offset: 0x000575B4
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
