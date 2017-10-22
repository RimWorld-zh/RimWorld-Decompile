using Verse;

namespace RimWorld
{
	public class JobGiver_BingeFood : JobGiver_Binge
	{
		private const int BaseIngestInterval = 1100;

		protected override int IngestInterval(Pawn pawn)
		{
			return 1100;
		}

		protected override Thing BestIngestTarget(Pawn pawn)
		{
			Thing thing = default(Thing);
			ThingDef thingDef = default(ThingDef);
			return (!FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, true, out thing, out thingDef, false, true, true, true, true, false)) ? null : thing;
		}
	}
}
