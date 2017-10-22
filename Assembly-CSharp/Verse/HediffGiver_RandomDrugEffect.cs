namespace Verse
{
	public class HediffGiver_RandomDrugEffect : HediffGiver
	{
		public float baseMtbDays = 0f;

		public float minSeverity = 0f;

		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if (!(cause.Severity < this.minSeverity) && Rand.MTBEventOccurs(this.baseMtbDays, 60000f, 60f) && base.TryApply(pawn, null))
			{
				base.SendLetter(pawn, cause);
			}
		}
	}
}
