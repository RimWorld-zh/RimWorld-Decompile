using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class RoomStatWorker_Space : RoomStatWorker
	{
		public RoomStatWorker_Space()
		{
		}

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
				foreach (IntVec3 c in room.Cells)
				{
					if (c.Standable(room.Map))
					{
						num += 1.4f;
					}
					else if (c.Walkable(room.Map))
					{
						num += 0.5f;
					}
				}
				result = Mathf.Min(num, 350f);
			}
			return result;
		}
	}
}
