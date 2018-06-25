using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000271 RID: 625
	public class IngestionOutcomeDoer_OffsetNeed : IngestionOutcomeDoer
	{
		// Token: 0x04000523 RID: 1315
		public NeedDef need;

		// Token: 0x04000524 RID: 1316
		public float offset = 0f;

		// Token: 0x04000525 RID: 1317
		public ChemicalDef toleranceChemical = null;

		// Token: 0x06000ABB RID: 2747 RVA: 0x000612B8 File Offset: 0x0005F6B8
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

		// Token: 0x06000ABC RID: 2748 RVA: 0x00061318 File Offset: 0x0005F718
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, this.need.LabelCap, this.offset.ToStringPercent(), 0, "");
			yield break;
		}
	}
}
