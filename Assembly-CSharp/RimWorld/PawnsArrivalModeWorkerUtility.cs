using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200049B RID: 1179
	public static class PawnsArrivalModeWorkerUtility
	{
		// Token: 0x06001512 RID: 5394 RVA: 0x000B9414 File Offset: 0x000B7814
		public static void DropInDropPodsNearSpawnCenter(IncidentParms parms, List<Pawn> pawns)
		{
			Map map = (Map)parms.target;
			DropPodUtility.DropThingsNear(parms.spawnCenter, map, pawns.Cast<Thing>(), parms.podOpenDelay, false, true, true, false);
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x000B944C File Offset: 0x000B784C
		public static List<Pair<List<Pawn>, IntVec3>> SplitIntoRandomGroupsNearMapEdge(List<Pawn> pawns, Map map, bool arriveInPods)
		{
			List<Pair<List<Pawn>, IntVec3>> list = new List<Pair<List<Pawn>, IntVec3>>();
			List<Pair<List<Pawn>, IntVec3>> result;
			if (!pawns.Any<Pawn>())
			{
				result = list;
			}
			else
			{
				int maxGroupsCount = PawnsArrivalModeWorkerUtility.GetMaxGroupsCount(pawns.Count);
				int num = (maxGroupsCount != 1) ? Rand.RangeInclusive(2, maxGroupsCount) : 1;
				for (int i = 0; i < num; i++)
				{
					IntVec3 second = PawnsArrivalModeWorkerUtility.FindNewMapEdgeGroupCenter(map, list, arriveInPods);
					list.Add(new Pair<List<Pawn>, IntVec3>(new List<Pawn>(), second)
					{
						First = 
						{
							pawns[i]
						}
					});
				}
				for (int j = num; j < pawns.Count; j++)
				{
					list.RandomElement<Pair<List<Pawn>, IntVec3>>().First.Add(pawns[j]);
				}
				result = list;
			}
			return result;
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x000B9520 File Offset: 0x000B7920
		private static IntVec3 FindNewMapEdgeGroupCenter(Map map, List<Pair<List<Pawn>, IntVec3>> groups, bool arriveInPods)
		{
			IntVec3 result = IntVec3.Invalid;
			float num = 0f;
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec;
				if (arriveInPods)
				{
					intVec = DropCellFinder.FindRaidDropCenterDistant(map);
				}
				else if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Hostile, null))
				{
					intVec = DropCellFinder.FindRaidDropCenterDistant(map);
				}
				if (!groups.Any<Pair<List<Pawn>, IntVec3>>())
				{
					result = intVec;
					break;
				}
				float num2 = float.MaxValue;
				for (int j = 0; j < groups.Count; j++)
				{
					float num3 = (float)intVec.DistanceToSquared(groups[j].Second);
					if (num3 < num2)
					{
						num2 = num3;
					}
				}
				if (!result.IsValid || num2 > num)
				{
					num = num2;
					result = intVec;
				}
			}
			return result;
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x000B9600 File Offset: 0x000B7A00
		private static int GetMaxGroupsCount(int pawnsCount)
		{
			int result;
			if (pawnsCount <= 1)
			{
				result = 1;
			}
			else
			{
				result = Mathf.Clamp(pawnsCount / 2, 2, 3);
			}
			return result;
		}

		// Token: 0x06001516 RID: 5398 RVA: 0x000B9630 File Offset: 0x000B7A30
		public static void SetPawnGroupsInfo(IncidentParms parms, List<Pair<List<Pawn>, IntVec3>> groups)
		{
			parms.pawnGroups = new Dictionary<Pawn, int>();
			for (int i = 0; i < groups.Count; i++)
			{
				for (int j = 0; j < groups[i].First.Count; j++)
				{
					parms.pawnGroups.Add(groups[i].First[j], i);
				}
			}
		}

		// Token: 0x04000C9B RID: 3227
		private const int MaxGroupsCount = 3;
	}
}
