using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FB5 RID: 4021
	public static class SimplePool<T> where T : new()
	{
		// Token: 0x0600612B RID: 24875 RVA: 0x00310B1C File Offset: 0x0030EF1C
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

		// Token: 0x0600612C RID: 24876 RVA: 0x00310B7B File Offset: 0x0030EF7B
		public static void Return(T item)
		{
			SimplePool<T>.freeItems.Add(item);
		}

		// Token: 0x04003F93 RID: 16275
		private static List<T> freeItems = new List<T>();
	}
}
