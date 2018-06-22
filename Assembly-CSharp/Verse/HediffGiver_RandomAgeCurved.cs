using System;

namespace Verse
{
	// Token: 0x02000D37 RID: 3383
	public class HediffGiver_RandomAgeCurved : HediffGiver
	{
		// Token: 0x06004A90 RID: 19088 RVA: 0x0026E338 File Offset: 0x0026C738
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

		// Token: 0x0400325A RID: 12890
		public SimpleCurve ageFractionMtbDaysCurve = null;
	}
}
