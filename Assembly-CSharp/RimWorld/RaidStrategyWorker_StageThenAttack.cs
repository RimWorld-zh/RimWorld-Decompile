using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001A8 RID: 424
	public class RaidStrategyWorker_StageThenAttack : RaidStrategyWorker
	{
		// Token: 0x060008C7 RID: 2247 RVA: 0x00052764 File Offset: 0x00050B64
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			IntVec3 entrySpot = (!parms.spawnCenter.IsValid) ? pawns[0].PositionHeld : parms.spawnCenter;
			IntVec3 stageLoc = RCellFinder.FindSiegePositionFrom(entrySpot, map);
			return new LordJob_StageThenAttack(parms.faction, stageLoc, raidSeed);
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x000527B8 File Offset: 0x00050BB8
		public override bool CanUseWith(IncidentParms parms)
		{
			return base.CanUseWith(parms) && parms.faction.def.canStageAttacks;
		}
	}
}
