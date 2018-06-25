using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class IncidentWorker_FarmAnimalsWanderIn : IncidentWorker
	{
		private const float MaxWildness = 0.35f;

		private const float TotalBodySizeToSpawn = 2.5f;

		[CompilerGenerated]
		private static Func<PawnKindDef, float> <>f__am$cache0;

		public IncidentWorker_FarmAnimalsWanderIn()
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
				PawnKindDef pawnKindDef;
				result = (RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, null) && this.TryFindRandomPawnKind(map, out pawnKindDef));
			}
			return result;
		}

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

		private bool TryFindRandomPawnKind(Map map, out PawnKindDef kind)
		{
			return (from x in DefDatabase<PawnKindDef>.AllDefs
			where x.RaceProps.Animal && x.RaceProps.wildness < 0.35f && map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(x.race)
			select x).TryRandomElementByWeight((PawnKindDef k) => 0.420000017f - k.RaceProps.wildness, out kind);
		}

		[CompilerGenerated]
		private static float <TryFindRandomPawnKind>m__0(PawnKindDef k)
		{
			return 0.420000017f - k.RaceProps.wildness;
		}

		[CompilerGenerated]
		private sealed class <TryFindRandomPawnKind>c__AnonStorey0
		{
			internal Map map;

			public <TryFindRandomPawnKind>c__AnonStorey0()
			{
			}

			internal bool <>m__0(PawnKindDef x)
			{
				return x.RaceProps.Animal && x.RaceProps.wildness < 0.35f && this.map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(x.race);
			}
		}
	}
}
