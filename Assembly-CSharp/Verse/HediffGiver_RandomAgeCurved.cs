using System;

namespace Verse
{
	// Token: 0x02000D3A RID: 3386
	public class HediffGiver_RandomAgeCurved : HediffGiver
	{
		// Token: 0x04003261 RID: 12897
		public SimpleCurve ageFractionMtbDaysCurve = null;

		// Token: 0x06004A94 RID: 19092 RVA: 0x0026E744 File Offset: 0x0026CB44
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
