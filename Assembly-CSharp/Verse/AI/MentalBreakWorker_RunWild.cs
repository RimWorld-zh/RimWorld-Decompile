using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A5C RID: 2652
	public class MentalBreakWorker_RunWild : MentalBreakWorker
	{
		// Token: 0x06003B02 RID: 15106 RVA: 0x001F5148 File Offset: 0x001F3548
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

		// Token: 0x06003B03 RID: 15107 RVA: 0x001F5210 File Offset: 0x001F3610
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
