using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001A8 RID: 424
	public class RaidStrategyWorker_StageThenAttack : RaidStrategyWorker
	{
		// Token: 0x060008C4 RID: 2244 RVA: 0x0005276C File Offset: 0x00050B6C
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			IntVec3 entrySpot = (!parms.spawnCenter.IsValid) ? pawns[0].PositionHeld : parms.spawnCenter;
			IntVec3 stageLoc = RCellFinder.FindSiegePositionFrom(entrySpot, map);
			return new LordJob_StageThenAttack(parms.faction, stageLoc, raidSeed);
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x000527C0 File Offset: 0x00050BC0
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && parms.faction.def.canStageAttacks;
		}
	}
}
