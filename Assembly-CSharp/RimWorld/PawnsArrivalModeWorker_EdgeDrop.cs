using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000499 RID: 1177
	public class PawnsArrivalModeWorker_EdgeDrop : PawnsArrivalModeWorker
	{
		// Token: 0x06001513 RID: 5395 RVA: 0x000B979F File Offset: 0x000B7B9F
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			PawnsArrivalModeWorkerUtility.DropInDropPodsNearSpawnCenter(parms, pawns);
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x000B97AC File Offset: 0x000B7BAC
		public override void TravelingTransportPodsArrived(List<ActiveDropPodInfo> dropPods, Map map)
		{
			IntVec3 near = DropCellFinder.FindRaidDropCenterDistant(map);
			TransportPodsArrivalActionUtility.DropTravelingTransportPods(dropPods, near, map);
		}

		// Token: 0x06001515 RID: 5397 RVA: 0x000B97CC File Offset: 0x000B7BCC
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
