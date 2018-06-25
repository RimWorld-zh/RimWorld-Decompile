using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_EatRandom : ThinkNode_JobGiver
	{
		public JobGiver_EatRandom()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (pawn.Downed)
			{
				result = null;
			}
			else
			{
				Predicate<Thing> validator = (Thing t) => t.def.category == ThingCategory.Item && t.IngestibleNow && pawn.RaceProps.CanEverEat(t) && pawn.CanReserve(t, 1, -1, null, false);
				Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableAlways), PathEndMode.OnCell, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 10f, validator, null, 0, -1, false, RegionType.Set_Passable, false);
				if (thing == null)
				{
					result = null;
				}
				else
				{
					result = new Job(JobDefOf.Ingest, thing);
				}
			}
			return result;
		}

		[CompilerGenerated]
		private sealed class <TryGiveJob>c__AnonStorey0
		{
			internal Pawn pawn;

			public <TryGiveJob>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing t)
			{
				return t.def.category == ThingCategory.Item && t.IngestibleNow && this.pawn.RaceProps.CanEverEat(t) && this.pawn.CanReserve(t, 1, -1, null, false);
			}
		}
	}
}
