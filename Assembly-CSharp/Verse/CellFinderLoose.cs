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
			for (int num = 0; num < maxTries; num++)
			{
				result = CellFinder.RandomCell(map);
				if (validator(result))
				{
					return true;
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		public static bool TryFindRandomNotEdgeCellWith(int minEdgeDistance, Predicate<IntVec3> validator, Map map, out IntVec3 result)
		{
			for (int i = 0; i < 1000; i++)
			{
				result = CellFinder.RandomNotEdgeCell(minEdgeDistance, map);
				if (result.IsValid && validator(result))
				{
					return true;
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		public static IntVec3 GetFleeDest(Pawn pawn, List<Thing> threats, float distance = 23f)
		{
			if (pawn.RaceProps.Animal)
			{
				return CellFinderLoose.GetFleeDestAnimal(pawn, threats, distance);
			}
			return CellFinderLoose.GetFleeDestToolUser(pawn, threats, distance);
		}

		public static IntVec3 GetFleeDestAnimal(Pawn pawn, List<Thing> threats, float distance = 23f)
		{
			Vector3 normalized = (pawn.Position - threats[0].Position).ToVector3().normalized;
			float num = distance - pawn.Position.DistanceTo(threats[0].Position);
			for (float num2 = 200f; num2 <= 360.0; num2 = (float)(num2 + 10.0))
			{
				IntVec3 intVec = pawn.Position + (normalized.RotatedBy(Rand.Range((float)((0.0 - num2) / 2.0), (float)(num2 / 2.0))) * num).ToIntVec3();
				if (CellFinderLoose.CanFleeToLocation(pawn, intVec))
				{
					return intVec;
				}
			}
			float num3 = num;
			while (num3 * 3.0 > num)
			{
				IntVec3 intVec2 = pawn.Position + IntVec3Utility.RandomHorizontalOffset(num3);
				if (CellFinderLoose.CanFleeToLocation(pawn, intVec2))
				{
					return intVec2;
				}
				num3 = (float)(num3 - distance / 10.0);
			}
			return IntVec3.Invalid;
		}

		public static bool CanFleeToLocation(Pawn pawn, IntVec3 location)
		{
			if (!location.Standable(pawn.Map))
			{
				return false;
			}
			if (pawn.Map.pawnDestinationManager.DestinationIsReserved(location, pawn))
			{
				return false;
			}
			Region region = location.GetRegion(pawn.Map, RegionType.Set_Passable);
			if (region.type == RegionType.Portal)
			{
				return false;
			}
			if (!pawn.CanReach(location, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				return false;
			}
			return true;
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
						if (map.pawnDestinationManager.DestinationIsReserved(cell, pawn))
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
				if (!c.Standable(map))
				{
					debug_numStand++;
					return false;
				}
				Room room = c.GetRoom(map, RegionType.Set_Passable);
				if (room == null)
				{
					debug_numRoom++;
					return false;
				}
				if (!room.TouchesMapEdge)
				{
					debug_numTouch++;
					return false;
				}
				if (room.CellCount < minCellCount)
				{
					debug_numRoomCellCount++;
					return false;
				}
				if ((object)extraValidator != null && !extraValidator(c))
				{
					debug_numExtraValidator++;
					return false;
				}
				return true;
			};
			for (int num = tightness; num >= 1; num--)
			{
				IntVec3 size = map.Size;
				int num2 = size.x / num;
				IntVec3 size2 = map.Size;
				int minEdgeDistance = (size2.x - num2) / 2;
				IntVec3 result = default(IntVec3);
				if (CellFinderLoose.TryFindRandomNotEdgeCellWith(minEdgeDistance, validator, map, out result))
				{
					return result;
				}
			}
			Log.Error("Found no good central spot. Choosing randomly. numStand=" + debug_numStand + ", numRoom=" + debug_numRoom + ", numTouch=" + debug_numTouch + ", numRoomCellCount=" + debug_numRoomCellCount + ", numExtraValidator=" + debug_numExtraValidator);
			return CellFinderLoose.RandomCellWith((Predicate<IntVec3>)((IntVec3 x) => x.Standable(map)), map, 1000);
		}
	}
}
