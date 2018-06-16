using System;

namespace Verse
{
	// Token: 0x02000D39 RID: 3385
	public class HediffGiver_Random : HediffGiver
	{
		// Token: 0x06004A7A RID: 19066 RVA: 0x0026CD1A File Offset: 0x0026B11A
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

		// Token: 0x0400324E RID: 12878
		public float mtbDays = 0f;
	}
}
