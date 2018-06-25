using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using UnityEngine.Profiling;

namespace Verse.AI
{
	public static class HaulAIUtility
	{
		private static string ForbiddenLowerTrans;

		private static string ForbiddenOutsideAllowedAreaLowerTrans;

		private static string ReservedForPrisonersTrans;

		private static string BurningLowerTrans;

		private static string NoEmptyPlaceLowerTrans;

		private static List<IntVec3> candidates = new List<IntVec3>();

		public static void Reset()
		{
			HaulAIUtility.ForbiddenLowerTrans = "ForbiddenLower".Translate();
			HaulAIUtility.ForbiddenOutsideAllowedAreaLowerTrans = "ForbiddenOutsideAllowedAreaLower".Translate();
			HaulAIUtility.ReservedForPrisonersTrans = "ReservedForPrisoners".Translate();
			HaulAIUtility.BurningLowerTrans = "BurningLower".Translate();
			HaulAIUtility.NoEmptyPlaceLowerTrans = "NoEmptyPlaceLower".Translate();
		}

		public static bool PawnCanAutomaticallyHaul(Pawn p, Thing t, bool forced)
		{
			bool result;
			if (!t.def.EverHaulable)
			{
				result = false;
			}
			else if (t.IsForbidden(p))
			{
				if (!t.Position.InAllowedArea(p))
				{
					JobFailReason.Is(HaulAIUtility.ForbiddenOutsideAllowedAreaLowerTrans, null);
				}
				else
				{
					JobFailReason.Is(HaulAIUtility.ForbiddenLowerTrans, null);
				}
				result = false;
			}
			else
			{
				result = ((t.def.alwaysHaulable || t.Map.designationManager.DesignationOn(t, DesignationDefOf.Haul) != null || t.IsInValidStorage()) && HaulAIUtility.PawnCanAutomaticallyHaulFast(p, t, forced));
			}
			return result;
		}

		public static bool PawnCanAutomaticallyHaulFast(Pawn p, Thing t, bool forced)
		{
			UnfinishedThing unfinishedThing = t as UnfinishedThing;
			bool result;
			if (unfinishedThing != null && unfinishedThing.BoundBill != null)
			{
				result = false;
			}
			else
			{
				Profiler.BeginSample("CanReach");
				if (!p.CanReach(t, PathEndMode.ClosestTouch, p.NormalMaxDanger(), false, TraverseMode.ByPawn))
				{
					Profiler.EndSample();
					result = false;
				}
				else
				{
					Profiler.EndSample();
					LocalTargetInfo target = t;
					if (!p.CanReserve(target, 1, -1, null, forced))
					{
						result = false;
					}
					else
					{
						if (t.def.IsNutritionGivingIngestible && t.def.ingestible.HumanEdible)
						{
							if (!t.IsSociallyProper(p, false, true))
							{
								JobFailReason.Is(HaulAIUtility.ReservedForPrisonersTrans, null);
								return false;
							}
						}
						if (t.IsBurning())
						{
							JobFailReason.Is(HaulAIUtility.BurningLowerTrans, null);
							result = false;
						}
						else
						{
							result = true;
						}
					}
				}
			}
			return result;
		}

		public static Job HaulToStorageJob(Pawn p, Thing t)
		{
			StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(t);
			IntVec3 storeCell;
			IHaulDestination haulDestination;
			Job result;
			if (!StoreUtility.TryFindBestBetterStorageFor(t, p, p.Map, currentPriority, p.Faction, out storeCell, out haulDestination, true))
			{
				JobFailReason.Is(HaulAIUtility.NoEmptyPlaceLowerTrans, null);
				result = null;
			}
			else if (haulDestination is ISlotGroupParent)
			{
				result = HaulAIUtility.HaulToCellStorageJob(p, t, storeCell, false);
			}
			else
			{
				Thing thing = haulDestination as Thing;
				if (thing != null && thing.TryGetInnerInteractableThingOwner() != null)
				{
					result = HaulAIUtility.HaulToContainerJob(p, t, thing);
				}
				else
				{
					Log.Error("Don't know how to handle HaulToStorageJob for storage " + haulDestination.ToStringSafe<IHaulDestination>() + ". thing=" + t.ToStringSafe<Thing>(), false);
					result = null;
				}
			}
			return result;
		}

		public static Job HaulToCellStorageJob(Pawn p, Thing t, IntVec3 storeCell, bool fitInStoreCell)
		{
			Job job = new Job(JobDefOf.HaulToCell, t, storeCell);
			SlotGroup slotGroup = p.Map.haulDestinationManager.SlotGroupAt(storeCell);
			if (slotGroup != null)
			{
				Thing thing = p.Map.thingGrid.ThingAt(storeCell, t.def);
				if (thing != null)
				{
					job.count = t.def.stackLimit;
					if (fitInStoreCell)
					{
						job.count -= thing.stackCount;
					}
				}
				else
				{
					job.count = 99999;
				}
				int num = 0;
				float statValue = p.GetStatValue(StatDefOf.CarryingCapacity, true);
				List<IntVec3> cellsList = slotGroup.CellsList;
				for (int i = 0; i < cellsList.Count; i++)
				{
					if (StoreUtility.IsGoodStoreCell(cellsList[i], p.Map, t, p, p.Faction))
					{
						Thing thing2 = p.Map.thingGrid.ThingAt(cellsList[i], t.def);
						if (thing2 != null && thing2 != t)
						{
							num += Mathf.Max(t.def.stackLimit - thing2.stackCount, 0);
						}
						else
						{
							num += t.def.stackLimit;
						}
						if (num >= job.count || (float)num >= statValue)
						{
							break;
						}
					}
				}
				job.count = Mathf.Min(job.count, num);
			}
			else
			{
				job.count = 99999;
			}
			job.haulOpportunisticDuplicates = true;
			job.haulMode = HaulMode.ToCellStorage;
			return job;
		}

		public static Job HaulToContainerJob(Pawn p, Thing t, Thing container)
		{
			ThingOwner thingOwner = container.TryGetInnerInteractableThingOwner();
			Job result;
			if (thingOwner == null)
			{
				Log.Error(container.ToStringSafe<Thing>() + " gave null ThingOwner.", false);
				result = null;
			}
			else
			{
				result = new Job(JobDefOf.HaulToContainer, t, container)
				{
					count = Mathf.Min(t.stackCount, thingOwner.GetCountCanAccept(t, true)),
					haulMode = HaulMode.ToContainer
				};
			}
			return result;
		}

		public static bool CanHaulAside(Pawn p, Thing t, out IntVec3 storeCell)
		{
			storeCell = IntVec3.Invalid;
			return t.def.EverHaulable && !t.IsBurning() && p.CanReserveAndReach(t, PathEndMode.ClosestTouch, p.NormalMaxDanger(), 1, -1, null, false) && HaulAIUtility.TryFindSpotToPlaceHaulableCloseTo(t, p, t.PositionHeld, out storeCell);
		}

		public static Job HaulAsideJobFor(Pawn p, Thing t)
		{
			IntVec3 c;
			Job result;
			if (!HaulAIUtility.CanHaulAside(p, t, out c))
			{
				result = null;
			}
			else
			{
				result = new Job(JobDefOf.HaulToCell, t, c)
				{
					count = 99999,
					haulOpportunisticDuplicates = false,
					haulMode = HaulMode.ToCellNonStorage,
					ignoreDesignations = true
				};
			}
			return result;
		}

		private static bool TryFindSpotToPlaceHaulableCloseTo(Thing haulable, Pawn worker, IntVec3 center, out IntVec3 spot)
		{
			Region region = center.GetRegion(worker.Map, RegionType.Set_Passable);
			bool result;
			if (region == null)
			{
				spot = center;
				result = false;
			}
			else
			{
				TraverseParms traverseParms = TraverseParms.For(worker, Danger.Deadly, TraverseMode.ByPawn, false);
				IntVec3 foundCell = IntVec3.Invalid;
				RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.Allows(traverseParms, false), delegate(Region r)
				{
					HaulAIUtility.candidates.Clear();
					HaulAIUtility.candidates.AddRange(r.Cells);
					HaulAIUtility.candidates.Sort((IntVec3 a, IntVec3 b) => a.DistanceToSquared(center).CompareTo(b.DistanceToSquared(center)));
					for (int i = 0; i < HaulAIUtility.candidates.Count; i++)
					{
						IntVec3 intVec = HaulAIUtility.candidates[i];
						if (HaulAIUtility.HaulablePlaceValidator(haulable, worker, intVec))
						{
							foundCell = intVec;
							return true;
						}
					}
					return false;
				}, 100, RegionType.Set_Passable);
				if (foundCell.IsValid)
				{
					spot = foundCell;
					result = true;
				}
				else
				{
					spot = center;
					result = false;
				}
			}
			return result;
		}

		private static bool HaulablePlaceValidator(Thing haulable, Pawn worker, IntVec3 c)
		{
			bool result;
			if (!worker.CanReserveAndReach(c, PathEndMode.OnCell, worker.NormalMaxDanger(), 1, -1, null, false))
			{
				result = false;
			}
			else if (GenPlace.HaulPlaceBlockerIn(haulable, c, worker.Map, true) != null)
			{
				result = false;
			}
			else if (!c.Standable(worker.Map))
			{
				result = false;
			}
			else if (c == haulable.Position && haulable.Spawned)
			{
				result = false;
			}
			else if (c.ContainsStaticFire(worker.Map))
			{
				result = false;
			}
			else
			{
				if (haulable != null && haulable.def.BlockPlanting)
				{
					Zone zone = worker.Map.zoneManager.ZoneAt(c);
					if (zone is Zone_Growing)
					{
						return false;
					}
				}
				if (haulable.def.passability != Traversability.Standable)
				{
					for (int i = 0; i < 8; i++)
					{
						IntVec3 c2 = c + GenAdj.AdjacentCells[i];
						if (worker.Map.designationManager.DesignationAt(c2, DesignationDefOf.Mine) != null)
						{
							return false;
						}
					}
				}
				Building edifice = c.GetEdifice(worker.Map);
				if (edifice != null)
				{
					Building_Trap building_Trap = edifice as Building_Trap;
					if (building_Trap != null)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static HaulAIUtility()
		{
		}

		[CompilerGenerated]
		private sealed class <TryFindSpotToPlaceHaulableCloseTo>c__AnonStorey0
		{
			internal TraverseParms traverseParms;

			internal IntVec3 center;

			internal Thing haulable;

			internal Pawn worker;

			internal IntVec3 foundCell;

			public <TryFindSpotToPlaceHaulableCloseTo>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Region from, Region r)
			{
				return r.Allows(this.traverseParms, false);
			}

			internal bool <>m__1(Region r)
			{
				HaulAIUtility.candidates.Clear();
				HaulAIUtility.candidates.AddRange(r.Cells);
				HaulAIUtility.candidates.Sort((IntVec3 a, IntVec3 b) => a.DistanceToSquared(this.center).CompareTo(b.DistanceToSquared(this.center)));
				for (int i = 0; i < HaulAIUtility.candidates.Count; i++)
				{
					IntVec3 c = HaulAIUtility.candidates[i];
					if (HaulAIUtility.HaulablePlaceValidator(this.haulable, this.worker, c))
					{
						this.foundCell = c;
						return true;
					}
				}
				return false;
			}

			internal int <>m__2(IntVec3 a, IntVec3 b)
			{
				return a.DistanceToSquared(this.center).CompareTo(b.DistanceToSquared(this.center));
			}
		}
	}
}
