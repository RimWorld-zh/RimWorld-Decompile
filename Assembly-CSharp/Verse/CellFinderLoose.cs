using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	public static class CellFinderLoose
	{
		public static IntVec3 RandomCellWith(Predicate<IntVec3> validator, Map map, int maxTries = 1000)
		{
			IntVec3 result;
			CellFinderLoose.TryGetRandomCellWith(validator, map, maxTries, out result);
			return result;
		}

		public static bool TryGetRandomCellWith(Predicate<IntVec3> validator, Map map, int maxTries, out IntVec3 result)
		{
			for (int i = 0; i < maxTries; i++)
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
			IntVec3 result;
			if (pawn.RaceProps.Animal)
			{
				result = CellFinderLoose.GetFleeDestAnimal(pawn, threats, distance);
			}
			else
			{
				result = CellFinderLoose.GetFleeDestToolUser(pawn, threats, distance);
			}
			return result;
		}

		public static IntVec3 GetFleeDestAnimal(Pawn pawn, List<Thing> threats, float distance = 23f)
		{
			Vector3 normalized = (pawn.Position - threats[0].Position).ToVector3().normalized;
			float num = distance - pawn.Position.DistanceTo(threats[0].Position);
			for (float num2 = 200f; num2 <= 360f; num2 += 10f)
			{
				IntVec3 intVec = pawn.Position + (normalized.RotatedBy(Rand.Range(-num2 / 2f, num2 / 2f)) * num).ToIntVec3();
				if (CellFinderLoose.CanFleeToLocation(pawn, intVec))
				{
					return intVec;
				}
			}
			float num3 = num;
			while (num3 * 3f > num)
			{
				IntVec3 intVec2 = pawn.Position + IntVec3Utility.RandomHorizontalOffset(num3);
				if (CellFinderLoose.CanFleeToLocation(pawn, intVec2))
				{
					return intVec2;
				}
				num3 -= distance / 10f;
			}
			return pawn.Position;
		}

		public static bool CanFleeToLocation(Pawn pawn, IntVec3 location)
		{
			bool result;
			if (!location.Standable(pawn.Map))
			{
				result = false;
			}
			else if (!pawn.Map.pawnDestinationReservationManager.CanReserve(location, pawn, false))
			{
				result = false;
			}
			else
			{
				Region region = location.GetRegion(pawn.Map, RegionType.Set_Passable);
				result = (region.type != RegionType.Portal && pawn.CanReach(location, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn));
			}
			return result;
		}

		public static IntVec3 GetFleeDestToolUser(Pawn pawn, List<Thing> threats, float distance = 23f)
		{
			IntVec3 bestPos = pawn.Position;
			float bestScore = -1f;
			TraverseParms traverseParms = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
			RegionTraverser.BreadthFirstTraverse(pawn.GetRegion(RegionType.Set_Passable), (Region from, Region reg) => reg.Allows(traverseParms, false), delegate(Region reg)
			{
				Danger danger = reg.DangerFor(pawn);
				Map map = pawn.Map;
				foreach (IntVec3 intVec in reg.Cells)
				{
					if (intVec.Standable(map))
					{
						if (reg.portal == null)
						{
							Thing thing = null;
							float num = 0f;
							for (int i = 0; i < threats.Count; i++)
							{
								float num2 = (float)intVec.DistanceToSquared(threats[i].Position);
								if (thing == null || num2 < num)
								{
									thing = threats[i];
									num = num2;
								}
							}
							float num3 = Mathf.Sqrt(num);
							float f = Mathf.Min(num3, distance);
							float num4 = Mathf.Pow(f, 1.2f);
							num4 *= Mathf.InverseLerp(50f, 0f, (intVec - pawn.Position).LengthHorizontal);
							if (intVec.GetRoom(map, RegionType.Set_Passable) != thing.GetRoom(RegionType.Set_Passable))
							{
								num4 *= 4.2f;
							}
							else if (num3 < 8f)
							{
								num4 *= 0.05f;
							}
							if (!map.pawnDestinationReservationManager.CanReserve(intVec, pawn, false))
							{
								num4 *= 0.5f;
							}
							if (danger == Danger.Deadly)
							{
								num4 *= 0.8f;
							}
							if (num4 > bestScore)
							{
								bestPos = intVec;
								bestScore = num4;
							}
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
			Predicate<IntVec3> validator = delegate(IntVec3 c)
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
					else if (extraValidator != null && !extraValidator(c))
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
			for (int i = tightness; i >= 1; i--)
			{
				int num = map.Size.x / i;
				int minEdgeDistance = (map.Size.x - num) / 2;
				IntVec3 result;
				if (CellFinderLoose.TryFindRandomNotEdgeCellWith(minEdgeDistance, validator, map, out result))
				{
					return result;
				}
			}
			Log.Error(string.Concat(new object[]
			{
				"Found no good central spot. Choosing randomly. numStand=",
				debug_numStand,
				", numRoom=",
				debug_numRoom,
				", numTouch=",
				debug_numTouch,
				", numRoomCellCount=",
				debug_numRoomCellCount,
				", numExtraValidator=",
				debug_numExtraValidator
			}), false);
			return CellFinderLoose.RandomCellWith((IntVec3 x) => x.Standable(map), map, 1000);
		}

		public static bool TryFindSkyfallerCell(ThingDef skyfaller, Map map, out IntVec3 cell, int minDistToEdge = 10, IntVec3 nearLoc = default(IntVec3), int nearLocMaxDist = -1, bool allowRoofedCells = true, bool allowCellsWithItems = false, bool allowCellsWithBuildings = false, bool colonyReachable = false, bool avoidColonistsIfExplosive = true, bool alwaysAvoidColonists = false, Predicate<IntVec3> extraValidator = null)
		{
			bool avoidColonists = (avoidColonistsIfExplosive && skyfaller.skyfaller.CausesExplosion) || alwaysAvoidColonists;
			Predicate<IntVec3> validator = delegate(IntVec3 x)
			{
				CellRect.CellRectIterator iterator = GenAdj.OccupiedRect(x, Rot4.North, skyfaller.size).GetIterator();
				while (!iterator.Done())
				{
					IntVec3 c = iterator.Current;
					bool result2;
					if (!c.InBounds(map) || c.Fogged(map) || !c.Standable(map) || (c.Roofed(map) && c.GetRoof(map).isThickRoof))
					{
						result2 = false;
					}
					else if (!allowRoofedCells && c.Roofed(map))
					{
						result2 = false;
					}
					else if (!allowCellsWithItems && c.GetFirstItem(map) != null)
					{
						result2 = false;
					}
					else if (!allowCellsWithBuildings && c.GetFirstBuilding(map) != null)
					{
						result2 = false;
					}
					else
					{
						if (c.GetFirstSkyfaller(map) == null)
						{
							iterator.MoveNext();
							continue;
						}
						result2 = false;
					}
					return result2;
				}
				return (!avoidColonists || !SkyfallerUtility.CanPossiblyFallOnColonist(skyfaller, x, map)) && (minDistToEdge <= 0 || x.DistanceToEdge(map) >= minDistToEdge) && (!colonyReachable || map.reachability.CanReachColony(x)) && (extraValidator == null || extraValidator(x));
			};
			bool result;
			if (nearLocMaxDist > 0)
			{
				result = CellFinder.TryFindRandomCellNear(nearLoc, map, nearLocMaxDist, validator, out cell, -1);
			}
			else
			{
				result = CellFinderLoose.TryFindRandomNotEdgeCellWith(minDistToEdge, validator, map, out cell);
			}
			return result;
		}

		[CompilerGenerated]
		private sealed class <GetFleeDestToolUser>c__AnonStorey0
		{
			internal TraverseParms traverseParms;

			internal Pawn pawn;

			internal List<Thing> threats;

			internal float distance;

			internal float bestScore;

			internal IntVec3 bestPos;

			public <GetFleeDestToolUser>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Region from, Region reg)
			{
				return reg.Allows(this.traverseParms, false);
			}

			internal bool <>m__1(Region reg)
			{
				Danger danger = reg.DangerFor(this.pawn);
				Map map = this.pawn.Map;
				foreach (IntVec3 intVec in reg.Cells)
				{
					if (intVec.Standable(map))
					{
						if (reg.portal == null)
						{
							Thing thing = null;
							float num = 0f;
							for (int i = 0; i < this.threats.Count; i++)
							{
								float num2 = (float)intVec.DistanceToSquared(this.threats[i].Position);
								if (thing == null || num2 < num)
								{
									thing = this.threats[i];
									num = num2;
								}
							}
							float num3 = Mathf.Sqrt(num);
							float f = Mathf.Min(num3, this.distance);
							float num4 = Mathf.Pow(f, 1.2f);
							num4 *= Mathf.InverseLerp(50f, 0f, (intVec - this.pawn.Position).LengthHorizontal);
							if (intVec.GetRoom(map, RegionType.Set_Passable) != thing.GetRoom(RegionType.Set_Passable))
							{
								num4 *= 4.2f;
							}
							else if (num3 < 8f)
							{
								num4 *= 0.05f;
							}
							if (!map.pawnDestinationReservationManager.CanReserve(intVec, this.pawn, false))
							{
								num4 *= 0.5f;
							}
							if (danger == Danger.Deadly)
							{
								num4 *= 0.8f;
							}
							if (num4 > this.bestScore)
							{
								this.bestPos = intVec;
								this.bestScore = num4;
							}
						}
					}
				}
				return false;
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindCentralCell>c__AnonStorey1
		{
			internal Map map;

			internal int debug_numStand;

			internal int debug_numRoom;

			internal int debug_numTouch;

			internal int minCellCount;

			internal int debug_numRoomCellCount;

			internal Predicate<IntVec3> extraValidator;

			internal int debug_numExtraValidator;

			public <TryFindCentralCell>c__AnonStorey1()
			{
			}

			internal bool <>m__0(IntVec3 c)
			{
				bool result;
				if (!c.Standable(this.map))
				{
					this.debug_numStand++;
					result = false;
				}
				else
				{
					Room room = c.GetRoom(this.map, RegionType.Set_Passable);
					if (room == null)
					{
						this.debug_numRoom++;
						result = false;
					}
					else if (!room.TouchesMapEdge)
					{
						this.debug_numTouch++;
						result = false;
					}
					else if (room.CellCount < this.minCellCount)
					{
						this.debug_numRoomCellCount++;
						result = false;
					}
					else if (this.extraValidator != null && !this.extraValidator(c))
					{
						this.debug_numExtraValidator++;
						result = false;
					}
					else
					{
						result = true;
					}
				}
				return result;
			}

			internal bool <>m__1(IntVec3 x)
			{
				return x.Standable(this.map);
			}
		}

		[CompilerGenerated]
		private sealed class <TryFindSkyfallerCell>c__AnonStorey2
		{
			internal ThingDef skyfaller;

			internal Map map;

			internal bool allowRoofedCells;

			internal bool allowCellsWithItems;

			internal bool allowCellsWithBuildings;

			internal bool avoidColonists;

			internal int minDistToEdge;

			internal bool colonyReachable;

			internal Predicate<IntVec3> extraValidator;

			public <TryFindSkyfallerCell>c__AnonStorey2()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				CellRect.CellRectIterator iterator = GenAdj.OccupiedRect(x, Rot4.North, this.skyfaller.size).GetIterator();
				while (!iterator.Done())
				{
					IntVec3 c = iterator.Current;
					bool result;
					if (!c.InBounds(this.map) || c.Fogged(this.map) || !c.Standable(this.map) || (c.Roofed(this.map) && c.GetRoof(this.map).isThickRoof))
					{
						result = false;
					}
					else if (!this.allowRoofedCells && c.Roofed(this.map))
					{
						result = false;
					}
					else if (!this.allowCellsWithItems && c.GetFirstItem(this.map) != null)
					{
						result = false;
					}
					else if (!this.allowCellsWithBuildings && c.GetFirstBuilding(this.map) != null)
					{
						result = false;
					}
					else
					{
						if (c.GetFirstSkyfaller(this.map) == null)
						{
							iterator.MoveNext();
							continue;
						}
						result = false;
					}
					return result;
				}
				return (!this.avoidColonists || !SkyfallerUtility.CanPossiblyFallOnColonist(this.skyfaller, x, this.map)) && (this.minDistToEdge <= 0 || x.DistanceToEdge(this.map) >= this.minDistToEdge) && (!this.colonyReachable || this.map.reachability.CanReachColony(x)) && (this.extraValidator == null || this.extraValidator(x));
			}
		}
	}
}
