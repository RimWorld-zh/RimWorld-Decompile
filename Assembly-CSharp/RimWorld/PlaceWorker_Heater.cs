using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6C RID: 3180
	public class PlaceWorker_Heater : PlaceWorker
	{
		// Token: 0x060045DC RID: 17884 RVA: 0x0024DC64 File Offset: 0x0024C064
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
			Map currentMap = Find.CurrentMap;
			RoomGroup roomGroup = center.GetRoomGroup(currentMap);
			if (roomGroup != null && !roomGroup.UsesOutdoorTemperature)
			{
				GenDraw.DrawFieldEdges(roomGroup.Cells.ToList<IntVec3>(), GenTemperature.ColorRoomHot);
			}
		}
	}
}
