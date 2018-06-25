using System;

namespace Verse
{
	// Token: 0x02000D39 RID: 3385
	public class HediffGiver_RandomDrugEffect : HediffGiver
	{
		// Token: 0x0400325F RID: 12895
		public float baseMtbDays = 0f;

		// Token: 0x04003260 RID: 12896
		public float minSeverity = 0f;

		// Token: 0x06004A92 RID: 19090 RVA: 0x0026E6E0 File Offset: 0x0026CAE0
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if (cause.Severity >= this.minSeverity)
			{
				if (Rand.MTBEventOccurs(this.baseMtbDays, 60000f, 60f))
				{
					if (base.TryApply(pawn, null))
					{
						base.SendLetter(pawn, cause);
					}
				}
			}
		}
	}
}
