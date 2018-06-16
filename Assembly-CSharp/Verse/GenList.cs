using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F42 RID: 3906
	public static class GenList
	{
		// Token: 0x06005E35 RID: 24117 RVA: 0x002FD900 File Offset: 0x002FBD00
		public static int CountAllowNull<T>(this IList<T> list)
		{
			return (list == null) ? 0 : list.Count;
		}

		// Token: 0x06005E36 RID: 24118 RVA: 0x002FD928 File Offset: 0x002FBD28
		public static bool NullOrEmpty<T>(this IList<T> list)
		{
			return list == null || list.Count == 0;
		}

		// Token: 0x06005E37 RID: 24119 RVA: 0x002FD950 File Offset: 0x002FBD50
		public static List<T> ListFullCopy<T>(this List<T> source)
		{
			List<T> list = new List<T>(source.Count);
			for (int i = 0; i < source.Count; i++)
			{
				list.Add(source[i]);
			}
			return list;
		}

		// Token: 0x06005E38 RID: 24120 RVA: 0x002FD998 File Offset: 0x002FBD98
		public static List<T> ListFullCopyOrNull<T>(this List<T> source)
		{
			List<T> result;
			if (source == null)
			{
				result = null;
			}
			else
			{
				result = source.ListFullCopy<T>();
			}
			return result;
		}

		// Token: 0x06005E39 RID: 24121 RVA: 0x002FD9C0 File Offset: 0x002FBDC0
		public static void RemoveDuplicates<T>(this List<T> list) where T : class
		{
			if (list.Count > 1)
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					for (int j = 0; j < i; j++)
					{
						if (list[i] == list[j])
						{
							list.RemoveAt(i);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06005E3A RID: 24122 RVA: 0x002FDA38 File Offset: 0x002FBE38
		public static void Shuffle<T>(this IList<T> list)
		{
			int i = list.Count;
			while (i > 1)
			{
				i--;
				int index = Rand.RangeInclusive(0, i);
				T value = list[index];
				list[index] = list[i];
				list[i] = value;
			}
		}

		// Token: 0x06005E3B RID: 24123 RVA: 0x002FDA88 File Offset: 0x002FBE88
		public static void InsertionSort<T>(this IList<T> list, Comparison<T> comparison)
		{
			int count = list.Count;
			for (int i = 1; i < count; i++)
			{
				T t = list[i];
				int num = i - 1;
				while (num >= 0 && comparison(list[num], t) > 0)
				{
					list[num + 1] = list[num];
					num--;
				}
				list[num + 1] = t;
			}
		}
	}
}
