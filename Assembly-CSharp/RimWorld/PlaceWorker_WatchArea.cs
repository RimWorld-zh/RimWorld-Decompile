using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C79 RID: 3193
	public class PlaceWorker_WatchArea : PlaceWorker
	{
		// Token: 0x060045F7 RID: 17911 RVA: 0x0024E6A0 File Offset: 0x0024CAA0
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
			Map currentMap = Find.CurrentMap;
			GenDraw.DrawFieldEdges(WatchBuildingUtility.CalculateWatchCells(def, center, rot, currentMap).ToList<IntVec3>());
		}
	}
}
