using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F30 RID: 3888
	public static class GenAdjFast
	{
		// Token: 0x06005D78 RID: 23928 RVA: 0x002F6990 File Offset: 0x002F4D90
		public static List<IntVec3> AdjacentCells8Way(LocalTargetInfo pack)
		{
			List<IntVec3> result;
			if (pack.HasThing)
			{
				result = GenAdjFast.AdjacentCells8Way((Thing)pack);
			}
			else
			{
				result = GenAdjFast.AdjacentCells8Way((IntVec3)pack);
			}
			return result;
		}

		// Token: 0x06005D79 RID: 23929 RVA: 0x002F69D0 File Offset: 0x002F4DD0
		public static List<IntVec3> AdjacentCells8Way(IntVec3 root)
		{
			if (GenAdjFast.working)
			{
				throw new InvalidOperationException("GenAdjFast is already working.");
			}
			GenAdjFast.resultList.Clear();
			GenAdjFast.working = true;
			for (int i = 0; i < 8; i++)
			{
				GenAdjFast.resultList.Add(root + GenAdj.AdjacentCells[i]);
			}
			GenAdjFast.working = false;
			return GenAdjFast.resultList;
		}

		// Token: 0x06005D7A RID: 23930 RVA: 0x002F6A4C File Offset: 0x002F4E4C
		private static List<IntVec3> AdjacentCells8Way(Thing t)
		{
			return GenAdjFast.AdjacentCells8Way(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x06005D7B RID: 23931 RVA: 0x002F6A80 File Offset: 0x002F4E80
		public static List<IntVec3> AdjacentCells8Way(IntVec3 thingCenter, Rot4 thingRot, IntVec2 thingSize)
		{
			List<IntVec3> result;
			if (thingSize.x == 1 && thingSize.z == 1)
			{
				result = GenAdjFast.AdjacentCells8Way(thingCenter);
			}
			else
			{
				if (GenAdjFast.working)
				{
					throw new InvalidOperationException("GenAdjFast is already working.");
				}
				GenAdjFast.resultList.Clear();
				GenAdjFast.working = true;
				GenAdj.AdjustForRotation(ref thingCenter, ref thingSize, thingRot);
				int num = thingCenter.x - (thingSize.x - 1) / 2 - 1;
				int num2 = num + thingSize.x + 1;
				int num3 = thingCenter.z - (thingSize.z - 1) / 2 - 1;
				int num4 = num3 + thingSize.z + 1;
				IntVec3 item = new IntVec3(num - 1, 0, num3);
				do
				{
					item.x++;
					GenAdjFast.resultList.Add(item);
				}
				while (item.x < num2);
				do
				{
					item.z++;
					GenAdjFast.resultList.Add(item);
				}
				while (item.z < num4);
				do
				{
					item.x--;
					GenAdjFast.resultList.Add(item);
				}
				while (item.x > num);
				do
				{
					item.z--;
					GenAdjFast.resultList.Add(item);
				}
				while (item.z > num3 + 1);
				GenAdjFast.working = false;
				result = GenAdjFast.resultList;
			}
			return result;
		}

		// Token: 0x06005D7C RID: 23932 RVA: 0x002F6BF4 File Offset: 0x002F4FF4
		public static List<IntVec3> AdjacentCellsCardinal(LocalTargetInfo pack)
		{
			List<IntVec3> result;
			if (pack.HasThing)
			{
				result = GenAdjFast.AdjacentCellsCardinal((Thing)pack);
			}
			else
			{
				result = GenAdjFast.AdjacentCellsCardinal((IntVec3)pack);
			}
			return result;
		}

		// Token: 0x06005D7D RID: 23933 RVA: 0x002F6C34 File Offset: 0x002F5034
		public static List<IntVec3> AdjacentCellsCardinal(IntVec3 root)
		{
			if (GenAdjFast.working)
			{
				throw new InvalidOperationException("GenAdjFast is already working.");
			}
			GenAdjFast.resultList.Clear();
			GenAdjFast.working = true;
			for (int i = 0; i < 4; i++)
			{
				GenAdjFast.resultList.Add(root + GenAdj.CardinalDirections[i]);
			}
			GenAdjFast.working = false;
			return GenAdjFast.resultList;
		}

		// Token: 0x06005D7E RID: 23934 RVA: 0x002F6CB0 File Offset: 0x002F50B0
		private static List<IntVec3> AdjacentCellsCardinal(Thing t)
		{
			return GenAdjFast.AdjacentCellsCardinal(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x06005D7F RID: 23935 RVA: 0x002F6CE4 File Offset: 0x002F50E4
		public static List<IntVec3> AdjacentCellsCardinal(IntVec3 thingCenter, Rot4 thingRot, IntVec2 thingSize)
		{
			List<IntVec3> result;
			if (thingSize.x == 1 && thingSize.z == 1)
			{
				result = GenAdjFast.AdjacentCellsCardinal(thingCenter);
			}
			else
			{
				if (GenAdjFast.working)
				{
					throw new InvalidOperationException("GenAdjFast is already working.");
				}
				GenAdjFast.resultList.Clear();
				GenAdjFast.working = true;
				GenAdj.AdjustForRotation(ref thingCenter, ref thingSize, thingRot);
				int num = thingCenter.x - (thingSize.x - 1) / 2 - 1;
				int num2 = num + thingSize.x + 1;
				int num3 = thingCenter.z - (thingSize.z - 1) / 2 - 1;
				int num4 = num3 + thingSize.z + 1;
				IntVec3 item = new IntVec3(num, 0, num3);
				do
				{
					item.x++;
					GenAdjFast.resultList.Add(item);
				}
				while (item.x < num2 - 1);
				item.x++;
				do
				{
					item.z++;
					GenAdjFast.resultList.Add(item);
				}
				while (item.z < num4 - 1);
				item.z++;
				do
				{
					item.x--;
					GenAdjFast.resultList.Add(item);
				}
				while (item.x > num + 1);
				item.x--;
				do
				{
					item.z--;
					GenAdjFast.resultList.Add(item);
				}
				while (item.z > num3 + 1);
				GenAdjFast.working = false;
				result = GenAdjFast.resultList;
			}
			return result;
		}

		// Token: 0x06005D80 RID: 23936 RVA: 0x002F6E88 File Offset: 0x002F5288
		public static void AdjacentThings8Way(Thing thing, List<Thing> outThings)
		{
			outThings.Clear();
			if (thing.Spawned)
			{
				Map map = thing.Map;
				List<IntVec3> list = GenAdjFast.AdjacentCells8Way(thing);
				for (int i = 0; i < list.Count; i++)
				{
					List<Thing> thingList = list[i].GetThingList(map);
					for (int j = 0; j < thingList.Count; j++)
					{
						if (!outThings.Contains(thingList[j]))
						{
							outThings.Add(thingList[j]);
						}
					}
				}
			}
		}

		// Token: 0x04003DB9 RID: 15801
		private static List<IntVec3> resultList = new List<IntVec3>();

		// Token: 0x04003DBA RID: 15802
		private static bool working = false;
	}
}
