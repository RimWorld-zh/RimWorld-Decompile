using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200032B RID: 811
	public class IncidentWorker_FarmAnimalsWanderIn : IncidentWorker
	{
		// Token: 0x040008CF RID: 2255
		private const float MaxWildness = 0.35f;

		// Token: 0x040008D0 RID: 2256
		private const float TotalBodySizeToSpawn = 2.5f;

		// Token: 0x06000DE0 RID: 3552 RVA: 0x000768C4 File Offset: 0x00074CC4
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
				PawnKindDef pawnKindDef;
				result = (RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, null) && this.TryFindRandomPawnKind(map, out pawnKindDef));
			}
			return result;
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x00076918 File Offset: 0x00074D18
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			bool result;
			PawnKindDef pawnKindDef;
			if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, null))
			{
				result = false;
			}
			else if (!this.TryFindRandomPawnKind(map, out pawnKindDef))
			{
				result = false;
			}
			else
			{
				int num = Mathf.Clamp(GenMath.RoundRandom(2.5f / pawnKindDef.RaceProps.baseBodySize), 2, 10);
				for (int i = 0; i < num; i++)
				{
					IntVec3 loc = CellFinder.RandomClosewalkCellNear(intVec, map, 12, null);
					Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, null);
					GenSpawn.Spawn(pawn, loc, map, Rot4.Random, WipeMode.Vanish, false);
					pawn.SetFaction(Faction.OfPlayer, null);
				}
				Find.LetterStack.ReceiveLetter("LetterLabelFarmAnimalsWanderIn".Translate(new object[]
				{
					pawnKindDef.GetLabelPlural(-1)
				}).CapitalizeFirst(), "LetterFarmAnimalsWanderIn".Translate(new object[]
				{
					pawnKindDef.GetLabelPlural(-1)
				}), LetterDefOf.PositiveEvent, new TargetInfo(intVec, map, false), null, null);
				result = true;
			}
			return result;
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x00076A30 File Offset: 0x00074E30
		private bool TryFindRandomPawnKind(Map map, out PawnKindDef kind)
		{
			return (from x in DefDatabase<PawnKindDef>.AllDefs
			where x.RaceProps.Animal && x.RaceProps.wildness < 0.35f && map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(x.race)
			select x).TryRandomElementByWeight((PawnKindDef k) => 0.420000017f - k.RaceProps.wildness, out kind);
		}
	}
}
