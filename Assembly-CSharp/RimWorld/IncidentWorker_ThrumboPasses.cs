using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000344 RID: 836
	public class IncidentWorker_ThrumboPasses : IncidentWorker
	{
		// Token: 0x06000E44 RID: 3652 RVA: 0x00079170 File Offset: 0x00077570
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return !map.gameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) && map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(ThingDefOf.Thrumbo) && this.TryFindEntryCell(map, out intVec);
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x000791D4 File Offset: 0x000775D4
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			bool result;
			if (!this.TryFindEntryCell(map, out intVec))
			{
				result = false;
			}
			else
			{
				PawnKindDef thrumbo = PawnKindDefOf.Thrumbo;
				float num = StorytellerUtility.DefaultThreatPointsNow(map);
				int num2 = GenMath.RoundRandom(num / thrumbo.combatPower);
				int max = Rand.RangeInclusive(2, 4);
				num2 = Mathf.Clamp(num2, 1, max);
				int num3 = Rand.RangeInclusive(90000, 150000);
				IntVec3 invalid = IntVec3.Invalid;
				if (!RCellFinder.TryFindRandomCellOutsideColonyNearTheCenterOfTheMap(intVec, map, 10f, out invalid))
				{
					invalid = IntVec3.Invalid;
				}
				Pawn pawn = null;
				for (int i = 0; i < num2; i++)
				{
					IntVec3 loc = CellFinder.RandomClosewalkCellNear(intVec, map, 10, null);
					pawn = PawnGenerator.GeneratePawn(thrumbo, null);
					GenSpawn.Spawn(pawn, loc, map, Rot4.Random, WipeMode.Vanish, false);
					pawn.mindState.exitMapAfterTick = Find.TickManager.TicksGame + num3;
					if (invalid.IsValid)
					{
						pawn.mindState.forcedGotoPosition = CellFinder.RandomClosewalkCellNear(invalid, map, 10, null);
					}
				}
				Find.LetterStack.ReceiveLetter("LetterLabelThrumboPasses".Translate(new object[]
				{
					thrumbo.label
				}).CapitalizeFirst(), "LetterThrumboPasses".Translate(new object[]
				{
					thrumbo.label
				}), LetterDefOf.PositiveEvent, pawn, null, null);
				result = true;
			}
			return result;
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x00079340 File Offset: 0x00077740
		private bool TryFindEntryCell(Map map, out IntVec3 cell)
		{
			return RCellFinder.TryFindRandomPawnEntryCell(out cell, map, CellFinder.EdgeRoadChance_Animal + 0.2f, null);
		}
	}
}
