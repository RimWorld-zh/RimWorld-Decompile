using System;

namespace Verse
{
	// Token: 0x02000D37 RID: 3383
	public class HediffGiver_Random : HediffGiver
	{
		// Token: 0x04003257 RID: 12887
		public float mtbDays = 0f;

		// Token: 0x06004A90 RID: 19088 RVA: 0x0026E3AE File Offset: 0x0026C7AE
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if (Rand.MTBEventOccurs(this.mtbDays, 60000f, 60f))
			{
				if (base.TryApply(pawn, null))
				{
					base.SendLetter(pawn, cause);
				}
			}
		}
	}
}
