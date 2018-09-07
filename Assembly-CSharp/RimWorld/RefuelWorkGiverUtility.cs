using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public static class RefuelWorkGiverUtility
	{
		[CompilerGenerated]
		private static Func<Thing, LocalTargetInfo> <>f__am$cache0;

		public static bool CanRefuel(Pawn pawn, Thing t, bool forced = false)
		{
			CompRefuelable compRefuelable = t.TryGetComp<CompRefuelable>();
			if (compRefuelable == null || compRefuelable.IsFull)
			{
				return false;
			}
			bool flag = !forced;
			if (flag && !compRefuelable.ShouldAutoRefuelNow)
			{
				return false;
			}
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
					if (t.TryGetComp<CompRefuelable>().Props.atomicFueling && RefuelWorkGiverUtility.FindAllFuel(pawn, t) == null)
					{
						ThingFilter fuelFilter2 = t.TryGetComp<CompRefuelable>().Props.fuelFilter;
						JobFailReason.Is("NoFuelToRefuel".Translate(new object[]
						{
							fuelFilter2.Summary
						}), null);
						return false;
					}
					return true;
				}
			}
			return false;
		}

		public static Job RefuelJob(Pawn pawn, Thing t, bool forced = false, JobDef customRefuelJob = null, JobDef customAtomicRefuelJob = null)
		{
			if (!t.TryGetComp<CompRefuelable>().Props.atomicFueling)
			{
				Thing t2 = RefuelWorkGiverUtility.FindBestFuel(pawn, t);
				return new Job(customRefuelJob ?? JobDefOf.Refuel, t, t2);
			}
			List<Thing> source = RefuelWorkGiverUtility.FindAllFuel(pawn, t);
			Job job = new Job(customAtomicRefuelJob ?? JobDefOf.RefuelAtomic, t);
			job.targetQueueB = (from f in source
			select new LocalTargetInfo(f)).ToList<LocalTargetInfo>();
			return job;
		}

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
			if (accumulatedQuantity >= quantity)
			{
				return chosenThings;
			}
			return null;
		}

		[CompilerGenerated]
		private static LocalTargetInfo <RefuelJob>m__0(Thing f)
		{
			return new LocalTargetInfo(f);
		}

		[CompilerGenerated]
		private sealed class <FindBestFuel>c__AnonStorey0
		{
			internal Pawn pawn;

			internal ThingFilter filter;

			public <FindBestFuel>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing x)
			{
				return !x.IsForbidden(this.pawn) && this.pawn.CanReserve(x, 1, -1, null, false) && this.filter.Allows(x);
			}
		}

		[CompilerGenerated]
		private sealed class <FindAllFuel>c__AnonStorey1
		{
			internal Pawn pawn;

			internal ThingFilter filter;

			internal TraverseParms traverseParams;

			internal Predicate<Thing> validator;

			internal List<Thing> chosenThings;

			internal int accumulatedQuantity;

			internal int quantity;

			public <FindAllFuel>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Thing x)
			{
				return !x.IsForbidden(this.pawn) && this.pawn.CanReserve(x, 1, -1, null, false) && this.filter.Allows(x);
			}

			internal bool <>m__1(Region from, Region r)
			{
				return r.Allows(this.traverseParams, false);
			}

			internal bool <>m__2(Region r)
			{
				List<Thing> list = r.ListerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.HaulableEver));
				for (int i = 0; i < list.Count; i++)
				{
					Thing thing = list[i];
					if (this.validator(thing))
					{
						if (!this.chosenThings.Contains(thing))
						{
							if (ReachabilityWithinRegion.ThingFromRegionListerReachable(thing, r, PathEndMode.ClosestTouch, this.pawn))
							{
								this.chosenThings.Add(thing);
								this.accumulatedQuantity += thing.stackCount;
								if (this.accumulatedQuantity >= this.quantity)
								{
									return true;
								}
							}
						}
					}
				}
				return false;
			}
		}
	}
}
