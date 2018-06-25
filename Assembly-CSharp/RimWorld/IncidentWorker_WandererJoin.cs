using System;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_WandererJoin : IncidentWorker
	{
		private const float RelationWithColonistWeight = 20f;

		public IncidentWorker_WandererJoin()
		{
		}

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

		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => map.reachability.CanReachColony(c), map, CellFinder.EdgeRoadChance_Neutral, out cell);
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
