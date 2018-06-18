using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200047A RID: 1146
	public class PawnCapacityWorker_Moving : PawnCapacityWorker
	{
		// Token: 0x0600141A RID: 5146 RVA: 0x000AEC1C File Offset: 0x000AD01C
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			float num = 0f;
			float num2 = PawnCapacityUtility.CalculateLimbEfficiency(diffSet, BodyPartTagDefOf.MovingLimbCore, BodyPartTagDefOf.MovingLimbSegment, BodyPartTagDefOf.MovingLimbDigit, 0.4f, out num, impactors);
			float result;
			if (num < 0.4999f)
			{
				result = 0f;
			}
			else
			{
				float num3 = num2;
				BodyPartTagDef tag = BodyPartTagDefOf.Pelvis;
				num2 = num3 * PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, float.MaxValue, default(FloatRange), impactors);
				float num4 = num2;
				tag = BodyPartTagDefOf.Spine;
				num2 = num4 * PawnCapacityUtility.CalculateTagEfficiency(diffSet, tag, float.MaxValue, default(FloatRange), impactors);
				num2 = Mathf.Lerp(num2, num2 * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Breathing, impactors), 0.2f);
				num2 = Mathf.Lerp(num2, num2 * base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.BloodPumping, impactors), 0.2f);
				num2 *= Mathf.Min(base.CalculateCapacityAndRecord(diffSet, PawnCapacityDefOf.Consciousness, impactors), 1f);
				result = num2;
			}
			return result;
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x000AED10 File Offset: 0x000AD110
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.MovingLimbCore);
		}
	}
}
