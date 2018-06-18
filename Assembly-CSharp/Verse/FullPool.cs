using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FB7 RID: 4023
	public static class FullPool<T> where T : IFullPoolable, new()
	{
		// Token: 0x0600612F RID: 24879 RVA: 0x00310B98 File Offset: 0x0030EF98
		public static T Get()
		{
			T result;
			if (FullPool<T>.freeItems.Count == 0)
			{
				result = Activator.CreateInstance<T>();
			}
			else
			{
				T t = FullPool<T>.freeItems[FullPool<T>.freeItems.Count - 1];
				FullPool<T>.freeItems.RemoveAt(FullPool<T>.freeItems.Count - 1);
				result = t;
			}
			return result;
		}

		// Token: 0x06006130 RID: 24880 RVA: 0x00310BF7 File Offset: 0x0030EFF7
		public static void Return(T item)
		{
			item.Reset();
			FullPool<T>.freeItems.Add(item);
		}

		// Token: 0x04003F94 RID: 16276
		private static List<T> freeItems = new List<T>();
	}
}
