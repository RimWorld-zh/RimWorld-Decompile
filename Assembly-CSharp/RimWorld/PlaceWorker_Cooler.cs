using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PlaceWorker_Cooler : PlaceWorker
	{
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot)
		{
			IntVec3 intVec = center + IntVec3.South.RotatedBy(rot);
			IntVec3 intVec2 = center + IntVec3.North.RotatedBy(rot);
			List<IntVec3> list = new List<IntVec3>();
			list.Add(intVec);
			GenDraw.DrawFieldEdges(list, GenTemperature.ColorSpotCold);
			list = new List<IntVec3>();
			list.Add(intVec2);
			GenDraw.DrawFieldEdges(list, GenTemperature.ColorSpotHot);
			RoomGroup roomGroup = intVec2.GetRoomGroup(base.Map);
			RoomGroup roomGroup2 = intVec.GetRoomGroup(base.Map);
			if (roomGroup != null && roomGroup2 != null)
			{
				if (roomGroup == roomGroup2 && !roomGroup.UsesOutdoorTemperature)
				{
					GenDraw.DrawFieldEdges(roomGroup.Cells.ToList(), new Color(1f, 0.7f, 0f, 0.5f));
				}
				else
				{
					if (!roomGroup.UsesOutdoorTemperature)
					{
						GenDraw.DrawFieldEdges(roomGroup.Cells.ToList(), GenTemperature.ColorRoomHot);
					}
					if (!roomGroup2.UsesOutdoorTemperature)
					{
						GenDraw.DrawFieldEdges(roomGroup2.Cells.ToList(), GenTemperature.ColorRoomCold);
					}
				}
			}
		}

		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Thing thingToIgnore = null)
		{
			IntVec3 c = center + IntVec3.South.RotatedBy(rot);
			IntVec3 c2 = center + IntVec3.North.RotatedBy(rot);
			if (!c.Impassable(base.Map) && !c2.Impassable(base.Map))
			{
				return true;
			}
			return "MustPlaceCoolerWithFreeSpaces".Translate();
		}
	}
}
