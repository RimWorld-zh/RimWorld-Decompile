using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000138 RID: 312
	public class WorkGiver_Warden_ReleasePrisoner : WorkGiver_Warden
	{
		// Token: 0x0600065E RID: 1630 RVA: 0x0004285C File Offset: 0x00040C5C
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Job result;
			if (!base.ShouldTakeCareOfPrisoner(pawn, t))
			{
				result = null;
			}
			else
			{
				Pawn pawn2 = (Pawn)t;
				if (pawn2.guest.interactionMode == PrisonerInteractionModeDefOf.Release && !pawn2.Downed && pawn2.Awake())
				{
					IntVec3 c;
					if (!RCellFinder.TryFindPrisonerReleaseCell(pawn2, pawn, out c))
					{
						result = null;
					}
					else
					{
						result = new Job(JobDefOf.ReleasePrisoner, pawn2, c)
						{
							count = 1
						};
					}
				}
				else
				{
					result = null;
				}
			}
			return result;
		}
	}
}
