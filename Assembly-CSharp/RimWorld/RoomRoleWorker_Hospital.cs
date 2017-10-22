using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class RoomRoleWorker_Hospital : RoomRoleWorker
	{
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			int num2 = 0;
			float result;
			while (true)
			{
				if (num2 < containedAndAdjacentThings.Count)
				{
					Thing thing = containedAndAdjacentThings[num2];
					Building_Bed building_Bed = thing as Building_Bed;
					if (building_Bed != null && building_Bed.def.building.bed_humanlike)
					{
						if (building_Bed.ForPrisoners)
						{
							result = 0f;
							break;
						}
						if (building_Bed.Medical)
						{
							num++;
						}
					}
					num2++;
					continue;
				}
				result = (float)((float)num * 100000.0);
				break;
			}
			return result;
		}
	}
}
