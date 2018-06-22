using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000116 RID: 278
	public class JobGiver_WanderColony : JobGiver_Wander
	{
		// Token: 0x060005A8 RID: 1448 RVA: 0x0003CBB8 File Offset: 0x0003AFB8
		public JobGiver_WanderColony()
		{
			this.wanderRadius = 7f;
			this.ticksBetweenWandersRange = new IntRange(125, 200);
			this.wanderDestValidator = ((Pawn pawn, IntVec3 loc, IntVec3 root) => true);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0003CC0C File Offset: 0x0003B00C
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
					return JobGiver_WanderColony.gatherSpots.RandomElement<IntVec3>();
				}
			}
			List<Building> allBuildingsColonist = pawn.Map.listerBuildings.allBuildingsColonist;
			IntVec3 result;
			if (allBuildingsColonist.Count == 0)
			{
				Pawn pawn2;
				if ((from c in pawn.Map.mapPawns.FreeColonistsSpawned
				where !c.Position.IsForbidden(pawn) && pawn.CanReach(c.Position, PathEndMode.Touch, Danger.None, false, TraverseMode.ByPawn)
				select c).TryRandomElement(out pawn2))
				{
					result = pawn2.Position;
				}
				else
				{
					result = pawn.Position;
				}
			}
			else
			{
				int num = 0;
				IntVec3 intVec;
				for (;;)
				{
					num++;
					if (num > 20)
					{
						break;
					}
					Building building = allBuildingsColonist.RandomElement<Building>();
					if (!building.Position.IsForbidden(pawn) && pawn.Map.areaManager.Home[building.Position])
					{
						int num2 = 15 + num * 2;
						if ((pawn.Position - building.Position).LengthHorizontalSquared <= num2 * num2)
						{
							intVec = GenAdjFast.AdjacentCells8Way(building).RandomElement<IntVec3>();
							if (intVec.Standable(building.Map) && !intVec.IsForbidden(pawn) && pawn.CanReach(intVec, PathEndMode.OnCell, Danger.None, false, TraverseMode.ByPawn) && !intVec.IsInPrisonCell(pawn.Map))
							{
								goto IL_265;
							}
						}
					}
				}
				return pawn.Position;
				IL_265:
				result = intVec;
			}
			return result;
		}

		// Token: 0x040002F9 RID: 761
		private static List<IntVec3> gatherSpots = new List<IntVec3>();
	}
}
