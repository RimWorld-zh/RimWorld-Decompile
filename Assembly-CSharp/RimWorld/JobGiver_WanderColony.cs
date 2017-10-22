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
			IntVec3 result;
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
					result = JobGiver_WanderColony.gatherSpots.RandomElement();
					goto IL_0272;
				}
			}
			List<Building> allBuildingsColonist = pawn.Map.listerBuildings.allBuildingsColonist;
			if (allBuildingsColonist.Count == 0)
			{
				Pawn pawn2 = default(Pawn);
				result = ((!(from c in pawn.Map.mapPawns.FreeColonistsSpawned
				where !c.Position.IsForbidden(pawn) && pawn.CanReach(c.Position, PathEndMode.Touch, Danger.None, false, TraverseMode.ByPawn)
				select c).TryRandomElement<Pawn>(out pawn2)) ? pawn.Position : pawn2.Position);
				goto IL_0272;
			}
			int num = 0;
			goto IL_0150;
			IL_0272:
			return result;
			IL_026d:
			goto IL_0150;
			IL_0150:
			IntVec3 intVec;
			while (true)
			{
				num++;
				if (num <= 20)
				{
					Building building = allBuildingsColonist.RandomElement();
					if (building.Position.IsForbidden(pawn))
						continue;
					if (!((Area)pawn.Map.areaManager.Home)[building.Position])
						continue;
					int num2 = 15 + num * 2;
					if ((pawn.Position - building.Position).LengthHorizontalSquared > num2 * num2)
						continue;
					intVec = GenAdjFast.AdjacentCells8Way((Thing)building).RandomElement();
					if (!intVec.Standable(building.Map))
						continue;
					if (intVec.IsForbidden(pawn))
						continue;
					if (!pawn.CanReach(intVec, PathEndMode.OnCell, Danger.None, false, TraverseMode.ByPawn))
						continue;
					if (intVec.IsInPrisonCell(pawn.Map))
						continue;
					goto IL_0265;
				}
				break;
			}
			result = pawn.Position;
			goto IL_0272;
			IL_0265:
			result = intVec;
			goto IL_0272;
		}
	}
}
