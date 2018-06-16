using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C74 RID: 3188
	public class PlaceWorker_CoolerSimple : PlaceWorker
	{
		// Token: 0x060045DE RID: 17886 RVA: 0x0024CA6C File Offset: 0x0024AE6C
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
			Map currentMap = Find.CurrentMap;
			RoomGroup roomGroup = center.GetRoomGroup(currentMap);
			if (roomGroup != null && !roomGroup.UsesOutdoorTemperature)
			{
				GenDraw.DrawFieldEdges(roomGroup.Cells.ToList<IntVec3>(), GenTemperature.ColorRoomCold);
			}
		}
	}
}
