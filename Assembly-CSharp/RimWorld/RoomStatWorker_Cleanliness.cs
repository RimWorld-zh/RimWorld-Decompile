using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200043C RID: 1084
	public class RoomStatWorker_Cleanliness : RoomStatWorker
	{
		// Token: 0x060012DA RID: 4826 RVA: 0x000A2D74 File Offset: 0x000A1174
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
