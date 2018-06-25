using System;
using System.Collections.Generic;

namespace Verse
{
	public static class GenAdjFast
	{
		private static List<IntVec3> resultList = new List<IntVec3>();

		private static bool working = false;

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

		private static List<IntVec3> AdjacentCells8Way(Thing t)
		{
			return GenAdjFast.AdjacentCells8Way(t.Position, t.Rotation, t.def.size);
		}

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

		private static List<IntVec3> AdjacentCellsCardinal(Thing t)
		{
			return GenAdjFast.AdjacentCellsCardinal(t.Position, t.Rotation, t.def.size);
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static GenAdjFast()
		{
		}
	}
}
