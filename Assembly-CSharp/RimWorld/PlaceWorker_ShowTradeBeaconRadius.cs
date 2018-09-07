using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PlaceWorker_ShowTradeBeaconRadius : PlaceWorker
	{
		public PlaceWorker_ShowTradeBeaconRadius()
		{
		}

		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
			Map currentMap = Find.CurrentMap;
			GenDraw.DrawFieldEdges(Building_OrbitalTradeBeacon.TradeableCellsAround(center, currentMap));
		}
	}
}
