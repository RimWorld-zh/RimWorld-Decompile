using System;

namespace Verse
{
	// Token: 0x02000D3B RID: 3387
	public class HediffGiver_RandomAgeCurved : HediffGiver
	{
		// Token: 0x06004A7E RID: 19070 RVA: 0x0026CDD0 File Offset: 0x0026B1D0
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

		// Token: 0x04003251 RID: 12881
		public SimpleCurve ageFractionMtbDaysCurve = null;
	}
}
