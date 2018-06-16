using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000442 RID: 1090
	public class RoomStatWorker_Space : RoomStatWorker
	{
		// Token: 0x060012E8 RID: 4840 RVA: 0x000A3198 File Offset: 0x000A1598
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
