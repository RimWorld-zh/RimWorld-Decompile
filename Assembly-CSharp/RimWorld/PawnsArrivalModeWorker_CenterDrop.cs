using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200049A RID: 1178
	public class PawnsArrivalModeWorker_CenterDrop : PawnsArrivalModeWorker
	{
		// Token: 0x04000C9C RID: 3228
		public const int PodOpenDelay = 520;

		// Token: 0x06001512 RID: 5394 RVA: 0x000B99FE File Offset: 0x000B7DFE
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			PawnsArrivalModeWorkerUtility.DropInDropPodsNearSpawnCenter(parms, pawns);
		}

		// Token: 0x06001513 RID: 5395 RVA: 0x000B9A08 File Offset: 0x000B7E08
		public override void TravelingTransportPodsArrived(List<ActiveDropPodInfo> dropPods, Map map)
		{
			IntVec3 near;
			if (!DropCellFinder.TryFindRaidDropCenterClose(out near, map))
			{
				near = DropCellFinder.FindRaidDropCenterDistant(map);
			}
			TransportPodsArrivalActionUtility.DropTravelingTransportPods(dropPods, near, map);
		}

		// Token: 0x06001514 RID: 5396 RVA: 0x000B9A34 File Offset: 0x000B7E34
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			parms.podOpenDelay = 520;
			parms.spawnRotation = Rot4.Random;
			if (!parms.spawnCenter.IsValid)
			{
				if (Rand.Chance(0.4f) && map.listerBuildings.ColonistsHaveBuildingWithPowerOn(ThingDefOf.OrbitalTradeBeacon))
				{
					parms.spawnCenter = DropCellFinder.TradeDropSpot(map);
				}
				else if (!DropCellFinder.TryFindRaidDropCenterClose(out parms.spawnCenter, map))
				{
					parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeDrop;
					return parms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(parms);
				}
			}
			return true;
		}
	}
}
