using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_FarmAnimalsWanderIn : IncidentWorker
	{
		private const float MaxWildness = 0.35f;

		private const float TotalBodySizeToSpawn = 2.5f;

		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec = default(IntVec3);
			bool result;
			PawnKindDef pawnKindDef = default(PawnKindDef);
			if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, (Predicate<IntVec3>)null))
			{
				result = false;
			}
			else if (!(from x in DefDatabase<PawnKindDef>.AllDefs
			where x.RaceProps.Animal && x.RaceProps.wildness < 0.34999999403953552 && map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(x.race)
			select x).TryRandomElementByWeight<PawnKindDef>((Func<PawnKindDef, float>)((PawnKindDef k) => (float)(0.42000001668930054 - k.RaceProps.wildness)), out pawnKindDef))
			{
				result = false;
			}
			else
			{
				int num = Mathf.Clamp(GenMath.RoundRandom((float)(2.5 / pawnKindDef.RaceProps.baseBodySize)), 2, 10);
				for (int num2 = 0; num2 < num; num2++)
				{
					IntVec3 loc = CellFinder.RandomClosewalkCellNear(intVec, map, 12, null);
					Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, null);
					GenSpawn.Spawn(pawn, loc, map, Rot4.Random, false);
					pawn.SetFaction(Faction.OfPlayer, null);
				}
				Find.LetterStack.ReceiveLetter("LetterLabelFarmAnimalsWanderIn".Translate(pawnKindDef.GetLabelPlural(-1)).CapitalizeFirst(), "LetterFarmAnimalsWanderIn".Translate(pawnKindDef.GetLabelPlural(-1)), LetterDefOf.PositiveEvent, new TargetInfo(intVec, map, false), (string)null);
				result = true;
			}
			return result;
		}
	}
}
