using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005DA RID: 1498
	public static class CaravanEnterMapUtility
	{
		// Token: 0x04001180 RID: 4480
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x06001D6C RID: 7532 RVA: 0x000FCF44 File Offset: 0x000FB344
		public static void Enter(Caravan caravan, Map map, CaravanEnterMode enterMode, CaravanDropInventoryMode dropInventoryMode = CaravanDropInventoryMode.DoNotDrop, bool draftColonists = false, Predicate<IntVec3> extraCellValidator = null)
		{
			if (enterMode == CaravanEnterMode.None)
			{
				Log.Error(string.Concat(new object[]
				{
					"Caravan ",
					caravan,
					" tried to enter map ",
					map,
					" with enter mode ",
					enterMode
				}), false);
				enterMode = CaravanEnterMode.Edge;
			}
			IntVec3 enterCell = CaravanEnterMapUtility.GetEnterCell(caravan, map, enterMode, extraCellValidator);
			Func<Pawn, IntVec3> spawnCellGetter = (Pawn p) => CellFinder.RandomSpawnCellForPawnNear(enterCell, map, 4);
			CaravanEnterMapUtility.Enter(caravan, map, spawnCellGetter, dropInventoryMode, draftColonists);
		}

		// Token: 0x06001D6D RID: 7533 RVA: 0x000FCFDC File Offset: 0x000FB3DC
		public static void Enter(Caravan caravan, Map map, Func<Pawn, IntVec3> spawnCellGetter, CaravanDropInventoryMode dropInventoryMode = CaravanDropInventoryMode.DoNotDrop, bool draftColonists = false)
		{
			CaravanEnterMapUtility.tmpPawns.Clear();
			CaravanEnterMapUtility.tmpPawns.AddRange(caravan.PawnsListForReading);
			for (int i = 0; i < CaravanEnterMapUtility.tmpPawns.Count; i++)
			{
				IntVec3 loc = spawnCellGetter(CaravanEnterMapUtility.tmpPawns[i]);
				GenSpawn.Spawn(CaravanEnterMapUtility.tmpPawns[i], loc, map, Rot4.Random, WipeMode.Vanish, false);
			}
			if (dropInventoryMode == CaravanDropInventoryMode.DropInstantly)
			{
				CaravanEnterMapUtility.DropAllInventory(CaravanEnterMapUtility.tmpPawns);
			}
			else if (dropInventoryMode == CaravanDropInventoryMode.UnloadIndividually)
			{
				for (int j = 0; j < CaravanEnterMapUtility.tmpPawns.Count; j++)
				{
					CaravanEnterMapUtility.tmpPawns[j].inventory.UnloadEverything = true;
				}
			}
			if (draftColonists)
			{
				CaravanEnterMapUtility.DraftColonists(CaravanEnterMapUtility.tmpPawns);
			}
			if (map.IsPlayerHome)
			{
				for (int k = 0; k < CaravanEnterMapUtility.tmpPawns.Count; k++)
				{
					if (CaravanEnterMapUtility.tmpPawns[k].IsPrisoner)
					{
						CaravanEnterMapUtility.tmpPawns[k].guest.WaitInsteadOfEscapingForDefaultTicks();
					}
				}
			}
			caravan.RemoveAllPawns();
			if (caravan.Spawned)
			{
				Find.WorldObjects.Remove(caravan);
			}
			CaravanEnterMapUtility.tmpPawns.Clear();
		}

		// Token: 0x06001D6E RID: 7534 RVA: 0x000FD12C File Offset: 0x000FB52C
		private static IntVec3 GetEnterCell(Caravan caravan, Map map, CaravanEnterMode enterMode, Predicate<IntVec3> extraCellValidator)
		{
			IntVec3 result;
			if (enterMode != CaravanEnterMode.Edge)
			{
				if (enterMode != CaravanEnterMode.Center)
				{
					throw new NotImplementedException("CaravanEnterMode");
				}
				result = CaravanEnterMapUtility.FindCenterCell(map, extraCellValidator);
			}
			else
			{
				result = CaravanEnterMapUtility.FindNearEdgeCell(map, extraCellValidator);
			}
			return result;
		}

		// Token: 0x06001D6F RID: 7535 RVA: 0x000FD174 File Offset: 0x000FB574
		private static IntVec3 FindNearEdgeCell(Map map, Predicate<IntVec3> extraCellValidator)
		{
			Predicate<IntVec3> baseValidator = (IntVec3 x) => x.Standable(map) && !x.Fogged(map);
			Faction hostFaction = map.ParentFaction;
			IntVec3 root;
			IntVec3 result;
			if (CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => baseValidator(x) && (extraCellValidator == null || extraCellValidator(x)) && ((hostFaction != null && map.reachability.CanReachFactionBase(x, hostFaction)) || (hostFaction == null && map.reachability.CanReachBiggestMapEdgeRoom(x))), map, CellFinder.EdgeRoadChance_Neutral, out root))
			{
				result = CellFinder.RandomClosewalkCellNear(root, map, 5, null);
			}
			else if (extraCellValidator != null && CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => baseValidator(x) && extraCellValidator(x), map, CellFinder.EdgeRoadChance_Neutral, out root))
			{
				result = CellFinder.RandomClosewalkCellNear(root, map, 5, null);
			}
			else if (CellFinder.TryFindRandomEdgeCellWith(baseValidator, map, CellFinder.EdgeRoadChance_Neutral, out root))
			{
				result = CellFinder.RandomClosewalkCellNear(root, map, 5, null);
			}
			else
			{
				Log.Warning("Could not find any valid edge cell.", false);
				result = CellFinder.RandomCell(map);
			}
			return result;
		}

		// Token: 0x06001D70 RID: 7536 RVA: 0x000FD284 File Offset: 0x000FB684
		private static IntVec3 FindCenterCell(Map map, Predicate<IntVec3> extraCellValidator)
		{
			TraverseParms traverseParms = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false);
			Predicate<IntVec3> baseValidator = (IntVec3 x) => x.Standable(map) && !x.Fogged(map) && map.reachability.CanReachMapEdge(x, traverseParms);
			IntVec3 intVec;
			IntVec3 result;
			if (extraCellValidator != null && RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => baseValidator(x) && extraCellValidator(x), map, out intVec))
			{
				result = intVec;
			}
			else if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith(baseValidator, map, out intVec))
			{
				result = intVec;
			}
			else
			{
				Log.Warning("Could not find any valid cell.", false);
				result = CellFinder.RandomCell(map);
			}
			return result;
		}

		// Token: 0x06001D71 RID: 7537 RVA: 0x000FD334 File Offset: 0x000FB734
		public static void DropAllInventory(List<Pawn> pawns)
		{
			for (int i = 0; i < pawns.Count; i++)
			{
				pawns[i].inventory.DropAllNearPawn(pawns[i].Position, false, true);
			}
		}

		// Token: 0x06001D72 RID: 7538 RVA: 0x000FD37C File Offset: 0x000FB77C
		private static void DraftColonists(List<Pawn> pawns)
		{
			for (int i = 0; i < pawns.Count; i++)
			{
				if (pawns[i].IsColonist)
				{
					pawns[i].drafter.Drafted = true;
				}
			}
		}

		// Token: 0x06001D73 RID: 7539 RVA: 0x000FD3C8 File Offset: 0x000FB7C8
		private static bool TryRandomNonOccupiedClosewalkCellNear(IntVec3 root, Map map, int radius, out IntVec3 result)
		{
			return CellFinder.TryFindRandomReachableCellNear(root, map, (float)radius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), (IntVec3 c) => c.Standable(map) && c.GetFirstPawn(map) == null, null, out result, 999999);
		}
	}
}
