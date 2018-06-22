using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000113 RID: 275
	public class JobGiver_SlaughterRandomAnimal : ThinkNode_JobGiver
	{
		// Token: 0x060005A3 RID: 1443 RVA: 0x0003C9E8 File Offset: 0x0003ADE8
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_Slaughterer mentalState_Slaughterer = pawn.MentalState as MentalState_Slaughterer;
			Job result;
			if (mentalState_Slaughterer != null && mentalState_Slaughterer.SlaughteredRecently)
			{
				result = null;
			}
			else
			{
				Pawn pawn2 = SlaughtererMentalStateUtility.FindAnimal(pawn);
				if (pawn2 == null || !pawn.CanReserveAndReach(pawn2, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
				{
					result = null;
				}
				else
				{
					result = new Job(JobDefOf.Slaughter, pawn2)
					{
						ignoreDesignations = true
					};
				}
			}
			return result;
		}
	}
}
