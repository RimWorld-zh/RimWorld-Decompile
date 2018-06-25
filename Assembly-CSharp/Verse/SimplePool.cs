using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FBB RID: 4027
	public static class SimplePool<T> where T : new()
	{
		// Token: 0x04003FC0 RID: 16320
		private static List<T> freeItems = new List<T>();

		// Token: 0x06006164 RID: 24932 RVA: 0x00313914 File Offset: 0x00311D14
		public static T Get()
		{
			T result;
			if (SimplePool<T>.freeItems.Count == 0)
			{
				result = Activator.CreateInstance<T>();
			}
			else
			{
				T t = SimplePool<T>.freeItems[SimplePool<T>.freeItems.Count - 1];
				SimplePool<T>.freeItems.RemoveAt(SimplePool<T>.freeItems.Count - 1);
				result = t;
			}
			return result;
		}

		// Token: 0x06006165 RID: 24933 RVA: 0x00313973 File Offset: 0x00311D73
		public static void Return(T item)
		{
			SimplePool<T>.freeItems.Add(item);
		}
	}
}
