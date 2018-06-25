using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C77 RID: 3191
	public class PlaceWorker_ShowTradeBeaconRadius : PlaceWorker
	{
		// Token: 0x060045F5 RID: 17909 RVA: 0x0024E398 File Offset: 0x0024C798
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
			Map currentMap = Find.CurrentMap;
			GenDraw.DrawFieldEdges(Building_OrbitalTradeBeacon.TradeableCellsAround(center, currentMap));
		}
	}
}
