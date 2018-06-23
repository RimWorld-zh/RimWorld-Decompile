using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200026D RID: 621
	public abstract class IngestionOutcomeDoer
	{
		// Token: 0x0400051B RID: 1307
		public float chance = 1f;

		// Token: 0x0400051C RID: 1308
		public bool doToGeneratedPawnIfAddicted;

		// Token: 0x06000AB1 RID: 2737 RVA: 0x00060D8B File Offset: 0x0005F18B
		public void DoIngestionOutcome(Pawn pawn, Thing ingested)
		{
			if (Rand.Value < this.chance)
			{
				this.DoIngestionOutcomeSpecial(pawn, ingested);
			}
		}

		// Token: 0x06000AB2 RID: 2738
		protected abstract void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested);

		// Token: 0x06000AB3 RID: 2739 RVA: 0x00060DA8 File Offset: 0x0005F1A8
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			yield break;
		}
	}
}
