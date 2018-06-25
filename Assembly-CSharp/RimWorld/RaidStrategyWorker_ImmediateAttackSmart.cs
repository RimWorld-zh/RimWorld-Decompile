using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001A7 RID: 423
	public class RaidStrategyWorker_ImmediateAttackSmart : RaidStrategyWorker
	{
		// Token: 0x060008C1 RID: 2241 RVA: 0x000526F4 File Offset: 0x00050AF4
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			return new LordJob_AssaultColony(parms.faction, true, true, false, true, true);
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x0005271C File Offset: 0x00050B1C
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && parms.faction.def.canUseAvoidGrid;
		}
	}
}
