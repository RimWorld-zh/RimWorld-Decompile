using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	public static class CellFinderLoose
	{
		public static IntVec3 RandomCellWith(Predicate<IntVec3> validator, Map map, int maxTries = 1000)
		{
			IntVec3 result = default(IntVec3);
			CellFinderLoose.TryGetRandomCellWith(validator, map, maxTries, out result);
			return result;
		}

		public static bool TryGetRandomCellWith(Predicate<IntVec3> validator, Map map, int maxTries, out IntVec3 result)
		{
			int num = 0;
			bool result2;
			while (true)
			{
				if (num < maxTries)
				{
					result = CellFinder.RandomCell(map);
					if (validator(result))
					{
						result2 = true;
						break;
					}
					num++;
					continue;
				}
				result = IntVec3.Invalid;
				result2 = false;
				break;
			}
			return result2;
		}

		public static bool TryFindRandomNotEdgeCellWith(int minEdgeDistance, Predicate<IntVec3> validator, Map map, out IntVec3 result)
		{
			int num = 0;
			bool result2;
			while (true)
			{
				if (num < 1000)
				{
					result = CellFinder.RandomNotEdgeCell(minEdgeDistance, map);
					if (result.IsValid && validator(result))
					{
						result2 = true;
						break;
					}
					num++;
					continue;
				}
				result = IntVec3.Invalid;
				result2 = false;
				break;
			}
			return result2;
		}

		public static IntVec3 GetFleeDest(Pawn pawn, List<Thing> threats, float distance = 23f)
		{
			return (!pawn.RaceProps.Animal) ? CellFinderLoose.GetFleeDestToolUser(pawn, threats, distance) : CellFinderLoose.GetFleeDestAnimal(pawn, threats, distance);
		}

		public static IntVec3 GetFleeDestAnimal(Pawn pawn, List<Thing> threats, float distance = 23f)
		{
			Vector3 normalized = (pawn.Position - threats[0].Position).ToVector3().normalized;
			float num = distance - pawn.Position.DistanceTo(threats[0].Position);
			float num2 = 200f;
			IntVec3 result;
			while (true)
			{
				if (num2 <= 360.0)
				{
					IntVec3 intVec = pawn.Position + (normalized.RotatedBy(Rand.Range((float)((0.0 - num2) / 2.0), (float)(num2 / 2.0))) * num).ToIntVec3();
					if (CellFinderLoose.CanFleeToLocation(pawn, intVec))
					{
						result = intVec;
						break;
					}
					num2 = (float)(num2 + 10.0);
					continue;
				}
				float num3 = num;
				IntVec3 intVec2;
				while (num3 * 3.0 > num)
				{
					intVec2 = pawn.Position + IntVec3Utility.RandomHorizontalOffset(num3);
					if (CellFinderLoose.CanFleeToLocation(pawn, intVec2))
						goto IL_00de;
					num3 = (float)(num3 - distance / 10.0);
				}
				result = pawn.Position;
				break;
				IL_00de:
				result = intVec2;
				break;
			}
			return result;
		}

		public static bool CanFleeToLocation(Pawn pawn, IntVec3 location)
		{
			bool result;
			if (!location.Standable(pawn.Map))
			{
				result = false;
			}
			else if (!pawn.Map.pawnDestinationReservationManager.CanReserve(location, pawn))
			{
				result = false;
			}
			else
			{
				Region region = location.GetRegion(pawn.Map, RegionType.Set_Passable);
				result = ((byte)((region.type != RegionType.Portal) ? (pawn.CanReach(location, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn) ? 1 : 0) : 0) != 0);
			}
			return result;
		}

		public static IntVec3 GetFleeDestToolUser(Pawn pawn, List<Thing> threats, float distance = 23f)
		{
			IntVec3 bestPos = pawn.Position;
			float bestScore = -1f;
			TraverseParms traverseParms = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
			RegionTraverser.BreadthFirstTraverse(pawn.GetRegion(RegionType.Set_Passable), (RegionEntryPredicate)((Region from, Region reg) => reg.Allows(traverseParms, false)), (RegionProcessor)delegate(Region reg)
			{
				Danger danger = reg.DangerFor(pawn);
				Map map = pawn.Map;
				foreach (IntVec3 cell in reg.Cells)
				{
					if (cell.Standable(map) && reg.portal == null)
					{
						Thing thing = null;
						float num = 0f;
						for (int i = 0; i < threats.Count; i++)
						{
							float num2 = (float)cell.DistanceToSquared(threats[i].Position);
							if (thing == null || num2 < num)
							{
								thing = threats[i];
								num = num2;
							}
						}
						float num3 = Mathf.Sqrt(num);
						float f = Mathf.Min(num3, distance);
						float num4 = Mathf.Pow(f, 1.2f);
						num4 *= Mathf.InverseLerp(50f, 0f, (cell - pawn.Position).LengthHorizontal);
						if (cell.GetRoom(map, RegionType.Set_Passable) != thing.GetRoom(RegionType.Set_Passable))
						{
							num4 = (float)(num4 * 4.1999998092651367);
						}
						else if (num3 < 8.0)
						{
							num4 = (float)(num4 * 0.05000000074505806);
						}
						if (!map.pawnDestinationReservationManager.CanReserve(cell, pawn))
						{
							num4 = (float)(num4 * 0.5);
						}
						if (danger == Danger.Deadly)
						{
							num4 = (float)(num4 * 0.800000011920929);
						}
						if (num4 > bestScore)
						{
							bestPos = cell;
							bestScore = num4;
						}
					}
				}
				return false;
			}, 20, RegionType.Set_Passable);
			return bestPos;
		}

		public static IntVec3 TryFindCentralCell(Map map, int tightness, int minCellCount, Predicate<IntVec3> extraValidator = null)
		{
			int debug_numStand = 0;
			int debug_numRoom = 0;
			int debug_numTouch = 0;
			int debug_numRoomCellCount = 0;
			int debug_numExtraValidator = 0;
			Predicate<IntVec3> validator = (Predicate<IntVec3>)delegate(IntVec3 c)
			{
				bool result2;
				if (!c.Standable(map))
				{
					debug_numStand++;
					result2 = false;
				}
				else
				{
					Room room = c.GetRoom(map, RegionType.Set_Passable);
					if (room == null)
					{
						debug_numRoom++;
						result2 = false;
					}
					else if (!room.TouchesMapEdge)
					{
						debug_numTouch++;
						result2 = false;
					}
					else if (room.CellCount < minCellCount)
					{
						debug_numRoomCellCount++;
						result2 = false;
					}
					else if ((object)extraValidator != null && !extraValidator(c))
					{
						debug_numExtraValidator++;
						result2 = false;
					}
					else
					{
						result2 = true;
					}
				}
				return result2;
			};
			int num = tightness;
			IntVec3 result;
			while (true)
			{
				if (num >= 1)
				{
					IntVec3 size = map.Size;
					int num2 = size.x / num;
					IntVec3 size2 = map.Size;
					int minEdgeDistance = (size2.x - num2) / 2;
					IntVec3 intVec = default(IntVec3);
					if (CellFinderLoose.TryFindRandomNotEdgeCellWith(minEdgeDistance, validator, map, out intVec))
					{
						result = intVec;
						break;
					}
					num--;
					continue;
				}
				Log.Error("Found no good central spot. Choosing randomly. numStand=" + debug_numStand + ", numRoom=" + debug_numRoom + ", numTouch=" + debug_numTouch + ", numRoomCellCount=" + debug_numRoomCellCount + ", numExtraValidator=" + debug_numExtraValidator);
				result = CellFinderLoose.RandomCellWith((Predicate<IntVec3>)((IntVec3 x) => x.Standable(map)), map, 1000);
				break;
			}
			return result;
		}

		public static bool TryFindSkyfallerCell(ThingDef skyfaller, Map map, out IntVec3 cell, int minDistToEdge = 10, IntVec3 nearLoc = default(IntVec3), int nearLocMaxDist = -1, bool allowRoofedCells = true, bool allowCellsWithItems = false, bool allowCellsWithBuildings = false, bool colonyReachable = false, Predicate<IntVec3> extraValidator = null)
		{
			Predicate<IntVec3> validator = (Predicate<IntVec3>)delegate(IntVec3 x)
			{
				CellRect.CellRectIterator iterator = GenAdj.OccupiedRect(x, Rot4.North, skyfaller.size).GetIterator();
				bool result;
				while (true)
				{
					if (!iterator.Done())
					{
						IntVec3 current = iterator.Current;
						if (current.InBounds(map) && !current.Fogged(map) && current.Standable(map) && (!current.Roofed(map) || !current.GetRoof(map).isThickRoof))
						{
							if (!allowRoofedCells && current.Roofed(map))
							{
								result = false;
								break;
							}
							if (!allowCellsWithItems && current.GetFirstItem(map) != null)
							{
								result = false;
								break;
							}
							if (!allowCellsWithBuildings && current.GetFirstBuilding(map) != null)
							{
								result = false;
								break;
							}
							if (current.GetFirstSkyfaller(map) != null)
							{
								result = false;
								break;
							}
							iterator.MoveNext();
							continue;
						}
						result = false;
					}
					else
					{
						result = ((byte)((minDistToEdge <= 0 || x.DistanceToEdge(map) >= minDistToEdge) ? ((!colonyReachable || map.reachability.CanReachColony(x)) ? (((object)extraValidator == null || extraValidator(x)) ? 1 : 0) : 0) : 0) != 0);
					}
					break;
				}
				return result;
			};
			return (nearLocMaxDist <= 0) ? CellFinderLoose.TryFindRandomNotEdgeCellWith(minDistToEdge, validator, map, out cell) : CellFinder.TryFindRandomCellNear(nearLoc, map, nearLocMaxDist, validator, out cell);
		}
	}
}
