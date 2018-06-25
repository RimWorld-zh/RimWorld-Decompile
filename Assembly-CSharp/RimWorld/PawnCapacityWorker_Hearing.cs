using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class PawnCapacityWorker_Hearing : PawnCapacityWorker
	{
		public PawnCapacityWorker_Hearing()
		{
		}

		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef hearingSource = BodyPartTagDefOf.HearingSource;
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, hearingSource, float.MaxValue, default(FloatRange), impactors);
		}

		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.HearingSource);
		}
	}
}
