using System.Collections.Generic;

namespace Verse
{
	public static class FullPool<T> where T : IFullPoolable, new()
	{
		private static List<T> freeItems = new List<T>();

		public static T Get()
		{
			T result;
			if (FullPool<T>.freeItems.Count == 0)
			{
				result = new T();
			}
			else
			{
				T val = FullPool<T>.freeItems[FullPool<T>.freeItems.Count - 1];
				FullPool<T>.freeItems.RemoveAt(FullPool<T>.freeItems.Count - 1);
				result = val;
			}
			return result;
		}

		public static void Return(T item)
		{
			item.Reset();
			FullPool<T>.freeItems.Add(item);
		}
	}
}
