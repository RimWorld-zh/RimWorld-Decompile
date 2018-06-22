using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001A7 RID: 423
	public class RaidStrategyWorker_ImmediateAttackSmart : RaidStrategyWorker
	{
		// Token: 0x060008C2 RID: 2242 RVA: 0x000526F8 File Offset: 0x00050AF8
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			return new LordJob_AssaultColony(parms.faction, true, true, false, true, true);
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x00052720 File Offset: 0x00050B20
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && parms.faction.def.canUseAvoidGrid;
		}
	}
}
