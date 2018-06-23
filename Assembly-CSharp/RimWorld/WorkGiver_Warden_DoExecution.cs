using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000135 RID: 309
	public class WorkGiver_Warden_DoExecution : WorkGiver_Warden
	{
		// Token: 0x04000312 RID: 786
		private static string IncapableOfViolenceLowerTrans;

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x00042638 File Offset: 0x00040A38
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.OnCell;
			}
		}

		// Token: 0x06000659 RID: 1625 RVA: 0x0004264E File Offset: 0x00040A4E
		public static void ResetStaticData()
		{
			WorkGiver_Warden_DoExecution.IncapableOfViolenceLowerTrans = "IncapableOfViolenceLower".Translate();
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x00042660 File Offset: 0x00040A60
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
