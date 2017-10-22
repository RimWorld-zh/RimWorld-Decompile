using RimWorld;
using System.Collections.Generic;

namespace Verse
{
	public abstract class HediffGiver
	{
		public HediffDef hediff;

		public List<BodyPartDef> partsToAffect;

		public bool canAffectAnyLivePart;

		public int countToAffect = 1;

		public virtual void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
		}

		public virtual bool OnHediffAdded(Pawn pawn, Hediff hediff)
		{
			return false;
		}

		public bool TryApply(Pawn pawn, List<Hediff> outAddedHediffs = null)
		{
			return HediffGiveUtility.TryApply(pawn, this.hediff, this.partsToAffect, this.canAffectAnyLivePart, this.countToAffect, outAddedHediffs);
		}

		protected void SendLetter(Pawn pawn, Hediff cause)
		{
			if (PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				if (cause == null)
				{
					Find.LetterStack.ReceiveLetter("LetterHediffFromRandomHediffGiverLabel".Translate(pawn.LabelShort, this.hediff.LabelCap), "LetterHediffFromRandomHediffGiver".Translate(pawn.LabelShort, this.hediff.LabelCap), LetterDefOf.BadNonUrgent, (Thing)pawn, (string)null);
				}
				else
				{
					Find.LetterStack.ReceiveLetter("LetterHealthComplicationsLabel".Translate(pawn.LabelShort, this.hediff.LabelCap), "LetterHealthComplications".Translate(pawn.LabelShort, this.hediff.LabelCap, cause.LabelCap), LetterDefOf.BadNonUrgent, (Thing)pawn, (string)null);
				}
			}
		}
	}
}
