using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000FB8 RID: 4024
	public static class FullPool<T> where T : IFullPoolable, new()
	{
		// Token: 0x04003FB1 RID: 16305
		private static List<T> freeItems = new List<T>();

		// Token: 0x06006158 RID: 24920 RVA: 0x00312C6C File Offset: 0x0031106C
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

		// Token: 0x06006159 RID: 24921 RVA: 0x00312CCB File Offset: 0x003110CB
		public static void Return(T item)
		{
			item.Reset();
			FullPool<T>.freeItems.Add(item);
		}
	}
}
