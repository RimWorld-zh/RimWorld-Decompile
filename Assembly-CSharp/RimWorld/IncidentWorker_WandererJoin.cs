using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000345 RID: 837
	public class IncidentWorker_WandererJoin : IncidentWorker
	{
		// Token: 0x040008EB RID: 2283
		private const float RelationWithColonistWeight = 20f;

		// Token: 0x06000E48 RID: 3656 RVA: 0x00079370 File Offset: 0x00077770
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			bool result;
			if (!base.CanFireNowSub(parms))
			{
				result = false;
			}
			else
			{
				Map map = (Map)parms.target;
				IntVec3 intVec;
				result = this.TryFindEntryCell(map, out intVec);
			}
			return result;
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x000793B0 File Offset: 0x000777B0
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 loc;
			bool result;
			if (!this.TryFindEntryCell(map, out loc))
			{
				result = false;
			}
			else
			{
				PawnKindDef villager = PawnKindDefOf.Villager;
				PawnGenerationRequest request = new PawnGenerationRequest(villager, Faction.OfPlayer, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 20f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
				Pawn pawn = PawnGenerator.GeneratePawn(request);
				GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
				string text = "WandererJoin".Translate(new object[]
				{
					villager.label,
					pawn.story.Title
				});
				text = text.AdjustedFor(pawn, "PAWN");
				string label = "LetterLabelWandererJoin".Translate();
				PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref label, pawn);
				Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.PositiveEvent, pawn, null, null);
				result = true;
			}
			return result;
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x000794C8 File Offset: 0x000778C8
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c), map, CellFinder.EdgeRoadChance_Neutral, out cell);
		}
	}
}
