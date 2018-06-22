using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200026F RID: 623
	public class IngestionOutcomeDoer_OffsetNeed : IngestionOutcomeDoer
	{
		// Token: 0x06000AB8 RID: 2744 RVA: 0x0006116C File Offset: 0x0005F56C
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

		// Token: 0x06000AB9 RID: 2745 RVA: 0x000611CC File Offset: 0x0005F5CC
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, this.need.LabelCap, this.offset.ToStringPercent(), 0, "");
			yield break;
		}

		// Token: 0x04000521 RID: 1313
		public NeedDef need;

		// Token: 0x04000522 RID: 1314
		public float offset = 0f;

		// Token: 0x04000523 RID: 1315
		public ChemicalDef toleranceChemical = null;
	}
}
