using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FB6 RID: 4022
	public static class SimplePool<T> where T : new()
	{
		// Token: 0x0600612D RID: 24877 RVA: 0x00310A40 File Offset: 0x0030EE40
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

		// Token: 0x0600612E RID: 24878 RVA: 0x00310A9F File Offset: 0x0030EE9F
		public static void Return(T item)
		{
			SimplePool<T>.freeItems.Add(item);
		}

		// Token: 0x04003F94 RID: 16276
		private static List<T> freeItems = new List<T>();
	}
}
