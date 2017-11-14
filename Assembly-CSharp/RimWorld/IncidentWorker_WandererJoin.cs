using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_WandererJoin : IncidentWorker
	{
		private const float RelationWithColonistWeight = 20f;

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 loc = default(IntVec3);
			if (!CellFinder.TryFindRandomEdgeCellWith((Predicate<IntVec3>)((IntVec3 c) => map.reachability.CanReachColony(c)), map, CellFinder.EdgeRoadChance_Neutral, out loc))
			{
				return false;
			}
			List<PawnKindDef> list = new List<PawnKindDef>();
			list.Add(PawnKindDefOf.Villager);
			PawnKindDef pawnKindDef = list.RandomElement();
			PawnGenerationRequest request = new PawnGenerationRequest(pawnKindDef, Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 20f, false, true, true, false, false, false, false, null, null, null, null, null, null, null);
			Pawn pawn = PawnGenerator.GeneratePawn(request);
			GenSpawn.Spawn(pawn, loc, map);
			string text = "WandererJoin".Translate(pawnKindDef.label, pawn.story.Title.ToLower());
			text = text.AdjustedFor(pawn);
			string label = "LetterLabelWandererJoin".Translate();
			PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref label, pawn);
			Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.PositiveEvent, pawn, null);
			return true;
		}
	}
}
