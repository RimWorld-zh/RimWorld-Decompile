using System;

namespace Verse
{
	// Token: 0x02000D3A RID: 3386
	public class HediffGiver_RandomDrugEffect : HediffGiver
	{
		// Token: 0x06004A7C RID: 19068 RVA: 0x0026CD6C File Offset: 0x0026B16C
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

		// Token: 0x0400324F RID: 12879
		public float baseMtbDays = 0f;

		// Token: 0x04003250 RID: 12880
		public float minSeverity = 0f;
	}
}
