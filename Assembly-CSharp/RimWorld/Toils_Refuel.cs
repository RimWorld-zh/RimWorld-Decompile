using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class Toils_Refuel
	{
		public Toils_Refuel()
		{
		}

		public static Toil FinalizeRefueling(TargetIndex refuelableInd, TargetIndex fuelInd)
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Job curJob = actor.CurJob;
				Thing thing = curJob.GetTarget(refuelableInd).Thing;
				if (toil.actor.CurJob.placedThings.NullOrEmpty<ThingCountClass>())
				{
					thing.TryGetComp<CompRefuelable>().Refuel(new List<Thing>
					{
						curJob.GetTarget(fuelInd).Thing
					});
				}
				else
				{
					thing.TryGetComp<CompRefuelable>().Refuel((from p in toil.actor.CurJob.placedThings
					select p.thing).ToList<Thing>());
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		[CompilerGenerated]
		private sealed class <FinalizeRefueling>c__AnonStorey0
		{
			internal Toil toil;

			internal TargetIndex refuelableInd;

			internal TargetIndex fuelInd;

			private static Func<ThingCountClass, Thing> <>f__am$cache0;

			public <FinalizeRefueling>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				Pawn actor = this.toil.actor;
				Job curJob = actor.CurJob;
				Thing thing = curJob.GetTarget(this.refuelableInd).Thing;
				if (this.toil.actor.CurJob.placedThings.NullOrEmpty<ThingCountClass>())
				{
					thing.TryGetComp<CompRefuelable>().Refuel(new List<Thing>
					{
						curJob.GetTarget(this.fuelInd).Thing
					});
				}
				else
				{
					thing.TryGetComp<CompRefuelable>().Refuel((from p in this.toil.actor.CurJob.placedThings
					select p.thing).ToList<Thing>());
				}
			}

			private static Thing <>m__1(ThingCountClass p)
			{
				return p.thing;
			}
		}
	}
}
