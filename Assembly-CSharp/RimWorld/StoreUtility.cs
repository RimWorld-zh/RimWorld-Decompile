using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class StoreUtility
	{
		public static IHaulDestination CurrentHaulDestinationOf(Thing t)
		{
			IHaulDestination result;
			if (t.Spawned)
			{
				result = t.Map.haulDestinationManager.SlotGroupParentAt(t.Position);
			}
			else
			{
				result = (t.ParentHolder as IHaulDestination);
			}
			return result;
		}

		public static StoragePriority CurrentStoragePriorityOf(Thing t)
		{
			return StoreUtility.StoragePriorityAtFor(StoreUtility.CurrentHaulDestinationOf(t), t);
		}

		public static StoragePriority StoragePriorityAtFor(IntVec3 c, Thing t)
		{
			return StoreUtility.StoragePriorityAtFor(t.Map.haulDestinationManager.SlotGroupParentAt(c), t);
		}

		public static StoragePriority StoragePriorityAtFor(IHaulDestination at, Thing t)
		{
			StoragePriority result;
			if (at == null || !at.Accepts(t))
			{
				result = StoragePriority.Unstored;
			}
			else
			{
				result = at.GetStoreSettings().Priority;
			}
			return result;
		}

		public static bool IsInAnyStorage(this Thing t)
		{
			return StoreUtility.CurrentHaulDestinationOf(t) != null;
		}

		public static bool IsInValidStorage(this Thing t)
		{
			IHaulDestination haulDestination = StoreUtility.CurrentHaulDestinationOf(t);
			return haulDestination != null && haulDestination.Accepts(t);
		}

		public static bool IsInValidBestStorage(this Thing t)
		{
			IHaulDestination haulDestination = StoreUtility.CurrentHaulDestinationOf(t);
			IntVec3 intVec;
			IHaulDestination haulDestination2;
			return haulDestination != null && haulDestination.Accepts(t) && !StoreUtility.TryFindBestBetterStorageFor(t, null, t.Map, haulDestination.GetStoreSettings().Priority, Faction.OfPlayer, out intVec, out haulDestination2, false);
		}

		public static Thing StoringThing(this Thing t)
		{
			return StoreUtility.CurrentHaulDestinationOf(t) as Thing;
		}

		public static SlotGroup GetSlotGroup(this Thing thing)
		{
			SlotGroup result;
			if (!thing.Spawned)
			{
				result = null;
			}
			else
			{
				result = thing.Position.GetSlotGroup(thing.Map);
			}
			return result;
		}

		public static SlotGroup GetSlotGroup(this IntVec3 c, Map map)
		{
			SlotGroup result;
			if (map.haulDestinationManager == null)
			{
				result = null;
			}
			else
			{
				result = map.haulDestinationManager.SlotGroupAt(c);
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
				result = (slotGroup != null && slotGroup.parent.Accepts(storable));
			}
			return result;
		}

		private static bool NoStorageBlockersIn(IntVec3 c, Map map, Thing thing)
		{
			List<Thing> list = map.thingGrid.ThingsListAt(c);
			int i = 0;
			while (i < list.Count)
			{
				Thing thing2 = list[i];
				if (thing2.def.EverStorable(false))
				{
					if (!thing2.CanStackWith(thing))
					{
						return false;
					}
					if (thing2.stackCount >= thing.def.stackLimit)
					{
						return false;
					}
				}
				bool result;
				if (thing2.def.entityDefToBuild != null && thing2.def.entityDefToBuild.passability != Traversability.Standable)
				{
					result = false;
				}
				else
				{
					if (thing2.def.surfaceType != SurfaceType.None || thing2.def.passability == Traversability.Standable)
					{
						i++;
						continue;
					}
					result = false;
				}
				return result;
			}
			return true;
		}

		public static bool TryFindBestBetterStorageFor(Thing t, Pawn carrier, Map map, StoragePriority currentPriority, Faction faction, out IntVec3 foundCell, out IHaulDestination haulDestination, bool needAccurateResult = true)
		{
			IntVec3 invalid = IntVec3.Invalid;
			StoragePriority storagePriority = StoragePriority.Unstored;
			if (StoreUtility.TryFindBestBetterStoreCellFor(t, carrier, map, currentPriority, faction, out invalid, needAccurateResult))
			{
				storagePriority = invalid.GetSlotGroup(map).Settings.Priority;
			}
			IHaulDestination haulDestination2;
			if (!StoreUtility.TryFindBestBetterNonSlotGroupStorageFor(t, carrier, map, currentPriority, faction, out haulDestination2))
			{
				haulDestination2 = null;
			}
			bool result;
			if (storagePriority == StoragePriority.Unstored && haulDestination2 == null)
			{
				foundCell = IntVec3.Invalid;
				haulDestination = null;
				result = false;
			}
			else if (haulDestination2 != null && (storagePriority == StoragePriority.Unstored || haulDestination2.GetStoreSettings().Priority > storagePriority))
			{
				foundCell = IntVec3.Invalid;
				haulDestination = haulDestination2;
				result = true;
			}
			else
			{
				foundCell = invalid;
				haulDestination = invalid.GetSlotGroup(map).parent;
				result = true;
			}
			return result;
		}

		public static bool TryFindBestBetterStoreCellFor(Thing t, Pawn carrier, Map map, StoragePriority currentPriority, Faction faction, out IntVec3 foundCell, bool needAccurateResult = true)
		{
			List<SlotGroup> allGroupsListInPriorityOrder = map.haulDestinationManager.AllGroupsListInPriorityOrder;
			bool result;
			if (allGroupsListInPriorityOrder.Count == 0)
			{
				foundCell = IntVec3.Invalid;
				result = false;
			}
			else
			{
				StoragePriority storagePriority = currentPriority;
				float num = 2.14748365E+09f;
				IntVec3 invalid = IntVec3.Invalid;
				int count = allGroupsListInPriorityOrder.Count;
				for (int i = 0; i < count; i++)
				{
					SlotGroup slotGroup = allGroupsListInPriorityOrder[i];
					StoragePriority priority = slotGroup.Settings.Priority;
					if (priority < storagePriority || priority <= currentPriority)
					{
						break;
					}
					StoreUtility.TryFindBestBetterStoreCellForWorker(t, carrier, map, faction, slotGroup, needAccurateResult, ref invalid, ref num, ref storagePriority);
				}
				if (!invalid.IsValid)
				{
					foundCell = IntVec3.Invalid;
					result = false;
				}
				else
				{
					foundCell = invalid;
					result = true;
				}
			}
			return result;
		}

		public static bool TryFindBestBetterStoreCellForIn(Thing t, Pawn carrier, Map map, StoragePriority currentPriority, Faction faction, SlotGroup slotGroup, out IntVec3 foundCell, bool needAccurateResult = true)
		{
			foundCell = IntVec3.Invalid;
			float num = 2.14748365E+09f;
			StoreUtility.TryFindBestBetterStoreCellForWorker(t, carrier, map, faction, slotGroup, needAccurateResult, ref foundCell, ref num, ref currentPriority);
			return foundCell.IsValid;
		}

		private static void TryFindBestBetterStoreCellForWorker(Thing t, Pawn carrier, Map map, Faction faction, SlotGroup slotGroup, bool needAccurateResult, ref IntVec3 closestSlot, ref float closestDistSquared, ref StoragePriority foundPriority)
		{
			if (slotGroup.parent.Accepts(t))
			{
				IntVec3 a = (!t.SpawnedOrAnyParentSpawned) ? carrier.PositionHeld : t.PositionHeld;
				List<IntVec3> cellsList = slotGroup.CellsList;
				int count = cellsList.Count;
				int num;
				if (needAccurateResult)
				{
					num = Mathf.FloorToInt((float)count * Rand.Range(0.005f, 0.018f));
				}
				else
				{
					num = 0;
				}
				for (int i = 0; i < count; i++)
				{
					IntVec3 intVec = cellsList[i];
					float num2 = (float)(a - intVec).LengthHorizontalSquared;
					if (num2 <= closestDistSquared)
					{
						if (StoreUtility.IsGoodStoreCell(intVec, map, t, carrier, faction))
						{
							closestSlot = intVec;
							closestDistSquared = num2;
							foundPriority = slotGroup.Settings.Priority;
							if (i >= num)
							{
								break;
							}
						}
					}
				}
			}
		}

		public static bool TryFindBestBetterNonSlotGroupStorageFor(Thing t, Pawn carrier, Map map, StoragePriority currentPriority, Faction faction, out IHaulDestination haulDestination)
		{
			List<IHaulDestination> allHaulDestinationsListInPriorityOrder = map.haulDestinationManager.AllHaulDestinationsListInPriorityOrder;
			IntVec3 intVec = (!t.SpawnedOrAnyParentSpawned) ? carrier.PositionHeld : t.PositionHeld;
			float num = float.MaxValue;
			StoragePriority storagePriority = StoragePriority.Unstored;
			haulDestination = null;
			for (int i = 0; i < allHaulDestinationsListInPriorityOrder.Count; i++)
			{
				if (!(allHaulDestinationsListInPriorityOrder[i] is ISlotGroupParent))
				{
					StoragePriority priority = allHaulDestinationsListInPriorityOrder[i].GetStoreSettings().Priority;
					if (priority < storagePriority || priority <= currentPriority)
					{
						break;
					}
					float num2 = (float)intVec.DistanceToSquared(allHaulDestinationsListInPriorityOrder[i].Position);
					if (num2 <= num)
					{
						if (allHaulDestinationsListInPriorityOrder[i].Accepts(t))
						{
							Thing thing = allHaulDestinationsListInPriorityOrder[i] as Thing;
							if (thing == null || thing.Faction == faction)
							{
								if (thing != null)
								{
									if (carrier != null)
									{
										if (thing.IsForbidden(carrier))
										{
											goto IL_204;
										}
									}
									else if (faction != null && thing.IsForbidden(faction))
									{
										goto IL_204;
									}
								}
								if (thing != null)
								{
									if (carrier != null)
									{
										if (!carrier.CanReserveNew(thing))
										{
											goto IL_204;
										}
									}
									else if (faction != null && map.reservationManager.IsReservedByAnyoneOf(thing, faction))
									{
										goto IL_204;
									}
								}
								if (carrier != null)
								{
									if (thing != null)
									{
										if (!carrier.Map.reachability.CanReach(intVec, thing, PathEndMode.ClosestTouch, TraverseParms.For(carrier, Danger.Deadly, TraverseMode.ByPawn, false)))
										{
											goto IL_204;
										}
									}
									else if (!carrier.Map.reachability.CanReach(intVec, allHaulDestinationsListInPriorityOrder[i].Position, PathEndMode.ClosestTouch, TraverseParms.For(carrier, Danger.Deadly, TraverseMode.ByPawn, false)))
									{
										goto IL_204;
									}
								}
								num = num2;
								storagePriority = priority;
								haulDestination = allHaulDestinationsListInPriorityOrder[i];
							}
						}
					}
				}
				IL_204:;
			}
			return haulDestination != null;
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
						return false;
					}
				}
				else if (faction != null && map.reservationManager.IsReservedByAnyoneOf(c, faction))
				{
					return false;
				}
				if (c.ContainsStaticFire(map))
				{
					result = false;
				}
				else
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (thingList[i] is IConstructible && GenConstruct.BlocksConstruction(thingList[i], t))
						{
							return false;
						}
					}
					result = (carrier == null || carrier.Map.reachability.CanReach((!t.SpawnedOrAnyParentSpawned) ? carrier.PositionHeld : t.PositionHeld, c, PathEndMode.ClosestTouch, TraverseParms.For(carrier, Danger.Deadly, TraverseMode.ByPawn, false)));
				}
			}
			return result;
		}

		public static bool TryFindStoreCellNearColonyDesperate(Thing item, Pawn carrier, out IntVec3 storeCell)
		{
			bool result;
			if (StoreUtility.TryFindBestBetterStoreCellFor(item, carrier, carrier.Map, StoragePriority.Unstored, carrier.Faction, out storeCell, true))
			{
				result = true;
			}
			else
			{
				for (int i = -4; i < 20; i++)
				{
					int num = (i >= 0) ? i : Rand.RangeInclusive(0, 4);
					IntVec3 intVec = carrier.Position + GenRadial.RadialPattern[num];
					if (intVec.InBounds(carrier.Map) && carrier.Map.areaManager.Home[intVec] && carrier.CanReach(intVec, PathEndMode.ClosestTouch, Danger.Deadly, false, TraverseMode.ByPawn) && intVec.GetSlotGroup(carrier.Map) == null && StoreUtility.IsGoodStoreCell(intVec, carrier.Map, item, carrier, carrier.Faction))
					{
						storeCell = intVec;
						return true;
					}
				}
				if (RCellFinder.TryFindRandomSpotJustOutsideColony(carrier.Position, carrier.Map, carrier, out storeCell, (IntVec3 x) => x.GetSlotGroup(carrier.Map) == null && StoreUtility.IsGoodStoreCell(x, carrier.Map, item, carrier, carrier.Faction)))
				{
					result = true;
				}
				else
				{
					storeCell = IntVec3.Invalid;
					result = false;
				}
			}
			return result;
		}

		[CompilerGenerated]
		private sealed class <TryFindStoreCellNearColonyDesperate>c__AnonStorey0
		{
			internal Pawn carrier;

			internal Thing item;

			public <TryFindStoreCellNearColonyDesperate>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return x.GetSlotGroup(this.carrier.Map) == null && StoreUtility.IsGoodStoreCell(x, this.carrier.Map, this.item, this.carrier, this.carrier.Faction);
			}
		}
	}
}
