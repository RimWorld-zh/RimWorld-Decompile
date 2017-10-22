using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_HaulCorpseToPublicPlace : ThinkNode_JobGiver
	{
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
					if (!pawn.CanReserveAndReach((Thing)building_Grave, PathEndMode.InteractionCell, Danger.Deadly, 1, -1, null, false))
					{
						result = null;
						goto IL_00ae;
					}
				}
				else if (!pawn.CanReserveAndReach((Thing)corpse, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
				{
					result = null;
					goto IL_00ae;
				}
				Job job = new Job(JobDefOf.HaulCorpseToPublicPlace, (Thing)corpse, (Thing)building_Grave);
				job.count = 1;
				result = job;
			}
			goto IL_00ae;
			IL_00ae:
			return result;
		}
	}
}
