using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000431 RID: 1073
	public class RoomRoleWorker_Hospital : RoomRoleWorker
	{
		// Token: 0x060012C3 RID: 4803 RVA: 0x000A280C File Offset: 0x000A0C0C
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				Building_Bed building_Bed = thing as Building_Bed;
				if (building_Bed != null && building_Bed.def.building.bed_humanlike)
				{
					if (building_Bed.ForPrisoners)
					{
						return 0f;
					}
					if (building_Bed.Medical)
					{
						num++;
					}
				}
			}
			return (float)num * 100000f;
		}
	}
}
