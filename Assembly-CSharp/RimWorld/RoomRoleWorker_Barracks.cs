using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class RoomRoleWorker_Barracks : RoomRoleWorker
	{
		public override float GetScore(Room room)
		{
			int num = 0;
			int num2 = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			int num3 = 0;
			float result;
			while (true)
			{
				if (num3 < containedAndAdjacentThings.Count)
				{
					Thing thing = containedAndAdjacentThings[num3];
					Building_Bed building_Bed = thing as Building_Bed;
					if (building_Bed != null && building_Bed.def.building.bed_humanlike)
					{
						if (building_Bed.ForPrisoners)
						{
							result = 0f;
							break;
						}
						num++;
						if (!building_Bed.Medical)
						{
							num2++;
						}
					}
					num3++;
					continue;
				}
				result = (float)((num > 1) ? ((float)num2 * 100100.0) : 0.0);
				break;
			}
			return result;
		}
	}
}
