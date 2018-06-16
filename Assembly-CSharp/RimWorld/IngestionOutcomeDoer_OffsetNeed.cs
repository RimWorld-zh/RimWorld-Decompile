using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200026F RID: 623
	public class IngestionOutcomeDoer_OffsetNeed : IngestionOutcomeDoer
	{
		// Token: 0x06000ABA RID: 2746 RVA: 0x00061110 File Offset: 0x0005F510
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

		// Token: 0x06000ABB RID: 2747 RVA: 0x00061170 File Offset: 0x0005F570
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, this.need.LabelCap, this.offset.ToStringPercent(), 0, "");
			yield break;
		}

		// Token: 0x04000523 RID: 1315
		public NeedDef need;

		// Token: 0x04000524 RID: 1316
		public float offset = 0f;

		// Token: 0x04000525 RID: 1317
		public ChemicalDef toleranceChemical = null;
	}
}
