using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D2F RID: 3375
	public abstract class HediffGiver
	{
		// Token: 0x04003245 RID: 12869
		[TranslationHandle]
		public HediffDef hediff;

		// Token: 0x04003246 RID: 12870
		public List<BodyPartDef> partsToAffect = null;

		// Token: 0x04003247 RID: 12871
		public bool canAffectAnyLivePart = false;

		// Token: 0x04003248 RID: 12872
		public int countToAffect = 1;

		// Token: 0x06004A76 RID: 19062 RVA: 0x0026D6B5 File Offset: 0x0026BAB5
		public virtual void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
		}

		// Token: 0x06004A77 RID: 19063 RVA: 0x0026D6B8 File Offset: 0x0026BAB8
		public virtual bool OnHediffAdded(Pawn pawn, Hediff hediff)
		{
			return false;
		}

		// Token: 0x06004A78 RID: 19064 RVA: 0x0026D6D0 File Offset: 0x0026BAD0
		public bool TryApply(Pawn pawn, List<Hediff> outAddedHediffs = null)
		{
			return HediffGiveUtility.TryApply(pawn, this.hediff, this.partsToAffect, this.canAffectAnyLivePart, this.countToAffect, outAddedHediffs);
		}

		// Token: 0x06004A79 RID: 19065 RVA: 0x0026D704 File Offset: 0x0026BB04
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
