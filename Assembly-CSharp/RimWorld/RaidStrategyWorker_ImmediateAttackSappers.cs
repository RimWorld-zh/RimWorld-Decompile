using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	public class RaidStrategyWorker_ImmediateAttackSappers : RaidStrategyWorker
	{
		private static readonly SimpleCurve StrengthChanceFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(10f, 1f),
				true
			},
			{
				new CurvePoint(50f, 5f),
				true
			}
		};

		public override float SelectionChance(Map map)
		{
			float num = base.SelectionChance(map);
			float strengthRating = map.strengthWatcher.StrengthRating;
			return num * RaidStrategyWorker_ImmediateAttackSappers.StrengthChanceFactorCurve.Evaluate(strengthRating);
		}

		public override bool CanUseWith(IncidentParms parms)
		{
			return (byte)((parms.faction.def.humanlikeFaction && (int)parms.faction.def.techLevel >= 4) ? (this.PawnGenOptionsWithSappers(parms.faction).Any() ? (base.CanUseWith(parms) ? 1 : 0) : 0) : 0) != 0;
		}

		public override float MinimumPoints(Faction faction)
		{
			return this.CheapestSapperCost(faction);
		}

		public override float MinMaxAllowedPawnGenOptionCost(Faction faction)
		{
			return this.CheapestSapperCost(faction);
		}

		private float CheapestSapperCost(Faction faction)
		{
			IEnumerable<PawnGroupMaker> enumerable = this.PawnGenOptionsWithSappers(faction);
			float result;
			if (!enumerable.Any())
			{
				Log.Error("Tried to get MinimumPoints for " + base.GetType().ToString() + " for faction " + faction.ToString() + " but the faction has no groups with sappers.");
				result = 99999f;
			}
			else
			{
				float num = 9999999f;
				foreach (PawnGroupMaker item in enumerable)
				{
					foreach (PawnGenOption item2 in from op in item.options
					where RaidStrategyWorker_ImmediateAttackSappers.CanBeSapper(op.kind)
					select op)
					{
						if (item2.Cost < num)
						{
							num = item2.Cost;
						}
					}
				}
				result = num;
			}
			return result;
		}

		public override bool CanUsePawnGenOption(PawnGenOption opt, List<PawnGenOption> chosenOpts)
		{
			return (byte)((chosenOpts.Count != 0 || (opt.kind.weaponTags.Count == 1 && RaidStrategyWorker_ImmediateAttackSappers.CanBeSapper(opt.kind))) ? 1 : 0) != 0;
		}

		private IEnumerable<PawnGroupMaker> PawnGenOptionsWithSappers(Faction faction)
		{
			return from gm in faction.def.pawnGroupMakers
			where gm.kindDef == PawnGroupKindDefOf.Normal && gm.options.Any((Predicate<PawnGenOption>)((PawnGenOption op) => RaidStrategyWorker_ImmediateAttackSappers.CanBeSapper(op.kind)))
			select gm;
		}

		public static bool CanBeSapper(PawnKindDef kind)
		{
			return !kind.weaponTags.NullOrEmpty() && kind.weaponTags[0] == "GrenadeDestructive";
		}

		public override LordJob MakeLordJob(IncidentParms parms, Map map)
		{
			return new LordJob_AssaultColony(parms.faction, true, true, true, true, true);
		}
	}
}
