using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000435 RID: 1077
	public class RoomRoleWorker_Laboratory : RoomRoleWorker
	{
		// Token: 0x060012CB RID: 4811 RVA: 0x000A2B0C File Offset: 0x000A0F0C
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing is Building_ResearchBench)
				{
					num++;
				}
			}
			return 30f * (float)num;
		}
	}
}
