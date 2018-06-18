using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F41 RID: 3905
	public static class GenList
	{
		// Token: 0x06005E33 RID: 24115 RVA: 0x002FD9DC File Offset: 0x002FBDDC
		public static int CountAllowNull<T>(this IList<T> list)
		{
			return (list == null) ? 0 : list.Count;
		}

		// Token: 0x06005E34 RID: 24116 RVA: 0x002FDA04 File Offset: 0x002FBE04
		public static bool NullOrEmpty<T>(this IList<T> list)
		{
			return list == null || list.Count == 0;
		}

		// Token: 0x06005E35 RID: 24117 RVA: 0x002FDA2C File Offset: 0x002FBE2C
		public static List<T> ListFullCopy<T>(this List<T> source)
		{
			List<T> list = new List<T>(source.Count);
			for (int i = 0; i < source.Count; i++)
			{
				list.Add(source[i]);
			}
			return list;
		}

		// Token: 0x06005E36 RID: 24118 RVA: 0x002FDA74 File Offset: 0x002FBE74
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

		// Token: 0x06005E37 RID: 24119 RVA: 0x002FDA9C File Offset: 0x002FBE9C
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

		// Token: 0x06005E38 RID: 24120 RVA: 0x002FDB14 File Offset: 0x002FBF14
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

		// Token: 0x06005E39 RID: 24121 RVA: 0x002FDB64 File Offset: 0x002FBF64
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
