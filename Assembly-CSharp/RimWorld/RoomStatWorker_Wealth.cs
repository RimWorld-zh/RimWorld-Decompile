using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000444 RID: 1092
	public class RoomStatWorker_Wealth : RoomStatWorker
	{
		// Token: 0x060012EC RID: 4844 RVA: 0x000A32B0 File Offset: 0x000A16B0
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
