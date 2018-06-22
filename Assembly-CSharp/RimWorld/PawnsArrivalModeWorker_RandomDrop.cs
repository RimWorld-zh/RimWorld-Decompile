using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200049D RID: 1181
	public class PawnsArrivalModeWorker_RandomDrop : PawnsArrivalModeWorker
	{
		// Token: 0x06001520 RID: 5408 RVA: 0x000B9A78 File Offset: 0x000B7E78
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			Map map = (Map)parms.target;
			for (int i = 0; i < pawns.Count; i++)
			{
				IntVec3 dropCenter = DropCellFinder.RandomDropSpot(map);
				DropPodUtility.DropThingsNear(dropCenter, map, Gen.YieldSingle<Thing>(pawns[i]), parms.podOpenDelay, false, true, true, false);
			}
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x000B9AD0 File Offset: 0x000B7ED0
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			parms.podOpenDelay = 520;
			parms.spawnRotation = Rot4.Random;
			return true;
		}
	}
}
