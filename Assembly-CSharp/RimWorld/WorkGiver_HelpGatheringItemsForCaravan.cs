using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	public class WorkGiver_HelpGatheringItemsForCaravan : WorkGiver
	{
		public override Job NonScanJob(Pawn pawn)
		{
			List<Lord> lords = pawn.Map.lordManager.lords;
			int num = 0;
			Job result;
			while (true)
			{
				if (num < lords.Count)
				{
					LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = lords[num].LordJob as LordJob_FormAndSendCaravan;
					if (lordJob_FormAndSendCaravan != null && lordJob_FormAndSendCaravan.GatheringItemsNow)
					{
						Thing thing = GatherItemsForCaravanUtility.FindThingToHaul(pawn, lords[num]);
						if (thing != null && this.AnyReachableCarrierOrColonist(pawn, lords[num]))
						{
							Job job = new Job(JobDefOf.PrepareCaravan_GatherItems, thing);
							job.lord = lords[num];
							result = job;
							break;
						}
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		private bool AnyReachableCarrierOrColonist(Pawn forPawn, Lord lord)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < lord.ownedPawns.Count)
				{
					if (JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(lord.ownedPawns[num], forPawn, false) && !lord.ownedPawns[num].IsForbidden(forPawn) && forPawn.CanReach((Thing)lord.ownedPawns[num], PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}
	}
}
