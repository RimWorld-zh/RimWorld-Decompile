using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000433 RID: 1075
	public class RoomRoleWorker_Laboratory : RoomRoleWorker
	{
		// Token: 0x060012C7 RID: 4807 RVA: 0x000A27D8 File Offset: 0x000A0BD8
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
