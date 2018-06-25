using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200042F RID: 1071
	public class RoomRoleWorker_Barn : RoomRoleWorker
	{
		// Token: 0x060012BF RID: 4799 RVA: 0x000A26EC File Offset: 0x000A0AEC
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
