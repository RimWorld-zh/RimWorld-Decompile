using System;
using Verse;

namespace RimWorld
{
	public class JobGiver_BingeFood : JobGiver_Binge
	{
		private const int BaseIngestInterval = 1100;

		public JobGiver_BingeFood()
		{
		}

		protected override int IngestInterval(Pawn pawn)
		{
			return 1100;
		}

		protected override Thing BestIngestTarget(Pawn pawn)
		{
			Thing thing;
			ThingDef thingDef;
			Thing result;
			if (FoodUtility.TryFindBestFoodSourceFor(pawn, pawn, true, out thing, out thingDef, false, true, true, true, true, false, false))
			{
				result = thing;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
