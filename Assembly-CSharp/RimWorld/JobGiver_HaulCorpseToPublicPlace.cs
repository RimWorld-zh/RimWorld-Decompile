using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200010D RID: 269
	public class JobGiver_HaulCorpseToPublicPlace : ThinkNode_JobGiver
	{
		// Token: 0x06000592 RID: 1426 RVA: 0x0003C474 File Offset: 0x0003A874
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_CorpseObsession mentalState_CorpseObsession = pawn.MentalState as MentalState_CorpseObsession;
			Job result;
			if (mentalState_CorpseObsession == null || mentalState_CorpseObsession.corpse == null)
			{
				result = null;
			}
			else
			{
				Corpse corpse = mentalState_CorpseObsession.corpse;
				Building_Grave building_Grave = mentalState_CorpseObsession.corpse.ParentHolder as Building_Grave;
				if (building_Grave != null)
				{
					if (!pawn.CanReserveAndReach(building_Grave, PathEndMode.InteractionCell, Danger.Deadly, 1, -1, null, false))
					{
						return null;
					}
				}
				else if (!pawn.CanReserveAndReach(corpse, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
				{
					return null;
				}
				result = new Job(JobDefOf.HaulCorpseToPublicPlace, corpse, building_Grave)
				{
					count = 1
				};
			}
			return result;
		}
	}
}
