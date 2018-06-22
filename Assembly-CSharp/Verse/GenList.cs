using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F41 RID: 3905
	public static class GenList
	{
		// Token: 0x06005E5B RID: 24155 RVA: 0x002FFA18 File Offset: 0x002FDE18
		public static int CountAllowNull<T>(this IList<T> list)
		{
			return (list == null) ? 0 : list.Count;
		}

		// Token: 0x06005E5C RID: 24156 RVA: 0x002FFA40 File Offset: 0x002FDE40
		public static bool NullOrEmpty<T>(this IList<T> list)
		{
			return list == null || list.Count == 0;
		}

		// Token: 0x06005E5D RID: 24157 RVA: 0x002FFA68 File Offset: 0x002FDE68
		public static List<T> ListFullCopy<T>(this List<T> source)
		{
			List<T> list = new List<T>(source.Count);
			for (int i = 0; i < source.Count; i++)
			{
				list.Add(source[i]);
			}
			return list;
		}

		// Token: 0x06005E5E RID: 24158 RVA: 0x002FFAB0 File Offset: 0x002FDEB0
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

		// Token: 0x06005E5F RID: 24159 RVA: 0x002FFAD8 File Offset: 0x002FDED8
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

		// Token: 0x06005E60 RID: 24160 RVA: 0x002FFB50 File Offset: 0x002FDF50
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

		// Token: 0x06005E61 RID: 24161 RVA: 0x002FFBA0 File Offset: 0x002FDFA0
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
