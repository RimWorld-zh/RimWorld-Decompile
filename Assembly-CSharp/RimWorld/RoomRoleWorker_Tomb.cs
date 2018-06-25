using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200043B RID: 1083
	public class RoomRoleWorker_Tomb : RoomRoleWorker
	{
		// Token: 0x060012D6 RID: 4822 RVA: 0x000A3020 File Offset: 0x000A1420
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
			return 50f * (float)num;
		}
	}
}
