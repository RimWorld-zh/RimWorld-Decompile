using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class PawnCapacityWorker_BloodFiltration : PawnCapacityWorker
	{
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyDef body = diffSet.pawn.RaceProps.body;
			float result;
			if (body.HasPartWithTag("BloodFiltrationKidney"))
			{
				string tag = "BloodFiltrationKidney";
				float num = PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, 3.40282347E+38f, impactors);
				tag = "BloodFiltrationLiver";
				result = num * PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, 3.40282347E+38f, impactors);
			}
			else
			{
				string tag = "BloodFiltrationSource";
				result = PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, 3.40282347E+38f, impactors);
			}
			return result;
		}

		public override bool CanHaveCapacity(BodyDef body)
		{
			return (body.HasPartWithTag("BloodFiltrationKidney") && body.HasPartWithTag("BloodFiltrationLiver")) || body.HasPartWithTag("BloodFiltrationSource");
		}
	}
}
