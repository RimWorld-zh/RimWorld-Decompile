using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D30 RID: 3376
	public abstract class HediffGiver
	{
		// Token: 0x0400324C RID: 12876
		[TranslationHandle]
		public HediffDef hediff;

		// Token: 0x0400324D RID: 12877
		public List<BodyPartDef> partsToAffect = null;

		// Token: 0x0400324E RID: 12878
		public bool canAffectAnyLivePart = false;

		// Token: 0x0400324F RID: 12879
		public int countToAffect = 1;

		// Token: 0x06004A76 RID: 19062 RVA: 0x0026D995 File Offset: 0x0026BD95
		public virtual void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
		}

		// Token: 0x06004A77 RID: 19063 RVA: 0x0026D998 File Offset: 0x0026BD98
		public virtual bool OnHediffAdded(Pawn pawn, Hediff hediff)
		{
			return false;
		}

		// Token: 0x06004A78 RID: 19064 RVA: 0x0026D9B0 File Offset: 0x0026BDB0
		public bool TryApply(Pawn pawn, List<Hediff> outAddedHediffs = null)
		{
			return HediffGiveUtility.TryApply(pawn, this.hediff, this.partsToAffect, this.canAffectAnyLivePart, this.countToAffect, outAddedHediffs);
		}

		// Token: 0x06004A79 RID: 19065 RVA: 0x0026D9E4 File Offset: 0x0026BDE4
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
	}
}
