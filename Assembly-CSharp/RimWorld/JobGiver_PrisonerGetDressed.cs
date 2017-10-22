using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_PrisonerGetDressed : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.guest.PrisonerIsSecure && pawn.apparel != null)
			{
				if (!pawn.apparel.BodyPartGroupIsCovered(BodyPartGroupDefOf.Legs))
				{
					Apparel apparel = this.FindGarmentCoveringPart(pawn, BodyPartGroupDefOf.Legs);
					if (apparel != null)
					{
						Job job = new Job(JobDefOf.Wear, (Thing)apparel);
						job.ignoreForbidden = true;
						result = job;
						goto IL_00bc;
					}
				}
				if (!pawn.apparel.BodyPartGroupIsCovered(BodyPartGroupDefOf.Torso))
				{
					Apparel apparel2 = this.FindGarmentCoveringPart(pawn, BodyPartGroupDefOf.Torso);
					if (apparel2 != null)
					{
						Job job2 = new Job(JobDefOf.Wear, (Thing)apparel2);
						job2.ignoreForbidden = true;
						result = job2;
						goto IL_00bc;
					}
				}
			}
			result = null;
			goto IL_00bc;
			IL_00bc:
			return result;
		}

		private Apparel FindGarmentCoveringPart(Pawn pawn, BodyPartGroupDef bodyPartGroupDef)
		{
			Room room = pawn.GetRoom(RegionType.Set_Passable);
			if (room.isPrisonCell)
			{
				foreach (IntVec3 cell in room.Cells)
				{
					List<Thing> thingList = cell.GetThingList(pawn.Map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Apparel apparel = thingList[i] as Apparel;
						if (apparel != null && apparel.def.apparel.bodyPartGroups.Contains(bodyPartGroupDef) && pawn.CanReserve((Thing)apparel, 1, -1, null, false) && ApparelUtility.HasPartsToWear(pawn, apparel.def))
						{
							return apparel;
						}
					}
				}
			}
			return null;
		}
	}
}
