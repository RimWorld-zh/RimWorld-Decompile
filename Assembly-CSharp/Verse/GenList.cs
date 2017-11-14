using System;
using System.Collections.Generic;

namespace Verse
{
	public static class GenList
	{
		public static int CountAllowNull<T>(this IList<T> list)
		{
			return (list != null) ? ((ICollection<T>)list).Count : 0;
		}

		public static bool NullOrEmpty<T>(this IList<T> list)
		{
			return list == null || ((ICollection<T>)list).Count == 0;
		}

		public static List<T> ListFullCopy<T>(this List<T> source)
		{
			List<T> list = new List<T>(source.Count);
			for (int i = 0; i < source.Count; i++)
			{
				list.Add(source[i]);
			}
			return list;
		}

		public static List<T> ListFullCopyOrNull<T>(this List<T> source)
		{
			if (source == null)
			{
				return null;
			}
			return source.ListFullCopy();
		}

		public static void RemoveDuplicates<T>(this List<T> list) where T : class
		{
			if (list.Count > 1)
			{
				for (int num = list.Count - 1; num >= 0; num--)
				{
					int num2 = 0;
					while (num2 < num)
					{
						if ((object)list[num] != (object)list[num2])
						{
							num2++;
							continue;
						}
						list.RemoveAt(num);
						break;
					}
				}
			}
		}

		public static void Shuffle<T>(this IList<T> list)
		{
			int num = ((ICollection<T>)list).Count;
			while (num > 1)
			{
				num--;
				int index = Rand.RangeInclusive(0, num);
				T value = list[index];
				list[index] = list[num];
				list[num] = value;
			}
		}

		public static void InsertionSort<T>(this IList<T> list, Comparison<T> comparison)
		{
			int count = ((ICollection<T>)list).Count;
			for (int i = 1; i < count; i++)
			{
				T val = list[i];
				int num = i - 1;
				while (num >= 0 && comparison(list[num], val) > 0)
				{
					list[num + 1] = list[num];
					num--;
				}
				list[num + 1] = val;
			}
		}
	}
}
