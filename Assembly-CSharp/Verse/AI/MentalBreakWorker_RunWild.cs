using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A5E RID: 2654
	public class MentalBreakWorker_RunWild : MentalBreakWorker
	{
		// Token: 0x06003B01 RID: 15105 RVA: 0x001F4C4C File Offset: 0x001F304C
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

		// Token: 0x06003B02 RID: 15106 RVA: 0x001F4D14 File Offset: 0x001F3114
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
