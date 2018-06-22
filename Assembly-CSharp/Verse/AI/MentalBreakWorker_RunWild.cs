using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A5A RID: 2650
	public class MentalBreakWorker_RunWild : MentalBreakWorker
	{
		// Token: 0x06003AFE RID: 15102 RVA: 0x001F501C File Offset: 0x001F341C
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

		// Token: 0x06003AFF RID: 15103 RVA: 0x001F50E4 File Offset: 0x001F34E4
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
