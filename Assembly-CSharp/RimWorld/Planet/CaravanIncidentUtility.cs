using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005DB RID: 1499
	public static class CaravanIncidentUtility
	{
		// Token: 0x04001190 RID: 4496
		private const int MapCellsPerPawn = 900;

		// Token: 0x04001191 RID: 4497
		private const int MinMapSize = 75;

		// Token: 0x04001192 RID: 4498
		private const int MaxMapSize = 110;

		// Token: 0x06001D94 RID: 7572 RVA: 0x000FF1B4 File Offset: 0x000FD5B4
		public static int CalculateIncidentMapSize(List<Pawn> caravanPawns, List<Pawn> enemies)
		{
			int num = Mathf.RoundToInt((float)((caravanPawns.Count + enemies.Count) * 900));
			return Mathf.Clamp(Mathf.RoundToInt(Mathf.Sqrt((float)num)), 75, 110);
		}

		// Token: 0x06001D95 RID: 7573 RVA: 0x000FF1F8 File Offset: 0x000FD5F8
		public static bool CanFireIncidentWhichWantsToGenerateMapAt(int tile)
		{
			bool result;
			if (Current.Game.FindMap(tile) != null)
			{
				result = false;
			}
			else if (!Find.WorldGrid[tile].biome.implemented)
			{
				result = false;
			}
			else
			{
				List<WorldObject> allWorldObjects = Find.WorldObjects.AllWorldObjects;
				for (int i = 0; i < allWorldObjects.Count; i++)
				{
					if (allWorldObjects[i].Tile == tile && !allWorldObjects[i].def.allowCaravanIncidentsWhichGenerateMap)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06001D96 RID: 7574 RVA: 0x000FF29C File Offset: 0x000FD69C
		public static Map SetupCaravanAttackMap(Caravan caravan, List<Pawn> enemies, bool sendLetterIfRelatedPawns)
		{
			int num = CaravanIncidentUtility.CalculateIncidentMapSize(caravan.PawnsListForReading, enemies);
			Map map = CaravanIncidentUtility.GetOrGenerateMapForIncident(caravan, new IntVec3(num, 1, num), WorldObjectDefOf.Ambush);
			IntVec3 playerStartingSpot;
			IntVec3 root;
			MultipleCaravansCellFinder.FindStartingCellsFor2Groups(map, out playerStartingSpot, out root);
			CaravanEnterMapUtility.Enter(caravan, map, (Pawn x) => CellFinder.RandomSpawnCellForPawnNear(playerStartingSpot, map, 4), CaravanDropInventoryMode.DoNotDrop, true);
			for (int i = 0; i < enemies.Count; i++)
			{
				IntVec3 loc = CellFinder.RandomSpawnCellForPawnNear(root, map, 4);
				GenSpawn.Spawn(enemies[i], loc, map, Rot4.Random, WipeMode.Vanish, false);
			}
			if (sendLetterIfRelatedPawns)
			{
				PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter_Send(enemies, "LetterRelatedPawnsGroupGeneric".Translate(new object[]
				{
					Faction.OfPlayer.def.pawnsPlural
				}), LetterDefOf.NeutralEvent, true, true);
			}
			return map;
		}

		// Token: 0x06001D97 RID: 7575 RVA: 0x000FF390 File Offset: 0x000FD790
		public static Map GetOrGenerateMapForIncident(Caravan caravan, IntVec3 size, WorldObjectDef suggestedMapParentDef)
		{
			int tile = caravan.Tile;
			bool flag = Current.Game.FindMap(tile) == null;
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(tile, size, suggestedMapParentDef);
			if (flag && orGenerateMap != null)
			{
				orGenerateMap.retainedCaravanData.Notify_GeneratedTempIncidentMapFor(caravan);
			}
			return orGenerateMap;
		}
	}
}
