using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000490 RID: 1168
	public class PawnGroupMaker
	{
		// Token: 0x04000C6A RID: 3178
		public PawnGroupKindDef kindDef;

		// Token: 0x04000C6B RID: 3179
		public float commonality = 100f;

		// Token: 0x04000C6C RID: 3180
		public List<RaidStrategyDef> disallowedStrategies;

		// Token: 0x04000C6D RID: 3181
		public float maxTotalPoints = 9999999f;

		// Token: 0x04000C6E RID: 3182
		public List<PawnGenOption> options = new List<PawnGenOption>();

		// Token: 0x04000C6F RID: 3183
		public List<PawnGenOption> traders = new List<PawnGenOption>();

		// Token: 0x04000C70 RID: 3184
		public List<PawnGenOption> carriers = new List<PawnGenOption>();

		// Token: 0x04000C71 RID: 3185
		public List<PawnGenOption> guards = new List<PawnGenOption>();

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x060014B4 RID: 5300 RVA: 0x000B61D0 File Offset: 0x000B45D0
		public float MinPointsToGenerateAnything
		{
			get
			{
				return this.kindDef.Worker.MinPointsToGenerateAnything(this);
			}
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x000B61F8 File Offset: 0x000B45F8
		public IEnumerable<Pawn> GeneratePawns(PawnGroupMakerParms parms, bool errorOnZeroResults = true)
		{
			return this.kindDef.Worker.GeneratePawns(parms, this, errorOnZeroResults);
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x000B6220 File Offset: 0x000B4620
		public bool CanGenerateFrom(PawnGroupMakerParms parms)
		{
			return parms.points <= this.maxTotalPoints && (this.disallowedStrategies == null || !this.disallowedStrategies.Contains(parms.raidStrategy)) && this.kindDef.Worker.CanGenerateFrom(parms, this);
		}
	}
}
