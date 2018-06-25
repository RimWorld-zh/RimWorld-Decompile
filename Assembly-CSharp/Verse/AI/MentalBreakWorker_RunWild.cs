using System;
using RimWorld;

namespace Verse.AI
{
	public class MentalBreakWorker_RunWild : MentalBreakWorker
	{
		public MentalBreakWorker_RunWild()
		{
		}

		public override bool BreakCanOccur(Pawn pawn)
		{
			bool result;
			if (!pawn.IsColonistPlayerControlled || pawn.Downed || !pawn.Spawned || !base.BreakCanOccur(pawn))
			{
				result = false;
			}
			else if (pawn.Map.GameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout))
			{
				result = false;
			}
			else
			{
				float seasonalTemp = Find.World.tileTemperatures.GetSeasonalTemp(pawn.Map.Tile);
				result = (seasonalTemp >= pawn.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) - 7f && seasonalTemp <= pawn.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null) + 7f);
			}
			return result;
		}

		public override bool TryStart(Pawn pawn, Thought reason, bool causedByMood)
		{
			base.TrySendLetter(pawn, "LetterRunWildMentalBreak", reason);
			pawn.ChangeKind(PawnKindDefOf.WildMan);
			if (pawn.Faction != null)
			{
				pawn.SetFaction(null, null);
			}
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.Catharsis, null);
			if (pawn.Spawned && !pawn.Downed)
			{
				pawn.jobs.StopAll(false);
			}
			return true;
		}
	}
}
