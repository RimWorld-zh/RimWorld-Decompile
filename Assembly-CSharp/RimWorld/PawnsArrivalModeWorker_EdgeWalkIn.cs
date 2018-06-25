using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200049D RID: 1181
	public class PawnsArrivalModeWorker_EdgeWalkIn : PawnsArrivalModeWorker
	{
		// Token: 0x0600151D RID: 5405 RVA: 0x000B9C14 File Offset: 0x000B8014
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			Map map = (Map)parms.target;
			for (int i = 0; i < pawns.Count; i++)
			{
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(parms.spawnCenter, map, 8, null);
				GenSpawn.Spawn(pawns[i], loc, map, parms.spawnRotation, WipeMode.Vanish, false);
			}
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x000B9C70 File Offset: 0x000B8070
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
