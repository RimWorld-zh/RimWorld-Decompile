using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class RCellFinder
	{
		private static List<Region> regions = new List<Region>();

		private static HashSet<Thing> tmpBuildings = new HashSet<Thing>();

		public static IntVec3 BestOrderedGotoDestNear(IntVec3 root, Pawn searcher)
		{
			Map map = searcher.Map;
			Predicate<IntVec3> predicate = (Predicate<IntVec3>)((IntVec3 c) => !map.pawnDestinationManager.DestinationIsReserved(c, searcher) && c.Standable(map) && searcher.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn));
			if (predicate(root))
			{
				return root;
			}
			int num = 1;
			IntVec3 result = default(IntVec3);
			float num2 = -1000f;
			bool flag = false;
			while (true)
			{
				IntVec3 intVec = root + GenRadial.RadialPattern[num];
				if (predicate(intVec))
				{
					float num3 = CoverUtility.TotalSurroundingCoverScore(intVec, map);
					if (num3 > num2)
					{
						num2 = num3;
						result = intVec;
						flag = true;
					}
				}
				if (num >= 8 && flag)
					break;
				num++;
			}
			return result;
		}

		public static bool TryFindBestExitSpot(Pawn pawn, out IntVec3 spot, TraverseMode mode = TraverseMode.ByPawn)
		{
			if (mode == TraverseMode.PassAllDestroyableThings && !pawn.Map.reachability.CanReachMapEdge(pawn.Position, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, true)))
			{
				return RCellFinder.TryFindRandomPawnEntryCell(out spot, pawn.Map, 0f, (Predicate<IntVec3>)delegate(IntVec3 x)
				{
					TraverseMode mode2 = mode;
					return pawn.CanReach(x, PathEndMode.OnCell, Danger.Deadly, false, mode2);
				});
			}
			int num = 0;
			int num2 = 0;
			IntVec3 intVec2;
			while (true)
			{
				num2++;
				if (num2 > 30)
				{
					spot = pawn.Position;
					return false;
				}
				IntVec3 intVec = default(IntVec3);
				bool flag = CellFinder.TryFindRandomCellNear(pawn.Position, pawn.Map, num, (Predicate<IntVec3>)null, out intVec);
				num += 4;
				if (flag)
				{
					int num3 = intVec.x;
					intVec2 = new IntVec3(0, 0, intVec.z);
					IntVec3 size = pawn.Map.Size;
					if (size.z - intVec.z < num3)
					{
						IntVec3 size2 = pawn.Map.Size;
						num3 = size2.z - intVec.z;
						int x2 = intVec.x;
						IntVec3 size3 = pawn.Map.Size;
						intVec2 = new IntVec3(x2, 0, size3.z - 1);
					}
					IntVec3 size4 = pawn.Map.Size;
					if (size4.x - intVec.x < num3)
					{
						IntVec3 size5 = pawn.Map.Size;
						num3 = size5.x - intVec.x;
						IntVec3 size6 = pawn.Map.Size;
						intVec2 = new IntVec3(size6.x - 1, 0, intVec.z);
					}
					if (intVec.z < num3)
					{
						intVec2 = new IntVec3(intVec.x, 0, 0);
					}
					if (intVec2.Standable(pawn.Map) && pawn.CanReach(intVec2, PathEndMode.OnCell, Danger.Deadly, true, mode))
						break;
				}
			}
			spot = intVec2;
			return true;
		}

		public static bool TryFindRandomExitSpot(Pawn pawn, out IntVec3 spot, TraverseMode mode = TraverseMode.ByPawn)
		{
			Danger maxDanger = Danger.Some;
			int num = 0;
			goto IL_0004;
			IL_0004:
			IntVec3 intVec;
			while (true)
			{
				num++;
				if (num > 40)
				{
					spot = pawn.Position;
					return false;
				}
				if (num > 15)
				{
					maxDanger = Danger.Deadly;
				}
				intVec = CellFinder.RandomCell(pawn.Map);
				int num2 = Rand.RangeInclusive(0, 3);
				if (num2 == 0)
				{
					intVec.x = 0;
				}
				if (num2 == 1)
				{
					IntVec3 size = pawn.Map.Size;
					intVec.x = size.x - 1;
				}
				if (num2 == 2)
				{
					intVec.z = 0;
				}
				if (num2 == 3)
				{
					IntVec3 size2 = pawn.Map.Size;
					intVec.z = size2.z - 1;
				}
				if (intVec.Standable(pawn.Map) && pawn.CanReach(intVec, PathEndMode.OnCell, maxDanger, false, mode))
					break;
			}
			spot = intVec;
			return true;
			IL_00de:
			goto IL_0004;
		}

		public static IntVec3 RandomWanderDestFor(Pawn pawn, IntVec3 root, float radius, Func<Pawn, IntVec3, bool> validator, Danger maxDanger)
		{
			if (radius > 12.0)
			{
				Log.Warning("wanderRadius of " + radius + " is greater than Region.GridSize of " + 12 + " and will break.");
			}
			if (root.GetRegion(pawn.Map, RegionType.Set_Passable) == null)
			{
				return root;
			}
			int maxRegions = Mathf.Max((int)radius / 3, 13);
			CellFinder.AllRegionsNear(RCellFinder.regions, root.GetRegion(pawn.Map, RegionType.Set_Passable), maxRegions, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), (Predicate<Region>)((Region reg) => reg.extentsClose.ClosestDistSquaredTo(root) <= radius * radius), null, RegionType.Set_Passable);
			bool flag = UnityData.isDebugBuild && DebugViewSettings.drawDestSearch;
			if (flag)
			{
				pawn.Map.debugDrawer.FlashCell(root, 0.6f, "root");
			}
			if (RCellFinder.regions.Count > 0)
			{
				for (int i = 0; i < 20; i++)
				{
					IntVec3 randomCell = RCellFinder.regions.RandomElementByWeightWithFallback((Func<Region, float>)((Region reg) => (float)reg.CellCount), null).RandomCell;
					if ((float)randomCell.DistanceToSquared(root) > radius * radius)
					{
						if (flag)
						{
							pawn.Map.debugDrawer.FlashCell(randomCell, 0.32f, "distance");
						}
						continue;
					}
					if (!RCellFinder.CanWanderToCell(randomCell, pawn, root, validator, i, maxDanger))
					{
						if (flag)
						{
							pawn.Map.debugDrawer.FlashCell(randomCell, 0.6f, "validation");
						}
						continue;
					}
					if (flag)
					{
						pawn.Map.debugDrawer.FlashCell(randomCell, 0.9f, "go!");
					}
					return randomCell;
				}
			}
			IntVec3 position = default(IntVec3);
			if (!CellFinder.TryFindRandomCellNear(root, pawn.Map, 20, (Predicate<IntVec3>)((IntVec3 c) => c.InBounds(pawn.Map) && pawn.CanReach(c, PathEndMode.OnCell, Danger.None, false, TraverseMode.ByPawn) && !c.IsForbidden(pawn)), out position) && !CellFinder.TryFindRandomCellNear(root, pawn.Map, 30, (Predicate<IntVec3>)((IntVec3 c) => c.InBounds(pawn.Map) && pawn.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn)), out position) && !CellFinder.TryFindRandomCellNear(pawn.Position, pawn.Map, 5, (Predicate<IntVec3>)((IntVec3 c) => c.InBounds(pawn.Map) && pawn.CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn)), out position))
			{
				position = pawn.Position;
			}
			if (flag)
			{
				pawn.Map.debugDrawer.FlashCell(position, 0.4f, "fallback");
			}
			return position;
		}

		private static bool CanWanderToCell(IntVec3 c, Pawn pawn, IntVec3 root, Func<Pawn, IntVec3, bool> validator, int tryIndex, Danger maxDanger)
		{
			bool flag = UnityData.isDebugBuild && DebugViewSettings.drawDestSearch;
			if (!c.Walkable(pawn.Map))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0f, "walk");
				}
				return false;
			}
			if (c.IsForbidden(pawn))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.25f, "forbid");
				}
				return false;
			}
			if (tryIndex < 10 && !c.Standable(pawn.Map))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.25f, "stand");
				}
				return false;
			}
			if (!pawn.CanReach(c, PathEndMode.OnCell, maxDanger, false, TraverseMode.ByPawn))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.6f, "reach");
				}
				return false;
			}
			if (RCellFinder.ContainsKnownTrap(c, pawn.Map, pawn))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.1f, "trap");
				}
				return false;
			}
			if (tryIndex < 10)
			{
				if (c.GetTerrain(pawn.Map).avoidWander)
				{
					if (flag)
					{
						pawn.Map.debugDrawer.FlashCell(c, 0.39f, "terr");
					}
					return false;
				}
				if (pawn.Map.pathGrid.PerceivedPathCostAt(c) > 20)
				{
					if (flag)
					{
						pawn.Map.debugDrawer.FlashCell(c, 0.4f, "pcost");
					}
					return false;
				}
				if ((int)c.GetDangerFor(pawn, pawn.Map) > 1)
				{
					if (flag)
					{
						pawn.Map.debugDrawer.FlashCell(c, 0.4f, "danger");
					}
					return false;
				}
			}
			else if (tryIndex < 15 && c.GetDangerFor(pawn, pawn.Map) == Danger.Deadly)
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.4f, "deadly");
				}
				return false;
			}
			if (pawn.Map.pawnDestinationManager.DestinationIsReserved(c, pawn))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.75f, "resvd");
				}
				return false;
			}
			if ((object)validator != null && !validator(pawn, c))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.15f, "valid");
				}
				return false;
			}
			if (c.GetDoor(pawn.Map) != null)
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.32f, "door");
				}
				return false;
			}
			if (c.ContainsStaticFire(pawn.Map))
			{
				if (flag)
				{
					pawn.Map.debugDrawer.FlashCell(c, 0.9f, "fire");
				}
				return false;
			}
			return true;
		}

		private static bool ContainsKnownTrap(IntVec3 c, Map map, Pawn pawn)
		{
			Building edifice = c.GetEdifice(map);
			if (edifice != null)
			{
				Building_Trap building_Trap = edifice as Building_Trap;
				if (building_Trap != null && building_Trap.Armed && building_Trap.KnowsOfTrap(pawn))
				{
					return true;
				}
			}
			return false;
		}

		public static bool TryFindGoodAdjacentSpotToTouch(Pawn toucher, Thing touchee, out IntVec3 result)
		{
			foreach (IntVec3 item in GenAdj.CellsAdjacent8Way(touchee).InRandomOrder(null))
			{
				if (item.Standable(toucher.Map) && !RCellFinder.ContainsKnownTrap(item, toucher.Map, toucher))
				{
					result = item;
					return true;
				}
			}
			foreach (IntVec3 item2 in GenAdj.CellsAdjacent8Way(touchee).InRandomOrder(null))
			{
				if (item2.Walkable(toucher.Map))
				{
					result = item2;
					return true;
				}
			}
			result = touchee.Position;
			return false;
		}

		public static bool TryFindRandomPawnEntryCell(out IntVec3 result, Map map, float roadChance, Predicate<IntVec3> extraValidator = null)
		{
			return CellFinder.TryFindRandomEdgeCellWith((Predicate<IntVec3>)((IntVec3 c) => c.Standable(map) && !map.roofGrid.Roofed(c) && map.reachability.CanReachColony(c) && c.GetRoom(map, RegionType.Set_Passable).TouchesMapEdge && ((object)extraValidator == null || extraValidator(c))), map, roadChance, out result);
		}

		public static bool TryFindPrisonerReleaseCell(Pawn prisoner, Pawn warden, out IntVec3 result)
		{
			if (prisoner.Map != warden.Map)
			{
				result = IntVec3.Invalid;
				return false;
			}
			Region region = prisoner.GetRegion(RegionType.Set_Passable);
			if (region == null)
			{
				result = default(IntVec3);
				return false;
			}
			TraverseParms traverseParms = TraverseParms.For(warden, Danger.Deadly, TraverseMode.ByPawn, false);
			bool needMapEdge = prisoner.Faction != warden.Faction;
			IntVec3 foundResult = IntVec3.Invalid;
			RegionProcessor regionProcessor = (RegionProcessor)delegate(Region r)
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
			RegionTraverser.BreadthFirstTraverse(region, (RegionEntryPredicate)((Region from, Region r) => r.Allows(traverseParms, false)), regionProcessor, 999, RegionType.Set_Passable);
			if (foundResult.IsValid)
			{
				result = foundResult;
				return true;
			}
			result = default(IntVec3);
			return false;
		}

		public static bool TryFindRandomCellToPlantInFromOffMap(ThingDef plantDef, Map map, out IntVec3 plantCell)
		{
			Predicate<IntVec3> validator = (Predicate<IntVec3>)delegate(IntVec3 c)
			{
				if (c.Roofed(map))
				{
					return false;
				}
				if (!plantDef.CanEverPlantAt(c, map))
				{
					return false;
				}
				Room room = c.GetRoom(map, RegionType.Set_Passable);
				if (room != null && room.TouchesMapEdge)
				{
					return true;
				}
				return false;
			};
			return CellFinder.TryFindRandomEdgeCellWith(validator, map, CellFinder.EdgeRoadChance_Animal, out plantCell);
		}

		public static IntVec3 RandomAnimalSpawnCell_MapGen(Map map)
		{
			int numStand = 0;
			int numRoom = 0;
			int numTouch = 0;
			Predicate<IntVec3> validator = (Predicate<IntVec3>)delegate(IntVec3 c)
			{
				if (!c.Standable(map))
				{
					numStand++;
					return false;
				}
				if (c.GetTerrain(map).avoidWander)
				{
					return false;
				}
				Room room = c.GetRoom(map, RegionType.Set_Passable);
				if (room == null)
				{
					numRoom++;
					return false;
				}
				if (!room.TouchesMapEdge)
				{
					numTouch++;
					return false;
				}
				return true;
			};
			IntVec3 intVec = default(IntVec3);
			if (!CellFinderLoose.TryGetRandomCellWith(validator, map, 1000, out intVec))
			{
				intVec = CellFinder.RandomCell(map);
				Log.Warning("RandomAnimalSpawnCell_MapGen failed: numStand=" + numStand + ", numRoom=" + numRoom + ", numTouch=" + numTouch + ". PlayerStartSpot=" + MapGenerator.PlayerStartSpot + ". Returning " + intVec);
			}
			return intVec;
		}

		public static bool TryFindSkygazeCell(IntVec3 root, Pawn searcher, out IntVec3 result)
		{
			Predicate<IntVec3> cellValidator = (Predicate<IntVec3>)((IntVec3 c) => !c.Roofed(searcher.Map) && !c.GetTerrain(searcher.Map).avoidWander);
			IntVec3 unused;
			Predicate<Region> validator = (Predicate<Region>)((Region r) => r.Room.PsychologicallyOutdoors && !r.IsForbiddenEntirely(searcher) && r.TryFindRandomCellInRegionUnforbidden(searcher, cellValidator, out unused));
			TraverseParms traverseParms = TraverseParms.For(searcher, Danger.Deadly, TraverseMode.ByPawn, false);
			Region root2 = default(Region);
			if (!CellFinder.TryFindClosestRegionWith(root.GetRegion(searcher.Map, RegionType.Set_Passable), traverseParms, validator, 300, out root2, RegionType.Set_Passable))
			{
				result = root;
				return false;
			}
			Region reg = CellFinder.RandomRegionNear(root2, 14, traverseParms, validator, searcher, RegionType.Set_Passable);
			return reg.TryFindRandomCellInRegionUnforbidden(searcher, cellValidator, out result);
		}

		public static bool TryFindTravelDestFrom(IntVec3 root, Map map, out IntVec3 travelDest)
		{
			travelDest = root;
			bool flag = false;
			Predicate<IntVec3> cellValidator = (Predicate<IntVec3>)((IntVec3 c) => map.reachability.CanReach(root, c, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.None) && !map.roofGrid.Roofed(c));
			if (root.x == 0)
			{
				flag = CellFinder.TryFindRandomEdgeCellWith((Predicate<IntVec3>)delegate(IntVec3 c)
				{
					int x2 = c.x;
					IntVec3 size4 = map.Size;
					return x2 == size4.x - 1 && cellValidator(c);
				}, map, CellFinder.EdgeRoadChance_Always, out travelDest);
			}
			else
			{
				int x = root.x;
				IntVec3 size = map.Size;
				if (x == size.x - 1)
				{
					flag = CellFinder.TryFindRandomEdgeCellWith((Predicate<IntVec3>)((IntVec3 c) => c.x == 0 && cellValidator(c)), map, CellFinder.EdgeRoadChance_Always, out travelDest);
				}
				else if (root.z == 0)
				{
					flag = CellFinder.TryFindRandomEdgeCellWith((Predicate<IntVec3>)delegate(IntVec3 c)
					{
						int z2 = c.z;
						IntVec3 size3 = map.Size;
						return z2 == size3.z - 1 && cellValidator(c);
					}, map, CellFinder.EdgeRoadChance_Always, out travelDest);
				}
				else
				{
					int z = root.z;
					IntVec3 size2 = map.Size;
					if (z == size2.z - 1)
					{
						flag = CellFinder.TryFindRandomEdgeCellWith((Predicate<IntVec3>)((IntVec3 c) => c.z == 0 && cellValidator(c)), map, CellFinder.EdgeRoadChance_Always, out travelDest);
					}
				}
			}
			if (!flag)
			{
				flag = CellFinder.TryFindRandomEdgeCellWith((Predicate<IntVec3>)((IntVec3 c) => (c - root).LengthHorizontalSquared > 10000 && cellValidator(c)), map, CellFinder.EdgeRoadChance_Always, out travelDest);
			}
			if (!flag)
			{
				flag = CellFinder.TryFindRandomEdgeCellWith((Predicate<IntVec3>)((IntVec3 c) => (c - root).LengthHorizontalSquared > 2500 && cellValidator(c)), map, CellFinder.EdgeRoadChance_Always, out travelDest);
			}
			return flag;
		}

		public static bool TryFindRandomSpotJustOutsideColony(IntVec3 originCell, Map map, out IntVec3 result)
		{
			return RCellFinder.TryFindRandomSpotJustOutsideColony(originCell, map, (Pawn)null, out result, (Predicate<IntVec3>)null);
		}

		public static bool TryFindRandomSpotJustOutsideColony(Pawn searcher, out IntVec3 result)
		{
			return RCellFinder.TryFindRandomSpotJustOutsideColony(searcher.Position, searcher.Map, searcher, out result, (Predicate<IntVec3>)null);
		}

		public static bool TryFindRandomSpotJustOutsideColony(IntVec3 root, Map map, Pawn searcher, out IntVec3 result, Predicate<IntVec3> extraValidator = null)
		{
			bool desperate = false;
			int minColonyBuildingsLOS = 0;
			Predicate<IntVec3> validator = (Predicate<IntVec3>)delegate(IntVec3 c)
			{
				if (!c.Standable(map))
				{
					return false;
				}
				Room room = c.GetRoom(map, RegionType.Set_Passable);
				if (room.PsychologicallyOutdoors && room.TouchesMapEdge)
				{
					if (room != null && room.CellCount >= 25)
					{
						if (!desperate && !map.reachability.CanReachColony(c))
						{
							return false;
						}
						if ((object)extraValidator != null && !extraValidator(c))
						{
							return false;
						}
						if (minColonyBuildingsLOS > 0)
						{
							int colonyBuildingsLOSFound = 0;
							RCellFinder.tmpBuildings.Clear();
							RegionTraverser.BreadthFirstTraverse(c, map, (RegionEntryPredicate)((Region from, Region to) => true), (RegionProcessor)delegate(Region reg)
							{
								Faction ofPlayer = Faction.OfPlayer;
								List<Thing> list = reg.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
								for (int i = 0; i < list.Count; i++)
								{
									Thing thing = list[i];
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
						if (root.IsValid)
						{
							TraverseParms traverseParams = (searcher == null) ? TraverseMode.PassDoors : TraverseParms.For(searcher, Danger.Deadly, TraverseMode.ByPawn, false);
							if (!map.reachability.CanReach(root, c, PathEndMode.Touch, traverseParams))
							{
								return false;
							}
						}
						return true;
					}
					return false;
				}
				return false;
			};
			int num = 0;
			while (num < 100)
			{
				Building building = null;
				if ((from b in map.listerBuildings.allBuildingsColonist
				where b.def.designationCategory != DesignationCategoryDefOf.Structure && b.def.building.ai_chillDestination
				select b).TryRandomElement<Building>(out building))
				{
					if (num < 10)
					{
						minColonyBuildingsLOS = 4;
					}
					else if (num < 25)
					{
						minColonyBuildingsLOS = 3;
					}
					else if (num < 40)
					{
						minColonyBuildingsLOS = 2;
					}
					else
					{
						minColonyBuildingsLOS = 1;
					}
					int squareRadius = 10 + num / 5;
					desperate = (num > 60);
					if (CellFinder.TryFindRandomCellNear(building.Position, map, squareRadius, validator, out result))
					{
						return true;
					}
					num++;
					continue;
				}
				break;
			}
			int num2 = 0;
			while (num2 < 50)
			{
				Building building2 = null;
				if (((IEnumerable<Building>)map.listerBuildings.allBuildingsColonist).TryRandomElement<Building>(out building2))
				{
					if (num2 < 10)
					{
						minColonyBuildingsLOS = 3;
					}
					else if (num2 < 20)
					{
						minColonyBuildingsLOS = 2;
					}
					else if (num2 < 30)
					{
						minColonyBuildingsLOS = 1;
					}
					else
					{
						minColonyBuildingsLOS = 0;
					}
					desperate = (num2 > 20);
					if (CellFinder.TryFindRandomCellNear(building2.Position, map, 14, validator, out result))
					{
						return true;
					}
					num2++;
					continue;
				}
				break;
			}
			int num3 = 0;
			while (num3 < 100)
			{
				Pawn pawn = null;
				if (map.mapPawns.FreeColonistsAndPrisonersSpawned.TryRandomElement<Pawn>(out pawn))
				{
					minColonyBuildingsLOS = 0;
					desperate = (num3 > 50);
					if (CellFinder.TryFindRandomCellNear(pawn.Position, map, 14, validator, out result))
					{
						return true;
					}
					num3++;
					continue;
				}
				break;
			}
			desperate = true;
			minColonyBuildingsLOS = 0;
			if (CellFinderLoose.TryGetRandomCellWith(validator, map, 1000, out result))
			{
				return true;
			}
			return false;
		}

		public static bool TryFindRandomCellInRegionUnforbidden(this Region reg, Pawn pawn, Predicate<IntVec3> validator, out IntVec3 result)
		{
			if (reg == null)
			{
				throw new ArgumentNullException("reg");
			}
			if (reg.IsForbiddenEntirely(pawn))
			{
				result = IntVec3.Invalid;
				return false;
			}
			return reg.TryFindRandomCellInRegion((Predicate<IntVec3>)((IntVec3 c) => !c.IsForbidden(pawn) && ((object)validator == null || validator(c))), out result);
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
					using (PawnPath path = pawn.Map.pathFinder.FindPath(pawn.Position, randomCell, pawn, PathEndMode.OnCell))
					{
						if (PawnPathUtility.TryFindCellAtIndex(path, (int)dist + 3, out result))
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
			if (minDistToColony > 0.0)
			{
				foreach (Pawn item in map.mapPawns.FreeColonistsSpawned)
				{
					list.Add(item.Position);
				}
				List<Building>.Enumerator enumerator2 = map.listerBuildings.allBuildingsColonist.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						Building current2 = enumerator2.Current;
						list.Add(current2.Position);
					}
				}
				finally
				{
					((IDisposable)(object)enumerator2).Dispose();
				}
			}
			float num2 = minDistToColony * minDistToColony;
			int num3 = 0;
			goto IL_00c6;
			IL_00c6:
			while (true)
			{
				num3++;
				if (num3 > 50)
				{
					int num4 = num;
					IntVec3 size = map.Size;
					if (num4 <= size.x)
					{
						num = (int)((float)num * 1.5);
						cellRect = CellRect.CenteredOn(map.Center, num);
						cellRect.ClipInsideMap(map);
						num3 = 0;
						goto IL_0112;
					}
					break;
				}
				goto IL_0112;
				IL_0112:
				IntVec3 randomCell = cellRect.RandomCell;
				if (!randomCell.Standable(map))
					continue;
				if (!map.reachability.CanReach(randomCell, pos, PathEndMode.ClosestTouch, TraverseMode.NoPassClosedDoors, Danger.Deadly))
					continue;
				bool flag = false;
				int num5 = 0;
				while (num5 < list.Count)
				{
					if (!((float)(list[num5] - randomCell).LengthHorizontalSquared < num2))
					{
						num5++;
						continue;
					}
					flag = true;
					break;
				}
				if (flag)
					continue;
				result = randomCell;
				return true;
			}
			result = pos;
			return false;
			IL_01a9:
			goto IL_00c6;
		}

		public static bool TryFindRandomCellNearTheCenterOfTheMapWith(Predicate<IntVec3> validator, Map map, out IntVec3 result)
		{
			IntVec3 size = map.Size;
			int x = size.x;
			IntVec3 size2 = map.Size;
			int startingSearchRadius = Mathf.Clamp(Mathf.Max(x, size2.z) / 20, 3, 25);
			return RCellFinder.TryFindRandomCellNearWith(map.Center, validator, map, out result, startingSearchRadius);
		}

		public static bool TryFindRandomCellNearWith(IntVec3 near, Predicate<IntVec3> validator, Map map, out IntVec3 result, int startingSearchRadius = 5)
		{
			int num = startingSearchRadius;
			CellRect cellRect = CellRect.CenteredOn(near, num);
			cellRect.ClipInsideMap(map);
			int num2 = 0;
			goto IL_0016;
			IL_0016:
			while (true)
			{
				num2++;
				if (num2 > 30)
				{
					int num3 = num;
					IntVec3 size = map.Size;
					if (num3 > size.x * 2)
					{
						int num4 = num;
						IntVec3 size2 = map.Size;
						if (num4 > size2.z * 2)
							break;
					}
					num = (int)((float)num * 1.5);
					cellRect = CellRect.CenteredOn(near, num);
					cellRect.ClipInsideMap(map);
					num2 = 0;
				}
				IntVec3 randomCell = cellRect.RandomCell;
				if (!validator(randomCell))
					continue;
				result = randomCell;
				return true;
			}
			result = near;
			return false;
			IL_0094:
			goto IL_0016;
		}

		public static IntVec3 SpotToChewStandingNear(Pawn pawn, Thing ingestible)
		{
			IntVec3 root = pawn.Position;
			Room rootRoom = pawn.GetRoom(RegionType.Set_Passable);
			bool desperate = false;
			bool ignoreDanger = false;
			float maxDist = 4f;
			Predicate<IntVec3> validator = (Predicate<IntVec3>)delegate(IntVec3 c)
			{
				if ((float)(root - c).LengthHorizontalSquared > maxDist * maxDist)
				{
					return false;
				}
				if (pawn.HostFaction != null && c.GetRoom(pawn.Map, RegionType.Set_Passable) != rootRoom)
				{
					return false;
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
				if (!ignoreDanger && c.GetDangerFor(pawn, pawn.Map) != Danger.None)
				{
					return false;
				}
				if (!c.ContainsStaticFire(pawn.Map) && !c.ContainsTrap(pawn.Map))
				{
					if (pawn.Map.pawnDestinationManager.DestinationIsReserved(c, pawn))
					{
						return false;
					}
					IntVec3 intVec2 = default(IntVec3);
					if (!Toils_Ingest.TryFindAdjacentIngestionPlaceSpot(c, ingestible.def, pawn, out intVec2))
					{
						return false;
					}
					return true;
				}
				return false;
			};
			int maxRegions = 1;
			Region region = pawn.GetRegion(RegionType.Set_Passable);
			for (int i = 0; i < 30; i++)
			{
				switch (i)
				{
				case 1:
				{
					desperate = true;
					break;
				}
				case 2:
				{
					desperate = false;
					maxRegions = 4;
					break;
				}
				case 6:
				{
					desperate = true;
					break;
				}
				case 10:
				{
					desperate = false;
					maxDist = 8f;
					maxRegions = 12;
					break;
				}
				case 15:
				{
					desperate = true;
					break;
				}
				case 20:
				{
					maxDist = 15f;
					maxRegions = 16;
					break;
				}
				case 26:
				{
					maxDist = 5f;
					maxRegions = 4;
					ignoreDanger = true;
					break;
				}
				case 29:
				{
					maxDist = 15f;
					maxRegions = 16;
					break;
				}
				}
				Region reg = CellFinder.RandomRegionNear(region, maxRegions, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), null, null, RegionType.Set_Passable);
				IntVec3 intVec = default(IntVec3);
				if (reg.TryFindRandomCellInRegionUnforbidden(pawn, validator, out intVec))
				{
					if (DebugViewSettings.drawDestSearch)
					{
						pawn.Map.debugDrawer.FlashCell(intVec, 0.5f, "go!");
					}
					return intVec;
				}
				if (DebugViewSettings.drawDestSearch)
				{
					pawn.Map.debugDrawer.FlashCell(intVec, 0f, i.ToString());
				}
			}
			return region.RandomCell;
		}

		public static bool TryFindMarriageSite(Pawn firstFiance, Pawn secondFiance, out IntVec3 result)
		{
			if (!firstFiance.CanReach((Thing)secondFiance, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				result = IntVec3.Invalid;
				return false;
			}
			Map map = firstFiance.Map;
			if ((from x in map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.MarriageSpot)
			where MarriageSpotUtility.IsValidMarriageSpotFor(x.Position, firstFiance, secondFiance, null)
			select x.Position).TryRandomElement<IntVec3>(out result))
			{
				return true;
			}
			Predicate<IntVec3> noMarriageSpotValidator = (Predicate<IntVec3>)delegate(IntVec3 cell)
			{
				IntVec3 c = cell + LordToil_MarriageCeremony.OtherFianceNoMarriageSpotCellOffset;
				if (!c.InBounds(map))
				{
					return false;
				}
				if (!c.IsForbidden(firstFiance) && !c.IsForbidden(secondFiance))
				{
					if (!c.Standable(map))
					{
						return false;
					}
					Room room = cell.GetRoom(map, RegionType.Set_Passable);
					if (room != null && !room.IsHuge && !room.PsychologicallyOutdoors && room.CellCount < 10)
					{
						return false;
					}
					return true;
				}
				return false;
			};
			foreach (CompGatherSpot item in map.gatherSpotLister.activeSpots.InRandomOrder(null))
			{
				for (int i = 0; i < 10; i++)
				{
					IntVec3 intVec = CellFinder.RandomClosewalkCellNear(item.parent.Position, item.parent.Map, 4, null);
					if (MarriageSpotUtility.IsValidMarriageSpotFor(intVec, firstFiance, secondFiance, null) && noMarriageSpotValidator(intVec))
					{
						result = intVec;
						return true;
					}
				}
			}
			if (CellFinder.TryFindRandomCellNear(firstFiance.Position, firstFiance.Map, 25, (Predicate<IntVec3>)((IntVec3 cell) => MarriageSpotUtility.IsValidMarriageSpotFor(cell, firstFiance, secondFiance, null) && noMarriageSpotValidator(cell)), out result))
			{
				return true;
			}
			result = IntVec3.Invalid;
			return false;
		}

		public static bool TryFindPartySpot(Pawn organizer, out IntVec3 result)
		{
			bool enjoyableOutside = JoyUtility.EnjoyableOutsideNow(organizer, null);
			Map map = organizer.Map;
			Predicate<IntVec3> baseValidator = (Predicate<IntVec3>)delegate(IntVec3 cell)
			{
				if (!cell.Standable(map))
				{
					return false;
				}
				if (cell.GetDangerFor(organizer, map) != Danger.None)
				{
					return false;
				}
				if (!enjoyableOutside && !cell.Roofed(map))
				{
					return false;
				}
				if (cell.IsForbidden(organizer))
				{
					return false;
				}
				if (!organizer.CanReserveAndReach(cell, PathEndMode.OnCell, Danger.None, 1, -1, null, false))
				{
					return false;
				}
				Room room2 = cell.GetRoom(map, RegionType.Set_Passable);
				bool flag = room2 != null && room2.isPrisonCell;
				if (organizer.IsPrisoner != flag)
				{
					return false;
				}
				return true;
			};
			if ((from x in map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.PartySpot)
			where baseValidator(x.Position)
			select x.Position).TryRandomElement<IntVec3>(out result))
			{
				return true;
			}
			Predicate<IntVec3> noPartySpotValidator = (Predicate<IntVec3>)delegate(IntVec3 cell)
			{
				Room room = cell.GetRoom(map, RegionType.Set_Passable);
				if (room != null && !room.IsHuge && !room.PsychologicallyOutdoors && room.CellCount < 10)
				{
					return false;
				}
				return true;
			};
			foreach (CompGatherSpot item in map.gatherSpotLister.activeSpots.InRandomOrder(null))
			{
				for (int i = 0; i < 10; i++)
				{
					IntVec3 intVec = CellFinder.RandomClosewalkCellNear(item.parent.Position, item.parent.Map, 4, null);
					if (baseValidator(intVec) && noPartySpotValidator(intVec))
					{
						result = intVec;
						return true;
					}
				}
			}
			if (CellFinder.TryFindRandomCellNear(organizer.Position, organizer.Map, 25, (Predicate<IntVec3>)((IntVec3 cell) => baseValidator(cell) && noPartySpotValidator(cell)), out result))
			{
				return true;
			}
			result = IntVec3.Invalid;
			return false;
		}

		internal static IntVec3 FindSiegePositionFrom(IntVec3 entrySpot, Map map)
		{
			for (int num = 70; num >= 20; num -= 10)
			{
				IntVec3 result = default(IntVec3);
				if (RCellFinder.TryFindSiegePosition(entrySpot, (float)num, map, out result))
				{
					return result;
				}
			}
			Log.Error("Could not find siege spot from " + entrySpot + ", using " + entrySpot);
			return entrySpot;
		}

		private static bool TryFindSiegePosition(IntVec3 entrySpot, float minDistToColony, Map map, out IntVec3 result)
		{
			CellRect cellRect = CellRect.CenteredOn(entrySpot, 60);
			cellRect.ClipInsideMap(map);
			cellRect = cellRect.ContractedBy(14);
			List<IntVec3> list = new List<IntVec3>();
			foreach (Pawn item in map.mapPawns.FreeColonistsSpawned)
			{
				list.Add(item.Position);
			}
			HashSet<Building>.Enumerator enumerator2 = map.listerBuildings.allBuildingsColonistCombatTargets.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					Building current2 = enumerator2.Current;
					list.Add(current2.Position);
				}
			}
			finally
			{
				((IDisposable)(object)enumerator2).Dispose();
			}
			float num = minDistToColony * minDistToColony;
			int num2 = 0;
			goto IL_00b9;
			IL_00b9:
			while (true)
			{
				num2++;
				if (num2 <= 200)
				{
					IntVec3 randomCell = cellRect.RandomCell;
					if (!randomCell.Standable(map))
						continue;
					if (!randomCell.SupportsStructureType(map, TerrainAffordance.Heavy))
						continue;
					if (!randomCell.SupportsStructureType(map, TerrainAffordance.Light))
						continue;
					if (!map.reachability.CanReach(randomCell, entrySpot, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Some))
						continue;
					if (!map.reachability.CanReachColony(randomCell))
						continue;
					bool flag = false;
					int num3 = 0;
					while (num3 < list.Count)
					{
						if (!((float)(list[num3] - randomCell).LengthHorizontalSquared < num))
						{
							num3++;
							continue;
						}
						flag = true;
						break;
					}
					if (flag)
						continue;
					if (randomCell.Roofed(map))
						continue;
					result = randomCell;
					return true;
				}
				break;
			}
			result = IntVec3.Invalid;
			return false;
			IL_01b1:
			goto IL_00b9;
		}
	}
}
