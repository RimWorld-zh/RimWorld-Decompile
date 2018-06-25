using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FBD RID: 4029
	public static class FullPool<T> where T : IFullPoolable, new()
	{
		// Token: 0x04003FC1 RID: 16321
		private static List<T> freeItems = new List<T>();

		// Token: 0x06006168 RID: 24936 RVA: 0x00313990 File Offset: 0x00311D90
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

		// Token: 0x06006169 RID: 24937 RVA: 0x003139EF File Offset: 0x00311DEF
		public static void Return(T item)
		{
			item.Reset();
			FullPool<T>.freeItems.Add(item);
		}
	}
}
