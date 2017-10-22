using System.Collections.Generic;
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
			float num = (!(this.severity > 0.0)) ? this.hediffDef.initialSeverity : this.severity;
			if (this.divideByBodySize)
			{
				num /= pawn.BodySize;
			}
			AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize(pawn, this.toleranceChemical, ref num);
			hediff.Severity = num;
			pawn.health.AddHediff(hediff, null, default(DamageInfo?));
		}

		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			if (parentDef.IsDrug && base.chance >= 1.0)
			{
				foreach (StatDrawEntry item in this.hediffDef.SpecialDisplayStats())
				{
					yield return item;
				}
			}
		}
	}
}
