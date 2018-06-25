using System;

namespace Verse
{
	// Token: 0x02000D33 RID: 3379
	public class HediffGiver_Bleeding : HediffGiver
	{
		// Token: 0x06004A84 RID: 19076 RVA: 0x0026DFDC File Offset: 0x0026C3DC
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			HediffSet hediffSet = pawn.health.hediffSet;
			bool flag = hediffSet.BleedRateTotal >= 0.1f;
			if (flag)
			{
				HealthUtility.AdjustSeverity(pawn, this.hediff, hediffSet.BleedRateTotal * 0.001f);
			}
			else
			{
				HealthUtility.AdjustSeverity(pawn, this.hediff, -0.00033333333f);
			}
		}
	}
}
