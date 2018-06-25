using System;

namespace Verse
{
	// Token: 0x02000D39 RID: 3385
	public class HediffGiver_RandomAgeCurved : HediffGiver
	{
		// Token: 0x0400325A RID: 12890
		public SimpleCurve ageFractionMtbDaysCurve = null;

		// Token: 0x06004A94 RID: 19092 RVA: 0x0026E464 File Offset: 0x0026C864
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
	}
}
