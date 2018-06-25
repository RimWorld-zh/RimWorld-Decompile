using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F1D RID: 3869
	public static class Dijkstra<T>
	{
		// Token: 0x04003D92 RID: 15762
		private static Dictionary<T, float> distances = new Dictionary<T, float>();

		// Token: 0x04003D93 RID: 15763
		private static FastPriorityQueue<KeyValuePair<T, float>> queue = new FastPriorityQueue<KeyValuePair<T, float>>(new Dijkstra<T>.DistanceComparer());

		// Token: 0x04003D94 RID: 15764
		private static List<T> singleNodeList = new List<T>();

		// Token: 0x04003D95 RID: 15765
		private static List<KeyValuePair<T, float>> tmpResult = new List<KeyValuePair<T, float>>();

		// Token: 0x06005CCA RID: 23754 RVA: 0x002F1434 File Offset: 0x002EF834
		public static void Run(T startingNode, Func<T, IEnumerable<T>> neighborsGetter, Func<T, T, float> distanceGetter, List<KeyValuePair<T, float>> outDistances, Dictionary<T, T> outParents = null)
		{
			Dijkstra<T>.singleNodeList.Clear();
			Dijkstra<T>.singleNodeList.Add(startingNode);
			Dijkstra<T>.Run(Dijkstra<T>.singleNodeList, neighborsGetter, distanceGetter, outDistances, outParents);
		}

		// Token: 0x06005CCB RID: 23755 RVA: 0x002F145C File Offset: 0x002EF85C
		public static void Run(IEnumerable<T> startingNodes, Func<T, IEnumerable<T>> neighborsGetter, Func<T, T, float> distanceGetter, List<KeyValuePair<T, float>> outDistances, Dictionary<T, T> outParents = null)
		{
			outDistances.Clear();
			Dijkstra<T>.distances.Clear();
			Dijkstra<T>.queue.Clear();
			if (outParents != null)
			{
				outParents.Clear();
			}
			IList<T> list = startingNodes as IList<T>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					T key = list[i];
					if (!Dijkstra<T>.distances.ContainsKey(key))
					{
						Dijkstra<T>.distances.Add(key, 0f);
						Dijkstra<T>.queue.Push(new KeyValuePair<T, float>(key, 0f));
					}
				}
			}
			else
			{
				foreach (T key2 in startingNodes)
				{
					if (!Dijkstra<T>.distances.ContainsKey(key2))
					{
						Dijkstra<T>.distances.Add(key2, 0f);
						Dijkstra<T>.queue.Push(new KeyValuePair<T, float>(key2, 0f));
					}
				}
			}
			while (Dijkstra<T>.queue.Count != 0)
			{
				KeyValuePair<T, float> node = Dijkstra<T>.queue.Pop();
				float num = Dijkstra<T>.distances[node.Key];
				if (node.Value == num)
				{
					IEnumerable<T> enumerable = neighborsGetter(node.Key);
					if (enumerable != null)
					{
						IList<T> list2 = enumerable as IList<T>;
						if (list2 != null)
						{
							for (int j = 0; j < list2.Count; j++)
							{
								Dijkstra<T>.HandleNeighbor(list2[j], num, node, distanceGetter, outParents);
							}
						}
						else
						{
							foreach (T n in enumerable)
							{
								Dijkstra<T>.HandleNeighbor(n, num, node, distanceGetter, outParents);
							}
						}
					}
				}
			}
			foreach (KeyValuePair<T, float> item in Dijkstra<T>.distances)
			{
				outDistances.Add(item);
			}
			Dijkstra<T>.distances.Clear();
		}

		// Token: 0x06005CCC RID: 23756 RVA: 0x002F16E4 File Offset: 0x002EFAE4
		public static void Run(T startingNode, Func<T, IEnumerable<T>> neighborsGetter, Func<T, T, float> distanceGetter, Dictionary<T, float> outDistances, Dictionary<T, T> outParents = null)
		{
			Dijkstra<T>.singleNodeList.Clear();
			Dijkstra<T>.singleNodeList.Add(startingNode);
			Dijkstra<T>.Run(Dijkstra<T>.singleNodeList, neighborsGetter, distanceGetter, outDistances, outParents);
		}

		// Token: 0x06005CCD RID: 23757 RVA: 0x002F170C File Offset: 0x002EFB0C
		public static void Run(IEnumerable<T> startingNodes, Func<T, IEnumerable<T>> neighborsGetter, Func<T, T, float> distanceGetter, Dictionary<T, float> outDistances, Dictionary<T, T> outParents = null)
		{
			Dijkstra<T>.Run(startingNodes, neighborsGetter, distanceGetter, Dijkstra<T>.tmpResult, outParents);
			outDistances.Clear();
			for (int i = 0; i < Dijkstra<T>.tmpResult.Count; i++)
			{
				outDistances.Add(Dijkstra<T>.tmpResult[i].Key, Dijkstra<T>.tmpResult[i].Value);
			}
			Dijkstra<T>.tmpResult.Clear();
		}

		// Token: 0x06005CCE RID: 23758 RVA: 0x002F1784 File Offset: 0x002EFB84
		private static void HandleNeighbor(T n, float nodeDist, KeyValuePair<T, float> node, Func<T, T, float> distanceGetter, Dictionary<T, T> outParents)
		{
			float num = nodeDist + Mathf.Max(distanceGetter(node.Key, n), 0f);
			bool flag = false;
			float num2;
			if (Dijkstra<T>.distances.TryGetValue(n, out num2))
			{
				if (num < num2)
				{
					Dijkstra<T>.distances[n] = num;
					flag = true;
				}
			}
			else
			{
				Dijkstra<T>.distances.Add(n, num);
				flag = true;
			}
			if (flag)
			{
				Dijkstra<T>.queue.Push(new KeyValuePair<T, float>(n, num));
				if (outParents != null)
				{
					if (outParents.ContainsKey(n))
					{
						outParents[n] = node.Key;
					}
					else
					{
						outParents.Add(n, node.Key);
					}
				}
			}
		}

		// Token: 0x02000F1E RID: 3870
		private class DistanceComparer : IComparer<KeyValuePair<T, float>>
		{
			// Token: 0x06005CD1 RID: 23761 RVA: 0x002F1878 File Offset: 0x002EFC78
			public int Compare(KeyValuePair<T, float> a, KeyValuePair<T, float> b)
			{
				return a.Value.CompareTo(b.Value);
			}
		}
	}
}
