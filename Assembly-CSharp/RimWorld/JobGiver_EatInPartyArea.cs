using System;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_EatInPartyArea : ThinkNode_JobGiver
	{
		public JobGiver_EatInPartyArea()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			PawnDuty duty = pawn.mindState.duty;
			if (duty == null)
			{
				return null;
			}
			float curLevelPercentage = pawn.needs.food.CurLevelPercentage;
			if ((double)curLevelPercentage > 0.9)
			{
				return null;
			}
			IntVec3 cell = duty.focus.Cell;
			Thing thing = this.FindFood(pawn, cell);
			if (thing == null)
			{
				return null;
			}
			return new Job(JobDefOf.Ingest, thing)
			{
				count = FoodUtility.WillIngestStackCountOf(pawn, thing.def, thing.def.GetStatValueAbstract(StatDefOf.Nutrition, null))
			};
		}

		private Thing FindFood(Pawn pawn, IntVec3 partySpot)
		{
			Predicate<Thing> validator = (Thing x) => x.IngestibleNow && x.def.IsNutritionGivingIngestible && PartyUtility.InPartyArea(x.Position, partySpot, pawn.Map) && !x.def.IsDrug && x.def.ingestible.preferability > FoodPreferability.RawBad && pawn.RaceProps.WillAutomaticallyEat(x) && !x.IsForbidden(pawn) && x.IsSociallyProper(pawn) && pawn.CanReserve(x, 1, -1, null, false);
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.FoodSourceNotPlantOrTree), PathEndMode.ClosestTouch, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), 14f, validator, null, 0, 12, false, RegionType.Set_Passable, false);
		}

		[CompilerGenerated]
		private sealed class <FindFood>c__AnonStorey0
		{
			internal IntVec3 partySpot;

			internal Pawn pawn;

			public <FindFood>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing x)
			{
				return x.IngestibleNow && x.def.IsNutritionGivingIngestible && PartyUtility.InPartyArea(x.Position, this.partySpot, this.pawn.Map) && !x.def.IsDrug && x.def.ingestible.preferability > FoodPreferability.RawBad && this.pawn.RaceProps.WillAutomaticallyEat(x) && !x.IsForbidden(this.pawn) && x.IsSociallyProper(this.pawn) && this.pawn.CanReserve(x, 1, -1, null, false);
			}
		}
	}
}
