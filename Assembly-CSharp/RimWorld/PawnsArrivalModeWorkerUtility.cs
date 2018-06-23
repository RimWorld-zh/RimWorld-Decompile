using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000497 RID: 1175
	public static class PawnsArrivalModeWorkerUtility
	{
		// Token: 0x04000C98 RID: 3224
		private const int MaxGroupsCount = 3;

		// Token: 0x06001509 RID: 5385 RVA: 0x000B9410 File Offset: 0x000B7810
		public static void DropInDropPodsNearSpawnCenter(IncidentParms parms, List<Pawn> pawns)
		{
			Map map = (Map)parms.target;
			DropPodUtility.DropThingsNear(parms.spawnCenter, map, pawns.Cast<Thing>(), parms.podOpenDelay, false, true, true, false);
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x000B9448 File Offset: 0x000B7848
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

		// Token: 0x0600150B RID: 5387 RVA: 0x000B951C File Offset: 0x000B791C
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

		// Token: 0x0600150C RID: 5388 RVA: 0x000B95FC File Offset: 0x000B79FC
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

		// Token: 0x0600150D RID: 5389 RVA: 0x000B962C File Offset: 0x000B7A2C
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
	}
}
