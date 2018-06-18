using System;

namespace Verse
{
	// Token: 0x02000D39 RID: 3385
	public class HediffGiver_RandomDrugEffect : HediffGiver
	{
		// Token: 0x06004A7A RID: 19066 RVA: 0x0026CD44 File Offset: 0x0026B144
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

		// Token: 0x0400324D RID: 12877
		public float baseMtbDays = 0f;

		// Token: 0x0400324E RID: 12878
		public float minSeverity = 0f;
	}
}
