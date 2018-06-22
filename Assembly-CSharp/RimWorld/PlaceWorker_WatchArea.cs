using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C76 RID: 3190
	public class PlaceWorker_WatchArea : PlaceWorker
	{
		// Token: 0x060045F4 RID: 17908 RVA: 0x0024E2E4 File Offset: 0x0024C6E4
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
			Map currentMap = Find.CurrentMap;
			GenDraw.DrawFieldEdges(WatchBuildingUtility.CalculateWatchCells(def, center, rot, currentMap).ToList<IntVec3>());
		}
	}
}
