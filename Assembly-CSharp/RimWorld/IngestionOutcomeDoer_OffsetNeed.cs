using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class IngestionOutcomeDoer_OffsetNeed : IngestionOutcomeDoer
	{
		public NeedDef need;

		public float offset;

		public ChemicalDef toleranceChemical;

		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
		{
			if (pawn.needs != null)
			{
				Need need = pawn.needs.TryGetNeed(this.need);
				if (need != null)
				{
					float num = this.offset;
					AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize(pawn, this.toleranceChemical, ref num);
					need.CurLevel += num;
				}
			}
		}

		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, this.need.LabelCap, this.offset.ToStringPercent(), 0);
		}
	}
}
