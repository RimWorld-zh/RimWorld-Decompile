using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class RaidStrategyWorker_ImmediateAttackSappers : RaidStrategyWorker
	{
		[CompilerGenerated]
		private static Func<PawnGenOption, bool> <>f__am$cache0;

		public RaidStrategyWorker_ImmediateAttackSappers()
		{
		}

		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return this.PawnGenOptionsWithSappers(parms.faction, groupKind).Any<PawnGroupMaker>() && base.CanUseWith(parms, groupKind);
		}

		public override float MinimumPoints(Faction faction, PawnGroupKindDef groupKind)
		{
			return Mathf.Max(base.MinimumPoints(faction, groupKind), this.CheapestSapperCost(faction, groupKind));
		}

		public override float MinMaxAllowedPawnGenOptionCost(Faction faction, PawnGroupKindDef groupKind)
		{
			return this.CheapestSapperCost(faction, groupKind);
		}

		private float CheapestSapperCost(Faction faction, PawnGroupKindDef groupKind)
		{
			IEnumerable<PawnGroupMaker> enumerable = this.PawnGenOptionsWithSappers(faction, groupKind);
			if (!enumerable.Any<PawnGroupMaker>())
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to get MinimumPoints for ",
					base.GetType().ToString(),
					" for faction ",
					faction.ToString(),
					" but the faction has no groups with sappers. groupKind=",
					groupKind
				}), false);
				return 99999f;
			}
			float num = 9999999f;
			foreach (PawnGroupMaker pawnGroupMaker in enumerable)
			{
				foreach (PawnGenOption pawnGenOption in from op in pawnGroupMaker.options
				where op.kind.canBeSapper
				select op)
				{
					if (pawnGenOption.Cost < num)
					{
						num = pawnGenOption.Cost;
					}
				}
			}
			return num;
		}

		public override bool CanUsePawnGenOption(PawnGenOption opt, List<PawnGenOption> chosenOpts)
		{
			return chosenOpts.Count != 0 || opt.kind.canBeSapper;
		}

		public override bool CanUsePawn(Pawn p, List<Pawn> otherPawns)
		{
			return (otherPawns.Count != 0 || SappersUtility.IsGoodSapper(p) || SappersUtility.IsGoodBackupSapper(p)) && (!p.kindDef.canBeSapper || !SappersUtility.HasBuildingDestroyerWeapon(p) || SappersUtility.IsGoodSapper(p));
		}

		private IEnumerable<PawnGroupMaker> PawnGenOptionsWithSappers(Faction faction, PawnGroupKindDef groupKind)
		{
			if (faction.def.pawnGroupMakers == null)
			{
				return Enumerable.Empty<PawnGroupMaker>();
			}
			return faction.def.pawnGroupMakers.Where(delegate(PawnGroupMaker gm)
			{
				bool result;
				if (gm.kindDef == groupKind && gm.options != null)
				{
					result = gm.options.Any((PawnGenOption op) => op.kind.canBeSapper);
				}
				else
				{
					result = false;
				}
				return result;
			});
		}

		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			return new LordJob_AssaultColony(parms.faction, true, true, true, true, true);
		}

		[CompilerGenerated]
		private static bool <CheapestSapperCost>m__0(PawnGenOption op)
		{
			return op.kind.canBeSapper;
		}

		[CompilerGenerated]
		private sealed class <PawnGenOptionsWithSappers>c__AnonStorey0
		{
			internal PawnGroupKindDef groupKind;

			private static Predicate<PawnGenOption> <>f__am$cache0;

			public <PawnGenOptionsWithSappers>c__AnonStorey0()
			{
			}

			internal bool <>m__0(PawnGroupMaker gm)
			{
				bool result;
				if (gm.kindDef == this.groupKind && gm.options != null)
				{
					result = gm.options.Any((PawnGenOption op) => op.kind.canBeSapper);
				}
				else
				{
					result = false;
				}
				return result;
			}

			private static bool <>m__1(PawnGenOption op)
			{
				return op.kind.canBeSapper;
			}
		}
	}
}
