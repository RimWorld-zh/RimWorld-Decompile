using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C75 RID: 3189
	public class PlaceWorker_ShowTradeBeaconRadius : PlaceWorker
	{
		// Token: 0x060045F2 RID: 17906 RVA: 0x0024E2BC File Offset: 0x0024C6BC
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
			Map currentMap = Find.CurrentMap;
			GenDraw.DrawFieldEdges(Building_OrbitalTradeBeacon.TradeableCellsAround(center, currentMap));
		}
	}
}
