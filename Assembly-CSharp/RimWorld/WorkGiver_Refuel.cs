using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiver_Refuel : WorkGiver_Scanner
	{
		public override ThingRequest PotentialWorkThingRequest
		{
			get
			{
				return ThingRequest.ForGroup(ThingRequestGroup.Refuelable);
			}
		}

		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.Touch;
			}
		}

		public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			CompRefuelable compRefuelable = t.TryGetComp<CompRefuelable>();
			bool result;
			if (compRefuelable != null && !compRefuelable.IsFull)
			{
				if (!forced && !compRefuelable.ShouldAutoRefuelNow)
				{
					result = false;
					goto IL_010c;
				}
				if (!t.IsForbidden(pawn))
				{
					LocalTargetInfo target = t;
					if (!pawn.CanReserve(target, 1, -1, null, forced))
						goto IL_0068;
					if (t.Faction != pawn.Faction)
					{
						result = false;
					}
					else
					{
						ThingWithComps thingWithComps = t as ThingWithComps;
						if (thingWithComps != null)
						{
							CompFlickable comp = thingWithComps.GetComp<CompFlickable>();
							if (comp != null && !comp.SwitchIsOn)
							{
								result = false;
								goto IL_010c;
							}
						}
						Thing thing = this.FindBestFuel(pawn, t);
						if (thing == null)
						{
							ThingFilter fuelFilter = t.TryGetComp<CompRefuelable>().Props.fuelFilter;
							JobFailReason.Is("NoFuelToRefuel".Translate(fuelFilter.Summary));
							result = false;
						}
						else
						{
							result = true;
						}
					}
					goto IL_010c;
				}
				goto IL_0068;
			}
			result = false;
			goto IL_010c;
			IL_0068:
			result = false;
			goto IL_010c;
			IL_010c:
			return result;
		}

		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Thing t2 = this.FindBestFuel(pawn, t);
			return new Job(JobDefOf.Refuel, t, t2);
		}

		private Thing FindBestFuel(Pawn pawn, Thing refuelable)
		{
			ThingFilter filter = refuelable.TryGetComp<CompRefuelable>().Props.fuelFilter;
			Predicate<Thing> predicate = (Predicate<Thing>)((Thing x) => (byte)((!x.IsForbidden(pawn) && pawn.CanReserve(x, 1, -1, null, false)) ? (filter.Allows(x) ? 1 : 0) : 0) != 0);
			IntVec3 position = pawn.Position;
			Map map = pawn.Map;
			ThingRequest bestThingRequest = filter.BestThingRequest;
			PathEndMode peMode = PathEndMode.ClosestTouch;
			TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
			Predicate<Thing> validator = predicate;
			return GenClosest.ClosestThingReachable(position, map, bestThingRequest, peMode, traverseParams, 9999f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
		}
	}
}
