using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PawnCapacityWorker_Moving : PawnCapacityWorker
	{
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			float num = 0f;
			float num2 = PawnCapacityUtility.CalculateLimbEfficiency(diffSet, "MovingLimbCore", "MovingLimbSegment", "MovingLimbDigit", 0.4f, out num, impactors);
			float result;
			if (num < 0.49990001320838928)
			{
				result = 0f;
			}
			else
			{
				float num3 = num2;
				string tag = "Pelvis";
				num2 = num3 * PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, 3.40282347E+38f, impactors);
				float num4 = num2;
				tag = "Spine";
				num2 = num4 * PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, 3.40282347E+38f, impactors);
				num2 = Mathf.Lerp(num2, num2 * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Breathing, impactors), 0.2f);
				num2 = Mathf.Lerp(num2, num2 * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.BloodPumping, impactors), 0.2f);
				num2 = (result = num2 * Mathf.Min(base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Consciousness, impactors), 1f));
			}
			return result;
		}

		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag("MovingLimbCore");
		}
	}
}
