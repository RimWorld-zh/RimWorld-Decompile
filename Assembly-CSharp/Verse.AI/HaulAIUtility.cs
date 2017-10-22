#define ENABLE_PROFILER
using RimWorld;
using System;
using System.Collections.Generic;
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
					JobFailReason.Is(HaulAIUtility.ForbiddenOutsideAllowedAreaLowerTrans);
				}
				else
				{
					JobFailReason.Is(HaulAIUtility.ForbiddenLowerTrans);
				}
				result = false;
			}
			else
			{
				result = ((byte)((t.def.alwaysHaulable || t.Map.designationManager.DesignationOn(t, DesignationDefOf.Haul) != null || t.IsInValidStorage()) ? (HaulAIUtility.PawnCanAutomaticallyHaulBasicChecks(p, t, forced) ? 1 : 0) : 0) != 0);
			}
			return result;
		}

		public static bool PawnCanAutomaticallyHaulFast(Pawn p, Thing t, bool forced)
		{
			return HaulAIUtility.PawnCanAutomaticallyHaulBasicChecks(p, t, forced);
		}

		private static bool PawnCanAutomaticallyHaulBasicChecks(Pawn p, Thing t, bool forced)
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
					else if (t.def.IsNutritionGivingIngestible && t.def.ingestible.HumanEdible && !t.IsSociallyProper(p, false, true))
					{
						JobFailReason.Is(HaulAIUtility.ReservedForPrisonersTrans);
						result = false;
					}
					else if (t.IsBurning())
					{
						JobFailReason.Is(HaulAIUtility.BurningLowerTrans);
						result = false;
					}
					else
					{
						result = true;
					}
				}
			}
			return result;
		}

		public static Job HaulToStorageJob(Pawn p, Thing t)
		{
			StoragePriority currentPriority = HaulAIUtility.StoragePriorityAtFor(t.Position, t);
			IntVec3 storeCell = default(IntVec3);
			Job result;
			if (!StoreUtility.TryFindBestBetterStoreCellFor(t, p, p.Map, currentPriority, p.Faction, out storeCell, true))
			{
				JobFailReason.Is(HaulAIUtility.NoEmptyPlaceLowerTrans);
				result = null;
			}
			else
			{
				result = HaulAIUtility.HaulMaxNumToCellJob(p, t, storeCell, false);
			}
			return result;
		}

		public static Job HaulMaxNumToCellJob(Pawn p, Thing t, IntVec3 storeCell, bool fitInStoreCell)
		{
			Job job = new Job(JobDefOf.HaulToCell, t, storeCell);
			SlotGroup slotGroup = p.Map.slotGroupManager.SlotGroupAt(storeCell);
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
						num = ((thing2 == null || thing2 == t) ? (num + t.def.stackLimit) : (num + Mathf.Max(t.def.stackLimit - thing2.stackCount, 0)));
						if (num >= job.count)
							break;
						if ((float)num >= statValue)
							break;
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

		public static StoragePriority StoragePriorityAtFor(IntVec3 c, Thing t)
		{
			StoragePriority result;
			if (!t.Spawned)
			{
				result = StoragePriority.Unstored;
			}
			else
			{
				SlotGroup slotGroup = t.Map.slotGroupManager.SlotGroupAt(c);
				result = ((slotGroup != null && slotGroup.Settings.AllowedToAccept(t)) ? slotGroup.Settings.Priority : StoragePriority.Unstored);
			}
			return result;
		}

		public static bool CanHaulAside(Pawn p, Thing t, out IntVec3 storeCell)
		{
			storeCell = IntVec3.Invalid;
			return (byte)(t.def.EverHaulable ? ((!t.IsBurning()) ? (p.CanReserveAndReach(t, PathEndMode.ClosestTouch, p.NormalMaxDanger(), 1, -1, null, false) ? (HaulAIUtility.TryFindSpotToPlaceHaulableCloseTo(t, p, t.PositionHeld, out storeCell) ? 1 : 0) : 0) : 0) : 0) != 0;
		}

		public static Job HaulAsideJobFor(Pawn p, Thing t)
		{
			IntVec3 c = default(IntVec3);
			Job result;
			if (!HaulAIUtility.CanHaulAside(p, t, out c))
			{
				result = null;
			}
			else
			{
				Job job = new Job(JobDefOf.HaulToCell, t, c);
				job.count = 99999;
				job.haulOpportunisticDuplicates = false;
				job.haulMode = HaulMode.ToCellNonStorage;
				job.ignoreDesignations = true;
				result = job;
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
				RegionTraverser.BreadthFirstTraverse(region, (RegionEntryPredicate)((Region from, Region r) => r.Allows(traverseParms, false)), (RegionProcessor)delegate(Region r)
				{
					HaulAIUtility.candidates.Clear();
					HaulAIUtility.candidates.AddRange(r.Cells);
					HaulAIUtility.candidates.Sort((Comparison<IntVec3>)((IntVec3 a, IntVec3 b) => a.DistanceToSquared(center).CompareTo(b.DistanceToSquared(center))));
					int num = 0;
					bool result2;
					while (true)
					{
						if (num < HaulAIUtility.candidates.Count)
						{
							IntVec3 intVec = HaulAIUtility.candidates[num];
							if (HaulAIUtility.HaulablePlaceValidator(haulable, worker, intVec))
							{
								IntVec3 foundCell2 = intVec;
								result2 = true;
								break;
							}
							num++;
							continue;
						}
						result2 = false;
						break;
					}
					return result2;
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
						result = false;
						goto IL_0161;
					}
				}
				if (haulable.def.passability != 0)
				{
					for (int i = 0; i < 8; i++)
					{
						IntVec3 c2 = c + GenAdj.AdjacentCells[i];
						if (worker.Map.designationManager.DesignationAt(c2, DesignationDefOf.Mine) != null)
							goto IL_0118;
					}
				}
				Building edifice = c.GetEdifice(worker.Map);
				if (edifice != null)
				{
					Building_Trap building_Trap = edifice as Building_Trap;
					if (building_Trap != null)
					{
						result = false;
						goto IL_0161;
					}
				}
				result = true;
			}
			goto IL_0161;
			IL_0161:
			return result;
			IL_0118:
			result = false;
			goto IL_0161;
		}
	}
}
