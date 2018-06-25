using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000437 RID: 1079
	public class RoomRoleWorker_PrisonBarracks : RoomRoleWorker
	{
		// Token: 0x060012CF RID: 4815 RVA: 0x000A2B90 File Offset: 0x000A0F90
		public override float GetScore(Room room)
		{
			int num = 0;
			int num2 = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				Building_Bed building_Bed = thing as Building_Bed;
				if (building_Bed != null && building_Bed.def.building.bed_humanlike)
				{
					if (!building_Bed.ForPrisoners)
					{
						return 0f;
					}
					if (building_Bed.Medical)
					{
						num++;
					}
					else
					{
						num2++;
					}
				}
			}
			if (num2 + num <= 1)
			{
				return 0f;
			}
			return (float)num2 * 100100f + (float)num * 50001f;
		}
	}
}
