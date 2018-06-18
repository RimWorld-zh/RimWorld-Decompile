using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200042D RID: 1069
	public class RoomRoleWorker_Barn : RoomRoleWorker
	{
		// Token: 0x060012BB RID: 4795 RVA: 0x000A23B8 File Offset: 0x000A07B8
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				Building_Bed building_Bed = thing as Building_Bed;
				if (building_Bed != null && !building_Bed.def.building.bed_humanlike)
				{
					num++;
				}
			}
			return (float)num * 7.6f;
		}
	}
}
