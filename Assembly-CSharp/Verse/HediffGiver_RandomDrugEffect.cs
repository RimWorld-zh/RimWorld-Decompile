using System;

namespace Verse
{
	// Token: 0x02000D38 RID: 3384
	public class HediffGiver_RandomDrugEffect : HediffGiver
	{
		// Token: 0x04003258 RID: 12888
		public float baseMtbDays = 0f;

		// Token: 0x04003259 RID: 12889
		public float minSeverity = 0f;

		// Token: 0x06004A92 RID: 19090 RVA: 0x0026E400 File Offset: 0x0026C800
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
