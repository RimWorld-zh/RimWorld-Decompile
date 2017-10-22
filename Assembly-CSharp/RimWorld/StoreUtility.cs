using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class StoreUtility
	{
		public static bool IsInAnyStorage(this Thing t)
		{
			bool result;
			if (!t.Spawned)
			{
				result = false;
			}
			else
			{
				SlotGroup slotGroup = t.Map.slotGroupManager.SlotGroupAt(t.Position);
				result = (slotGroup != null);
			}
			return result;
		}

		public static bool IsInValidStorage(this Thing t)
		{
			bool result;
			if (!t.Spawned)
			{
				result = false;
			}
			else
			{
				SlotGroup slotGroup = t.GetSlotGroup();
				result = ((byte)((slotGroup != null && slotGroup.Settings.AllowedToAccept(t)) ? 1 : 0) != 0);
			}
			return result;
		}

		public static bool IsInValidBestStorage(this Thing t)
		{
			bool result;
			if (!t.Spawned)
			{
				result = false;
			}
			else
			{
				SlotGroup slotGroup = t.GetSlotGroup();
				IntVec3 intVec = default(IntVec3);
				result = ((byte)((slotGroup != null && slotGroup.Settings.AllowedToAccept(t)) ? ((!StoreUtility.TryFindBestBetterStoreCellFor(t, (Pawn)null, t.Map, slotGroup.Settings.Priority, Faction.OfPlayer, out intVec, false)) ? 1 : 0) : 0) != 0);
			}
			return result;
		}

		public static Building StoringBuilding(this Thing t)
		{
			Building result;
			if (!t.Spawned)
			{
				result = null;
			}
			else
			{
				SlotGroup slotGroup = t.GetSlotGroup();
				if (slotGroup != null)
				{
					Building building = slotGroup.parent as Building;
					result = ((building == null) ? null : building);
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		public static SlotGroup GetSlotGroup(this Thing thing)
		{
			return thing.Spawned ? thing.Position.GetSlotGroup(thing.Map) : null;
		}

		public static SlotGroup GetSlotGroup(this IntVec3 c, Map map)
		{
			return (Current.ProgramState == ProgramState.Playing) ? map.slotGroupManager.SlotGroupAt(c) : null;
		}

		public static Thing GetStorable(this IntVec3 c, Map map)
		{
			List<Thing> thingList = c.GetThingList(map);
			int num = 0;
			Thing result;
			while (true)
			{
				if (num < thingList.Count)
				{
					if (thingList[num].def.EverStoreable)
					{
						result = thingList[num];
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		public static bool IsValidStorageFor(this IntVec3 c, Map map, Thing storable)
		{
			bool result;
			if (!StoreUtility.NoStorageBlockersIn(c, map, storable))
			{
				result = false;
			}
			else
			{
				SlotGroup slotGroup = c.GetSlotGroup(map);
				result = ((byte)((slotGroup != null && slotGroup.Settings.AllowedToAccept(storable)) ? 1 : 0) != 0);
			}
			return result;
		}

		private static bool NoStorageBlockersIn(IntVec3 c, Map map, Thing thing)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			int num = 0;
			bool result;
			while (true)
			{
				if (num < list.Count)
				{
					Thing thing2 = list[num];
					if (thing2.def.EverStoreable)
					{
						if (!thing2.CanStackWith(thing))
						{
							result = false;
							break;
						}
						if (thing2.stackCount >= thing.def.stackLimit)
						{
							result = false;
							break;
						}
					}
					if (((thing2.def.entityDefToBuild != null) ? thing2.def.entityDefToBuild.passability : Traversability.Standable) != 0)
					{
						result = false;
						break;
					}
					if (((thing2.def.surfaceType == SurfaceType.None) ? thing2.def.passability : Traversability.Standable) != 0)
					{
						result = false;
						break;
					}
					num++;
					continue;
				}
				result = true;
				break;
			}
			return result;
		}

		public static bool TryFindBestBetterStoreCellFor(Thing t, Pawn carrier, Map map, StoragePriority currentPriority, Faction faction, out IntVec3 foundCell, bool needAccurateResult = true)
		{
			List<SlotGroup> allGroupsListInPriorityOrder = map.slotGroupManager.AllGroupsListInPriorityOrder;
			bool result;
			if (allGroupsListInPriorityOrder.Count == 0)
			{
				foundCell = IntVec3.Invalid;
				result = false;
			}
			else
			{
				IntVec3 a = (!t.SpawnedOrAnyParentSpawned) ? carrier.PositionHeld : t.PositionHeld;
				StoragePriority storagePriority = currentPriority;
				float num = 2.14748365E+09f;
				IntVec3 intVec = default(IntVec3);
				bool flag = false;
				int count = allGroupsListInPriorityOrder.Count;
				int num2 = 0;
				while (num2 < count)
				{
					SlotGroup slotGroup = allGroupsListInPriorityOrder[num2];
					StoragePriority priority = slotGroup.Settings.Priority;
					if ((int)priority >= (int)storagePriority && (int)priority > (int)currentPriority)
					{
						if (slotGroup.Settings.AllowedToAccept(t))
						{
							List<IntVec3> cellsList = slotGroup.CellsList;
							int count2 = cellsList.Count;
							int num3 = needAccurateResult ? Mathf.FloorToInt((float)count2 * Rand.Range(0.005f, 0.018f)) : 0;
							for (int num4 = 0; num4 < count2; num4++)
							{
								IntVec3 intVec2 = cellsList[num4];
								float num5 = (float)(a - intVec2).LengthHorizontalSquared;
								if (!(num5 > num) && StoreUtility.IsGoodStoreCell(intVec2, map, t, carrier, faction))
								{
									flag = true;
									intVec = intVec2;
									num = num5;
									storagePriority = priority;
									if (num4 >= num3)
										break;
								}
							}
						}
						num2++;
						continue;
					}
					break;
				}
				if (!flag)
				{
					foundCell = IntVec3.Invalid;
					result = false;
				}
				else
				{
					foundCell = intVec;
					result = true;
				}
			}
			return result;
		}

		public static bool IsGoodStoreCell(IntVec3 c, Map map, Thing t, Pawn carrier, Faction faction)
		{
			bool result;
			if (carrier != null && c.IsForbidden(carrier))
			{
				result = false;
			}
			else if (!StoreUtility.NoStorageBlockersIn(c, map, t))
			{
				result = false;
			}
			else
			{
				if (carrier != null)
				{
					if (!carrier.CanReserveNew(c))
					{
						result = false;
						goto IL_00e1;
					}
				}
				else if (faction != null && map.reservationManager.IsReservedByAnyoneOf(c, faction))
				{
					result = false;
					goto IL_00e1;
				}
				result = ((byte)((!c.ContainsStaticFire(map)) ? ((carrier == null || carrier.Map.reachability.CanReach((!t.SpawnedOrAnyParentSpawned) ? carrier.PositionHeld : t.PositionHeld, c, PathEndMode.ClosestTouch, TraverseParms.For(carrier, Danger.Deadly, TraverseMode.ByPawn, false))) ? 1 : 0) : 0) != 0);
			}
			goto IL_00e1;
			IL_00e1:
			return result;
		}

		public static bool TryFindStoreCellNearColonyDesperate(Thing item, Pawn carrier, out IntVec3 storeCell)
		{
			bool result;
			IntVec3 intVec;
			if (StoreUtility.TryFindBestBetterStoreCellFor(item, carrier, carrier.Map, StoragePriority.Unstored, carrier.Faction, out storeCell, true))
			{
				result = true;
			}
			else
			{
				for (int i = -4; i < 20; i++)
				{
					int num = (i >= 0) ? i : Rand.RangeInclusive(0, 4);
					intVec = carrier.Position + GenRadial.RadialPattern[num];
					if (intVec.InBounds(carrier.Map) && ((Area)carrier.Map.areaManager.Home)[intVec] && carrier.CanReach(intVec, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn) && intVec.GetSlotGroup(carrier.Map) == null && StoreUtility.IsGoodStoreCell(intVec, carrier.Map, item, carrier, carrier.Faction))
						goto IL_0123;
				}
				if (RCellFinder.TryFindRandomSpotJustOutsideColony(carrier.Position, carrier.Map, carrier, out storeCell, (Predicate<IntVec3>)((IntVec3 x) => x.GetSlotGroup(carrier.Map) == null && StoreUtility.IsGoodStoreCell(x, carrier.Map, item, carrier, carrier.Faction))))
				{
					result = true;
				}
				else
				{
					storeCell = IntVec3.Invalid;
					result = false;
				}
			}
			goto IL_018d;
			IL_0123:
			storeCell = intVec;
			result = true;
			goto IL_018d;
			IL_018d:
			return result;
		}
	}
}
