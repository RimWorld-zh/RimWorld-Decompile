using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005DD RID: 1501
	public static class CaravanIncidentUtility
	{
		// Token: 0x04001194 RID: 4500
		private const int MapCellsPerPawn = 900;

		// Token: 0x04001195 RID: 4501
		private const int MinMapSize = 75;

		// Token: 0x04001196 RID: 4502
		private const int MaxMapSize = 110;

		// Token: 0x06001D97 RID: 7575 RVA: 0x000FF56C File Offset: 0x000FD96C
		public static int CalculateIncidentMapSize(List<Pawn> caravanPawns, List<Pawn> enemies)
		{
			int num = Mathf.RoundToInt((float)((caravanPawns.Count + enemies.Count) * 900));
			return Mathf.Clamp(Mathf.RoundToInt(Mathf.Sqrt((float)num)), 75, 110);
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x000FF5B0 File Offset: 0x000FD9B0
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

		// Token: 0x06001D99 RID: 7577 RVA: 0x000FF654 File Offset: 0x000FDA54
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

		// Token: 0x06001D9A RID: 7578 RVA: 0x000FF748 File Offset: 0x000FDB48
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
