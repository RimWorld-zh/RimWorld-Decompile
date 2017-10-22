using RimWorld;

namespace Verse.AI
{
	public class MentalBreakWorker_RunWild : MentalBreakWorker
	{
		public override bool BreakCanOccur(Pawn pawn)
		{
			return pawn.IsColonistPlayerControlled && !pawn.Downed && pawn.Spawned && pawn.Map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(pawn.def) && base.BreakCanOccur(pawn);
		}

		public override bool TryStart(Pawn pawn, Thought reason, bool causedByMood)
		{
			pawn.ChangeKind(PawnKindDefOf.WildMan);
			if (pawn.Faction != null)
			{
				pawn.SetFaction(null, null);
			}
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.Catharsis, null);
			base.TrySendLetter(pawn, "LetterRunWildMentalBreak", reason);
			return true;
		}
	}
}
