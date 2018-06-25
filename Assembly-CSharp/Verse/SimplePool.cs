using System;
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

		public static void Return(T item)
		{
			SimplePool<T>.freeItems.Add(item);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static SimplePool()
		{
		}
	}
}
