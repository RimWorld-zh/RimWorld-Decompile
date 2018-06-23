using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F2F RID: 3887
	public static class GenAdjFast
	{
		// Token: 0x04003DCA RID: 15818
		private static List<IntVec3> resultList = new List<IntVec3>();

		// Token: 0x04003DCB RID: 15819
		private static bool working = false;

		// Token: 0x06005D9E RID: 23966 RVA: 0x002F8A98 File Offset: 0x002F6E98
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

		// Token: 0x06005D9F RID: 23967 RVA: 0x002F8AD8 File Offset: 0x002F6ED8
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

		// Token: 0x06005DA0 RID: 23968 RVA: 0x002F8B54 File Offset: 0x002F6F54
		private static List<IntVec3> AdjacentCells8Way(Thing t)
		{
			return GenAdjFast.AdjacentCells8Way(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x06005DA1 RID: 23969 RVA: 0x002F8B88 File Offset: 0x002F6F88
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

		// Token: 0x06005DA2 RID: 23970 RVA: 0x002F8CFC File Offset: 0x002F70FC
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

		// Token: 0x06005DA3 RID: 23971 RVA: 0x002F8D3C File Offset: 0x002F713C
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

		// Token: 0x06005DA4 RID: 23972 RVA: 0x002F8DB8 File Offset: 0x002F71B8
		private static List<IntVec3> AdjacentCellsCardinal(Thing t)
		{
			return GenAdjFast.AdjacentCellsCardinal(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x06005DA5 RID: 23973 RVA: 0x002F8DEC File Offset: 0x002F71EC
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

		// Token: 0x06005DA6 RID: 23974 RVA: 0x002F8F90 File Offset: 0x002F7390
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
	}
}
