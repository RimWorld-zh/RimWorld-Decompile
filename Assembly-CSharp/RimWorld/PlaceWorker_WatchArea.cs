using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C7A RID: 3194
	public class PlaceWorker_WatchArea : PlaceWorker
	{
		// Token: 0x060045ED RID: 17901 RVA: 0x0024CF3C File Offset: 0x0024B33C
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
			Map currentMap = Find.CurrentMap;
			GenDraw.DrawFieldEdges(WatchBuildingUtility.CalculateWatchCells(def, center, rot, currentMap).ToList<IntVec3>());
		}
	}
}
