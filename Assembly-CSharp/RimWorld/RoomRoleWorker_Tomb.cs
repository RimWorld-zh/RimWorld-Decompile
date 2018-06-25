using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200043B RID: 1083
	public class RoomRoleWorker_Tomb : RoomRoleWorker
	{
		// Token: 0x060012D7 RID: 4823 RVA: 0x000A2E20 File Offset: 0x000A1220
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
