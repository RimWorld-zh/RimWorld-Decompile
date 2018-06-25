using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200049C RID: 1180
	public class PawnsArrivalModeWorker_EdgeDropGroups : PawnsArrivalModeWorker
	{
		// Token: 0x0600151B RID: 5403 RVA: 0x000B9970 File Offset: 0x000B7D70
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

		// Token: 0x0600151C RID: 5404 RVA: 0x000B99E8 File Offset: 0x000B7DE8
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			parms.spawnRotation = Rot4.Random;
			return true;
		}
	}
}
