namespace Verse
{
	public class HediffGiver_Bleeding : HediffGiver
	{
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			HediffSet hediffSet = pawn.health.hediffSet;
			if (hediffSet.BleedRateTotal >= 0.10000000149011612)
			{
				HealthUtility.AdjustSeverity(pawn, base.hediff, (float)(hediffSet.BleedRateTotal * 0.0010000000474974513));
			}
			else
			{
				HealthUtility.AdjustSeverity(pawn, base.hediff, -0.00033333333f);
			}
		}
	}
}
