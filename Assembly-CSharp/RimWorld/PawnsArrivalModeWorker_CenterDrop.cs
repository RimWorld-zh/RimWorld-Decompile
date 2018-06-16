using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200049C RID: 1180
	public class PawnsArrivalModeWorker_CenterDrop : PawnsArrivalModeWorker
	{
		// Token: 0x06001518 RID: 5400 RVA: 0x000B9696 File Offset: 0x000B7A96
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			PawnsArrivalModeWorkerUtility.DropInDropPodsNearSpawnCenter(parms, pawns);
		}

		// Token: 0x06001519 RID: 5401 RVA: 0x000B96A0 File Offset: 0x000B7AA0
		public override void TravelingTransportPodsArrived(List<ActiveDropPodInfo> dropPods, Map map)
		{
			IntVec3 near;
			if (!DropCellFinder.TryFindRaidDropCenterClose(out near, map))
			{
				near = DropCellFinder.FindRaidDropCenterDistant(map);
			}
			TransportPodsArrivalActionUtility.DropTravelingTransportPods(dropPods, near, map);
		}

		// Token: 0x0600151A RID: 5402 RVA: 0x000B96CC File Offset: 0x000B7ACC
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

		// Token: 0x04000C9C RID: 3228
		public const int PodOpenDelay = 520;
	}
}
