using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200026E RID: 622
	public class IngestionOutcomeDoer_GiveHediff : IngestionOutcomeDoer
	{
		// Token: 0x06000AB7 RID: 2743 RVA: 0x00060E2C File Offset: 0x0005F22C
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
			pawn.health.AddHediff(hediff, null, null, null);
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x00060EB0 File Offset: 0x0005F2B0
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			if (parentDef.IsDrug && this.chance >= 1f)
			{
				foreach (StatDrawEntry s in this.hediffDef.SpecialDisplayStats())
				{
					yield return s;
				}
			}
			yield break;
		}

		// Token: 0x0400051F RID: 1311
		public HediffDef hediffDef;

		// Token: 0x04000520 RID: 1312
		public float severity = -1f;

		// Token: 0x04000521 RID: 1313
		public ChemicalDef toleranceChemical = null;

		// Token: 0x04000522 RID: 1314
		private bool divideByBodySize = false;
	}
}
