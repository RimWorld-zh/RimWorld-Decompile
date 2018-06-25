using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005DB RID: 1499
	public static class CaravanExitMapUtility
	{
		// Token: 0x04001181 RID: 4481
		private static List<int> tmpNeighbors = new List<int>();

		// Token: 0x04001182 RID: 4482
		private static List<Pawn> tmpPawns = new List<Pawn>();

		// Token: 0x04001183 RID: 4483
		private static List<int> retTiles = new List<int>();

		// Token: 0x04001184 RID: 4484
		private static List<int> tileCandidates = new List<int>();

		// Token: 0x06001D75 RID: 7541 RVA: 0x000FD620 File Offset: 0x000FBA20
		public static Caravan ExitMapAndCreateCaravan(IEnumerable<Pawn> pawns, Faction faction, int exitFromTile, Direction8Way dir, int destinationTile, bool sendMessage = true)
		{
			int directionTile = CaravanExitMapUtility.FindRandomStartingTileBasedOnExitDir(exitFromTile, dir);
			return CaravanExitMapUtility.ExitMapAndCreateCaravan(pawns, faction, exitFromTile, directionTile, destinationTile, sendMessage);
		}

		// Token: 0x06001D76 RID: 7542 RVA: 0x000FD64C File Offset: 0x000FBA4C
		public static Caravan ExitMapAndCreateCaravan(IEnumerable<Pawn> pawns, Faction faction, int exitFromTile, int directionTile, int destinationTile, bool sendMessage = true)
		{
			Caravan result;
			if (!GenWorldClosest.TryFindClosestPassableTile(exitFromTile, out exitFromTile))
			{
				Log.Error("Could not find any passable tile for a new caravan.", false);
				result = null;
			}
			else
			{
				if (Find.World.Impassable(directionTile))
				{
					directionTile = exitFromTile;
				}
				CaravanExitMapUtility.tmpPawns.Clear();
				CaravanExitMapUtility.tmpPawns.AddRange(pawns);
				Map map = null;
				for (int i = 0; i < CaravanExitMapUtility.tmpPawns.Count; i++)
				{
					CaravanExitMapUtility.AddCaravanExitTaleIfShould(CaravanExitMapUtility.tmpPawns[i]);
					map = CaravanExitMapUtility.tmpPawns[i].MapHeld;
					if (map != null)
					{
						break;
					}
				}
				Caravan caravan = CaravanMaker.MakeCaravan(CaravanExitMapUtility.tmpPawns, faction, exitFromTile, false);
				Rot4 exitDir = (map == null) ? Rot4.Invalid : Find.WorldGrid.GetRotFromTo(exitFromTile, directionTile);
				for (int j = 0; j < CaravanExitMapUtility.tmpPawns.Count; j++)
				{
					CaravanExitMapUtility.tmpPawns[j].ExitMap(false, exitDir);
				}
				List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
				for (int k = 0; k < pawnsListForReading.Count; k++)
				{
					if (!pawnsListForReading[k].IsWorldPawn())
					{
						Find.WorldPawns.PassToWorld(pawnsListForReading[k], PawnDiscardDecideMode.Decide);
					}
				}
				if (map != null)
				{
					map.Parent.Notify_CaravanFormed(caravan);
					map.retainedCaravanData.Notify_CaravanFormed(caravan);
				}
				if (!caravan.pather.Moving && caravan.Tile != directionTile)
				{
					caravan.pather.StartPath(directionTile, null, true, true);
					caravan.pather.nextTileCostLeft /= 2f;
					caravan.tweener.ResetTweenedPosToRoot();
				}
				if (destinationTile != -1)
				{
					List<FloatMenuOption> list = FloatMenuMakerWorld.ChoicesAtFor(destinationTile, caravan);
					if (list.Any((FloatMenuOption x) => !x.Disabled))
					{
						FloatMenuOption floatMenuOption = list.First((FloatMenuOption x) => !x.Disabled);
						floatMenuOption.action();
					}
					else
					{
						caravan.pather.StartPath(destinationTile, null, true, true);
					}
				}
				if (sendMessage)
				{
					string text = "MessageFormedCaravan".Translate(new object[]
					{
						caravan.Name
					}).CapitalizeFirst();
					if (caravan.pather.Moving && caravan.pather.ArrivalAction != null)
					{
						string text2 = text;
						text = string.Concat(new string[]
						{
							text2,
							" ",
							"MessageFormedCaravan_Orders".Translate(),
							": ",
							caravan.pather.ArrivalAction.Label,
							"."
						});
					}
					Messages.Message(text, caravan, MessageTypeDefOf.TaskCompletion, true);
				}
				result = caravan;
			}
			return result;
		}

		// Token: 0x06001D77 RID: 7543 RVA: 0x000FD944 File Offset: 0x000FBD44
		public static void ExitMapAndJoinOrCreateCaravan(Pawn pawn, Rot4 exitDir)
		{
			Caravan caravan = CaravanExitMapUtility.FindCaravanToJoinFor(pawn);
			if (caravan != null)
			{
				CaravanExitMapUtility.AddCaravanExitTaleIfShould(pawn);
				pawn.DeSpawn(DestroyMode.Vanish);
				caravan.AddPawn(pawn, true);
				pawn.ExitMap(false, exitDir);
			}
			else if (pawn.IsColonist)
			{
				Map map = pawn.Map;
				int directionTile = CaravanExitMapUtility.FindRandomStartingTileBasedOnExitDir(map.Tile, exitDir);
				Caravan caravan2 = CaravanExitMapUtility.ExitMapAndCreateCaravan(Gen.YieldSingle<Pawn>(pawn), pawn.Faction, map.Tile, directionTile, -1, false);
				caravan2.autoJoinable = true;
				bool flag = false;
				List<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (CaravanExitMapUtility.FindCaravanToJoinFor(allPawnsSpawned[i]) != null && !allPawnsSpawned[i].Downed && !allPawnsSpawned[i].Drafted)
					{
						if (allPawnsSpawned[i].RaceProps.Animal)
						{
							flag = true;
						}
						RestUtility.WakeUp(allPawnsSpawned[i]);
						allPawnsSpawned[i].jobs.CheckForJobOverride();
					}
				}
				string text = "MessagePawnLeftMapAndCreatedCaravan".Translate(new object[]
				{
					pawn.LabelShort
				}).CapitalizeFirst();
				if (flag)
				{
					text = text + " " + "MessagePawnLeftMapAndCreatedCaravan_AnimalsWantToJoin".Translate();
				}
				Messages.Message(text, caravan2, MessageTypeDefOf.TaskCompletion, true);
			}
			else
			{
				Log.Error("Pawn " + pawn + " didn't find any caravan to join, and he can't create one.", false);
			}
		}

		// Token: 0x06001D78 RID: 7544 RVA: 0x000FDAD8 File Offset: 0x000FBED8
		public static bool CanExitMapAndJoinOrCreateCaravanNow(Pawn pawn)
		{
			return pawn.Spawned && pawn.Map.exitMapGrid.MapUsesExitGrid && (pawn.IsColonist || CaravanExitMapUtility.FindCaravanToJoinFor(pawn) != null);
		}

		// Token: 0x06001D79 RID: 7545 RVA: 0x000FDB38 File Offset: 0x000FBF38
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
					IntVec3 intVec;
					if (CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => x.Walkable(map) && !x.Fogged(map), map, rotFromTo, CellFinder.EdgeRoadChance_Ignore, out intVec))
					{
						CaravanExitMapUtility.retTiles.Add(num);
					}
				}
			}
			CaravanExitMapUtility.retTiles.SortBy((int x) => grid.GetHeadingFromTo(currentTileID, x));
			return CaravanExitMapUtility.retTiles;
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x000FDC38 File Offset: 0x000FC038
		public static int RandomBestExitTileFrom(Map map)
		{
			Tile tileInfo = map.TileInfo;
			List<int> options = CaravanExitMapUtility.AvailableExitTilesAt(map);
			int result;
			if (!options.Any<int>())
			{
				result = -1;
			}
			else
			{
				List<Tile.RoadLink> roads = tileInfo.Roads;
				if (roads == null)
				{
					result = options.RandomElement<int>();
				}
				else
				{
					int bestRoadIndex = -1;
					for (int i = 0; i < roads.Count; i++)
					{
						if (options.Contains(roads[i].neighbor))
						{
							if (bestRoadIndex == -1 || roads[i].road.priority > roads[bestRoadIndex].road.priority)
							{
								bestRoadIndex = i;
							}
						}
					}
					if (bestRoadIndex == -1)
					{
						result = options.RandomElement<int>();
					}
					else
					{
						result = (from rl in roads
						where options.Contains(rl.neighbor) && rl.road == roads[bestRoadIndex].road
						select rl).RandomElement<Tile.RoadLink>().neighbor;
					}
				}
			}
			return result;
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x000FDD9C File Offset: 0x000FC19C
		public static int BestExitTileToGoTo(int destinationTile, Map from)
		{
			int num = -1;
			using (WorldPath worldPath = Find.WorldPathFinder.FindPath(from.Tile, destinationTile, null, null))
			{
				if (worldPath.Found && worldPath.NodesLeftCount >= 2)
				{
					num = worldPath.NodesReversed[worldPath.NodesReversed.Count - 2];
				}
			}
			int result;
			if (num == -1)
			{
				result = CaravanExitMapUtility.RandomBestExitTileFrom(from);
			}
			else
			{
				float num2 = 0f;
				int num3 = -1;
				List<int> list = CaravanExitMapUtility.AvailableExitTilesAt(from);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i] == num)
					{
						return list[i];
					}
					float num4 = (Find.WorldGrid.GetTileCenter(list[i]) - Find.WorldGrid.GetTileCenter(num)).MagnitudeHorizontalSquared();
					if (num3 == -1 || num4 < num2)
					{
						num3 = list[i];
						num2 = num4;
					}
				}
				result = num3;
			}
			return result;
		}

		// Token: 0x06001D7C RID: 7548 RVA: 0x000FDEC8 File Offset: 0x000FC2C8
		private static int FindRandomStartingTileBasedOnExitDir(int tileID, Rot4 exitDir)
		{
			CaravanExitMapUtility.tileCandidates.Clear();
			World world = Find.World;
			WorldGrid grid = world.grid;
			grid.GetTileNeighbors(tileID, CaravanExitMapUtility.tmpNeighbors);
			for (int i = 0; i < CaravanExitMapUtility.tmpNeighbors.Count; i++)
			{
				int num = CaravanExitMapUtility.tmpNeighbors[i];
				if (CaravanExitMapUtility.IsGoodCaravanStartingTile(num))
				{
					if (!exitDir.IsValid || !(grid.GetRotFromTo(tileID, num) != exitDir))
					{
						CaravanExitMapUtility.tileCandidates.Add(num);
					}
				}
			}
			int num2;
			int result;
			if (CaravanExitMapUtility.tileCandidates.TryRandomElement(out num2))
			{
				result = num2;
			}
			else if (CaravanExitMapUtility.tmpNeighbors.Where(delegate(int x)
			{
				bool result2;
				if (!CaravanExitMapUtility.IsGoodCaravanStartingTile(x))
				{
					result2 = false;
				}
				else
				{
					Rot4 rotFromTo = grid.GetRotFromTo(tileID, x);
					result2 = (((exitDir == Rot4.North || exitDir == Rot4.South) && (rotFromTo == Rot4.East || rotFromTo == Rot4.West)) || ((exitDir == Rot4.East || exitDir == Rot4.West) && (rotFromTo == Rot4.North || rotFromTo == Rot4.South)));
				}
				return result2;
			}).TryRandomElement(out num2))
			{
				result = num2;
			}
			else if ((from x in CaravanExitMapUtility.tmpNeighbors
			where CaravanExitMapUtility.IsGoodCaravanStartingTile(x)
			select x).TryRandomElement(out num2))
			{
				result = num2;
			}
			else
			{
				result = tileID;
			}
			return result;
		}

		// Token: 0x06001D7D RID: 7549 RVA: 0x000FE028 File Offset: 0x000FC428
		private static int FindRandomStartingTileBasedOnExitDir(int tileID, Direction8Way exitDir)
		{
			CaravanExitMapUtility.tileCandidates.Clear();
			World world = Find.World;
			WorldGrid grid = world.grid;
			grid.GetTileNeighbors(tileID, CaravanExitMapUtility.tmpNeighbors);
			for (int i = 0; i < CaravanExitMapUtility.tmpNeighbors.Count; i++)
			{
				int num = CaravanExitMapUtility.tmpNeighbors[i];
				if (CaravanExitMapUtility.IsGoodCaravanStartingTile(num))
				{
					if (grid.GetDirection8WayFromTo(tileID, num) == exitDir)
					{
						CaravanExitMapUtility.tileCandidates.Add(num);
					}
				}
			}
			int num2;
			int result;
			if (CaravanExitMapUtility.tileCandidates.TryRandomElement(out num2))
			{
				result = num2;
			}
			else if ((from x in CaravanExitMapUtility.tmpNeighbors
			where CaravanExitMapUtility.IsGoodCaravanStartingTile(x)
			select x).TryRandomElement(out num2))
			{
				result = num2;
			}
			else
			{
				result = tileID;
			}
			return result;
		}

		// Token: 0x06001D7E RID: 7550 RVA: 0x000FE110 File Offset: 0x000FC510
		private static bool IsGoodCaravanStartingTile(int tile)
		{
			return !Find.World.Impassable(tile);
		}

		// Token: 0x06001D7F RID: 7551 RVA: 0x000FE134 File Offset: 0x000FC534
		public static Caravan FindCaravanToJoinFor(Pawn pawn)
		{
			Caravan result;
			if (pawn.Faction != Faction.OfPlayer && pawn.HostFaction != Faction.OfPlayer)
			{
				result = null;
			}
			else if (!pawn.Spawned || !pawn.CanReachMapEdge())
			{
				result = null;
			}
			else
			{
				int tile = pawn.Map.Tile;
				Find.WorldGrid.GetTileNeighbors(tile, CaravanExitMapUtility.tmpNeighbors);
				CaravanExitMapUtility.tmpNeighbors.Add(tile);
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int i = 0; i < caravans.Count; i++)
				{
					Caravan caravan = caravans[i];
					if (CaravanExitMapUtility.tmpNeighbors.Contains(caravan.Tile))
					{
						if (caravan.autoJoinable)
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
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06001D80 RID: 7552 RVA: 0x000FE258 File Offset: 0x000FC658
		public static bool AnyoneTryingToJoinCaravan(Caravan c)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (!map.IsPlayerHome && Find.WorldGrid.IsNeighborOrSame(c.Tile, map.Tile))
				{
					List<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
					for (int j = 0; j < allPawnsSpawned.Count; j++)
					{
						if (!allPawnsSpawned[j].IsColonistPlayerControlled && !allPawnsSpawned[j].Downed && CaravanExitMapUtility.FindCaravanToJoinFor(allPawnsSpawned[j]) == c)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06001D81 RID: 7553 RVA: 0x000FE32B File Offset: 0x000FC72B
		public static void OpenSomeoneTryingToJoinCaravanDialog(Caravan c, Action confirmAction)
		{
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmMoveAutoJoinableCaravan".Translate(), confirmAction, false, null));
		}

		// Token: 0x06001D82 RID: 7554 RVA: 0x000FE34C File Offset: 0x000FC74C
		private static void AddCaravanExitTaleIfShould(Pawn pawn)
		{
			if (pawn.Spawned && pawn.IsFreeColonist)
			{
				if (pawn.Map.IsPlayerHome)
				{
					TaleRecorder.RecordTale(TaleDefOf.CaravanFormed, new object[]
					{
						pawn
					});
				}
				else if (GenHostility.AnyHostileActiveThreatToPlayer(pawn.Map))
				{
					TaleRecorder.RecordTale(TaleDefOf.CaravanFled, new object[]
					{
						pawn
					});
				}
			}
		}
	}
}
