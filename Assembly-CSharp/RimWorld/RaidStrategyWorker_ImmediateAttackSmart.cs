using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001A7 RID: 423
	public class RaidStrategyWorker_ImmediateAttackSmart : RaidStrategyWorker
	{
		// Token: 0x060008C4 RID: 2244 RVA: 0x000526EC File Offset: 0x00050AEC
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			return new LordJob_AssaultColony(parms.faction, true, true, false, true, true);
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x00052714 File Offset: 0x00050B14
		public override bool CanUseWith(IncidentParms parms)
		{
			return base.CanUseWith(parms) && parms.faction.def.canUseAvoidGrid;
		}
	}
}
