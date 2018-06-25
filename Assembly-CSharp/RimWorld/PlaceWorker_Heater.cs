using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C6E RID: 3182
	public class PlaceWorker_Heater : PlaceWorker
	{
		// Token: 0x060045DF RID: 17887 RVA: 0x0024DD40 File Offset: 0x0024C140
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
