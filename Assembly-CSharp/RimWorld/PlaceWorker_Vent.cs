using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PlaceWorker_Vent : PlaceWorker
	{
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot)
		{
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
			RoomGroup roomGroup = intVec2.GetRoomGroup(base.Map);
			RoomGroup roomGroup2 = intVec.GetRoomGroup(base.Map);
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

		public override AcceptanceReport AllowsPlacing(BuildableDef def, IntVec3 center, Rot4 rot, Thing thingToIgnore = null)
		{
			IntVec3 c = center + IntVec3.South.RotatedBy(rot);
			IntVec3 c2 = center + IntVec3.North.RotatedBy(rot);
			if (c.Impassable(base.Map) || c2.Impassable(base.Map))
			{
				return "MustPlaceVentWithFreeSpaces".Translate();
			}
			return true;
		}
	}
}
