using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class PawnCapacityWorker_Sight : PawnCapacityWorker
	{
		public PawnCapacityWorker_Sight()
		{
		}

		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef sightSource = BodyPartTagDefOf.SightSource;
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, sightSource, float.MaxValue, default(FloatRange), impactors);
		}

		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.SightSource);
		}
	}
}
