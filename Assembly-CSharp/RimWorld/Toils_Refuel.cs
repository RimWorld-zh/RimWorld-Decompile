using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200008A RID: 138
	public class Toils_Refuel
	{
		// Token: 0x0600038E RID: 910 RVA: 0x00028024 File Offset: 0x00026424
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
	}
}
