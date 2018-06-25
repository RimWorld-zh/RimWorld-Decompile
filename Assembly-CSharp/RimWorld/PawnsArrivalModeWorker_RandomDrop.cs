using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200049F RID: 1183
	public class PawnsArrivalModeWorker_RandomDrop : PawnsArrivalModeWorker
	{
		// Token: 0x06001524 RID: 5412 RVA: 0x000B9BC8 File Offset: 0x000B7FC8
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			Map map = (Map)parms.target;
			for (int i = 0; i < pawns.Count; i++)
			{
				IntVec3 dropCenter = DropCellFinder.RandomDropSpot(map);
				DropPodUtility.DropThingsNear(dropCenter, map, Gen.YieldSingle<Thing>(pawns[i]), parms.podOpenDelay, false, true, true, false);
			}
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x000B9C20 File Offset: 0x000B8020
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			parms.podOpenDelay = 520;
			parms.spawnRotation = Rot4.Random;
			return true;
		}
	}
}
