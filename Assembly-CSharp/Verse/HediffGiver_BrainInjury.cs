using RimWorld;

namespace Verse
{
	public class HediffGiver_BrainInjury : HediffGiver
	{
		public float chancePerDamagePct;

		public string letterLabel;

		public string letter;

		public override bool OnHediffAdded(Pawn pawn, Hediff hediff)
		{
			bool result;
			if (!(hediff is Hediff_Injury))
			{
				result = false;
			}
			else if (hediff.Part != pawn.health.hediffSet.GetBrain())
			{
				result = false;
			}
			else
			{
				float num = hediff.Severity / hediff.Part.def.GetMaxHealth(pawn);
				if (Rand.Value < num * this.chancePerDamagePct && base.TryApply(pawn, null))
				{
					if ((pawn.Faction == Faction.OfPlayer || pawn.IsPrisonerOfColony) && !this.letter.NullOrEmpty())
					{
						Find.LetterStack.ReceiveLetter(this.letterLabel, this.letter.AdjustedFor(pawn), LetterDefOf.NegativeEvent, (Thing)pawn, (string)null);
					}
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}
	}
}
