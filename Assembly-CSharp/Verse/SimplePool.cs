using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FBA RID: 4026
	public static class SimplePool<T> where T : new()
	{
		// Token: 0x04003FB8 RID: 16312
		private static List<T> freeItems = new List<T>();

		// Token: 0x06006164 RID: 24932 RVA: 0x003136D0 File Offset: 0x00311AD0
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

		// Token: 0x06006165 RID: 24933 RVA: 0x0031372F File Offset: 0x00311B2F
		public static void Return(T item)
		{
			SimplePool<T>.freeItems.Add(item);
		}
	}
}
