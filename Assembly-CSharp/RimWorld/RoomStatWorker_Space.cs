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
			if (room.PsychologicallyOutdoors)
			{
				return 350f;
			}
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
			return Mathf.Min(num, 350f);
		}
	}
}
