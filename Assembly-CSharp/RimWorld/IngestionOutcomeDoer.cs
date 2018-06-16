using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200026D RID: 621
	public abstract class IngestionOutcomeDoer
	{
		// Token: 0x06000AB3 RID: 2739 RVA: 0x00060D2F File Offset: 0x0005F12F
		public void DoIngestionOutcome(Pawn pawn, Thing ingested)
		{
			if (Rand.Value < this.chance)
			{
				this.DoIngestionOutcomeSpecial(pawn, ingested);
			}
		}

		// Token: 0x06000AB4 RID: 2740
		protected abstract void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested);

		// Token: 0x06000AB5 RID: 2741 RVA: 0x00060D4C File Offset: 0x0005F14C
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			yield break;
		}

		// Token: 0x0400051D RID: 1309
		public float chance = 1f;

		// Token: 0x0400051E RID: 1310
		public bool doToGeneratedPawnIfAddicted;
	}
}
