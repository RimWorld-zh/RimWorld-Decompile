using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000291 RID: 657
	public sealed class DifficultyDef : Def
	{
		// Token: 0x0400059A RID: 1434
		public int difficulty = -1;

		// Token: 0x0400059B RID: 1435
		public float threatScale;

		// Token: 0x0400059C RID: 1436
		public bool allowBigThreats = true;

		// Token: 0x0400059D RID: 1437
		public bool allowIntroThreats = true;

		// Token: 0x0400059E RID: 1438
		public bool allowCaveHives = true;

		// Token: 0x0400059F RID: 1439
		public bool peacefulTemples = false;

		// Token: 0x040005A0 RID: 1440
		public float colonistMoodOffset;

		// Token: 0x040005A1 RID: 1441
		public float tradePriceFactorLoss;

		// Token: 0x040005A2 RID: 1442
		public float cropYieldFactor = 1f;

		// Token: 0x040005A3 RID: 1443
		public float diseaseIntervalFactor = 1f;

		// Token: 0x040005A4 RID: 1444
		public float enemyReproductionRateFactor = 1f;

		// Token: 0x040005A5 RID: 1445
		public float playerPawnInfectionChanceFactor = 1f;

		// Token: 0x040005A6 RID: 1446
		public float manhunterChanceOnDamageFactor = 1f;

		// Token: 0x040005A7 RID: 1447
		public float deepDrillInfestationChanceFactor = 1f;
	}
}
