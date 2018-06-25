using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200043E RID: 1086
	public class RoomStatWorker_Cleanliness : RoomStatWorker
	{
		// Token: 0x060012DE RID: 4830 RVA: 0x000A30B4 File Offset: 0x000A14B4
		public override float GetScore(Room room)
		{
			float num = 0f;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing.def.category == ThingCategory.Building || thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Filth || thing.def.category == ThingCategory.Plant)
				{
					num += (float)thing.stackCount * thing.GetStatValue(StatDefOf.Cleanliness, true);
				}
			}
			foreach (IntVec3 c in room.Cells)
			{
				num += c.GetTerrain(room.Map).GetStatValueAbstract(StatDefOf.Cleanliness, null);
			}
			return num / (float)room.CellCount;
		}
	}
}
