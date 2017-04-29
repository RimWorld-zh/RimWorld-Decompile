using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public class IngestionOutcomeDoer_GiveHediff : IngestionOutcomeDoer
	{
		public HediffDef hediffDef;

		public float severity = -1f;

		public ChemicalDef toleranceChemical;

		private bool divideByBodySize;

		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
		{
			Hediff hediff = HediffMaker.MakeHediff(this.hediffDef, pawn, null);
			float num;
			if (this.severity > 0f)
			{
				num = this.severity;
			}
			else
			{
				num = this.hediffDef.initialSeverity;
			}
			if (this.divideByBodySize)
			{
				num /= pawn.BodySize;
			}
			AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize(pawn, this.toleranceChemical, ref num);
			hediff.Severity = num;
			pawn.health.AddHediff(hediff, null, null);
		}

		[DebuggerHidden]
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			IngestionOutcomeDoer_GiveHediff.<SpecialDisplayStats>c__Iterator81 <SpecialDisplayStats>c__Iterator = new IngestionOutcomeDoer_GiveHediff.<SpecialDisplayStats>c__Iterator81();
			<SpecialDisplayStats>c__Iterator.parentDef = parentDef;
			<SpecialDisplayStats>c__Iterator.<$>parentDef = parentDef;
			<SpecialDisplayStats>c__Iterator.<>f__this = this;
			IngestionOutcomeDoer_GiveHediff.<SpecialDisplayStats>c__Iterator81 expr_1C = <SpecialDisplayStats>c__Iterator;
			expr_1C.$PC = -2;
			return expr_1C;
		}
	}
}
