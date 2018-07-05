using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class RCellFinder
	{
		private static List<Region> regions = new List<Region>();

		private static HashSet<Thing> tmpBuildings = new HashSet<Thing>();

		[CompilerGenerated]
		private static Func<Region, float> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Building, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<Building, IntVec3> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<Building, IntVec3> <>f__am$cache3;

		public static IntVec3 BestOrderedGotoDestNear(IntVec3 root, Pawn searcher)
		{
			Map map = searcher.Map;
			Predicate<IntVec3> predicate = delegate(IntVec3 c)
			{
				bool result3;
				if (!map.pawnDestinationReservationManager.CanReserve(c, searcher, true) || !c.Standable(map) || !searcher.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					result3 = false;
				}
				else
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Pawn pawn = thingList[i] as Pawn;
						if (pawn != null && pawn != searcher && pawn.RaceProps.Humanlike)
						{
							return false;
						}
					}
					result3 = true;
				}
				return result3;
			};
			IntVec3 result;
			if (predicate(root))
			{
				result = root;
			}
			else
			{
				int num = 1;
				IntVec3 result2 = default(IntVec3);
				float num2 = -1000f;
				bool flag = false;
				int num3 = GenRadial.NumCellsInRadius(30f);
				for (;;)
				{
					IntVec3 intVec = root + GenRadial.RadialPattern[num];
					if (predicate(intVec))
					{
						float num4 = CoverUtility.TotalSurroundingCoverScore(intVec, map);
						if (num4 > num2)
						{
							num2 = num4;
							result2 = intVec;
							flag = true;
						}
					}
					if (num >= 8 && flag)
					{
						break;
					}
					num++;
					if (num >= num3)
					{
						goto Block_6;
					}
				}
				return result2;
				Block_6:
				result = searcher.Position;
			}
			return result;
		}

		public static bool TryFindBestExitSpot(Pawn pawn, out IntVec3 spot, TraverseMode mode = TraverseMode.ByPawn)
		{
			bool result;
			if (mode == TraverseMode.PassAllDestroyableThings && !pawn.Map.reachability.CanReachMapEdge(pawn.Position, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, true)))
			{
				result = RCellFinder.TryFindRandomPawnEntryCell(out spot, pawn.Map, 0f, delegate(IntVec3 x)
				{
					Pawn pawn2 = pawn;
					LocalTargetInfo dest = x;
					PathEndMode peMode = PathEndMode.OnCell;
					Danger maxDanger = Danger.Deadly;
					TraverseMode mode2 = mode;
					return pawn2.CanReach(dest, peMode, maxDanger, false, mode2);
				});
			}
			else
			{
				int num = 0;
				int num2 = 0;
				IntVec3 intVec2;
				for (;;)
				{
					num2++;
					if (num2 > 30)
					{
						break;
					}
					IntVec3 intVec;
					bool flag = CellFinder.TryFindRandomCellNear(pawn.Position, pawn.Map, num, null, out intVec, -1);
					num += 4;
					if (flag)
					{
						int num3 = intVec.x;
						intVec2 = new IntVec3(0, 0, intVec.z);
						if (pawn.Map.Size.z - intVec.z < num3)
						{
							num3 = pawn.Map.Size.z - intVec.z;
							intVec2 = new IntVec3(intVec.x, 0, pawn.Map.Size.z - 1);
						}
						if (pawn.Map.Size.x - intVec.x < num3)
						{
							num3 = pawn.Map.Size.x - intVec.x;
							intVec2 = new IntVec3(pawn.Map.Size.x - 1, 0, intVec.z);
						}
						if (intVec.z < num3)
						{
							intVec2 = new IntVec3(intVec.x, 0, 0);
						}
						if (intVec2.Standable(pawn.Map) && pawn.CanReach(intVec2, PathEndMode.OnCell, Danger.Deadly, true, mode))
						{
							goto Block_9;
						}
					}
				}
				spot = pawn.Position;
				return false;
				Block_9:
				spot = intVec2;
				result = true;
			}
			return result;
		}

		public static bool TryFindRandomExitSpot(Pawn pawn, out IntVec3 spot, TraverseMode mode = TraverseMode.ByPawn)
		{
			Danger danger = Danger.Some;
			int num = 0;
			IntVec3 intVec;
			for (;;)
			{
				num++;
				if (num > 40)
				{
					break;
				}
				if (num > 15)
				{
					danger = Danger.Deadly;
				}
				intVec = CellFinder.RandomCell(pawn.Map);
				int num2 = Rand.RangeInclusive(0, 3);
				if (num2 == 0)
				{
					intVec.x = 0;
				}
				if (num2 == 1)
				{
					intVec.x = pawn.Map.Size.x - 1;
				}
				if (num2 == 2)
				{
					intVec.z = 0;
				}
				if (num2 == 3)
				{
					intVec.z = pawn.Map.Size.z - 1;
				}
				if (intVec.Standable(pawn.Map))
				{
					LocalTargetInfo dest = intVec;
					PathEndMode peMode = PathEndMode.OnCell;
					Danger maxDanger = danger;
					if (pawn.CanReach(dest, peMode, maxDanger, false, mode))
					{
						goto IL_F3;
					}
				}
			}
			spot = pawn.Position;
			return false;
			IL_F3:
			spot = intVec;
			return true;
		}

		public static bool TryFindExitSpotNear(Pawn pawn, IntVec3 near, float radius, out IntVec3 spot, TraverseMode mode = TraverseMode.ByPawn)
		{
			if (mode == TraverseMode.PassAllDestroyableThings)
			{
				if (CellFinder.TryFindRandomEdgeCellNearWith(near, radius, pawn.Map, (IntVec3 x) => pawn.CanReach(x, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn), out spot))
				{
					return true;
				}
			}
			return CellFinder.TryFindRandomEdgeCellNearWith(near, radius, pawn.Map, delegate(IntVec3 x)
			{
				Pawn pawn2 = pawn;
				LocalTargetInfo dest = x;
				PathEndMode peMode = PathEndMode.OnCell;
				Danger maxDanger = Danger.Deadly;
				TraverseMode mode2 = mode;
				return pawn2.CanReach(dest, peMode, maxDanger, false, mode2);
			}, out spot);
		}

		public static IntVec3 RandomWanderDestFor(Pawn pawn, IntVec3 root, float radius, Func<Pawn, IntVec3, IntVec3, bool> validator, Danger maxDanger)
		{
			if (radius > 12f)
			{
				Log.Warning(string.Concat(new object[]
				{
					"wanderRadius of ",
					radius,
					" is greater than Region.GridSize of ",
					12,
					" and will break."
				}), false);
			}
			bool flag = UnityData.isDebugBuild && DebugViewSettings.drawDestSearch;
			if (root.GetRegion(pawn.Map, RegionType.Set_Passable) != null)
			{
				int maxRegions = Mathf.Max((int)radius / 3, 13);
				CellFinder.AllRegionsNear(RCellFinder.regions, root.GetRegion(pawn.Map, RegionType.Set_Passable), maxRegions, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), (Region reg) => reg.extentsClose.ClosestDistSquaredTo(root) <= radius * radius, null, RegionType.Set_Passable);
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(root, 0.6f, "root", 50);
				}
				if (RCellFinder.regions.Count > 0)
				{
					for (int i = 0; i < 35; i++)
					{
						IntVec3 intVec = IntVec3.Invalid;
						for (int j = 0; j < 5; j++)
						{
							IntVec3 randomCell = RCellFinder.regions.RandomElementByWeight((Region reg) => (float)reg.CellCount).RandomCell;
							if ((float)randomCell.DistanceToSquared(root) <= radius * radius)
							{
								intVec = randomCell;
								break;
							}
						}
						if (!intVec.IsValid)
						{
							if (flag)
							{
								pawn.Map.debugDrawer.FlashCell(intVec, 0.32f, "distance", 50);
							}
						}
						else
						{
							if (RCellFinder.CanWanderToCell(intVec, pawn, root, validator, i, maxDanger))
							{
								if (flag)
								{
									pawn.Map.debugDrawer.FlashCell(intVec, 0.9f, "go!", 50);
								}
								RCellFinder.regions.Clear();
								return intVec;
							}
							if (flag)
							{
								pawn.Map.debugDrawer.FlashCell(intVec, 0.6f, "validation", 50);
							}
						}
					}
				}
				RCellFinder.regions.Clear();
			}
			IntVec3 position;
			if (!CellFinder.TryFindRandomCellNear(root, pawn.Map, Mathf.FloorToInt(radius), (IntVec3 c) => c.InBounds(pawn.Map) && pawn.CanReach(c, PathEndMode.OnCell, Danger.None, false, TraverseMode.ByPawn) && !c.IsForbidden(pawn) && (validator == null || validator(pawn, c, root)), out position, -1) && !CellFinder.TryFindRandomCellNear(root, pawn.Map, Mathf.FloorToInt(radius), (IntVec3 c) => c.InBounds(pawn.Map) && pawn.CanReach(c, PathEndMode.OnCell, Danger.None, false, TraverseMode.ByPawn) && !c.IsForbidden(pawn), out position, -1) && !CellFinder.TryFindRandomCellNear(root, pawn.Map, Mathf.FloorToInt(radius), (IntVec3 c) => c.InBounds(pawn.Map) && pawn.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn), out position, -1) && !CellFinder.TryFindRandomCellNear(root, pawn.Map, 20, (IntVec3 c) => c.InBounds(pawn.Map) && pawn.CanReach(c, PathEndMode.OnCell, Danger.None, false, TraverseMode.ByPawn) && !c.IsForbidden(pawn), out position, -1) && !CellFinder.TryFindRandomCellNear(root, pawn.Map, 30, (IntVec3 c) => c.InBounds(pawn.Map) && pawn.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn), out position, -1) && !CellFinder.TryFindRandomCellNear(pawn.Position, pawn.Map, 5, (IntVec3 c) => c.InBounds(pawn.Map) && pawn.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn), out position, -1))
			{
				position = pawn.Position;
			}
			if (flag)
			{
				pawn.Map.debugDrawer.FlashCell(position, 0.4f, "fallback", 50);
			}
			return position;
		}

		private static bool CanWanderToCell(IntVec3 c, Pawn pawn, IntVec3 root, Func<Pawn, IntVec3, IntVec3, bool> validator, int tryIndex, Danger maxDanger)
		{
			bool flag = UnityData.isDebugBuild && DebugViewSettings.drawDestSearch;
			bool result;
			if (!c.Walkable(pawn.Map))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0f, "walk", 50);
				}
				result = false;
			}
			else if (c.IsForbidden(pawn))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.25f, "forbid", 50);
				}
				result = false;
			}
			else
			{
				if (tryIndex < 10)
				{
					if (!c.Standable(pawn.Map))
					{
						if (flag)
						{
							pawn.Map.debugDrawer.FlashCell(c, 0.25f, "stand", 50);
						}
						return false;
					}
				}
				if (!pawn.CanReach(c, PathEndMode.OnCell, maxDanger, false, TraverseMode.ByPawn))
				{
					if (flag)
					{
						pawn.Map.debugDrawer.FlashCell(c, 0.6f, "reach", 50);
					}
					result = false;
				}
				else if (PawnUtility.KnownDangerAt(c, pawn.Map, pawn))
				{
					if (flag)
					{
						pawn.Map.debugDrawer.FlashCell(c, 0.1f, "trap", 50);
					}
					result = false;
				}
				else
				{
					if (tryIndex < 10)
					{
						if (c.GetTerrain(pawn.Map).avoidWander)
						{
							if (flag)
							{
								pawn.Map.debugDrawer.FlashCell(c, 0.39f, "terr", 50);
							}
							return false;
						}
						if (pawn.Map.pathGrid.PerceivedPathCostAt(c) > 20)
						{
							if (flag)
							{
								pawn.Map.debugDrawer.FlashCell(c, 0.4f, "pcost", 50);
							}
							return false;
						}
						if (c.GetDangerFor(pawn, pawn.Map) > Danger.None)
						{
							if (flag)
							{
								pawn.Map.debugDrawer.FlashCell(c, 0.4f, "danger", 50);
							}
							return false;
						}
					}
					else if (tryIndex < 15)
					{
						if (c.GetDangerFor(pawn, pawn.Map) == Danger.Deadly)
						{
							if (flag)
							{
								pawn.Map.debugDrawer.FlashCell(c, 0.4f, "deadly", 50);
							}
							return false;
						}
					}
					if (!pawn.Map.pawnDestinationReservationManager.CanReserve(c, pawn, false))
					{
						if (flag)
						{
							pawn.Map.debugDrawer.FlashCell(c, 0.75f, "resvd", 50);
						}
						result = false;
					}
					else if (validator != null && !validator(pawn, c, root))
					{
						if (flag)
						{
							pawn.Map.debugDrawer.FlashCell(c, 0.15f, "valid", 50);
						}
						result = false;
					}
					else if (c.GetDoor(pawn.Map) != null)
					{
						if (flag)
						{
							pawn.Map.debugDrawer.FlashCell(c, 0.32f, "door", 50);
						}
						result = false;
					}
					else if (c.ContainsStaticFire(pawn.Map))
					{
						if (flag)
						{
							pawn.Map.debugDrawer.FlashCell(c, 0.9f, "fire", 50);
						}
						result = false;
					}
					else
					{
						result = true;
					}
				}
			}
			return result;
		}

		public static bool TryFindGoodAdjacentSpotToTouch(Pawn toucher, Thing touchee, out IntVec3 result)
		{
			foreach (IntVec3 intVec in GenAdj.CellsAdjacent8Way(touchee).InRandomOrder(null))
			{
				if (intVec.Standable(toucher.Map) && !PawnUtility.KnownDangerAt(intVec, toucher.Map, toucher))
				{
					result = intVec;
					return true;
				}
			}
			foreach (IntVec3 intVec2 in GenAdj.CellsAdjacent8Way(touchee).InRandomOrder(null))
			{
				if (intVec2.Walkable(toucher.Map))
				{
					result = intVec2;
					return true;
				}
			}
			result = touchee.Position;
			return false;
		}

		public static bool TryFindRandomPawnEntryCell(out IntVec3 result, Map map, float roadChance, Predicate<IntVec3> extraValidator = null)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.Standable(map) && !map.roofGrid.Roofed(c) && map.reachability.CanReachColony(c) && c.GetRoom(map, RegionType.Set_Passable).TouchesMapEdge && (extraValidator == null || extraValidator(c)), map, roadChance, out result);
		}

		public static bool TryFindPrisonerReleaseCell(Pawn prisoner, Pawn warden, out IntVec3 result)
		{
			bool result2;
			if (prisoner.Map != warden.Map)
			{
				result = IntVec3.Invalid;
				result2 = false;
			}
			else
			{
				Region region = prisoner.GetRegion(RegionType.Set_Passable);
				if (region == null)
				{
					result = default(IntVec3);
					result2 = false;
				}
				else
				{
					TraverseParms traverseParms = TraverseParms.For(warden, Danger.Deadly, TraverseMode.ByPawn, false);
					bool needMapEdge = prisoner.Faction != warden.Faction;
					IntVec3 foundResult = IntVec3.Invalid;
					RegionProcessor regionProcessor = delegate(Region r)
					{
						if (needMapEdge)
						{
							if (!r.Room.TouchesMapEdge)
							{
								return false;
							}
						}
						else if (r.Room.isPrisonCell)
						{
							return false;
						}
						foundResult = r.RandomCell;
						return true;
					};
					RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.Allows(traverseParms, false), regionProcessor, 999, RegionType.Set_Passable);
					if (foundResult.IsValid)
					{
						result = foundResult;
						result2 = true;
					}
					else
					{
						result = default(IntVec3);
						result2 = false;
					}
				}
			}
			return result2;
		}

		public static IntVec3 RandomAnimalSpawnCell_MapGen(Map map)
		{
			int numStand = 0;
			int numRoom = 0;
			int numTouch = 0;
			Predicate<IntVec3> validator = delegate(IntVec3 c)
			{
				bool result;
				if (!c.Standable(map))
				{
					numStand++;
					result = false;
				}
				else if (c.GetTerrain(map).avoidWander)
				{
					result = false;
				}
				else
				{
					Room room = c.GetRoom(map, RegionType.Set_Passable);
					if (room == null)
					{
						numRoom++;
						result = false;
					}
					else if (!room.TouchesMapEdge)
					{
						numTouch++;
						result = false;
					}
					else
					{
						result = true;
					}
				}
				return result;
			};
			IntVec3 intVec;
			if (!CellFinderLoose.TryGetRandomCellWith(validator, map, 1000, out intVec))
			{
				intVec = CellFinder.RandomCell(map);
				Log.Warning(string.Concat(new object[]
				{
					"RandomAnimalSpawnCell_MapGen failed: numStand=",
					numStand,
					", numRoom=",
					numRoom,
					", numTouch=",
					numTouch,
					". PlayerStartSpot=",
					MapGenerator.PlayerStartSpot,
					". Returning ",
					intVec
				}), false);
			}
			return intVec;
		}

		public static bool TryFindSkygazeCell(IntVec3 root, Pawn searcher, out IntVec3 result)
		{
			Predicate<IntVec3> cellValidator = (IntVec3 c) => !c.Roofed(searcher.Map) && !c.GetTerrain(searcher.Map).avoidWander;
			IntVec3 unused;
			Predicate<Region> validator = (Region r) => r.Room.PsychologicallyOutdoors && !r.IsForbiddenEntirely(searcher) && r.TryFindRandomCellInRegionUnforbidden(searcher, cellValidator, out unused);
			TraverseParms traverseParms = TraverseParms.For(searcher, Danger.Deadly, TraverseMode.ByPawn, false);
			Region root2;
			bool result2;
			if (!CellFinder.TryFindClosestRegionWith(root.GetRegion(searcher.Map, RegionType.Set_Passable), traverseParms, validator, 300, out root2, RegionType.Set_Passable))
			{
				result = root;
				result2 = false;
			}
			else
			{
				Region reg = CellFinder.RandomRegionNear(root2, 14, traverseParms, validator, searcher, RegionType.Set_Passable);
				result2 = reg.TryFindRandomCellInRegionUnforbidden(searcher, cellValidator, out result);
			}
			return result2;
		}

		public static bool TryFindTravelDestFrom(IntVec3 root, Map map, out IntVec3 travelDest)
		{
			travelDest = root;
			bool flag = false;
			Predicate<IntVec3> cellValidator = (IntVec3 c) => map.reachability.CanReach(root, c, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.None) && !map.roofGrid.Roofed(c);
			if (root.x == 0)
			{
				flag = CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.x == map.Size.x - 1 && cellValidator(c), map, CellFinder.EdgeRoadChance_Always, out travelDest);
			}
			else if (root.x == map.Size.x - 1)
			{
				flag = CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.x == 0 && cellValidator(c), map, CellFinder.EdgeRoadChance_Always, out travelDest);
			}
			else if (root.z == 0)
			{
				flag = CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.z == map.Size.z - 1 && cellValidator(c), map, CellFinder.EdgeRoadChance_Always, out travelDest);
			}
			else if (root.z == map.Size.z - 1)
			{
				flag = CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.z == 0 && cellValidator(c), map, CellFinder.EdgeRoadChance_Always, out travelDest);
			}
			if (!flag)
			{
				flag = CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => (c - root).LengthHorizontalSquared > 10000 && cellValidator(c), map, CellFinder.EdgeRoadChance_Always, out travelDest);
			}
			if (!flag)
			{
				flag = CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => (c - root).LengthHorizontalSquared > 2500 && cellValidator(c), map, CellFinder.EdgeRoadChance_Always, out travelDest);
			}
			return flag;
		}

		public static bool TryFindRandomSpotJustOutsideColony(IntVec3 originCell, Map map, out IntVec3 result)
		{
			return RCellFinder.TryFindRandomSpotJustOutsideColony(originCell, map, null, out result, null);
		}

		public static bool TryFindRandomSpotJustOutsideColony(Pawn searcher, out IntVec3 result)
		{
			return RCellFinder.TryFindRandomSpotJustOutsideColony(searcher.Position, searcher.Map, searcher, out result, null);
		}

		public static bool TryFindRandomSpotJustOutsideColony(IntVec3 root, Map map, Pawn searcher, out IntVec3 result, Predicate<IntVec3> extraValidator = null)
		{
			bool desperate = false;
			int minColonyBuildingsLOS = 0;
			Predicate<IntVec3> validator = delegate(IntVec3 c)
			{
				bool result2;
				if (!c.Standable(map))
				{
					result2 = false;
				}
				else
				{
					Room room = c.GetRoom(map, RegionType.Set_Passable);
					if (!room.PsychologicallyOutdoors || !room.TouchesMapEdge)
					{
						result2 = false;
					}
					else if (room == null || room.CellCount < 25)
					{
						result2 = false;
					}
					else
					{
						if (root.IsValid)
						{
							TraverseParms traverseParams = (searcher == null) ? TraverseMode.PassDoors : TraverseParms.For(searcher, Danger.Deadly, TraverseMode.ByPawn, false);
							if (!map.reachability.CanReach(root, c, PathEndMode.Touch, traverseParams))
							{
								return false;
							}
						}
						if (!desperate)
						{
							if (!map.reachability.CanReachColony(c))
							{
								return false;
							}
						}
						if (extraValidator != null && !extraValidator(c))
						{
							result2 = false;
						}
						else
						{
							if (minColonyBuildingsLOS > 0)
							{
								int colonyBuildingsLOSFound = 0;
								RCellFinder.tmpBuildings.Clear();
								RegionTraverser.BreadthFirstTraverse(c, map, (Region from, Region to) => true, delegate(Region reg)
								{
									Faction ofPlayer = Faction.OfPlayer;
									List<Thing> list = reg.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
									for (int l = 0; l < list.Count; l++)
									{
										Thing thing = list[l];
										if (thing.Faction == ofPlayer && thing.Position.InHorDistOf(c, 16f) && GenSight.LineOfSight(thing.Position, c, map, true, null, 0, 0) && !RCellFinder.tmpBuildings.Contains(thing))
										{
											RCellFinder.tmpBuildings.Add(thing);
											colonyBuildingsLOSFound++;
											if (colonyBuildingsLOSFound >= minColonyBuildingsLOS)
											{
												return true;
											}
										}
									}
									return false;
								}, 12, RegionType.Set_Passable);
								RCellFinder.tmpBuildings.Clear();
								if (colonyBuildingsLOSFound < minColonyBuildingsLOS)
								{
									return false;
								}
							}
							result2 = true;
						}
					}
				}
				return result2;
			};
			for (int i = 0; i < 100; i++)
			{
				Building building = null;
				if (!(from b in map.listerBuildings.allBuildingsColonist
				where b.def.designationCategory != DesignationCategoryDefOf.Structure && b.def.building.ai_chillDestination
				select b).TryRandomElement(out building))
				{
					break;
				}
				if (i < 10)
				{
					minColonyBuildingsLOS = 4;
				}
				else if (i < 25)
				{
					minColonyBuildingsLOS = 3;
				}
				else if (i < 40)
				{
					minColonyBuildingsLOS = 2;
				}
				else
				{
					minColonyBuildingsLOS = 1;
				}
				int squareRadius = 10 + i / 5;
				desperate = (i > 60);
				if (CellFinder.TryFindRandomCellNear(building.Position, map, squareRadius, validator, out result, 50))
				{
					return true;
				}
			}
			for (int j = 0; j < 50; j++)
			{
				Building building2 = null;
				if (!map.listerBuildings.allBuildingsColonist.TryRandomElement(out building2))
				{
					break;
				}
				if (j < 10)
				{
					minColonyBuildingsLOS = 3;
				}
				else if (j < 20)
				{
					minColonyBuildingsLOS = 2;
				}
				else if (j < 30)
				{
					minColonyBuildingsLOS = 1;
				}
				else
				{
					minColonyBuildingsLOS = 0;
				}
				desperate = (j > 20);
				if (CellFinder.TryFindRandomCellNear(building2.Position, map, 14, validator, out result, 50))
				{
					return true;
				}
			}
			for (int k = 0; k < 50; k++)
			{
				Pawn pawn = null;
				if (!map.mapPawns.FreeColonistsAndPrisonersSpawned.TryRandomElement(out pawn))
				{
					break;
				}
				minColonyBuildingsLOS = 0;
				desperate = (k > 25);
				if (CellFinder.TryFindRandomCellNear(pawn.Position, map, 14, validator, out result, 50))
				{
					return true;
				}
			}
			desperate = true;
			minColonyBuildingsLOS = 0;
			return CellFinderLoose.TryGetRandomCellWith(validator, map, 1000, out result);
		}

		public static bool TryFindRandomCellInRegionUnforbidden(this Region reg, Pawn pawn, Predicate<IntVec3> validator, out IntVec3 result)
		{
			if (reg == null)
			{
				throw new ArgumentNullException("reg");
			}
			bool result2;
			if (reg.IsForbiddenEntirely(pawn))
			{
				result = IntVec3.Invalid;
				result2 = false;
			}
			else
			{
				result2 = reg.TryFindRandomCellInRegion((IntVec3 c) => !c.IsForbidden(pawn) && (validator == null || validator(c)), out result);
			}
			return result2;
		}

		public static bool TryFindDirectFleeDestination(IntVec3 root, float dist, Pawn pawn, out IntVec3 result)
		{
			for (int i = 0; i < 30; i++)
			{
				result = root + IntVec3.FromVector3(Vector3Utility.HorizontalVectorFromAngle((float)Rand.Range(0, 360)) * dist);
				if (result.Walkable(pawn.Map) && result.DistanceToSquared(pawn.Position) < result.DistanceToSquared(root) && GenSight.LineOfSight(root, result, pawn.Map, true, null, 0, 0))
				{
					return true;
				}
			}
			Region region = pawn.GetRegion(RegionType.Set_Passable);
			for (int j = 0; j < 30; j++)
			{
				Region region2 = CellFinder.RandomRegionNear(region, 15, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), null, null, RegionType.Set_Passable);
				IntVec3 randomCell = region2.RandomCell;
				if (randomCell.Walkable(pawn.Map) && (float)(root - randomCell).LengthHorizontalSquared > dist * dist)
				{
					using (PawnPath pawnPath = pawn.Map.pathFinder.FindPath(pawn.Position, randomCell, pawn, PathEndMode.OnCell))
					{
						if (PawnPathUtility.TryFindCellAtIndex(pawnPath, (int)dist + 3, out result))
						{
							return true;
						}
					}
				}
			}
			result = pawn.Position;
			return false;
		}

		public static bool TryFindRandomCellOutsideColonyNearTheCenterOfTheMap(IntVec3 pos, Map map, float minDistToColony, out IntVec3 result)
		{
			int num = 30;
			CellRect cellRect = CellRect.CenteredOn(map.Center, num);
			cellRect.ClipInsideMap(map);
			List<IntVec3> list = new List<IntVec3>();
			if (minDistToColony > 0f)
			{
				foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
				{
					list.Add(pawn.Position);
				}
				foreach (Building building in map.listerBuildings.allBuildingsColonist)
				{
					list.Add(building.Position);
				}
			}
			float num2 = minDistToColony * minDistToColony;
			int num3 = 0;
			IntVec3 randomCell;
			for (;;)
			{
				num3++;
				if (num3 > 50)
				{
					if (num > map.Size.x)
					{
						break;
					}
					num = (int)((float)num * 1.5f);
					cellRect = CellRect.CenteredOn(map.Center, num);
					cellRect.ClipInsideMap(map);
					num3 = 0;
				}
				randomCell = cellRect.RandomCell;
				if (randomCell.Standable(map))
				{
					if (map.reachability.CanReach(randomCell, pos, PathEndMode.ClosestTouch, TraverseMode.NoPassClosedDoors, Danger.Deadly))
					{
						bool flag = false;
						for (int i = 0; i < list.Count; i++)
						{
							if ((float)(list[i] - randomCell).LengthHorizontalSquared < num2)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							goto IL_1B2;
						}
					}
				}
			}
			result = pos;
			return false;
			IL_1B2:
			result = randomCell;
			return true;
		}

		public static bool TryFindRandomCellNearTheCenterOfTheMapWith(Predicate<IntVec3> validator, Map map, out IntVec3 result)
		{
			int startingSearchRadius = Mathf.Clamp(Mathf.Max(map.Size.x, map.Size.z) / 20, 3, 25);
			return RCellFinder.TryFindRandomCellNearWith(map.Center, validator, map, out result, startingSearchRadius, int.MaxValue);
		}

		public static bool TryFindRandomCellNearWith(IntVec3 near, Predicate<IntVec3> validator, Map map, out IntVec3 result, int startingSearchRadius = 5, int maxSearchRadius = 2147483647)
		{
			int num = startingSearchRadius;
			CellRect cellRect = CellRect.CenteredOn(near, num);
			cellRect.ClipInsideMap(map);
			int num2 = 0;
			IntVec3 randomCell;
			for (;;)
			{
				num2++;
				if (num2 > 30)
				{
					if (num >= maxSearchRadius || (num > map.Size.x * 2 && num > map.Size.z * 2))
					{
						break;
					}
					num = Mathf.Min((int)((float)num * 1.5f), maxSearchRadius);
					cellRect = CellRect.CenteredOn(near, num);
					cellRect.ClipInsideMap(map);
					num2 = 0;
				}
				randomCell = cellRect.RandomCell;
				if (validator(randomCell))
				{
					goto IL_A0;
				}
			}
			result = near;
			return false;
			IL_A0:
			result = randomCell;
			return true;
		}

		public static IntVec3 SpotToChewStandingNear(Pawn pawn, Thing ingestible)
		{
			IntVec3 root = pawn.Position;
			Room rootRoom = pawn.GetRoom(RegionType.Set_Passable);
			bool desperate = false;
			bool ignoreDanger = false;
			float maxDist = 4f;
			Predicate<IntVec3> validator = delegate(IntVec3 c)
			{
				bool result;
				if ((float)(root - c).LengthHorizontalSquared > maxDist * maxDist)
				{
					result = false;
				}
				else
				{
					if (pawn.HostFaction != null)
					{
						if (c.GetRoom(pawn.Map, RegionType.Set_Passable) != rootRoom)
						{
							return false;
						}
					}
					if (!desperate)
					{
						if (!c.Standable(pawn.Map))
						{
							return false;
						}
						if (GenPlace.HaulPlaceBlockerIn(null, c, pawn.Map, false) != null)
						{
							return false;
						}
						if (c.GetRegion(pawn.Map, RegionType.Set_Passable).type == RegionType.Portal)
						{
							return false;
						}
					}
					IntVec3 intVec2;
					result = ((ignoreDanger || c.GetDangerFor(pawn, pawn.Map) == Danger.None) && !c.ContainsStaticFire(pawn.Map) && !c.ContainsTrap(pawn.Map) && pawn.Map.pawnDestinationReservationManager.CanReserve(c, pawn, false) && Toils_Ingest.TryFindAdjacentIngestionPlaceSpot(c, ingestible.def, pawn, out intVec2));
				}
				return result;
			};
			int maxRegions = 1;
			Region region = pawn.GetRegion(RegionType.Set_Passable);
			for (int i = 0; i < 30; i++)
			{
				if (i == 1)
				{
					desperate = true;
				}
				else if (i == 2)
				{
					desperate = false;
					maxRegions = 4;
				}
				else if (i == 6)
				{
					desperate = true;
				}
				else if (i == 10)
				{
					desperate = false;
					maxDist = 8f;
					maxRegions = 12;
				}
				else if (i == 15)
				{
					desperate = true;
				}
				else if (i == 20)
				{
					maxDist = 15f;
					maxRegions = 16;
				}
				else if (i == 26)
				{
					maxDist = 5f;
					maxRegions = 4;
					ignoreDanger = true;
				}
				else if (i == 29)
				{
					maxDist = 15f;
					maxRegions = 16;
				}
				Region reg = CellFinder.RandomRegionNear(region, maxRegions, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), null, null, RegionType.Set_Passable);
				IntVec3 intVec;
				if (reg.TryFindRandomCellInRegionUnforbidden(pawn, validator, out intVec))
				{
					if (DebugViewSettings.drawDestSearch)
					{
						pawn.Map.debugDrawer.FlashCell(intVec, 0.5f, "go!", 50);
					}
					return intVec;
				}
				if (DebugViewSettings.drawDestSearch)
				{
					pawn.Map.debugDrawer.FlashCell(intVec, 0f, i.ToString(), 50);
				}
			}
			return region.RandomCell;
		}

		public static bool TryFindMarriageSite(Pawn firstFiance, Pawn secondFiance, out IntVec3 result)
		{
			bool result2;
			if (!firstFiance.CanReach(secondFiance, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				result = IntVec3.Invalid;
				result2 = false;
			}
			else
			{
				Map map = firstFiance.Map;
				if ((from x in map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.MarriageSpot)
				where MarriageSpotUtility.IsValidMarriageSpotFor(x.Position, firstFiance, secondFiance, null)
				select x.Position).TryRandomElement(out result))
				{
					result2 = true;
				}
				else
				{
					Predicate<IntVec3> noMarriageSpotValidator = delegate(IntVec3 cell)
					{
						IntVec3 c = cell + LordToil_MarriageCeremony.OtherFianceNoMarriageSpotCellOffset;
						bool result3;
						if (!c.InBounds(map))
						{
							result3 = false;
						}
						else if (c.IsForbidden(firstFiance) || c.IsForbidden(secondFiance))
						{
							result3 = false;
						}
						else if (!c.Standable(map))
						{
							result3 = false;
						}
						else
						{
							Room room = cell.GetRoom(map, RegionType.Set_Passable);
							result3 = (room == null || room.IsHuge || room.PsychologicallyOutdoors || room.CellCount >= 10);
						}
						return result3;
					};
					foreach (CompGatherSpot compGatherSpot in map.gatherSpotLister.activeSpots.InRandomOrder(null))
					{
						for (int i = 0; i < 10; i++)
						{
							IntVec3 intVec = CellFinder.RandomClosewalkCellNear(compGatherSpot.parent.Position, compGatherSpot.parent.Map, 4, null);
							if (MarriageSpotUtility.IsValidMarriageSpotFor(intVec, firstFiance, secondFiance, null) && noMarriageSpotValidator(intVec))
							{
								result = intVec;
								return true;
							}
						}
					}
					if (CellFinder.TryFindRandomCellNear(firstFiance.Position, firstFiance.Map, 25, (IntVec3 cell) => MarriageSpotUtility.IsValidMarriageSpotFor(cell, firstFiance, secondFiance, null) && noMarriageSpotValidator(cell), out result, -1))
					{
						result2 = true;
					}
					else
					{
						result = IntVec3.Invalid;
						result2 = false;
					}
				}
			}
			return result2;
		}

		public static bool TryFindPartySpot(Pawn organizer, out IntVec3 result)
		{
			bool enjoyableOutside = JoyUtility.EnjoyableOutsideNow(organizer, null);
			Map map = organizer.Map;
			Predicate<IntVec3> baseValidator = delegate(IntVec3 cell)
			{
				bool result3;
				if (!cell.Standable(map))
				{
					result3 = false;
				}
				else if (cell.GetDangerFor(organizer, map) != Danger.None)
				{
					result3 = false;
				}
				else if (!enjoyableOutside && !cell.Roofed(map))
				{
					result3 = false;
				}
				else if (cell.IsForbidden(organizer))
				{
					result3 = false;
				}
				else if (!organizer.CanReserveAndReach(cell, PathEndMode.OnCell, Danger.None, 1, -1, null, false))
				{
					result3 = false;
				}
				else
				{
					Room room = cell.GetRoom(map, RegionType.Set_Passable);
					bool flag = room != null && room.isPrisonCell;
					result3 = (organizer.IsPrisoner == flag && PartyUtility.EnoughPotentialGuestsToStartParty(map, new IntVec3?(cell)));
				}
				return result3;
			};
			bool result2;
			if ((from x in map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.PartySpot)
			where baseValidator(x.Position)
			select x.Position).TryRandomElement(out result))
			{
				result2 = true;
			}
			else
			{
				Predicate<IntVec3> noPartySpotValidator = delegate(IntVec3 cell)
				{
					Room room = cell.GetRoom(map, RegionType.Set_Passable);
					return room == null || room.IsHuge || room.PsychologicallyOutdoors || room.CellCount >= 10;
				};
				foreach (CompGatherSpot compGatherSpot in map.gatherSpotLister.activeSpots.InRandomOrder(null))
				{
					for (int i = 0; i < 10; i++)
					{
						IntVec3 intVec = CellFinder.RandomClosewalkCellNear(compGatherSpot.parent.Position, compGatherSpot.parent.Map, 4, null);
						if (baseValidator(intVec) && noPartySpotValidator(intVec))
						{
							result = intVec;
							return true;
						}
					}
				}
				if (CellFinder.TryFindRandomCellNear(organizer.Position, organizer.Map, 25, (IntVec3 cell) => baseValidator(cell) && noPartySpotValidator(cell), out result, -1))
				{
					result2 = true;
				}
				else
				{
					result = IntVec3.Invalid;
					result2 = false;
				}
			}
			return result2;
		}

		public static IntVec3 FindSiegePositionFrom(IntVec3 entrySpot, Map map)
		{
			IntVec3 result;
			if (!entrySpot.IsValid)
			{
				IntVec3 intVec;
				if (!CellFinder.TryFindRandomEdgeCellWith((IntVec3 x) => x.Standable(map) && !x.Fogged(map), map, CellFinder.EdgeRoadChance_Ignore, out intVec))
				{
					intVec = CellFinder.RandomCell(map);
				}
				Log.Error("Tried to find a siege position from an invalid cell. Using " + intVec, false);
				result = intVec;
			}
			else
			{
				IntVec3 intVec2;
				for (int i = 70; i >= 20; i -= 10)
				{
					if (RCellFinder.TryFindSiegePosition(entrySpot, (float)i, map, out intVec2))
					{
						return intVec2;
					}
				}
				if (RCellFinder.TryFindSiegePosition(entrySpot, 100f, map, out intVec2))
				{
					result = intVec2;
				}
				else
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not find siege spot from ",
						entrySpot,
						", using ",
						entrySpot
					}), false);
					result = entrySpot;
				}
			}
			return result;
		}

		private static bool TryFindSiegePosition(IntVec3 entrySpot, float minDistToColony, Map map, out IntVec3 result)
		{
			CellRect cellRect = CellRect.CenteredOn(entrySpot, 60);
			cellRect.ClipInsideMap(map);
			cellRect = cellRect.ContractedBy(14);
			List<IntVec3> list = new List<IntVec3>();
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				list.Add(pawn.Position);
			}
			foreach (Building building in map.listerBuildings.allBuildingsColonistCombatTargets)
			{
				list.Add(building.Position);
			}
			float num = minDistToColony * minDistToColony;
			int num2 = 0;
			IntVec3 randomCell;
			for (;;)
			{
				num2++;
				if (num2 > 200)
				{
					break;
				}
				randomCell = cellRect.RandomCell;
				if (randomCell.Standable(map))
				{
					if (randomCell.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy) && randomCell.SupportsStructureType(map, TerrainAffordanceDefOf.Light))
					{
						if (map.reachability.CanReach(randomCell, entrySpot, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Some))
						{
							if (map.reachability.CanReachColony(randomCell))
							{
								bool flag = false;
								for (int i = 0; i < list.Count; i++)
								{
									if ((float)(list[i] - randomCell).LengthHorizontalSquared < num)
									{
										flag = true;
										break;
									}
								}
								if (!flag)
								{
									if (!randomCell.Roofed(map))
									{
										int num3 = 0;
										CellRect.CellRectIterator iterator = CellRect.CenteredOn(randomCell, 10).ClipInsideMap(map).GetIterator();
										while (!iterator.Done())
										{
											if (randomCell.SupportsStructureType(map, TerrainAffordanceDefOf.Heavy) && randomCell.SupportsStructureType(map, TerrainAffordanceDefOf.Light))
											{
												num3++;
											}
											iterator.MoveNext();
										}
										if (num3 >= 35)
										{
											goto IL_231;
										}
									}
								}
							}
						}
					}
				}
			}
			result = IntVec3.Invalid;
			return false;
			IL_231:
			result = randomCell;
			return true;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static RCellFinder()
		{
		}

		[CompilerGenerated]
		private static float <RandomWanderDestFor>m__0(Region reg)
		{
			return (float)reg.CellCount;
		}

		[CompilerGenerated]
		private static bool <TryFindRandomSpotJustOutsideColony>m__1(Building b)
		{
			return b.def.designationCategory != DesignationCategoryDefOf.Structure && b.def.building.ai_chillDestination;
		}

		[CompilerGenerated]
		private static IntVec3 <TryFindMarriageSite>m__2(Building x)
		{
			return x.Position;
		}

		[CompilerGenerated]
		private static IntVec3 <TryFindPartySpot>m__3(Building x)
		{
			return x.Position;
		}

		[CompilerGenerated]
		private sealed class <BestOrderedGotoDestNear>c__AnonStorey0
		{
			internal Map map;

			internal Pawn searcher;

			public <BestOrderedGotoDestNear>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 c)
			{
				bool result;
				if (!this.map.pawnDestinationReservationManager.CanReserve(c, this.searcher, true) || !c.Standable(this.map) || !this.searcher.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					result = false;
				}
				else
				{
					List<Thing> thingList = c.GetThingList(this.map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Pawn pawn = thingList[i] as Pawn;
						if (pawn != null && pawn != this.searcher && pawn.RaceProps.Humanlike)
						{
							return false;
						}
					}
					result = true;
				}
				return result;
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindBestExitSpot>c__AnonStorey1
		{
			internal Pawn pawn;

			internal TraverseMode mode;

			public <TryFindBestExitSpot>c__AnonStorey1()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				Pawn pawn = this.pawn;
				LocalTargetInfo dest = x;
				PathEndMode peMode = PathEndMode.OnCell;
				Danger maxDanger = Danger.Deadly;
				TraverseMode traverseMode = this.mode;
				return pawn.CanReach(dest, peMode, maxDanger, false, traverseMode);
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindExitSpotNear>c__AnonStorey2
		{
			internal Pawn pawn;

			internal TraverseMode mode;

			public <TryFindExitSpotNear>c__AnonStorey2()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return this.pawn.CanReach(x, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn);
			}

			internal bool <>m__1(IntVec3 x)
			{
				Pawn pawn = this.pawn;
				LocalTargetInfo dest = x;
				PathEndMode peMode = PathEndMode.OnCell;
				Danger maxDanger = Danger.Deadly;
				TraverseMode traverseMode = this.mode;
				return pawn.CanReach(dest, peMode, maxDanger, false, traverseMode);
			}
		}

		[CompilerGenerated]
		private sealed class <RandomWanderDestFor>c__AnonStorey3
		{
			internal IntVec3 root;

			internal float radius;

			internal Pawn pawn;

			internal Func<Pawn, IntVec3, IntVec3, bool> validator;

			public <RandomWanderDestFor>c__AnonStorey3()
			{
			}

			internal bool <>m__0(Region reg)
			{
				return reg.extentsClose.ClosestDistSquaredTo(this.root) <= this.radius * this.radius;
			}

			internal bool <>m__1(IntVec3 c)
			{
				return c.InBounds(this.pawn.Map) && this.pawn.CanReach(c, PathEndMode.OnCell, Danger.None, false, TraverseMode.ByPawn) && !c.IsForbidden(this.pawn) && (this.validator == null || this.validator(this.pawn, c, this.root));
			}

			internal bool <>m__2(IntVec3 c)
			{
				return c.InBounds(this.pawn.Map) && this.pawn.CanReach(c, PathEndMode.OnCell, Danger.None, false, TraverseMode.ByPawn) && !c.IsForbidden(this.pawn);
			}

			internal bool <>m__3(IntVec3 c)
			{
				return c.InBounds(this.pawn.Map) && this.pawn.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn);
			}

			internal bool <>m__4(IntVec3 c)
			{
				return c.InBounds(this.pawn.Map) && this.pawn.CanReach(c, PathEndMode.OnCell, Danger.None, false, TraverseMode.ByPawn) && !c.IsForbidden(this.pawn);
			}

			internal bool <>m__5(IntVec3 c)
			{
				return c.InBounds(this.pawn.Map) && this.pawn.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn);
			}

			internal bool <>m__6(IntVec3 c)
			{
				return c.InBounds(this.pawn.Map) && this.pawn.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn);
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindRandomPawnEntryCell>c__AnonStorey4
		{
			internal Map map;

			internal Predicate<IntVec3> extraValidator;

			public <TryFindRandomPawnEntryCell>c__AnonStorey4()
			{
			}

			internal bool <>m__0(IntVec3 c)
			{
				return c.Standable(this.map) && !this.map.roofGrid.Roofed(c) && this.map.reachability.CanReachColony(c) && c.GetRoom(this.map, RegionType.Set_Passable).TouchesMapEdge && (this.extraValidator == null || this.extraValidator(c));
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindPrisonerReleaseCell>c__AnonStorey5
		{
			internal bool needMapEdge;

			internal IntVec3 foundResult;

			internal TraverseParms traverseParms;

			public <TryFindPrisonerReleaseCell>c__AnonStorey5()
			{
			}

			internal bool <>m__0(Region r)
			{
				if (this.needMapEdge)
				{
					if (!r.Room.TouchesMapEdge)
					{
						return false;
					}
				}
				else if (r.Room.isPrisonCell)
				{
					return false;
				}
				this.foundResult = r.RandomCell;
				return true;
			}

			internal bool <>m__1(Region from, Region r)
			{
				return r.Allows(this.traverseParms, false);
			}
		}

		[CompilerGenerated]
		private sealed class <RandomAnimalSpawnCell_MapGen>c__AnonStorey6
		{
			internal Map map;

			internal int numStand;

			internal int numRoom;

			internal int numTouch;

			public <RandomAnimalSpawnCell_MapGen>c__AnonStorey6()
			{
			}

			internal bool <>m__0(IntVec3 c)
			{
				bool result;
				if (!c.Standable(this.map))
				{
					this.numStand++;
					result = false;
				}
				else if (c.GetTerrain(this.map).avoidWander)
				{
					result = false;
				}
				else
				{
					Room room = c.GetRoom(this.map, RegionType.Set_Passable);
					if (room == null)
					{
						this.numRoom++;
						result = false;
					}
					else if (!room.TouchesMapEdge)
					{
						this.numTouch++;
						result = false;
					}
					else
					{
						result = true;
					}
				}
				return result;
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindSkygazeCell>c__AnonStorey7
		{
			internal Pawn searcher;

			internal Predicate<IntVec3> cellValidator;

			internal IntVec3 unused;

			public <TryFindSkygazeCell>c__AnonStorey7()
			{
			}

			internal bool <>m__0(IntVec3 c)
			{
				return !c.Roofed(this.searcher.Map) && !c.GetTerrain(this.searcher.Map).avoidWander;
			}

			internal bool <>m__1(Region r)
			{
				return r.Room.PsychologicallyOutdoors && !r.IsForbiddenEntirely(this.searcher) && r.TryFindRandomCellInRegionUnforbidden(this.searcher, this.cellValidator, out this.unused);
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindTravelDestFrom>c__AnonStorey8
		{
			internal Map map;

			internal IntVec3 root;

			internal Predicate<IntVec3> cellValidator;

			public <TryFindTravelDestFrom>c__AnonStorey8()
			{
			}

			internal bool <>m__0(IntVec3 c)
			{
				return this.map.reachability.CanReach(this.root, c, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.None) && !this.map.roofGrid.Roofed(c);
			}

			internal bool <>m__1(IntVec3 c)
			{
				return c.x == this.map.Size.x - 1 && this.cellValidator(c);
			}

			internal bool <>m__2(IntVec3 c)
			{
				return c.x == 0 && this.cellValidator(c);
			}

			internal bool <>m__3(IntVec3 c)
			{
				return c.z == this.map.Size.z - 1 && this.cellValidator(c);
			}

			internal bool <>m__4(IntVec3 c)
			{
				return c.z == 0 && this.cellValidator(c);
			}

			internal bool <>m__5(IntVec3 c)
			{
				return (c - this.root).LengthHorizontalSquared > 10000 && this.cellValidator(c);
			}

			internal bool <>m__6(IntVec3 c)
			{
				return (c - this.root).LengthHorizontalSquared > 2500 && this.cellValidator(c);
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindRandomSpotJustOutsideColony>c__AnonStorey9
		{
			internal Map map;

			internal IntVec3 root;

			internal Pawn searcher;

			internal bool desperate;

			internal Predicate<IntVec3> extraValidator;

			internal int minColonyBuildingsLOS;

			private static RegionEntryPredicate <>f__am$cache0;

			public <TryFindRandomSpotJustOutsideColony>c__AnonStorey9()
			{
			}

			internal bool <>m__0(IntVec3 c)
			{
				bool result;
				if (!c.Standable(this.map))
				{
					result = false;
				}
				else
				{
					Room room = c.GetRoom(this.map, RegionType.Set_Passable);
					if (!room.PsychologicallyOutdoors || !room.TouchesMapEdge)
					{
						result = false;
					}
					else if (room == null || room.CellCount < 25)
					{
						result = false;
					}
					else
					{
						if (this.root.IsValid)
						{
							TraverseParms traverseParams = (this.searcher == null) ? TraverseMode.PassDoors : TraverseParms.For(this.searcher, Danger.Deadly, TraverseMode.ByPawn, false);
							if (!this.map.reachability.CanReach(this.root, c, PathEndMode.Touch, traverseParams))
							{
								return false;
							}
						}
						if (!this.desperate)
						{
							if (!this.map.reachability.CanReachColony(c))
							{
								return false;
							}
						}
						if (this.extraValidator != null && !this.extraValidator(c))
						{
							result = false;
						}
						else
						{
							if (this.minColonyBuildingsLOS > 0)
							{
								int colonyBuildingsLOSFound = 0;
								RCellFinder.tmpBuildings.Clear();
								RegionTraverser.BreadthFirstTraverse(c, this.map, (Region from, Region to) => true, delegate(Region reg)
								{
									Faction ofPlayer = Faction.OfPlayer;
									List<Thing> list = reg.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
									for (int i = 0; i < list.Count; i++)
									{
										Thing thing = list[i];
										if (thing.Faction == ofPlayer && thing.Position.InHorDistOf(c, 16f) && GenSight.LineOfSight(thing.Position, c, this.map, true, null, 0, 0) && !RCellFinder.tmpBuildings.Contains(thing))
										{
											RCellFinder.tmpBuildings.Add(thing);
											colonyBuildingsLOSFound++;
											if (colonyBuildingsLOSFound >= this.minColonyBuildingsLOS)
											{
												return true;
											}
										}
									}
									return false;
								}, 12, RegionType.Set_Passable);
								RCellFinder.tmpBuildings.Clear();
								if (colonyBuildingsLOSFound < this.minColonyBuildingsLOS)
								{
									return false;
								}
							}
							result = true;
						}
					}
				}
				return result;
			}

			private static bool <>m__1(Region from, Region to)
			{
				return true;
			}

			private sealed class <TryFindRandomSpotJustOutsideColony>c__AnonStoreyA
			{
				internal IntVec3 c;

				internal RCellFinder.<TryFindRandomSpotJustOutsideColony>c__AnonStorey9 <>f__ref$9;

				public <TryFindRandomSpotJustOutsideColony>c__AnonStoreyA()
				{
				}
			}

			private sealed class <TryFindRandomSpotJustOutsideColony>c__AnonStoreyB
			{
				internal int colonyBuildingsLOSFound;

				internal RCellFinder.<TryFindRandomSpotJustOutsideColony>c__AnonStorey9 <>f__ref$9;

				internal RCellFinder.<TryFindRandomSpotJustOutsideColony>c__AnonStorey9.<TryFindRandomSpotJustOutsideColony>c__AnonStoreyA <>f__ref$10;

				public <TryFindRandomSpotJustOutsideColony>c__AnonStoreyB()
				{
				}

				internal bool <>m__0(Region reg)
				{
					Faction ofPlayer = Faction.OfPlayer;
					List<Thing> list = reg.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
					for (int i = 0; i < list.Count; i++)
					{
						Thing thing = list[i];
						if (thing.Faction == ofPlayer && thing.Position.InHorDistOf(this.<>f__ref$10.c, 16f) && GenSight.LineOfSight(thing.Position, this.<>f__ref$10.c, this.<>f__ref$9.map, true, null, 0, 0) && !RCellFinder.tmpBuildings.Contains(thing))
						{
							RCellFinder.tmpBuildings.Add(thing);
							this.colonyBuildingsLOSFound++;
							if (this.colonyBuildingsLOSFound >= this.<>f__ref$9.minColonyBuildingsLOS)
							{
								return true;
							}
						}
					}
					return false;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindRandomCellInRegionUnforbidden>c__AnonStoreyC
		{
			internal Pawn pawn;

			internal Predicate<IntVec3> validator;

			public <TryFindRandomCellInRegionUnforbidden>c__AnonStoreyC()
			{
			}

			internal bool <>m__0(IntVec3 c)
			{
				return !c.IsForbidden(this.pawn) && (this.validator == null || this.validator(c));
			}
		}

		[CompilerGenerated]
		private sealed class <SpotToChewStandingNear>c__AnonStoreyD
		{
			internal IntVec3 root;

			internal float maxDist;

			internal Pawn pawn;

			internal Room rootRoom;

			internal bool desperate;

			internal bool ignoreDanger;

			internal Thing ingestible;

			public <SpotToChewStandingNear>c__AnonStoreyD()
			{
			}

			internal bool <>m__0(IntVec3 c)
			{
				bool result;
				if ((float)(this.root - c).LengthHorizontalSquared > this.maxDist * this.maxDist)
				{
					result = false;
				}
				else
				{
					if (this.pawn.HostFaction != null)
					{
						if (c.GetRoom(this.pawn.Map, RegionType.Set_Passable) != this.rootRoom)
						{
							return false;
						}
					}
					if (!this.desperate)
					{
						if (!c.Standable(this.pawn.Map))
						{
							return false;
						}
						if (GenPlace.HaulPlaceBlockerIn(null, c, this.pawn.Map, false) != null)
						{
							return false;
						}
						if (c.GetRegion(this.pawn.Map, RegionType.Set_Passable).type == RegionType.Portal)
						{
							return false;
						}
					}
					IntVec3 intVec;
					result = ((this.ignoreDanger || c.GetDangerFor(this.pawn, this.pawn.Map) == Danger.None) && !c.ContainsStaticFire(this.pawn.Map) && !c.ContainsTrap(this.pawn.Map) && this.pawn.Map.pawnDestinationReservationManager.CanReserve(c, this.pawn, false) && Toils_Ingest.TryFindAdjacentIngestionPlaceSpot(c, this.ingestible.def, this.pawn, out intVec));
				}
				return result;
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindMarriageSite>c__AnonStoreyE
		{
			internal Pawn firstFiance;

			internal Pawn secondFiance;

			internal Map map;

			internal Predicate<IntVec3> noMarriageSpotValidator;

			public <TryFindMarriageSite>c__AnonStoreyE()
			{
			}

			internal bool <>m__0(Building x)
			{
				return MarriageSpotUtility.IsValidMarriageSpotFor(x.Position, this.firstFiance, this.secondFiance, null);
			}

			internal bool <>m__1(IntVec3 cell)
			{
				IntVec3 c = cell + LordToil_MarriageCeremony.OtherFianceNoMarriageSpotCellOffset;
				bool result;
				if (!c.InBounds(this.map))
				{
					result = false;
				}
				else if (c.IsForbidden(this.firstFiance) || c.IsForbidden(this.secondFiance))
				{
					result = false;
				}
				else if (!c.Standable(this.map))
				{
					result = false;
				}
				else
				{
					Room room = cell.GetRoom(this.map, RegionType.Set_Passable);
					result = (room == null || room.IsHuge || room.PsychologicallyOutdoors || room.CellCount >= 10);
				}
				return result;
			}

			internal bool <>m__2(IntVec3 cell)
			{
				return MarriageSpotUtility.IsValidMarriageSpotFor(cell, this.firstFiance, this.secondFiance, null) && this.noMarriageSpotValidator(cell);
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindPartySpot>c__AnonStoreyF
		{
			internal Map map;

			internal Pawn organizer;

			internal bool enjoyableOutside;

			internal Predicate<IntVec3> baseValidator;

			internal Predicate<IntVec3> noPartySpotValidator;

			public <TryFindPartySpot>c__AnonStoreyF()
			{
			}

			internal bool <>m__0(IntVec3 cell)
			{
				bool result;
				if (!cell.Standable(this.map))
				{
					result = false;
				}
				else if (cell.GetDangerFor(this.organizer, this.map) != Danger.None)
				{
					result = false;
				}
				else if (!this.enjoyableOutside && !cell.Roofed(this.map))
				{
					result = false;
				}
				else if (cell.IsForbidden(this.organizer))
				{
					result = false;
				}
				else if (!this.organizer.CanReserveAndReach(cell, PathEndMode.OnCell, Danger.None, 1, -1, null, false))
				{
					result = false;
				}
				else
				{
					Room room = cell.GetRoom(this.map, RegionType.Set_Passable);
					bool flag = room != null && room.isPrisonCell;
					result = (this.organizer.IsPrisoner == flag && PartyUtility.EnoughPotentialGuestsToStartParty(this.map, new IntVec3?(cell)));
				}
				return result;
			}

			internal bool <>m__1(Building x)
			{
				return this.baseValidator(x.Position);
			}

			internal bool <>m__2(IntVec3 cell)
			{
				Room room = cell.GetRoom(this.map, RegionType.Set_Passable);
				return room == null || room.IsHuge || room.PsychologicallyOutdoors || room.CellCount >= 10;
			}

			internal bool <>m__3(IntVec3 cell)
			{
				return this.baseValidator(cell) && this.noPartySpotValidator(cell);
			}
		}

		[CompilerGenerated]
		private sealed class <FindSiegePositionFrom>c__AnonStorey10
		{
			internal Map map;

			public <FindSiegePositionFrom>c__AnonStorey10()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return x.Standable(this.map) && !x.Fogged(this.map);
			}
		}
	}
}
