using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020002BD RID: 701
	public abstract class RaidStrategyWorker
	{
		// Token: 0x040006DB RID: 1755
		public RaidStrategyDef def;

		// Token: 0x06000BBC RID: 3004 RVA: 0x000520E8 File Offset: 0x000504E8
		public virtual float SelectionWeight(Map map, float basePoints)
		{
			return this.def.selectionWeightPerPointsCurve.Evaluate(basePoints);
		}

		// Token: 0x06000BBD RID: 3005
		protected abstract LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed);

		// Token: 0x06000BBE RID: 3006 RVA: 0x00052110 File Offset: 0x00050510
		public virtual void MakeLords(IncidentParms parms, List<Pawn> pawns)
		{
			Map map = (Map)parms.target;
			List<List<Pawn>> list = IncidentParmsUtility.SplitIntoGroups(pawns, parms.pawnGroups);
			int @int = Rand.Int;
			for (int i = 0; i < list.Count; i++)
			{
				List<Pawn> list2 = list[i];
				Lord lord = LordMaker.MakeNewLord(parms.faction, this.MakeLordJob(parms, map, list2, @int), map, list2);
				if (DebugViewSettings.drawStealDebug && parms.faction.HostileTo(Faction.OfPlayer))
				{
					Log.Message(string.Concat(new object[]
					{
						"Market value threshold to start stealing (raiders=",
						lord.ownedPawns.Count,
						"): ",
						StealAIUtility.StartStealingMarketValueThreshold(lord),
						" (colony wealth=",
						map.wealthWatcher.WealthTotal,
						")"
					}), false);
				}
			}
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x00052204 File Offset: 0x00050604
		public virtual bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return parms.points >= this.MinimumPoints(parms.faction, groupKind);
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x0005223C File Offset: 0x0005063C
		public virtual float MinimumPoints(Faction faction, PawnGroupKindDef groupKind)
		{
			return faction.def.MinPointsToGeneratePawnGroup(groupKind);
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x00052260 File Offset: 0x00050660
		public virtual float MinMaxAllowedPawnGenOptionCost(Faction faction, PawnGroupKindDef groupKind)
		{
			return 0f;
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x0005227C File Offset: 0x0005067C
		public virtual bool CanUsePawnGenOption(PawnGenOption g, List<PawnGenOption> chosenGroups)
		{
			return true;
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x00052294 File Offset: 0x00050694
		public virtual bool CanUsePawn(Pawn p, List<Pawn> otherPawns)
		{
			return true;
		}
	}
}
