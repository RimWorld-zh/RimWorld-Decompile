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
			return CellFinderLoose.RandomCellWith((Predicate<IntVec3>)((IntVec3 c) => c.Standable(map) && !c.Roofed(map) && !c.Fogged(map)), map, 1000);
		}

		public static IntVec3 TradeDropSpot(Map map)
		{
			IEnumerable<Building> collection = from b in map.listerBuildings.allBuildingsColonist
			where b.def.IsCommsConsole
			select b;
			IEnumerable<Building> enumerable = from b in map.listerBuildings.allBuildingsColonist
			where b.def.IsOrbitalTradeBeacon
			select b;
			Building building = enumerable.FirstOrDefault((Func<Building, bool>)((Building b) => !map.roofGrid.Roofed(b.Position) && DropCellFinder.AnyAdjacentGoodDropSpot(b.Position, map, false, false)));
			IntVec3 position = default(IntVec3);
			IntVec3 result;
			if (building != null)
			{
				position = building.Position;
				IntVec3 intVec = default(IntVec3);
				if (!DropCellFinder.TryFindDropSpotNear(position, map, out intVec, false, false))
				{
					Log.Error("Could find no good TradeDropSpot near dropCenter " + position + ". Using a random standable unfogged cell.");
					intVec = CellFinderLoose.RandomCellWith((Predicate<IntVec3>)((IntVec3 c) => c.Standable(map) && !c.Fogged(map)), map, 1000);
				}
				result = intVec;
			}
			else
			{
				List<Building> list = new List<Building>();
				list.AddRange(enumerable);
				list.AddRange(collection);
				list.RemoveAll((Predicate<Building>)delegate(Building b)
				{
					CompPowerTrader compPowerTrader = b.TryGetComp<CompPowerTrader>();
					return compPowerTrader != null && !compPowerTrader.PowerOn;
				});
				Predicate<IntVec3> validator = (Predicate<IntVec3>)((IntVec3 c) => DropCellFinder.IsGoodDropSpot(c, map, false, false));
				if (!list.Any())
				{
					list.AddRange(map.listerBuildings.allBuildingsColonist);
					list.Shuffle();
					if (!list.Any())
					{
						result = CellFinderLoose.RandomCellWith(validator, map, 1000);
						goto IL_023b;
					}
				}
				int num = 8;
				while (true)
				{
					for (int i = 0; i < list.Count; i++)
					{
						IntVec3 position2 = list[i].Position;
						if (CellFinder.TryFindRandomCellNear(position2, map, num, validator, out position))
							goto IL_016e;
					}
					num = Mathf.RoundToInt((float)((float)num * 1.1000000238418579));
					int num2 = num;
					IntVec3 size = map.Size;
					if (num2 > size.x)
						break;
				}
				Log.Error("Failed to generate trade drop center. Giving random.");
				result = CellFinderLoose.RandomCellWith(validator, map, 1000);
			}
			goto IL_023b;
			IL_016e:
			result = position;
			goto IL_023b;
			IL_023b:
			return result;
		}

		public static bool TryFindDropSpotNear(IntVec3 center, Map map, out IntVec3 result, bool allowFogged, bool canRoofPunch)
		{
			if (DebugViewSettings.drawDestSearch)
			{
				map.debugDrawer.FlashCell(center, 1f, "center", 50);
			}
			Predicate<IntVec3> validator = (Predicate<IntVec3>)((IntVec3 c) => (byte)(DropCellFinder.IsGoodDropSpot(c, map, allowFogged, canRoofPunch) ? (map.reachability.CanReach(center, c, PathEndMode.OnCell, TraverseMode.PassDoors, Danger.Deadly) ? 1 : 0) : 0) != 0);
			int num = 5;
			bool result2;
			while (true)
			{
				if (CellFinder.TryFindRandomCellNear(center, map, num, validator, out result))
				{
					result2 = true;
				}
				else
				{
					num += 3;
					if (num <= 16)
						continue;
					result = center;
					result2 = false;
				}
				break;
			}
			return result2;
		}

		public static bool IsGoodDropSpot(IntVec3 c, Map map, bool allowFogged, bool canRoofPunch)
		{
			bool result;
			if (!c.InBounds(map) || !c.Standable(map))
			{
				result = false;
			}
			else if (!DropCellFinder.CanPhysicallyDropInto(c, map, canRoofPunch))
			{
				if (DebugViewSettings.drawDestSearch)
				{
					map.debugDrawer.FlashCell(c, 0f, "phys", 50);
				}
				result = false;
			}
			else if (Current.ProgramState == ProgramState.Playing && !allowFogged && c.Fogged(map))
			{
				result = false;
			}
			else
			{
				List<Thing> thingList = c.GetThingList(map);
				int num = 0;
				while (num < thingList.Count)
				{
					Thing thing = thingList[num];
					if (!(thing is IActiveDropPod) && thing.def.category != ThingCategory.Skyfaller)
					{
						if (thing.def.category != ThingCategory.Plant && GenSpawn.SpawningWipes(ThingDefOf.ActiveDropPod, thing.def))
							goto IL_00de;
						num++;
						continue;
					}
					goto IL_00b0;
				}
				result = true;
			}
			goto IL_00fe;
			IL_00b0:
			result = false;
			goto IL_00fe;
			IL_00de:
			result = false;
			goto IL_00fe;
			IL_00fe:
			return result;
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
			goto IL_008f;
			IL_008f:
			IntVec3 result;
			while (true)
			{
				IntVec3 intVec = CellFinder.RandomCell(map);
				num++;
				if (!DropCellFinder.CanPhysicallyDropInto(intVec, map, true))
					continue;
				if (num > 300)
				{
					result = intVec;
				}
				else
				{
					if (intVec.Roofed(map))
						continue;
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
					if (flag)
						continue;
					if (!map.reachability.CanReachFactionBase(intVec, hostFaction))
						continue;
					result = intVec;
				}
				break;
			}
			return result;
			IL_0170:
			goto IL_008f;
		}

		public static bool TryFindRaidDropCenterClose(out IntVec3 spot, Map map)
		{
			Faction faction = map.ParentFaction ?? Faction.OfPlayer;
			int num = 0;
			bool result;
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
					result = true;
				}
				else
				{
					num++;
					if (num <= 300)
						continue;
					spot = CellFinderLoose.RandomCellWith((Predicate<IntVec3>)((IntVec3 c) => DropCellFinder.CanPhysicallyDropInto(c, map, true)), map, 1000);
					result = false;
				}
				break;
			}
			return result;
		}

		private static bool CanPhysicallyDropInto(IntVec3 c, Map map, bool canRoofPunch)
		{
			bool result;
			if (!c.Walkable(map))
			{
				result = false;
			}
			else
			{
				RoofDef roof = c.GetRoof(map);
				if (roof != null)
				{
					if (!canRoofPunch)
					{
						result = false;
						goto IL_004c;
					}
					if (roof.isThickRoof)
					{
						result = false;
						goto IL_004c;
					}
				}
				result = true;
			}
			goto IL_004c;
			IL_004c:
			return result;
		}
	}
}
