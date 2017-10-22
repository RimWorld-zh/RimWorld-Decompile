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
			int num = values.Count;
			if (listIndex + 1 < offsets.Count)
			{
				num = offsets[listIndex + 1];
			}
			for (int num2 = offsets[listIndex]; num2 != num; num2++)
			{
				outList.Add(values[num2]);
			}
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
				List<int> list;
				List<int> obj = list = PackedListOfLists.vertAdjacentTrisCount;
				int v;
				int index = v = triangleIndices.v1;
				v = list[v];
				obj[index] = v + 1;
				List<int> list2;
				List<int> obj2 = list2 = PackedListOfLists.vertAdjacentTrisCount;
				int index2 = v = triangleIndices.v2;
				v = list2[v];
				obj2[index2] = v + 1;
				List<int> list3;
				List<int> obj3 = list3 = PackedListOfLists.vertAdjacentTrisCount;
				int index3 = v = triangleIndices.v3;
				v = list3[v];
				obj3[index3] = v + 1;
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
				List<int> list4;
				List<int> obj4 = list4 = PackedListOfLists.vertAdjacentTrisCount;
				int v;
				int index4 = v = triangleIndices2.v1;
				v = list4[v];
				obj4[index4] = v + 1;
				List<int> list5;
				List<int> obj5 = list5 = PackedListOfLists.vertAdjacentTrisCount;
				int index5 = v = triangleIndices2.v2;
				v = list5[v];
				obj5[index5] = v + 1;
				List<int> list6;
				List<int> obj6 = list6 = PackedListOfLists.vertAdjacentTrisCount;
				int index6 = v = triangleIndices2.v3;
				v = list6[v];
				obj6[index6] = v + 1;
				num7++;
			}
		}
	}
}
