using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000346 RID: 838
	public class IncidentWorker_WildManWandersIn : IncidentWorker
	{
		// Token: 0x06000E4B RID: 3659 RVA: 0x00079548 File Offset: 0x00077948
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			bool result;
			Faction faction;
			if (!base.CanFireNowSub(parms))
			{
				result = false;
			}
			else if (!this.TryFindFormerFaction(out faction))
			{
				result = false;
			}
			else
			{
				Map map = (Map)parms.target;
				IntVec3 intVec;
				result = (!map.GameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) && map.mapTemperature.SeasonAcceptableFor(ThingDefOf.Human) && this.TryFindEntryCell(map, out intVec));
			}
			return result;
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x000795D4 File Offset: 0x000779D4
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 loc;
			bool result;
			Faction faction;
			if (!this.TryFindEntryCell(map, out loc))
			{
				result = false;
			}
			else if (!this.TryFindFormerFaction(out faction))
			{
				result = false;
			}
			else
			{
				Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.WildMan, faction);
				pawn.SetFaction(null, null);
				GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
				string letterLabel = this.def.letterLabel;
				string text = string.Format(this.def.letterText.AdjustedFor(pawn, "PAWN"), pawn.LabelShort).CapitalizeFirst();
				PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref letterLabel, pawn);
				Find.LetterStack.ReceiveLetter(letterLabel, text, this.def.letterDef, pawn, null, null);
				result = true;
			}
			return result;
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x000796A8 File Offset: 0x00077AA8
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c), map, CellFinder.EdgeRoadChance_Ignore, out cell);
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x000796E8 File Offset: 0x00077AE8
		private bool TryFindFormerFaction(out Faction formerFaction)
		{
			return Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out formerFaction, false, true, TechLevel.Undefined);
		}
	}
}
