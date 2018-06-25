using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000270 RID: 624
	public class IngestionOutcomeDoer_GiveHediff : IngestionOutcomeDoer
	{
		// Token: 0x0400051D RID: 1309
		public HediffDef hediffDef;

		// Token: 0x0400051E RID: 1310
		public float severity = -1f;

		// Token: 0x0400051F RID: 1311
		public ChemicalDef toleranceChemical = null;

		// Token: 0x04000520 RID: 1312
		private bool divideByBodySize = false;

		// Token: 0x06000AB9 RID: 2745 RVA: 0x00060FD8 File Offset: 0x0005F3D8
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

		// Token: 0x06000ABA RID: 2746 RVA: 0x0006105C File Offset: 0x0005F45C
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
	}
}
