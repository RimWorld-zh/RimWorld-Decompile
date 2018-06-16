using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A1 RID: 1185
	public class PawnsArrivalModeWorker_RandomDrop : PawnsArrivalModeWorker
	{
		// Token: 0x06001529 RID: 5417 RVA: 0x000B9A60 File Offset: 0x000B7E60
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			Map map = (Map)parms.target;
			for (int i = 0; i < pawns.Count; i++)
			{
				IntVec3 dropCenter = DropCellFinder.RandomDropSpot(map);
				DropPodUtility.DropThingsNear(dropCenter, map, Gen.YieldSingle<Thing>(pawns[i]), parms.podOpenDelay, false, true, true, false);
			}
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x000B9AB8 File Offset: 0x000B7EB8
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			parms.podOpenDelay = 520;
			parms.spawnRotation = Rot4.Random;
			return true;
		}
	}
}
