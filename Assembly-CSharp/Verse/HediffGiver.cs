using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D30 RID: 3376
	public abstract class HediffGiver
	{
		// Token: 0x06004A5E RID: 19038 RVA: 0x0026BFFD File Offset: 0x0026A3FD
		public virtual void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
		}

		// Token: 0x06004A5F RID: 19039 RVA: 0x0026C000 File Offset: 0x0026A400
		public virtual bool OnHediffAdded(Pawn pawn, Hediff hediff)
		{
			return false;
		}

		// Token: 0x06004A60 RID: 19040 RVA: 0x0026C018 File Offset: 0x0026A418
		public bool TryApply(Pawn pawn, List<Hediff> outAddedHediffs = null)
		{
			return HediffGiveUtility.TryApply(pawn, this.hediff, this.partsToAffect, this.canAffectAnyLivePart, this.countToAffect, outAddedHediffs);
		}

		// Token: 0x06004A61 RID: 19041 RVA: 0x0026C04C File Offset: 0x0026A44C
		protected void SendLetter(Pawn pawn, Hediff cause)
		{
			if (PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				if (cause == null)
				{
					Find.LetterStack.ReceiveLetter("LetterHediffFromRandomHediffGiverLabel".Translate(new object[]
					{
						pawn.LabelShort,
						this.hediff.LabelCap
					}).CapitalizeFirst(), "LetterHediffFromRandomHediffGiver".Translate(new object[]
					{
						pawn.LabelShort,
						this.hediff.LabelCap
					}).CapitalizeFirst(), LetterDefOf.NegativeEvent, pawn, null, null);
				}
				else
				{
					Find.LetterStack.ReceiveLetter("LetterHealthComplicationsLabel".Translate(new object[]
					{
						pawn.LabelShort,
						this.hediff.LabelCap
					}).CapitalizeFirst(), "LetterHealthComplications".Translate(new object[]
					{
						pawn.LabelShort,
						this.hediff.LabelCap,
						cause.LabelCap
					}).CapitalizeFirst(), LetterDefOf.NegativeEvent, pawn, null, null);
				}
			}
		}

		// Token: 0x0400323A RID: 12858
		public HediffDef hediff;

		// Token: 0x0400323B RID: 12859
		public List<BodyPartDef> partsToAffect = null;

		// Token: 0x0400323C RID: 12860
		public bool canAffectAnyLivePart = false;

		// Token: 0x0400323D RID: 12861
		public int countToAffect = 1;
	}
}
