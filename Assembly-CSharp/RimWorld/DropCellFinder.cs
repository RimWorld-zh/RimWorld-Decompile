using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020008FD RID: 2301
	public static class DropCellFinder
	{
		// Token: 0x06003539 RID: 13625 RVA: 0x001C75A0 File Offset: 0x001C59A0
		public static IntVec3 RandomDropSpot(Map map)
		{
			return CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(map) && !c.Roofed(map) && !c.Fogged(map), map, 1000);
		}

		// Token: 0x0600353A RID: 13626 RVA: 0x001C75E0 File Offset: 0x001C59E0
		public static IntVec3 TradeDropSpot(Map map)
		{
			IEnumerable<Building> collection = from b in map.listerBuildings.allBuildingsColonist
			where b.def.IsCommsConsole
			select b;
			IEnumerable<Building> enumerable = from b in map.listerBuildings.allBuildingsColonist
			where b.def.IsOrbitalTradeBeacon
			select b;
			Building building = enumerable.FirstOrDefault((Building b) => !map.roofGrid.Roofed(b.Position) && DropCellFinder.AnyAdjacentGoodDropSpot(b.Position, map, false, false));
			IntVec3 result;
			if (building != null)
			{
				IntVec3 position = building.Position;
				IntVec3 intVec;
				if (!DropCellFinder.TryFindDropSpotNear(position, map, out intVec, false, false, false))
				{
					Log.Error("Could find no good TradeDropSpot near dropCenter " + position + ". Using a random standable unfogged cell.", false);
					intVec = CellFinderLoose.RandomCellWith((IntVec3 c) => c.Standable(map) && !c.Fogged(map), map, 1000);
				}
				result = intVec;
			}
			else
			{
				List<Building> list = new List<Building>();
				list.AddRange(enumerable);
				list.AddRange(collection);
				list.RemoveAll(delegate(Building b)
				{
					CompPowerTrader compPowerTrader = b.TryGetComp<CompPowerTrader>();
					return compPowerTrader != null && !compPowerTrader.PowerOn;
				});
				Predicate<IntVec3> validator = (IntVec3 c) => DropCellFinder.IsGoodDropSpot(c, map, false, false);
				if (!list.Any<Building>())
				{
					list.AddRange(map.listerBuildings.allBuildingsColonist);
					list.Shuffle<Building>();
					if (!list.Any<Building>())
					{
						return CellFinderLoose.RandomCellWith(validator, map, 1000);
					}
				}
				int num = 8;
				IntVec3 position;
				for (;;)
				{
					for (int i = 0; i < list.Count; i++)
					{
						IntVec3 position2 = list[i].Position;
						if (CellFinder.TryFindRandomCellNear(position2, map, num, validator, out position, -1))
						{
							goto Block_7;
						}
					}
					num = Mathf.RoundToInt((float)num * 1.1f);
					if (num > map.Size.x)
					{
						goto Block_9;
					}
				}
				Block_7:
				return position;
				Block_9:
				Log.Error("Failed to generate trade drop center. Giving random.", false);
				result = CellFinderLoose.RandomCellWith(validator, map, 1000);
			}
			return result;
		}

		// Token: 0x0600353B RID: 13627 RVA: 0x001C7830 File Offset: 0x001C5C30
		public static bool TryFindDropSpotNear(IntVec3 center, Map map, out IntVec3 result, bool allowFogged, bool canRoofPunch, bool willExplode)
		{
			if (DebugViewSettings.drawDestSearch)
			{
				map.debugDrawer.FlashCell(center, 1f, "center", 50);
			}
			Predicate<IntVec3> validator = (IntVec3 c) => DropCellFinder.IsGoodDropSpot(c, map, allowFogged, canRoofPunch) && map.reachability.CanReach(center, c, PathEndMode.OnCell, TraverseMode.PassDoors, Danger.Deadly) && (!willExplode || !SkyfallerUtility.CanPossiblyFallOnColonist(ThingDefOf.ExplosiveDropPodIncoming, c, map));
			int num = 5;
			while (!CellFinder.TryFindRandomCellNear(center, map, num, validator, out result, -1))
			{
				num += 3;
				if (num > 16)
				{
					result = center;
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600353C RID: 13628 RVA: 0x001C78F0 File Offset: 0x001C5CF0
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
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if (thing is IActiveDropPod || thing is Skyfaller)
					{
						return false;
					}
					if (thing.def.category != ThingCategory.Plant)
					{
						if (GenSpawn.SpawningWipes(ThingDefOf.ActiveDropPod, thing.def))
						{
							return false;
						}
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x0600353D RID: 13629 RVA: 0x001C79F8 File Offset: 0x001C5DF8
		private static bool AnyAdjacentGoodDropSpot(IntVec3 c, Map map, bool allowFogged, bool canRoofPunch)
		{
			return DropCellFinder.IsGoodDropSpot(c + IntVec3.North, map, allowFogged, canRoofPunch) || DropCellFinder.IsGoodDropSpot(c + IntVec3.East, map, allowFogged, canRoofPunch) || DropCellFinder.IsGoodDropSpot(c + IntVec3.South, map, allowFogged, canRoofPunch) || DropCellFinder.IsGoodDropSpot(c + IntVec3.West, map, allowFogged, canRoofPunch);
		}

		// Token: 0x0600353E RID: 13630 RVA: 0x001C7A6C File Offset: 0x001C5E6C
		public static IntVec3 FindRaidDropCenterDistant(Map map)
		{
			Faction hostFaction = map.ParentFaction ?? Faction.OfPlayer;
			IEnumerable<Thing> enumerable = map.mapPawns.FreeHumanlikesSpawnedOfFaction(hostFaction).Cast<Thing>();
			if (hostFaction == Faction.OfPlayer)
			{
				enumerable = enumerable.Concat(map.listerBuildings.allBuildingsColonist.Cast<Thing>());
			}
			else
			{
				enumerable = enumerable.Concat(from x in map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial)
				where x.Faction == hostFaction
				select x);
			}
			int num = 0;
			float num2 = 65f;
			IntVec3 intVec;
			for (;;)
			{
				intVec = CellFinder.RandomCell(map);
				num++;
				if (DropCellFinder.CanPhysicallyDropInto(intVec, map, true))
				{
					if (num > 300)
					{
						break;
					}
					if (!intVec.Roofed(map))
					{
						num2 -= 0.2f;
						bool flag = false;
						foreach (Thing thing in enumerable)
						{
							if ((float)(intVec - thing.Position).LengthHorizontalSquared < num2 * num2)
							{
								flag = true;
								break;
							}
						}
						if (!flag && map.reachability.CanReachFactionBase(intVec, hostFaction))
						{
							goto IL_167;
						}
					}
				}
			}
			return intVec;
			IL_167:
			return intVec;
		}

		// Token: 0x0600353F RID: 13631 RVA: 0x001C7C00 File Offset: 0x001C6000
		public static bool TryFindRaidDropCenterClose(out IntVec3 spot, Map map)
		{
			Faction faction = map.ParentFaction ?? Faction.OfPlayer;
			int num = 0;
			for (;;)
			{
				IntVec3 root = IntVec3.Invalid;
				if (map.mapPawns.FreeHumanlikesSpawnedOfFaction(faction).Count<Pawn>() > 0)
				{
					root = map.mapPawns.FreeHumanlikesSpawnedOfFaction(faction).RandomElement<Pawn>().Position;
				}
				else
				{
					if (faction == Faction.OfPlayer)
					{
						List<Building> allBuildingsColonist = map.listerBuildings.allBuildingsColonist;
						for (int i = 0; i < allBuildingsColonist.Count; i++)
						{
							if (DropCellFinder.TryFindDropSpotNear(allBuildingsColonist[i].Position, map, out root, true, true, false))
							{
								break;
							}
						}
					}
					else
					{
						List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
						for (int j = 0; j < list.Count; j++)
						{
							if (list[j].Faction == faction && DropCellFinder.TryFindDropSpotNear(list[j].Position, map, out root, true, true, false))
							{
								break;
							}
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
					break;
				}
				num++;
				if (num > 300)
				{
					goto Block_9;
				}
			}
			return true;
			Block_9:
			spot = CellFinderLoose.RandomCellWith((IntVec3 c) => DropCellFinder.CanPhysicallyDropInto(c, map, true), map, 1000);
			return false;
		}

		// Token: 0x06003540 RID: 13632 RVA: 0x001C7DE4 File Offset: 0x001C61E4
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
						return false;
					}
					if (roof.isThickRoof)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}
	}
}
