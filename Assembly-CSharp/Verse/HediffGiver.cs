using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D2D RID: 3373
	public abstract class HediffGiver
	{
		// Token: 0x06004A72 RID: 19058 RVA: 0x0026D589 File Offset: 0x0026B989
		public virtual void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
		}

		// Token: 0x06004A73 RID: 19059 RVA: 0x0026D58C File Offset: 0x0026B98C
		public virtual bool OnHediffAdded(Pawn pawn, Hediff hediff)
		{
			return false;
		}

		// Token: 0x06004A74 RID: 19060 RVA: 0x0026D5A4 File Offset: 0x0026B9A4
		public bool TryApply(Pawn pawn, List<Hediff> outAddedHediffs = null)
		{
			return HediffGiveUtility.TryApply(pawn, this.hediff, this.partsToAffect, this.canAffectAnyLivePart, this.countToAffect, outAddedHediffs);
		}

		// Token: 0x06004A75 RID: 19061 RVA: 0x0026D5D8 File Offset: 0x0026B9D8
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

		// Token: 0x04003245 RID: 12869
		[TranslationHandle]
		public HediffDef hediff;

		// Token: 0x04003246 RID: 12870
		public List<BodyPartDef> partsToAffect = null;

		// Token: 0x04003247 RID: 12871
		public bool canAffectAnyLivePart = false;

		// Token: 0x04003248 RID: 12872
		public int countToAffect = 1;
	}
}
