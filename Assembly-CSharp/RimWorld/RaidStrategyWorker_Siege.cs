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
		// Token: 0x060008CA RID: 2250 RVA: 0x000527F8 File Offset: 0x00050BF8
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

		// Token: 0x060008CB RID: 2251 RVA: 0x00052874 File Offset: 0x00050C74
		public override float MinimumPoints(Faction fac)
		{
			return Mathf.Max(base.MinimumPoints(fac), 350f);
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x0005289C File Offset: 0x00050C9C
		public override bool CanUseWith(IncidentParms parms)
		{
			return base.CanUseWith(parms) && parms.faction.def.canSiege;
		}

		// Token: 0x040003B1 RID: 945
		private const float MinPointsForSiege = 350f;
	}
}
