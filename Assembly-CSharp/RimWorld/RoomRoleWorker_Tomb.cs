using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class RoomRoleWorker_Tomb : RoomRoleWorker
	{
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i] is Building_Sarcophagus)
				{
					num++;
				}
			}
			return (float)(50.0 * (float)num);
		}
	}
}
