using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200014A RID: 330
	public class WorkGiver_HaulCorpses : WorkGiver_Haul
	{
		// Token: 0x060006D4 RID: 1748 RVA: 0x000461AC File Offset: 0x000445AC
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job result;
			if (!(t is Corpse))
			{
				result = null;
			}
			else
			{
				result = base.JobOnThing(pawn, t, forced);
			}
			return result;
		}
	}
}
