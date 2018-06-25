using System;
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

		public static void Return(T item)
		{
			item.Reset();
			FullPool<T>.freeItems.Add(item);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static FullPool()
		{
		}
	}
}
