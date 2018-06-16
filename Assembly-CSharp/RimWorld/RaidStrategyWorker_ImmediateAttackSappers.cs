using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001A5 RID: 421
	public class RaidStrategyWorker_ImmediateAttackSappers : RaidStrategyWorker
	{
		// Token: 0x060008B6 RID: 2230 RVA: 0x000522C8 File Offset: 0x000506C8
		public override bool CanUseWith(IncidentParms parms)
		{
			return this.PawnGenOptionsWithSappers(parms.faction).Any<PawnGroupMaker>() && base.CanUseWith(parms);
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x00052310 File Offset: 0x00050710
		public override float MinimumPoints(Faction faction)
		{
			return this.CheapestSapperCost(faction);
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0005232C File Offset: 0x0005072C
		public override float MinMaxAllowedPawnGenOptionCost(Faction faction)
		{
			return this.CheapestSapperCost(faction);
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x00052348 File Offset: 0x00050748
		private float CheapestSapperCost(Faction faction)
		{
			IEnumerable<PawnGroupMaker> enumerable = this.PawnGenOptionsWithSappers(faction);
			float result;
			if (!enumerable.Any<PawnGroupMaker>())
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to get MinimumPoints for ",
					base.GetType().ToString(),
					" for faction ",
					faction.ToString(),
					" but the faction has no groups with sappers."
				}), false);
				result = 99999f;
			}
			else
			{
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
				result = num;
			}
			return result;
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x0005248C File Offset: 0x0005088C
		public override bool CanUsePawnGenOption(PawnGenOption opt, List<PawnGenOption> chosenOpts)
		{
			if (chosenOpts.Count == 0)
			{
				if (!opt.kind.canBeSapper)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x000524C8 File Offset: 0x000508C8
		public override bool CanUsePawn(Pawn p, List<Pawn> otherPawns)
		{
			if (otherPawns.Count == 0)
			{
				if (!SappersUtility.IsGoodSapper(p) && !SappersUtility.IsGoodBackupSapper(p))
				{
					return false;
				}
			}
			return !p.kindDef.canBeSapper || !SappersUtility.HasBuildingDestroyerWeapon(p) || SappersUtility.IsGoodSapper(p);
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x00052538 File Offset: 0x00050938
		private IEnumerable<PawnGroupMaker> PawnGenOptionsWithSappers(Faction faction)
		{
			IEnumerable<PawnGroupMaker> result;
			if (faction.def.pawnGroupMakers == null)
			{
				result = Enumerable.Empty<PawnGroupMaker>();
			}
			else
			{
				result = faction.def.pawnGroupMakers.Where(delegate(PawnGroupMaker gm)
				{
					bool result2;
					if (gm.kindDef == PawnGroupKindDefOf.Combat && gm.options != null)
					{
						result2 = gm.options.Any((PawnGenOption op) => op.kind.canBeSapper);
					}
					else
					{
						result2 = false;
					}
					return result2;
				});
			}
			return result;
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x00052598 File Offset: 0x00050998
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			return new LordJob_AssaultColony(parms.faction, true, true, true, true, true);
		}
	}
}
