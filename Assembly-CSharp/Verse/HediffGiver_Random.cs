using System;

namespace Verse
{
	// Token: 0x02000D38 RID: 3384
	public class HediffGiver_Random : HediffGiver
	{
		// Token: 0x06004A78 RID: 19064 RVA: 0x0026CCF2 File Offset: 0x0026B0F2
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

		// Token: 0x0400324C RID: 12876
		public float mtbDays = 0f;
	}
}
