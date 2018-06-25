using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F34 RID: 3892
	public static class GenAdjFast
	{
		// Token: 0x04003DD5 RID: 15829
		private static List<IntVec3> resultList = new List<IntVec3>();

		// Token: 0x04003DD6 RID: 15830
		private static bool working = false;

		// Token: 0x06005DA8 RID: 23976 RVA: 0x002F9338 File Offset: 0x002F7738
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

		// Token: 0x06005DA9 RID: 23977 RVA: 0x002F9378 File Offset: 0x002F7778
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

		// Token: 0x06005DAA RID: 23978 RVA: 0x002F93F4 File Offset: 0x002F77F4
		private static List<IntVec3> AdjacentCells8Way(Thing t)
		{
			return GenAdjFast.AdjacentCells8Way(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x06005DAB RID: 23979 RVA: 0x002F9428 File Offset: 0x002F7828
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

		// Token: 0x06005DAC RID: 23980 RVA: 0x002F959C File Offset: 0x002F799C
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

		// Token: 0x06005DAD RID: 23981 RVA: 0x002F95DC File Offset: 0x002F79DC
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

		// Token: 0x06005DAE RID: 23982 RVA: 0x002F9658 File Offset: 0x002F7A58
		private static List<IntVec3> AdjacentCellsCardinal(Thing t)
		{
			return GenAdjFast.AdjacentCellsCardinal(t.Position, t.Rotation, t.def.size);
		}

		// Token: 0x06005DAF RID: 23983 RVA: 0x002F968C File Offset: 0x002F7A8C
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

		// Token: 0x06005DB0 RID: 23984 RVA: 0x002F9830 File Offset: 0x002F7C30
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
