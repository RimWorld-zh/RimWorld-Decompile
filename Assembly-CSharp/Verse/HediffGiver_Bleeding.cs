using System;

namespace Verse
{
	// Token: 0x02000D34 RID: 3380
	public class HediffGiver_Bleeding : HediffGiver
	{
		// Token: 0x06004A6E RID: 19054 RVA: 0x0026C66C File Offset: 0x0026AA6C
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
