using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020001A9 RID: 425
	public class RaidStrategyWorker_Siege : RaidStrategyWorker
	{
		// Token: 0x060008C8 RID: 2248 RVA: 0x00052808 File Offset: 0x00050C08
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			IntVec3 entrySpot = (!parms.spawnCenter.IsValid) ? pawns[0].PositionHeld : parms.spawnCenter;
			IntVec3 siegeSpot = RCellFinder.FindSiegePositionFrom(entrySpot, map);
			float num = parms.points * Rand.Range(0.2f, 0.3f);
			if (num < 60f)
			{
				num = 60f;
			}
			return new LordJob_Siege(parms.faction, siegeSpot, num);
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x00052884 File Offset: 0x00050C84
		public override float MinimumPoints(Faction fac, PawnGroupKindDef groupKind)
		{
			return Mathf.Max(base.MinimumPoints(fac, groupKind), 350f);
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x000528AC File Offset: 0x00050CAC
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && parms.faction.def.canSiege;
		}

		// Token: 0x040003AF RID: 943
		private const float MinPointsForSiege = 350f;
	}
}
