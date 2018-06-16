using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000343 RID: 835
	public class IncidentWorker_WandererJoin : IncidentWorker
	{
		// Token: 0x06000E44 RID: 3652 RVA: 0x00079120 File Offset: 0x00077520
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

		// Token: 0x06000E45 RID: 3653 RVA: 0x00079160 File Offset: 0x00077560
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
				text = text.AdjustedFor(pawn);
				string label = "LetterLabelWandererJoin".Translate();
				PawnRelationUtility.TryAppendRelationsWithColonistsInfo(ref text, ref label, pawn);
				Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.PositiveEvent, pawn, null, null);
				result = true;
			}
			return result;
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x00079270 File Offset: 0x00077670
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c), map, CellFinder.EdgeRoadChance_Neutral, out cell);
		}

		// Token: 0x040008E9 RID: 2281
		private const float RelationWithColonistWeight = 20f;
	}
}
