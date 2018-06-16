using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FB8 RID: 4024
	public static class FullPool<T> where T : IFullPoolable, new()
	{
		// Token: 0x06006131 RID: 24881 RVA: 0x00310ABC File Offset: 0x0030EEBC
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

		// Token: 0x06006132 RID: 24882 RVA: 0x00310B1B File Offset: 0x0030EF1B
		public static void Return(T item)
		{
			item.Reset();
			FullPool<T>.freeItems.Add(item);
		}

		// Token: 0x04003F95 RID: 16277
		private static List<T> freeItems = new List<T>();
	}
}
