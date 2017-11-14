using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	public static class CaravanExitMapUtility
	{
		private static List<int> tmpNeighbors = new List<int>();

		private static List<Pawn> tmpPawns = new List<Pawn>();

		private static List<int> retTiles = new List<int>();

		private static List<int> tileCandidates = new List<int>();

		[CompilerGenerated]
		private static Func<int, bool> _003C_003Ef__mg_0024cache0;

		public static Caravan ExitMapAndCreateCaravan(IEnumerable<Pawn> pawns, Faction faction, int exitFromTile, Direction8Way dir)
		{
			int directionTile = CaravanExitMapUtility.FindRandomStartingTileBasedOnExitDir(exitFromTile, dir);
			return CaravanExitMapUtility.ExitMapAndCreateCaravan(pawns, faction, exitFromTile, directionTile);
		}

		public static Caravan ExitMapAndCreateCaravan(IEnumerable<Pawn> pawns, Faction faction, int exitFromTile, int directionTile)
		{
			if (Find.World.Impassable(directionTile))
			{
				directionTile = exitFromTile;
			}
			if (Find.World.Impassable(exitFromTile))
			{
				exitFromTile = directionTile;
			}
			Caravan caravan = CaravanExitMapUtility.ExitMapAndCreateCaravan(pawns, faction, exitFromTile);
			if (caravan.Tile != directionTile)
			{
				caravan.pather.StartPath(directionTile, null, true);
				caravan.pather.nextTileCostLeft /= 2f;
				caravan.tweener.ResetToPosition();
			}
			return caravan;
		}

		public static Caravan ExitMapAndCreateCaravan(IEnumerable<Pawn> pawns, Faction faction, int startingTile)
		{
			if (!GenWorldClosest.TryFindClosestPassableTile(startingTile, out startingTile))
			{
				Log.Error("Could not find any passable tile for a new caravan.");
				return null;
			}
			CaravanExitMapUtility.tmpPawns.Clear();
			CaravanExitMapUtility.tmpPawns.AddRange(pawns);
			Map map = null;
			int num = 0;
			while (num < CaravanExitMapUtility.tmpPawns.Count)
			{
				map = CaravanExitMapUtility.tmpPawns[num].MapHeld;
				if (map == null)
				{
					num++;
					continue;
				}
				break;
			}
			Caravan caravan = CaravanMaker.MakeCaravan(CaravanExitMapUtility.tmpPawns, faction, startingTile, false);
			for (int i = 0; i < CaravanExitMapUtility.tmpPawns.Count; i++)
			{
				CaravanExitMapUtility.tmpPawns[i].ExitMap(false);
			}
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int j = 0; j < pawnsListForReading.Count; j++)
			{
				if (!pawnsListForReading[j].IsWorldPawn())
				{
					Find.WorldPawns.PassToWorld(pawnsListForReading[j], PawnDiscardDecideMode.Decide);
				}
			}
			if (map != null)
			{
				map.info.parent.Notify_CaravanFormed(caravan);
			}
			return caravan;
		}

		public static void ExitMapAndJoinOrCreateCaravan(Pawn pawn)
		{
			CaravanExitMapUtility.GenerateCaravanExitTale(pawn);
			Caravan caravan = CaravanExitMapUtility.FindCaravanToJoinFor(pawn);
			if (caravan != null)
			{
				pawn.DeSpawn();
				caravan.AddPawn(pawn, true);
				pawn.ExitMap(false);
			}
			else if (pawn.IsColonist)
			{
				List<int> list = CaravanExitMapUtility.AvailableExitTilesAt(pawn.Map);
				Caravan caravan2 = CaravanExitMapUtility.ExitMapAndCreateCaravan(Gen.YieldSingle(pawn), pawn.Faction, pawn.Map.Tile, (!list.Any()) ? pawn.Map.Tile : list.RandomElement());
				caravan2.autoJoinable = true;
				if (pawn.Faction == Faction.OfPlayer)
				{
					Messages.Message("MessagePawnLeftMapAndCreatedCaravan".Translate(pawn.LabelShort).CapitalizeFirst(), caravan2, MessageTypeDefOf.TaskCompletion);
				}
			}
			else
			{
				Log.Error("Pawn " + pawn + " didn't find any caravan to join, and he can't create one.");
			}
		}

		public static bool CanExitMapAndJoinOrCreateCaravanNow(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return false;
			}
			if (!pawn.Map.exitMapGrid.MapUsesExitGrid)
			{
				return false;
			}
			return pawn.IsColonist || CaravanExitMapUtility.FindCaravanToJoinFor(pawn) != null;
		}

		public static List<int> AvailableExitTilesAt(Map map)
		{
			CaravanExitMapUtility.retTiles.Clear();
			int currentTileID = map.Tile;
			World world = Find.World;
			WorldGrid grid = world.grid;
			grid.GetTileNeighbors(currentTileID, CaravanExitMapUtility.tmpNeighbors);
			for (int i = 0; i < CaravanExitMapUtility.tmpNeighbors.Count; i++)
			{
				int num = CaravanExitMapUtility.tmpNeighbors[i];
				if (CaravanExitMapUtility.IsGoodCaravanStartingTile(num))
				{
					Rot4 rotFromTo = grid.GetRotFromTo(currentTileID, num);
					IntVec3 intVec = default(IntVec3);
					if (CellFinder.TryFindRandomEdgeCellWith((Predicate<IntVec3>)((IntVec3 x) => x.Walkable(map) && !x.Fogged(map)), map, rotFromTo, CellFinder.EdgeRoadChance_Ignore, out intVec))
					{
						CaravanExitMapUtility.retTiles.Add(num);
					}
				}
			}
			CaravanExitMapUtility.retTiles.SortBy((int x) => grid.GetHeadingFromTo(currentTileID, x));
			return CaravanExitMapUtility.retTiles;
		}

		public static int RandomBestExitTileFrom(Map map)
		{
			Tile tile = map.TileInfo;
			List<int> options = CaravanExitMapUtility.AvailableExitTilesAt(map);
			if (!options.Any())
			{
				return -1;
			}
			if (tile.roads == null)
			{
				return options.RandomElement();
			}
			int bestRoadIndex = -1;
			for (int i = 0; i < tile.roads.Count; i++)
			{
				List<int> list = options;
				Tile.RoadLink roadLink = tile.roads[i];
				if (list.Contains(roadLink.neighbor))
				{
					if (bestRoadIndex == -1)
					{
						goto IL_00e9;
					}
					Tile.RoadLink roadLink2 = tile.roads[i];
					int priority = roadLink2.road.priority;
					Tile.RoadLink roadLink3 = tile.roads[bestRoadIndex];
					if (priority > roadLink3.road.priority)
						goto IL_00e9;
				}
				continue;
				IL_00e9:
				bestRoadIndex = i;
			}
			if (bestRoadIndex == -1)
			{
				return options.RandomElement();
			}
			Tile.RoadLink roadLink4 = tile.roads.Where(delegate(Tile.RoadLink rl)
			{
				int result;
				if (options.Contains(rl.neighbor))
				{
					RoadDef road = rl.road;
					Tile.RoadLink roadLink5 = tile.roads[bestRoadIndex];
					result = ((road == roadLink5.road) ? 1 : 0);
				}
				else
				{
					result = 0;
				}
				return (byte)result != 0;
			}).RandomElement();
			return roadLink4.neighbor;
		}

		private static int FindRandomStartingTileBasedOnExitDir(int tileID, Direction8Way exitDir)
		{
			CaravanExitMapUtility.tileCandidates.Clear();
			World world = Find.World;
			WorldGrid grid = world.grid;
			grid.GetTileNeighbors(tileID, CaravanExitMapUtility.tmpNeighbors);
			for (int i = 0; i < CaravanExitMapUtility.tmpNeighbors.Count; i++)
			{
				int num = CaravanExitMapUtility.tmpNeighbors[i];
				if (grid.GetDirection8WayFromTo(tileID, num) == exitDir)
				{
					CaravanExitMapUtility.tileCandidates.Add(num);
				}
			}
			if (CaravanExitMapUtility.tileCandidates.Count == 0)
			{
				return tileID;
			}
			int result = default(int);
			if (CaravanExitMapUtility.tileCandidates.Where(CaravanExitMapUtility.IsGoodCaravanStartingTile).TryRandomElement<int>(out result))
			{
				return result;
			}
			return CaravanExitMapUtility.tileCandidates.RandomElement();
		}

		private static bool IsGoodCaravanStartingTile(int tile)
		{
			return !Find.World.Impassable(tile);
		}

		public static Caravan FindCaravanToJoinFor(Pawn pawn)
		{
			if (pawn.Faction != Faction.OfPlayer && pawn.HostFaction != Faction.OfPlayer)
			{
				return null;
			}
			int tile = pawn.Map.Tile;
			Find.WorldGrid.GetTileNeighbors(tile, CaravanExitMapUtility.tmpNeighbors);
			CaravanExitMapUtility.tmpNeighbors.Add(tile);
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				Caravan caravan = caravans[i];
				if (CaravanExitMapUtility.tmpNeighbors.Contains(caravan.Tile) && caravan.autoJoinable)
				{
					if (pawn.HostFaction == null)
					{
						if (caravan.Faction == pawn.Faction)
						{
							return caravan;
						}
					}
					else if (caravan.Faction == pawn.HostFaction)
					{
						return caravan;
					}
				}
			}
			return null;
		}

		public static bool IsTheOnlyJoinableCaravanForAnyPrisonerOrAnimal(Caravan c)
		{
			if (!c.autoJoinable)
			{
				Log.Warning("This caravan is not auto joinable.");
				return false;
			}
			if (c.Faction != Faction.OfPlayer)
			{
				Log.Warning("Only player caravans supported.");
				return false;
			}
			List<Map> maps = Find.Maps;
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (!map.IsPlayerHome && Find.WorldGrid.IsNeighborOrSame(c.Tile, map.Tile) && map.mapPawns.FreeColonistsCount == 0 && map.mapPawns.AllPawns.Any((Pawn x) => x.Faction == Faction.OfPlayer || x.HostFaction == Faction.OfPlayer))
				{
					bool flag = false;
					for (int j = 0; j < caravans.Count; j++)
					{
						Caravan caravan = caravans[j];
						if (c != caravan && caravan.autoJoinable && caravan.Faction == c.Faction && Find.WorldGrid.IsNeighborOrSame(caravan.Tile, map.Tile))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return true;
					}
				}
			}
			return false;
		}

		public static void OpenTheOnlyJoinableCaravanForPrisonerOrAnimalDialog(Caravan c, Action confirmAction)
		{
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmLeavePrisonersOrAnimalsBehind".Translate(), confirmAction, false, null));
		}

		public static void GenerateCaravanExitTale(Pawn pawn)
		{
			if (pawn.Spawned && pawn.IsFreeColonist)
			{
				if (pawn.Map.IsPlayerHome)
				{
					TaleRecorder.RecordTale(TaleDefOf.CaravanFormed, pawn);
				}
				else if (GenHostility.AnyHostileActiveThreatToPlayer(pawn.Map))
				{
					TaleRecorder.RecordTale(TaleDefOf.CaravanFled, pawn);
				}
			}
		}
	}
}
