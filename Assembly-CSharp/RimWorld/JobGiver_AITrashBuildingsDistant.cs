using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_AITrashBuildingsDistant : ThinkNode_JobGiver
	{
		protected override Job TryGiveJob(Pawn pawn)
		{
			List<Building> allBuildingsColonist = pawn.Map.listerBuildings.allBuildingsColonist;
			int count = allBuildingsColonist.Count;
			Job result;
			Building building;
			if (count == 0)
			{
				result = null;
			}
			else
			{
				for (int i = 0; i < 75; i++)
				{
					int index = Rand.Range(0, count);
					building = allBuildingsColonist[index];
					if (TrashUtility.ShouldTrashBuilding(pawn, building))
						goto IL_0053;
				}
				result = null;
			}
			goto IL_0074;
			IL_0053:
			result = TrashUtility.TrashJob(pawn, building);
			goto IL_0074;
			IL_0074:
			return result;
		}
	}
}
