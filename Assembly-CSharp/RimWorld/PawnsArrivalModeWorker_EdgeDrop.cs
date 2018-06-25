using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200049B RID: 1179
	public class PawnsArrivalModeWorker_EdgeDrop : PawnsArrivalModeWorker
	{
		// Token: 0x06001517 RID: 5399 RVA: 0x000B98EF File Offset: 0x000B7CEF
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			PawnsArrivalModeWorkerUtility.DropInDropPodsNearSpawnCenter(parms, pawns);
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x000B98FC File Offset: 0x000B7CFC
		public override void TravelingTransportPodsArrived(List<ActiveDropPodInfo> dropPods, Map map)
		{
			IntVec3 near = DropCellFinder.FindRaidDropCenterDistant(map);
			TransportPodsArrivalActionUtility.DropTravelingTransportPods(dropPods, near, map);
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x000B991C File Offset: 0x000B7D1C
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!parms.spawnCenter.IsValid)
			{
				parms.spawnCenter = DropCellFinder.FindRaidDropCenterDistant(map);
			}
			parms.spawnRotation = Rot4.Random;
			return true;
		}
	}
}
