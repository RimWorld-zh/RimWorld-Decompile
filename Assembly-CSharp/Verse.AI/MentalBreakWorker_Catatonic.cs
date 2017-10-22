using RimWorld;

namespace Verse.AI
{
	public class MentalBreakWorker_Catatonic : MentalBreakWorker
	{
		public override bool BreakCanOccur(Pawn pawn)
		{
			return pawn.IsColonist && pawn.Spawned && base.BreakCanOccur(pawn);
		}

		public override bool TryStart(Pawn pawn, Thought reason, bool causedByMood)
		{
			pawn.health.AddHediff(HediffDefOf.CatatonicBreakdown, null, default(DamageInfo?));
			base.TrySendLetter(pawn, "LetterCatatonicMentalBreak", reason);
			return true;
		}
	}
}
