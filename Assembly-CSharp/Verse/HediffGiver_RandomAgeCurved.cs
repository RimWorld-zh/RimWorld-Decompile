using System;

namespace Verse
{
	// Token: 0x02000D3A RID: 3386
	public class HediffGiver_RandomAgeCurved : HediffGiver
	{
		// Token: 0x06004A7C RID: 19068 RVA: 0x0026CDA8 File Offset: 0x0026B1A8
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			float x = (float)pawn.ageTracker.AgeBiologicalYears / pawn.RaceProps.lifeExpectancy;
			if (Rand.MTBEventOccurs(this.ageFractionMtbDaysCurve.Evaluate(x), 60000f, 60f))
			{
				if (base.TryApply(pawn, null))
				{
					base.SendLetter(pawn, cause);
				}
			}
		}

		// Token: 0x0400324F RID: 12879
		public SimpleCurve ageFractionMtbDaysCurve = null;
	}
}
