using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D31 RID: 3377
	public abstract class HediffGiver
	{
		// Token: 0x06004A60 RID: 19040 RVA: 0x0026C025 File Offset: 0x0026A425
		public virtual void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
		}

		// Token: 0x06004A61 RID: 19041 RVA: 0x0026C028 File Offset: 0x0026A428
		public virtual bool OnHediffAdded(Pawn pawn, Hediff hediff)
		{
			return false;
		}

		// Token: 0x06004A62 RID: 19042 RVA: 0x0026C040 File Offset: 0x0026A440
		public bool TryApply(Pawn pawn, List<Hediff> outAddedHediffs = null)
		{
			return HediffGiveUtility.TryApply(pawn, this.hediff, this.partsToAffect, this.canAffectAnyLivePart, this.countToAffect, outAddedHediffs);
		}

		// Token: 0x06004A63 RID: 19043 RVA: 0x0026C074 File Offset: 0x0026A474
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

		// Token: 0x0400323C RID: 12860
		public HediffDef hediff;

		// Token: 0x0400323D RID: 12861
		public List<BodyPartDef> partsToAffect = null;

		// Token: 0x0400323E RID: 12862
		public bool canAffectAnyLivePart = false;

		// Token: 0x0400323F RID: 12863
		public int countToAffect = 1;
	}
}
