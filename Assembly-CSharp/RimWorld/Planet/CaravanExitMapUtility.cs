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
		private static Predicate<FloatMenuOption> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<FloatMenuOption, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<int, bool> <>f__mg$cache0;

		[CompilerGenerated]
		private static Func<int, bool> <>f__mg$cache1;

		public static Caravan ExitMapAndCreateCaravan(IEnumerable<Pawn> pawns, Faction faction, int exitFromTile, Direction8Way dir, int destinationTile, bool sendMessage = true)
		{
			int directionTile = CaravanExitMapUtility.FindRandomStartingTileBasedOnExitDir(exitFromTile, dir);
			return CaravanExitMapUtility.ExitMapAndCreateCaravan(pawns, faction, exitFromTile, directionTile, destinationTile, sendMessage);
		}

		public static Caravan ExitMapAndCreateCaravan(IEnumerable<Pawn> pawns, Faction faction, int exitFromTile, int directionTile, int destinationTile, bool sendMessage = true)
		{
			if (!GenWorldClosest.TryFindClosestPassableTile(exitFromTile, out exitFromTile))
			{
				Log.Error("Could not find any passable tile for a new caravan.", false);
				return null;
			}
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
			return caravan;
		}

		public static void ExitMapAndJoinOrCreateCaravan(Pawn pawn, Rot4 exitDir)
		{
			Caravan caravan = CaravanExitMapUtility.FindCaravanToJoinFor(pawn);
			if (caravan != null)
			{
				CaravanExitMapUtility.AddCaravanExitTaleIfShould(pawn);
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

		public static bool CanExitMapAndJoinOrCreateCaravanNow(Pawn pawn)
		{
			return pawn.Spawned && pawn.Map.exitMapGrid.MapUsesExitGrid && (pawn.IsColonist || CaravanExitMapUtility.FindCaravanToJoinFor(pawn) != null);
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

		public static int RandomBestExitTileFrom(Map map)
		{
			CaravanExitMapUtility.<RandomBestExitTileFrom>c__AnonStorey1 <RandomBestExitTileFrom>c__AnonStorey = new CaravanExitMapUtility.<RandomBestExitTileFrom>c__AnonStorey1();
			Tile tileInfo = map.TileInfo;
			<RandomBestExitTileFrom>c__AnonStorey.options = CaravanExitMapUtility.AvailableExitTilesAt(map);
			if (!<RandomBestExitTileFrom>c__AnonStorey.options.Any<int>())
			{
				return -1;
			}
			<RandomBestExitTileFrom>c__AnonStorey.roads = tileInfo.Roads;
			if (<RandomBestExitTileFrom>c__AnonStorey.roads == null)
			{
				return <RandomBestExitTileFrom>c__AnonStorey.options.RandomElement<int>();
			}
			int bestRoadIndex = -1;
			for (int i = 0; i < <RandomBestExitTileFrom>c__AnonStorey.roads.Count; i++)
			{
				if (<RandomBestExitTileFrom>c__AnonStorey.options.Contains(<RandomBestExitTileFrom>c__AnonStorey.roads[i].neighbor))
				{
					if (bestRoadIndex == -1 || <RandomBestExitTileFrom>c__AnonStorey.roads[i].road.priority > <RandomBestExitTileFrom>c__AnonStorey.roads[bestRoadIndex].road.priority)
					{
						bestRoadIndex = i;
					}
				}
			}
			if (bestRoadIndex == -1)
			{
				return <RandomBestExitTileFrom>c__AnonStorey.options.RandomElement<int>();
			}
			return (from rl in <RandomBestExitTileFrom>c__AnonStorey.roads
			where <RandomBestExitTileFrom>c__AnonStorey.options.Contains(rl.neighbor) && rl.road == <RandomBestExitTileFrom>c__AnonStorey.roads[bestRoadIndex].road
			select rl).RandomElement<Tile.RoadLink>().neighbor;
		}

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
			if (num == -1)
			{
				return CaravanExitMapUtility.RandomBestExitTileFrom(from);
			}
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
			return num3;
		}

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
			int result;
			if (CaravanExitMapUtility.tileCandidates.TryRandomElement(out result))
			{
				return result;
			}
			if (CaravanExitMapUtility.tmpNeighbors.Where(delegate(int x)
			{
				if (!CaravanExitMapUtility.IsGoodCaravanStartingTile(x))
				{
					return false;
				}
				Rot4 rotFromTo = grid.GetRotFromTo(tileID, x);
				return ((exitDir == Rot4.North || exitDir == Rot4.South) && (rotFromTo == Rot4.East || rotFromTo == Rot4.West)) || ((exitDir == Rot4.East || exitDir == Rot4.West) && (rotFromTo == Rot4.North || rotFromTo == Rot4.South));
			}).TryRandomElement(out result))
			{
				return result;
			}
			IEnumerable<int> source = CaravanExitMapUtility.tmpNeighbors;
			if (CaravanExitMapUtility.<>f__mg$cache0 == null)
			{
				CaravanExitMapUtility.<>f__mg$cache0 = new Func<int, bool>(CaravanExitMapUtility.IsGoodCaravanStartingTile);
			}
			if (source.Where(CaravanExitMapUtility.<>f__mg$cache0).TryRandomElement(out result))
			{
				return result;
			}
			return tileID;
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
				if (CaravanExitMapUtility.IsGoodCaravanStartingTile(num))
				{
					if (grid.GetDirection8WayFromTo(tileID, num) == exitDir)
					{
						CaravanExitMapUtility.tileCandidates.Add(num);
					}
				}
			}
			int result;
			if (CaravanExitMapUtility.tileCandidates.TryRandomElement(out result))
			{
				return result;
			}
			IEnumerable<int> source = CaravanExitMapUtility.tmpNeighbors;
			if (CaravanExitMapUtility.<>f__mg$cache1 == null)
			{
				CaravanExitMapUtility.<>f__mg$cache1 = new Func<int, bool>(CaravanExitMapUtility.IsGoodCaravanStartingTile);
			}
			if (source.Where(CaravanExitMapUtility.<>f__mg$cache1).TryRandomElement(out result))
			{
				return result;
			}
			return tileID;
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
			if (!pawn.Spawned || !pawn.CanReachMapEdge())
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
			return null;
		}

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

		public static void OpenSomeoneTryingToJoinCaravanDialog(Caravan c, Action confirmAction)
		{
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmMoveAutoJoinableCaravan".Translate(), confirmAction, false, null));
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static CaravanExitMapUtility()
		{
		}

		[CompilerGenerated]
		private static bool <ExitMapAndCreateCaravan>m__0(FloatMenuOption x)
		{
			return !x.Disabled;
		}

		[CompilerGenerated]
		private static bool <ExitMapAndCreateCaravan>m__1(FloatMenuOption x)
		{
			return !x.Disabled;
		}

		[CompilerGenerated]
		private sealed class <AvailableExitTilesAt>c__AnonStorey0
		{
			internal Map map;

			internal WorldGrid grid;

			internal int currentTileID;

			public <AvailableExitTilesAt>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return x.Walkable(this.map) && !x.Fogged(this.map);
			}

			internal float <>m__1(int x)
			{
				return this.grid.GetHeadingFromTo(this.currentTileID, x);
			}
		}

		[CompilerGenerated]
		private sealed class <RandomBestExitTileFrom>c__AnonStorey1
		{
			internal List<int> options;

			internal List<Tile.RoadLink> roads;

			public <RandomBestExitTileFrom>c__AnonStorey1()
			{
			}
		}

		[CompilerGenerated]
		private sealed class <RandomBestExitTileFrom>c__AnonStorey2
		{
			internal int bestRoadIndex;

			internal CaravanExitMapUtility.<RandomBestExitTileFrom>c__AnonStorey1 <>f__ref$1;

			public <RandomBestExitTileFrom>c__AnonStorey2()
			{
			}

			internal bool <>m__0(Tile.RoadLink rl)
			{
				return this.<>f__ref$1.options.Contains(rl.neighbor) && rl.road == this.<>f__ref$1.roads[this.bestRoadIndex].road;
			}
		}

		[CompilerGenerated]
		private sealed class <FindRandomStartingTileBasedOnExitDir>c__AnonStorey3
		{
			internal WorldGrid grid;

			internal int tileID;

			internal Rot4 exitDir;

			public <FindRandomStartingTileBasedOnExitDir>c__AnonStorey3()
			{
			}

			internal bool <>m__0(int x)
			{
				if (!CaravanExitMapUtility.IsGoodCaravanStartingTile(x))
				{
					return false;
				}
				Rot4 rotFromTo = this.grid.GetRotFromTo(this.tileID, x);
				return ((this.exitDir == Rot4.North || this.exitDir == Rot4.South) && (rotFromTo == Rot4.East || rotFromTo == Rot4.West)) || ((this.exitDir == Rot4.East || this.exitDir == Rot4.West) && (rotFromTo == Rot4.North || rotFromTo == Rot4.South));
			}
		}
	}
}
