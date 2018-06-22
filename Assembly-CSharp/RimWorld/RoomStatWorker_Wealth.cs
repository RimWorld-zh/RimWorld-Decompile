using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000440 RID: 1088
	public class RoomStatWorker_Wealth : RoomStatWorker
	{
		// Token: 0x060012E3 RID: 4835 RVA: 0x000A32CC File Offset: 0x000A16CC
		public override float GetScore(Room room)
		{
			float num = 0f;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Plant)
				{
					num += (float)thing.stackCount * thing.MarketValue;
				}
			}
			foreach (IntVec3 c in room.Cells)
			{
				num += c.GetTerrain(room.Map).GetStatValueAbstract(StatDefOf.MarketValue, null);
			}
			return num;
		}
	}
}
