using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C74 RID: 3188
	public class PlaceWorker_Vent : PlaceWorker
	{
		// Token: 0x060045ED RID: 17901 RVA: 0x0024E0CC File Offset: 0x0024C4CC
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol)
		{
			Map currentMap = Find.CurrentMap;
			IntVec3 intVec = center + IntVec3.South.RotatedBy(rot);
			IntVec3 intVec2 = center + IntVec3.North.RotatedBy(rot);
			GenDraw.DrawFieldEdges(new List<IntVec3>
			{
				intVec
			}, Color.white);
			GenDraw.DrawFieldEdges(new List<IntVec3>
			{
				intVec2
			}, Color.white);
			RoomGroup roomGroup = intVec2.GetRoomGroup(currentMap);
			RoomGroup roomGroup2 = intVec.GetRoomGroup(currentMap);
			if (roomGroup != null && roomGroup2 != null)
			{
				if (roomGroup == roomGroup2 && !roomGroup.UsesOutdoorTemperature)
				{
					GenDraw.DrawFieldEdges(roomGroup.Cells.ToList<IntVec3>(), Color.white);
				}
				else
				{
					if (!roomGroup.UsesOutdoorTemperature)
					{
						GenDraw.DrawFieldEdges(roomGroup.Cells.ToList<IntVec3>(), Color.white);
					}
					if (!roomGroup2.UsesOutdoorTemperature)
					{
						GenDraw.DrawFieldEdges(roomGroup2.Cells.ToList<IntVec3>(), Color.white);
					}
				}
			}
		}

		// Token: 0x060045EE RID: 17902 RVA: 0x0024E1CC File Offset: 0x0024C5CC
		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			IntVec3 c = center + IntVec3.South.RotatedBy(rot);
			IntVec3 c2 = center + IntVec3.North.RotatedBy(rot);
			AcceptanceReport result;
			if (c.Impassable(map) || c2.Impassable(map))
			{
				result = "MustPlaceVentWithFreeSpaces".Translate();
			}
			else
			{
				result = true;
			}
			return result;
		}
	}
}
