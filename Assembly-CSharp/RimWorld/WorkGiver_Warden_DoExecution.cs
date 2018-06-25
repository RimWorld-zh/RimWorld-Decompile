using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000135 RID: 309
	public class WorkGiver_Warden_DoExecution : WorkGiver_Warden
	{
		// Token: 0x04000313 RID: 787
		private static string IncapableOfViolenceLowerTrans;

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x00042634 File Offset: 0x00040A34
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x0004264A File Offset: 0x00040A4A
		public static void ResetStaticData()
		{
			WorkGiver_Warden_DoExecution.IncapableOfViolenceLowerTrans = "IncapableOfViolenceLower".Translate();
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0004265C File Offset: 0x00040A5C
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
				if (pawn2.guest.interactionMode != PrisonerInteractionModeDefOf.Execution || !pawn.CanReserve(t, 1, -1, null, false))
				{
					result = null;
				}
				else if (pawn.story != null && pawn.story.WorkTagIsDisabled(WorkTags.Violent))
				{
					JobFailReason.Is(WorkGiver_Warden_DoExecution.IncapableOfViolenceLowerTrans, null);
					result = null;
				}
				else
				{
					result = new Job(JobDefOf.PrisonerExecution, t);
				}
			}
			return result;
		}
	}
}
