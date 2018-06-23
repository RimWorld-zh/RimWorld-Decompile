using System;

namespace Verse
{
	// Token: 0x02000D36 RID: 3382
	public class HediffGiver_RandomDrugEffect : HediffGiver
	{
		// Token: 0x04003258 RID: 12888
		public float baseMtbDays = 0f;

		// Token: 0x04003259 RID: 12889
		public float minSeverity = 0f;

		// Token: 0x06004A8E RID: 19086 RVA: 0x0026E2D4 File Offset: 0x0026C6D4
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
