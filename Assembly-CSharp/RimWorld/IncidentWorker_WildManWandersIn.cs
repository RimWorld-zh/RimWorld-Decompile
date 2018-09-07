using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_WildManWandersIn : IncidentWorker
	{
		public IncidentWorker_WildManWandersIn()
		{
		}

		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Faction faction;
			if (!this.TryFindFormerFaction(out faction))
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return !map.GameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) && map.mapTemperature.SeasonAcceptableFor(ThingDefOf.Human) && this.TryFindEntryCell(map, out intVec);
		}

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 loc;
			if (!this.TryFindEntryCell(map, out loc))
			{
				return false;
			}
			Faction faction;
			if (!this.TryFindFormerFaction(out faction))
			{
				return false;
			}
			Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.WildMan, faction);
			pawn.SetFaction(null, null);
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
			string letterLabel = this.def.letterLabel;
			string text = string.Format(this.def.letterText.AdjustedFor(pawn, "PAWN"), pawn.LabelShort).CapitalizeFirst();
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref letterLabel, pawn);
			Find.LetterStack.ReceiveLetter(letterLabel, text, this.def.letterDef, pawn, null, null);
			return true;
		}

		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c), map, CellFinder.EdgeRoadChance_Ignore, out cell);
		}

		private bool TryFindFormerFaction(out Faction formerFaction)
		{
			return Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out formerFaction, false, true, TechLevel.Undefined);
		}

		[CompilerGenerated]
		private sealed class <TryFindEntryCell>c__AnonStorey0
		{
			internal Map map;

			public <TryFindEntryCell>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 c)
			{
				return this.map.reachability.CanReachColony(c);
			}
		}
	}
}
