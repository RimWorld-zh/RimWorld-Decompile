using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001A5 RID: 421
	public class RaidStrategyWorker_ImmediateAttackSappers : RaidStrategyWorker
	{
		// Token: 0x060008B5 RID: 2229 RVA: 0x000522B0 File Offset: 0x000506B0
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return this.PawnGenOptionsWithSappers(parms.faction, groupKind).Any<PawnGroupMaker>() && base.CanUseWith(parms, groupKind);
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x000522F8 File Offset: 0x000506F8
		public override float MinimumPoints(Faction faction, PawnGroupKindDef groupKind)
		{
			return Mathf.Max(base.MinimumPoints(faction, groupKind), this.CheapestSapperCost(faction, groupKind));
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x00052324 File Offset: 0x00050724
		public override float MinMaxAllowedPawnGenOptionCost(Faction faction, PawnGroupKindDef groupKind)
		{
			return this.CheapestSapperCost(faction, groupKind);
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x00052344 File Offset: 0x00050744
		private float CheapestSapperCost(Faction faction, PawnGroupKindDef groupKind)
		{
			IEnumerable<PawnGroupMaker> enumerable = this.PawnGenOptionsWithSappers(faction, groupKind);
			float result;
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

		// Token: 0x060008B9 RID: 2233 RVA: 0x00052490 File Offset: 0x00050890
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

		// Token: 0x060008BA RID: 2234 RVA: 0x000524CC File Offset: 0x000508CC
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

		// Token: 0x060008BB RID: 2235 RVA: 0x0005253C File Offset: 0x0005093C
		private IEnumerable<PawnGroupMaker> PawnGenOptionsWithSappers(Faction faction, PawnGroupKindDef groupKind)
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
					if (gm.kindDef == groupKind && gm.options != null)
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

		// Token: 0x060008BC RID: 2236 RVA: 0x00052598 File Offset: 0x00050998
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			return new LordJob_AssaultColony(parms.faction, true, true, true, true, true);
		}
	}
}
