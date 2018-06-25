using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200026F RID: 623
	public abstract class IngestionOutcomeDoer
	{
		// Token: 0x0400051D RID: 1309
		public float chance = 1f;

		// Token: 0x0400051E RID: 1310
		public bool doToGeneratedPawnIfAddicted;

		// Token: 0x06000AB4 RID: 2740 RVA: 0x00060ED7 File Offset: 0x0005F2D7
		public void DoIngestionOutcome(Pawn pawn, Thing ingested)
		{
			if (Rand.Value < this.chance)
			{
				this.DoIngestionOutcomeSpecial(pawn, ingested);
			}
		}

		// Token: 0x06000AB5 RID: 2741
		protected abstract void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested);

		// Token: 0x06000AB6 RID: 2742 RVA: 0x00060EF4 File Offset: 0x0005F2F4
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			yield break;
		}
	}
}
