using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200049D RID: 1181
	public class PawnsArrivalModeWorker_EdgeWalkIn : PawnsArrivalModeWorker
	{
		// Token: 0x0600151E RID: 5406 RVA: 0x000B9A14 File Offset: 0x000B7E14
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			Map map = (Map)parms.target;
			for (int i = 0; i < pawns.Count; i++)
			{
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(parms.spawnCenter, map, 8, null);
				GenSpawn.Spawn(pawns[i], loc, map, parms.spawnRotation, WipeMode.Vanish, false);
			}
		}

		// Token: 0x0600151F RID: 5407 RVA: 0x000B9A70 File Offset: 0x000B7E70
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!parms.spawnCenter.IsValid)
			{
				if (!RCellFinder.TryFindRandomPawnEntryCell(out parms.spawnCenter, map, CellFinder.EdgeRoadChance_Hostile, null))
				{
					return false;
				}
			}
			parms.spawnRotation = Rot4.FromAngleFlat((map.Center - parms.spawnCenter).AngleFlat);
			return true;
		}
	}
}
