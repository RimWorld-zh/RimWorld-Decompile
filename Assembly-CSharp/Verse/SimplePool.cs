using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FB6 RID: 4022
	public static class SimplePool<T> where T : new()
	{
		// Token: 0x04003FB0 RID: 16304
		private static List<T> freeItems = new List<T>();

		// Token: 0x06006154 RID: 24916 RVA: 0x00312BF0 File Offset: 0x00310FF0
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

		// Token: 0x06006155 RID: 24917 RVA: 0x00312C4F File Offset: 0x0031104F
		public static void Return(T item)
		{
			SimplePool<T>.freeItems.Add(item);
		}
	}
}
