using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000430 RID: 1072
	public class RoomRoleWorker_DiningRoom : RoomRoleWorker
	{
		// Token: 0x060012C1 RID: 4801 RVA: 0x000A25B0 File Offset: 0x000A09B0
		public override float GetScore(Room room)
		{
			int num = 0;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing.def.category == ThingCategory.Building && thing.def.surfaceType == SurfaceType.Eat)
				{
					num++;
				}
			}
			return (float)num * 8f;
		}
	}
}
