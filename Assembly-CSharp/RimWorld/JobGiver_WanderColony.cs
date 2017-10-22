using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_WanderColony : JobGiver_Wander
	{
		private static List<IntVec3> gatherSpots = new List<IntVec3>();

		public JobGiver_WanderColony()
		{
			base.wanderRadius = 7f;
			base.ticksBetweenWandersRange = new IntRange(125, 200);
			base.wanderDestValidator = (Func<Pawn, IntVec3, bool>)((Pawn pawn, IntVec3 loc) => true);
		}

		protected override IntVec3 GetWanderRoot(Pawn pawn)
		{
			if (pawn.RaceProps.Humanlike)
			{
				JobGiver_WanderColony.gatherSpots.Clear();
				for (int i = 0; i < pawn.Map.gatherSpotLister.activeSpots.Count; i++)
				{
					IntVec3 position = pawn.Map.gatherSpotLister.activeSpots[i].parent.Position;
					if (!position.IsForbidden(pawn) && pawn.CanReach(position, PathEndMode.Touch, Danger.None, false, TraverseMode.ByPawn))
					{
						JobGiver_WanderColony.gatherSpots.Add(position);
					}
				}
				if (JobGiver_WanderColony.gatherSpots.Count > 0)
				{
					return JobGiver_WanderColony.gatherSpots.RandomElement();
				}
			}
			List<Building> allBuildingsColonist = pawn.Map.listerBuildings.allBuildingsColonist;
			if (allBuildingsColonist.Count == 0)
			{
				Pawn pawn2 = default(Pawn);
				if ((from c in pawn.Map.mapPawns.FreeColonistsSpawned
				where !c.Position.IsForbidden(pawn) && pawn.CanReach(c.Position, PathEndMode.Touch, Danger.None, false, TraverseMode.ByPawn)
				select c).TryRandomElement<Pawn>(out pawn2))
				{
					return pawn2.Position;
				}
				return pawn.Position;
			}
			int num = 0;
			goto IL_0142;
			IL_0142:
			IntVec3 intVec;
			while (true)
			{
				num++;
				if (num > 20)
				{
					return pawn.Position;
				}
				Building building = allBuildingsColonist.RandomElement();
				if (!building.Position.IsForbidden(pawn) && ((Area)pawn.Map.areaManager.Home)[building.Position])
				{
					int num2 = 15 + num * 2;
					if ((pawn.Position - building.Position).LengthHorizontalSquared <= num2 * num2)
					{
						intVec = GenAdjFast.AdjacentCells8Way((Thing)building).RandomElement();
						if (intVec.Standable(building.Map) && !intVec.IsForbidden(pawn) && pawn.CanReach(intVec, PathEndMode.OnCell, Danger.None, false, TraverseMode.ByPawn) && !intVec.IsInPrisonCell(pawn.Map))
							break;
					}
				}
			}
			return intVec;
			IL_0258:
			goto IL_0142;
		}
	}
}
