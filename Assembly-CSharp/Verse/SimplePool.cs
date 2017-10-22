using System.Collections.Generic;

namespace Verse
{
	public static class SimplePool<T> where T : new()
	{
		private static List<T> freeItems = new List<T>();

		public static T Get()
		{
			T result;
			if (SimplePool<T>.freeItems.Count == 0)
			{
				result = new T();
			}
			else
			{
				T val = SimplePool<T>.freeItems[SimplePool<T>.freeItems.Count - 1];
				SimplePool<T>.freeItems.RemoveAt(SimplePool<T>.freeItems.Count - 1);
				result = val;
			}
			return result;
		}

		public static void Return(T item)
		{
			SimplePool<T>.freeItems.Add(item);
		}
	}
}
