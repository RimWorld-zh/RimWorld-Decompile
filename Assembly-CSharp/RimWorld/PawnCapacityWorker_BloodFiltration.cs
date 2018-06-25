using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class PawnCapacityWorker_BloodFiltration : PawnCapacityWorker
	{
		public PawnCapacityWorker_BloodFiltration()
		{
		}

		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyDef body = diffSet.pawn.RaceProps.body;
			float result;
			if (body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationKidney))
			{
				BodyPartTagDef tag = BodyPartTagDefOf.BloodFiltrationKidney;
				float num = PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, float.MaxValue, default(FloatRange), impactors);
				tag = BodyPartTagDefOf.BloodFiltrationLiver;
				result = num * PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, float.MaxValue, default(FloatRange), impactors);
			}
			else
			{
				BodyPartTagDef tag = BodyPartTagDefOf.BloodFiltrationSource;
				result = PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, float.MaxValue, default(FloatRange), impactors);
			}
			return result;
		}

		public override bool CanHaveCapacity(BodyDef body)
		{
			return (body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationKidney) && body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationLiver)) || body.HasPartWithTag(BodyPartTagDefOf.BloodFiltrationSource);
		}
	}
}
