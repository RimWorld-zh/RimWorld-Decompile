using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class DropCellFinder
	{
		public static IntVec3 RandomDropSpot(Map map)
		{
			return CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(map) && !c.Roofed(map) && !c.Fogged(map), map, 1000);
		}

		public static IntVec3 TradeDropSpot(Map map)
		{
			IEnumerable<Building> collection = from b in map.listerBuildings.allBuildingsColonist
			where b.def.IsCommsConsole
			select b;
			IEnumerable<Building> enumerable = from b in map.listerBuildings.allBuildingsColonist
			where b.def.IsOrbitalTradeBeacon
			select b;
			Building building = enumerable.FirstOrDefault((Building b) => !map.roofGrid.Roofed(b.Position) && DropCellFinder.AnyAdjacentGoodDropSpot(b.Position, map, false, false));
			IntVec3 position = default(IntVec3);
			if (building != null)
			{
				position = building.Position;
				IntVec3 result = default(IntVec3);
				if (!DropCellFinder.TryFindDropSpotNear(position, map, out result, false, false))
				{
					Log.Error("Could find no good TradeDropSpot near dropCenter " + position + ". Using a random standable unfogged cell.");
					result = CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(map) && !c.Fogged(map), map, 1000);
					return result;
				}
				return result;
			}
			List<Building> list = new List<Building>();
			list.AddRange(enumerable);
			list.AddRange(collection);
			list.RemoveAll(delegate(Building b)
			{
				CompPowerTrader compPowerTrader = b.TryGetComp<CompPowerTrader>();
				return compPowerTrader != null && !compPowerTrader.PowerOn;
			});
			Predicate<IntVec3> validator = (IntVec3 c) => DropCellFinder.IsGoodDropSpot(c, map, false, false);
			if (!list.Any())
			{
				list.AddRange(map.listerBuildings.allBuildingsColonist);
				list.Shuffle();
				if (!list.Any())
				{
					return CellFinderLoose.RandomCellWith(validator, map, 1000);
				}
			}
			int num = 8;
			while (true)
			{
				for (int i = 0; i < list.Count; i++)
				{
					IntVec3 position2 = list[i].Position;
					if (CellFinder.TryFindRandomCellNear(position2, map, num, validator, out position))
					{
						return position;
					}
				}
				num = Mathf.RoundToInt((float)((float)num * 1.1000000238418579));
				int num2 = num;
				IntVec3 size = map.Size;
				if (num2 > size.x)
					break;
			}
			Log.Error("Failed to generate trade drop center. Giving random.");
			return CellFinderLoose.RandomCellWith(validator, map, 1000);
		}

		public static bool TryFindDropSpotNear(IntVec3 center, Map map, out IntVec3 result, bool allowFogged, bool canRoofPunch)
		{
			if (DebugViewSettings.drawDestSearch)
			{
				map.debugDrawer.FlashCell(center, 1f, "center", 50);
			}
			Predicate<IntVec3> validator = delegate(IntVec3 c)
			{
				if (!DropCellFinder.IsGoodDropSpot(c, map, allowFogged, canRoofPunch))
				{
					return false;
				}
				if (!map.reachability.CanReach(center, c, PathEndMode.OnCell, TraverseMode.PassDoors, Danger.Deadly))
				{
					return false;
				}
				return true;
			};
			int num = 5;
			while (true)
			{
				if (CellFinder.TryFindRandomCellNear(center, map, num, validator, out result))
				{
					return true;
				}
				num += 3;
				if (num > 16)
					break;
			}
			result = center;
			return false;
		}

		public static bool IsGoodDropSpot(IntVec3 c, Map map, bool allowFogged, bool canRoofPunch)
		{
			if (c.InBounds(map) && c.Standable(map))
			{
				if (!DropCellFinder.CanPhysicallyDropInto(c, map, canRoofPunch))
				{
					if (DebugViewSettings.drawDestSearch)
					{
						map.debugDrawer.FlashCell(c, 0f, "phys", 50);
					}
					return false;
				}
				if (Current.ProgramState == ProgramState.Playing && !allowFogged && c.Fogged(map))
				{
					return false;
				}
				List<Thing> thingList = c.GetThingList(map);
				int num = 0;
				while (num < thingList.Count)
				{
					Thing thing = thingList[num];
					if (!(thing is IActiveDropPod) && thing.def.category != ThingCategory.Skyfaller)
					{
						if (thing.def.category != ThingCategory.Plant && GenSpawn.SpawningWipes(ThingDefOf.ActiveDropPod, thing.def))
						{
							return false;
						}
						num++;
						continue;
					}
					return false;
				}
				return true;
			}
			return false;
		}

		private static bool AnyAdjacentGoodDropSpot(IntVec3 c, Map map, bool allowFogged, bool canRoofPunch)
		{
			return DropCellFinder.IsGoodDropSpot(c + IntVec3.North, map, allowFogged, canRoofPunch) || DropCellFinder.IsGoodDropSpot(c + IntVec3.East, map, allowFogged, canRoofPunch) || DropCellFinder.IsGoodDropSpot(c + IntVec3.South, map, allowFogged, canRoofPunch) || DropCellFinder.IsGoodDropSpot(c + IntVec3.West, map, allowFogged, canRoofPunch);
		}

		public static IntVec3 FindRaidDropCenterDistant(Map map)
		{
			Faction hostFaction = map.ParentFaction ?? Faction.OfPlayer;
			IEnumerable<Thing> first = map.mapPawns.FreeHumanlikesSpawnedOfFaction(hostFaction).Cast<Thing>();
			first = ((hostFaction != Faction.OfPlayer) ? first.Concat(from x in map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial)
			where x.Faction == hostFaction
			select x) : first.Concat(map.listerBuildings.allBuildingsColonist.Cast<Thing>()));
			int num = 0;
			float num2 = 65f;
			goto IL_008e;
			IL_008e:
			IntVec3 intVec;
			while (true)
			{
				intVec = CellFinder.RandomCell(map);
				num++;
				if (DropCellFinder.CanPhysicallyDropInto(intVec, map, true))
				{
					if (num > 300)
					{
						return intVec;
					}
					if (!intVec.Roofed(map))
					{
						num2 = (float)(num2 - 0.20000000298023224);
						bool flag = false;
						foreach (Thing item in first)
						{
							if ((float)(intVec - item.Position).LengthHorizontalSquared < num2 * num2)
							{
								flag = true;
								break;
							}
						}
						if (!flag && map.reachability.CanReachFactionBase(intVec, hostFaction))
							break;
					}
				}
			}
			return intVec;
			IL_015d:
			goto IL_008e;
		}

		public static bool TryFindRaidDropCenterClose(out IntVec3 spot, Map map)
		{
			Faction faction = map.ParentFaction ?? Faction.OfPlayer;
			int num = 0;
			while (true)
			{
				IntVec3 root = IntVec3.Invalid;
				if (map.mapPawns.FreeHumanlikesSpawnedOfFaction(faction).Count() > 0)
				{
					root = map.mapPawns.FreeHumanlikesSpawnedOfFaction(faction).RandomElement().Position;
				}
				else
				{
					if (faction == Faction.OfPlayer)
					{
						List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
						int num2 = 0;
						while (num2 < allBuildingsColonist.Count && !DropCellFinder.TryFindDropSpotNear(allBuildingsColonist[num2].Position, map, out root, true, true))
						{
							num2++;
						}
					}
					else
					{
						List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
						int num3 = 0;
						while (num3 < list.Count && (list[num3].Faction != faction || !DropCellFinder.TryFindDropSpotNear(list[num3].Position, map, out root, true, true)))
						{
							num3++;
						}
					}
					if (!root.IsValid)
					{
						root = DropCellFinder.RandomDropSpot(map);
					}
				}
				spot = CellFinder.RandomClosewalkCellNear(root, map, 10, null);
				if (DropCellFinder.CanPhysicallyDropInto(spot, map, true))
				{
					return true;
				}
				num++;
				if (num > 300)
					break;
			}
			spot = CellFinderLoose.RandomCellWith((IntVec3 c) => DropCellFinder.CanPhysicallyDropInto(c, map, true), map, 1000);
			return false;
		}

		private static bool CanPhysicallyDropInto(IntVec3 c, Map map, bool canRoofPunch)
		{
			if (!c.Walkable(map))
			{
				return false;
			}
			RoofDef roof = c.GetRoof(map);
			if (roof != null)
			{
				if (!canRoofPunch)
				{
					return false;
				}
				if (roof.isThickRoof)
				{
					return false;
				}
			}
			return true;
		}
	}
}
