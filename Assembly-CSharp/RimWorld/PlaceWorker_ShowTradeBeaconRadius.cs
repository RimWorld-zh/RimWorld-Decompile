using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C79 RID: 3193
	public class PlaceWorker_ShowTradeBeaconRadius : PlaceWorker
	{
		// Token: 0x060045EB RID: 17899 RVA: 0x0024CF14 File Offset: 0x0024B314
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
			Map currentMap = Find.CurrentMap;
			GenDraw.DrawFieldEdges(Building_OrbitalTradeBeacon.TradeableCellsAround(center, currentMap));
		}
	}
}
