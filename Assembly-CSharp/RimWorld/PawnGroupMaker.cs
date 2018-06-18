using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000492 RID: 1170
	public class PawnGroupMaker
	{
		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x060014B9 RID: 5305 RVA: 0x000B6084 File Offset: 0x000B4484
		public float MinPointsToGenerateAnything
		{
			get
			{
				return this.kindDef.Worker.MinPointsToGenerateAnything(this);
			}
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x000B60AC File Offset: 0x000B44AC
		public IEnumerable<Pawn> GeneratePawns(PawnGroupMakerParms parms, bool errorOnZeroResults = true)
		{
			return this.kindDef.Worker.GeneratePawns(parms, this, errorOnZeroResults);
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x000B60D4 File Offset: 0x000B44D4
		public bool CanGenerateFrom(PawnGroupMakerParms parms)
		{
			return parms.points <= this.maxTotalPoints && (this.disallowedStrategies == null || !this.disallowedStrategies.Contains(parms.raidStrategy)) && this.kindDef.Worker.CanGenerateFrom(parms, this);
		}

		// Token: 0x04000C6D RID: 3181
		public PawnGroupKindDef kindDef;

		// Token: 0x04000C6E RID: 3182
		public float commonality = 100f;

		// Token: 0x04000C6F RID: 3183
		public List<RaidStrategyDef> disallowedStrategies;

		// Token: 0x04000C70 RID: 3184
		public float maxTotalPoints = 9999999f;

		// Token: 0x04000C71 RID: 3185
		public List<PawnGenOption> options = new List<PawnGenOption>();

		// Token: 0x04000C72 RID: 3186
		public List<PawnGenOption> traders = new List<PawnGenOption>();

		// Token: 0x04000C73 RID: 3187
		public List<PawnGenOption> carriers = new List<PawnGenOption>();

		// Token: 0x04000C74 RID: 3188
		public List<PawnGenOption> guards = new List<PawnGenOption>();
	}
}
