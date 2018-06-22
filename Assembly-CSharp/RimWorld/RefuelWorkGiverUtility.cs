using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000158 RID: 344
	public static class RefuelWorkGiverUtility
	{
		// Token: 0x06000715 RID: 1813 RVA: 0x00047F70 File Offset: 0x00046370
		public static bool CanRefuel(Pawn pawn, Thing t, bool forced = false)
		{
			CompRefuelable compRefuelable = t.TryGetComp<CompRefuelable>();
			bool result;
			if (compRefuelable == null || compRefuelable.IsFull)
			{
				result = false;
			}
			else
			{
				bool flag = !forced;
				if (flag && !compRefuelable.ShouldAutoRefuelNow)
				{
					result = false;
				}
				else
				{
					if (!t.IsForbidden(pawn))
					{
						LocalTargetInfo target = t;
						if (pawn.CanReserve(target, 1, -1, null, forced))
						{
							if (t.Faction != pawn.Faction)
							{
								return false;
							}
							if (RefuelWorkGiverUtility.FindBestFuel(pawn, t) == null)
							{
								ThingFilter fuelFilter = t.TryGetComp<CompRefuelable>().Props.fuelFilter;
								JobFailReason.Is("NoFuelToRefuel".Translate(new object[]
								{
									fuelFilter.Summary
								}), null);
								return false;
							}
							if (t.TryGetComp<CompRefuelable>().Props.atomicFueling)
							{
								if (RefuelWorkGiverUtility.FindAllFuel(pawn, t) == null)
								{
									ThingFilter fuelFilter2 = t.TryGetComp<CompRefuelable>().Props.fuelFilter;
									JobFailReason.Is("NoFuelToRefuel".Translate(new object[]
									{
										fuelFilter2.Summary
									}), null);
									return false;
								}
							}
							return true;
						}
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x000480B4 File Offset: 0x000464B4
		public static Job RefuelJob(Pawn pawn, Thing t, bool forced = false, JobDef customRefuelJob = null, JobDef customAtomicRefuelJob = null)
		{
			Job result;
			if (!t.TryGetComp<CompRefuelable>().Props.atomicFueling)
			{
				Thing t2 = RefuelWorkGiverUtility.FindBestFuel(pawn, t);
				result = new Job(customRefuelJob ?? JobDefOf.Refuel, t, t2);
			}
			else
			{
				List<Thing> source = RefuelWorkGiverUtility.FindAllFuel(pawn, t);
				Job job = new Job(customAtomicRefuelJob ?? JobDefOf.RefuelAtomic, t);
				job.targetQueueB = (from f in source
				select new LocalTargetInfo(f)).ToList<LocalTargetInfo>();
				result = job;
			}
			return result;
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x00048160 File Offset: 0x00046560
		private static Thing FindBestFuel(Pawn pawn, Thing refuelable)
		{
			ThingFilter filter = refuelable.TryGetComp<CompRefuelable>().Props.fuelFilter;
			Predicate<Thing> predicate = (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false) && filter.Allows(x);
			IntVec3 position = pawn.Position;
			Map map = pawn.Map;
			ThingRequest bestThingRequest = filter.BestThingRequest;
			PathEndMode peMode = PathEndMode.ClosestTouch;
			TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
			Predicate<Thing> validator = predicate;
			return GenClosest.ClosestThingReachable(position, map, bestThingRequest, peMode, traverseParams, 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x000481FC File Offset: 0x000465FC
		private static List<Thing> FindAllFuel(Pawn pawn, Thing refuelable)
		{
			int quantity = refuelable.TryGetComp<CompRefuelable>().GetFuelCountToFullyRefuel();
			ThingFilter filter = refuelable.TryGetComp<CompRefuelable>().Props.fuelFilter;
			Predicate<Thing> validator = (Thing x) => !x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false) && filter.Allows(x);
			IntVec3 position = refuelable.Position;
			Region region = position.GetRegion(pawn.Map, RegionType.Set_Passable);
			TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
			RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(traverseParams, false);
			List<Thing> chosenThings = new List<Thing>();
			int accumulatedQuantity = 0;
			RegionProcessor regionProcessor = delegate(Region r)
			{
				List<Thing> list = r.ListerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.HaulableEver));
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing = list[i];
					if (validator(thing))
					{
						if (!chosenThings.Contains(thing))
						{
							if (ReachabilityWithinRegion.ThingFromRegionListerReachable(thing, r, PathEndMode.ClosestTouch, pawn))
							{
								chosenThings.Add(thing);
								accumulatedQuantity += thing.stackCount;
								if (accumulatedQuantity >= quantity)
								{
									return true;
								}
							}
						}
					}
				}
				return false;
			};
			RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, 99999, RegionType.Set_Passable);
			List<Thing> result;
			if (accumulatedQuantity >= quantity)
			{
				result = chosenThings;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
