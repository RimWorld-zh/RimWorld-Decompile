using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class PawnCapacityWorker_Metabolism : PawnCapacityWorker
	{
		public PawnCapacityWorker_Metabolism()
		{
		}

		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef metabolismSource = BodyPartTagDefOf.MetabolismSource;
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, metabolismSource, float.MaxValue, default(FloatRange), impactors);
		}

		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.MetabolismSource);
		}
	}
}
