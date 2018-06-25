using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200043D RID: 1085
	public class RoomStatWorker_Beauty : RoomStatWorker
	{
		// Token: 0x04000B74 RID: 2932
		private static readonly SimpleCurve CellCountCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 20f),
				true
			},
			{
				new CurvePoint(40f, 40f),
				true
			},
			{
				new CurvePoint(100000f, 100000f),
				true
			}
		};

		// Token: 0x04000B75 RID: 2933
		private static List<Thing> countedThings = new List<Thing>();

		// Token: 0x04000B76 RID: 2934
		private static List<IntVec3> countedAdjCells = new List<IntVec3>();

		// Token: 0x060012DA RID: 4826 RVA: 0x000A30FC File Offset: 0x000A14FC
		public override float GetScore(Room room)
		{
			float num = 0f;
			int num2 = 0;
			RoomStatWorker_Beauty.countedThings.Clear();
			foreach (IntVec3 c in room.Cells)
			{
				num += BeautyUtility.CellBeauty(c, room.Map, RoomStatWorker_Beauty.countedThings);
				num2++;
			}
			RoomStatWorker_Beauty.countedAdjCells.Clear();
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Thing thing = containedAndAdjacentThings[i];
				if (thing.GetRoom(RegionType.Set_Passable) != room && !RoomStatWorker_Beauty.countedAdjCells.Contains(thing.Position))
				{
					num += BeautyUtility.CellBeauty(thing.Position, room.Map, RoomStatWorker_Beauty.countedThings);
					RoomStatWorker_Beauty.countedAdjCells.Add(thing.Position);
				}
			}
			RoomStatWorker_Beauty.countedThings.Clear();
			float result;
			if (num2 == 0)
			{
				result = 0f;
			}
			else
			{
				result = num / RoomStatWorker_Beauty.CellCountCurve.Evaluate((float)num2);
			}
			return result;
		}
	}
}
