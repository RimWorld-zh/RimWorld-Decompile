using UnityEngine;
using Verse;

namespace RimWorld
{
	public class RoomStatWorker_Space : RoomStatWorker
	{
		public override float GetScore(Room room)
		{
			float result;
			if (room.PsychologicallyOutdoors)
			{
				result = 350f;
			}
			else
			{
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
				result = Mathf.Min(num, 350f);
			}
			return result;
		}
	}
}
