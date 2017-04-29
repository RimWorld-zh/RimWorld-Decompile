using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public abstract class IngestionOutcomeDoer
	{
		public float chance = 1f;

		public bool doToGeneratedPawnIfAddicted;

		public void DoIngestionOutcome(Pawn pawn, Thing ingested)
		{
			if (Rand.Value < this.chance)
			{
				this.DoIngestionOutcomeSpecial(pawn, ingested);
			}
		}

		protected abstract void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested);

		[DebuggerHidden]
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			IngestionOutcomeDoer.<SpecialDisplayStats>c__Iterator80 <SpecialDisplayStats>c__Iterator = new IngestionOutcomeDoer.<SpecialDisplayStats>c__Iterator80();
			IngestionOutcomeDoer.<SpecialDisplayStats>c__Iterator80 expr_07 = <SpecialDisplayStats>c__Iterator;
			expr_07.$PC = -2;
			return expr_07;
		}
	}
}
