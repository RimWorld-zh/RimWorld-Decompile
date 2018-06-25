using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200026F RID: 623
	public abstract class IngestionOutcomeDoer
	{
		// Token: 0x0400051B RID: 1307
		public float chance = 1f;

		// Token: 0x0400051C RID: 1308
		public bool doToGeneratedPawnIfAddicted;

		// Token: 0x06000AB5 RID: 2741 RVA: 0x00060EDB File Offset: 0x0005F2DB
		public void DoIngestionOutcome(Pawn pawn, Thing ingested)
		{
			if (Rand.Value < this.chance)
			{
				this.DoIngestionOutcomeSpecial(pawn, ingested);
			}
		}

		// Token: 0x06000AB6 RID: 2742
		protected abstract void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested);

		// Token: 0x06000AB7 RID: 2743 RVA: 0x00060EF8 File Offset: 0x0005F2F8
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			yield break;
		}
	}
}
