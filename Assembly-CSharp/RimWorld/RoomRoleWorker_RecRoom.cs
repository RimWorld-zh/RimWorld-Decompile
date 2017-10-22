using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class RoomRoleWorker_RecRoom : RoomRoleWorker
	{
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing.def.category == ThingCategory.Building)
				{
					List<JoyGiverDef> allDefsListForReading = DefDatabase<JoyGiverDef>.AllDefsListForReading;
					int num2 = 0;
					while (num2 < allDefsListForReading.Count)
					{
						if (allDefsListForReading[num2].thingDefs == null || !allDefsListForReading[num2].thingDefs.Contains(thing.def))
						{
							num2++;
							continue;
						}
						num++;
						break;
					}
				}
			}
			return (float)((float)num * 5.0);
		}
	}
}
