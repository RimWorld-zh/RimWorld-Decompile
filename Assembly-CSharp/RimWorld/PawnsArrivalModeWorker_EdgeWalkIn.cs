using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200049B RID: 1179
	public class PawnsArrivalModeWorker_EdgeWalkIn : PawnsArrivalModeWorker
	{
		// Token: 0x0600151A RID: 5402 RVA: 0x000B98C4 File Offset: 0x000B7CC4
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			Map map = (Map)parms.target;
			for (int i = 0; i < pawns.Count; i++)
			{
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(parms.spawnCenter, map, 8, null);
				GenSpawn.Spawn(pawns[i], loc, map, parms.spawnRotation, WipeMode.Vanish, false);
			}
		}

		// Token: 0x0600151B RID: 5403 RVA: 0x000B9920 File Offset: 0x000B7D20
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
