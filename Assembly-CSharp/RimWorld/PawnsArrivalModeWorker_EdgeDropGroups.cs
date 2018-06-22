using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200049A RID: 1178
	public class PawnsArrivalModeWorker_EdgeDropGroups : PawnsArrivalModeWorker
	{
		// Token: 0x06001517 RID: 5399 RVA: 0x000B9820 File Offset: 0x000B7C20
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			Map map = (Map)parms.target;
			List<Pair<List<Pawn>, IntVec3>> list = PawnsArrivalModeWorkerUtility.SplitIntoRandomGroupsNearMapEdge(pawns, map, true);
			PawnsArrivalModeWorkerUtility.SetPawnGroupsInfo(parms, list);
			for (int i = 0; i < list.Count; i++)
			{
				DropPodUtility.DropThingsNear(list[i].Second, map, list[i].First.Cast<Thing>(), parms.podOpenDelay, false, true, true, false);
			}
		}

		// Token: 0x06001518 RID: 5400 RVA: 0x000B9898 File Offset: 0x000B7C98
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			parms.spawnRotation = Rot4.Random;
			return true;
		}
	}
}
