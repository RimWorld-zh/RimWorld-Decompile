using System.Collections.Generic;
using UnityEngine;

namespace RimWorld.Planet
{
	public static class PackedListOfLists
	{
		private static List<int> vertAdjacentTrisCount = new List<int>();

		public static void AddList<T>(List<int> offsets, List<T> values, List<T> listToAdd)
		{
			offsets.Add(values.Count);
			values.AddRange((IEnumerable<T>)listToAdd);
		}

		public static void GetList<T>(List<int> offsets, List<T> values, int listIndex, List<T> outList)
		{
			outList.Clear();
			int num = offsets[listIndex];
			int num2 = values.Count;
			if (listIndex + 1 < offsets.Count)
			{
				num2 = offsets[listIndex + 1];
			}
			for (int num3 = num; num3 < num2; num3++)
			{
				outList.Add(values[num3]);
			}
		}

		public static void GetListValuesIndices<T>(List<int> offsets, List<T> values, int listIndex, List<int> outList)
		{
			outList.Clear();
			int num = offsets[listIndex];
			int num2 = values.Count;
			if (listIndex + 1 < offsets.Count)
			{
				num2 = offsets[listIndex + 1];
			}
			for (int num3 = num; num3 < num2; num3++)
			{
				outList.Add(num3);
			}
		}

		public static int GetListCount<T>(List<int> offsets, List<T> values, int listIndex)
		{
			int num = offsets[listIndex];
			int num2 = values.Count;
			if (listIndex + 1 < offsets.Count)
			{
				num2 = offsets[listIndex + 1];
			}
			return num2 - num;
		}

		public static void GenerateVertToTrisPackedList(List<Vector3> verts, List<TriangleIndices> tris, List<int> outOffsets, List<int> outValues)
		{
			outOffsets.Clear();
			outValues.Clear();
			PackedListOfLists.vertAdjacentTrisCount.Clear();
			int num = 0;
			int count = verts.Count;
			while (num < count)
			{
				PackedListOfLists.vertAdjacentTrisCount.Add(0);
				num++;
			}
			int num2 = 0;
			int count2 = tris.Count;
			while (num2 < count2)
			{
				TriangleIndices triangleIndices = tris[num2];
				int v;
				List<int> list;
				(list = PackedListOfLists.vertAdjacentTrisCount)[v = triangleIndices.v1] = list[v] + 1;
				int v2;
				(list = PackedListOfLists.vertAdjacentTrisCount)[v2 = triangleIndices.v2] = list[v2] + 1;
				int v3;
				(list = PackedListOfLists.vertAdjacentTrisCount)[v3 = triangleIndices.v3] = list[v3] + 1;
				num2++;
			}
			int num3 = 0;
			int num4 = 0;
			int count3 = verts.Count;
			while (num4 < count3)
			{
				outOffsets.Add(num3);
				int num5 = PackedListOfLists.vertAdjacentTrisCount[num4];
				PackedListOfLists.vertAdjacentTrisCount[num4] = 0;
				for (int num6 = 0; num6 < num5; num6++)
				{
					outValues.Add(-1);
				}
				num3 += num5;
				num4++;
			}
			int num7 = 0;
			int count4 = tris.Count;
			while (num7 < count4)
			{
				TriangleIndices triangleIndices2 = tris[num7];
				outValues[outOffsets[triangleIndices2.v1] + PackedListOfLists.vertAdjacentTrisCount[triangleIndices2.v1]] = num7;
				outValues[outOffsets[triangleIndices2.v2] + PackedListOfLists.vertAdjacentTrisCount[triangleIndices2.v2]] = num7;
				outValues[outOffsets[triangleIndices2.v3] + PackedListOfLists.vertAdjacentTrisCount[triangleIndices2.v3]] = num7;
				int v4;
				List<int> list;
				(list = PackedListOfLists.vertAdjacentTrisCount)[v4 = triangleIndices2.v1] = list[v4] + 1;
				int v5;
				(list = PackedListOfLists.vertAdjacentTrisCount)[v5 = triangleIndices2.v2] = list[v5] + 1;
				int v6;
				(list = PackedListOfLists.vertAdjacentTrisCount)[v6 = triangleIndices2.v3] = list[v6] + 1;
				num7++;
			}
		}
	}
}
