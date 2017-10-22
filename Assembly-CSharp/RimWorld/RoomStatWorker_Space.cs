using UnityEngine;
using Verse;

namespace RimWorld
{
	public class RoomStatWorker_Space : RoomStatWorker
	{
		public override float GetScore(Room room)
		{
			if (room.PsychologicallyOutdoors)
			{
				return 350f;
			}
			float num = 0f;
			foreach (IntVec3 cell in room.Cells)
			{
				if (cell.Standable(room.Map))
				{
					num = (float)(num + 1.3999999761581421);
				}
				else if (cell.Walkable(room.Map))
				{
					num = (float)(num + 0.5);
				}
			}
			return Mathf.Min(num, 350f);
		}
	}
}
