using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class IngestionOutcomeDoer_GiveHediff : IngestionOutcomeDoer
	{
		public HediffDef hediffDef;

		public float severity = -1f;

		public ChemicalDef toleranceChemical = null;

		private bool divideByBodySize = false;

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
				using (IEnumerator<StatDrawEntry> enumerator = this.hediffDef.SpecialDisplayStats().GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						StatDrawEntry s = enumerator.Current;
						yield return s;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			yield break;
			IL_00e9:
			/*Error near IL_00ea: Unexpected return in MoveNext()*/;
		}
	}
}
