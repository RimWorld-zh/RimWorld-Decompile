using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class PawnCapacityWorker_BloodPumping : PawnCapacityWorker
	{
		public PawnCapacityWorker_BloodPumping()
		{
		}

		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef bloodPumpingSource = BodyPartTagDefOf.BloodPumpingSource;
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, bloodPumpingSource, float.MaxValue, default(FloatRange), impactors);
		}

		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.BloodPumpingSource);
		}
	}
}
