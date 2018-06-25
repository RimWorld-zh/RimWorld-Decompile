using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FBC RID: 4028
	public static class FullPool<T> where T : IFullPoolable, new()
	{
		// Token: 0x04003FB9 RID: 16313
		private static List<T> freeItems = new List<T>();

		// Token: 0x06006168 RID: 24936 RVA: 0x0031374C File Offset: 0x00311B4C
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

		// Token: 0x06006169 RID: 24937 RVA: 0x003137AB File Offset: 0x00311BAB
		public static void Return(T item)
		{
			item.Reset();
			FullPool<T>.freeItems.Add(item);
		}
	}
}
