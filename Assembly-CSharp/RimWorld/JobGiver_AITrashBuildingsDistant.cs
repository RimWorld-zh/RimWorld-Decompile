using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_AITrashBuildingsDistant : ThinkNode_JobGiver
	{
		public bool attackAllInert;

		public override ThinkNode DeepCopy(bool resolve = true)
		{
			JobGiver_AITrashBuildingsDistant jobGiver_AITrashBuildingsDistant = (JobGiver_AITrashBuildingsDistant)base.DeepCopy(resolve);
			jobGiver_AITrashBuildingsDistant.attackAllInert = this.attackAllInert;
			return jobGiver_AITrashBuildingsDistant;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			List<Building> allBuildingsColonist = pawn.Map.listerBuildings.allBuildingsColonist;
			Job result;
			Building building;
			if (allBuildingsColonist.Count == 0)
			{
				result = null;
			}
			else
			{
				for (int i = 0; i < 75; i++)
				{
					building = allBuildingsColonist.RandomElement();
					if (TrashUtility.ShouldTrashBuilding(pawn, building, this.attackAllInert))
						goto IL_004e;
				}
				result = null;
			}
			goto IL_006f;
			IL_004e:
			result = TrashUtility.TrashJob(pawn, building);
			goto IL_006f;
			IL_006f:
			return result;
		}
	}
}
