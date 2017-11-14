using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Profile
{
	public static class CollectionsTracker
	{
		private static Dictionary<WeakReference, int> collections = new Dictionary<WeakReference, int>();

		public static void RememberCollections()
		{
			if (!MemoryTracker.AnythingTracked)
			{
				Log.Message("No objects tracked, memory tracker markup may not be applied.");
			}
			else
			{
				CollectionsTracker.collections.Clear();
				foreach (WeakReference foundCollection in MemoryTracker.FoundCollections)
				{
					if (foundCollection.IsAlive)
					{
						ICollection collection = foundCollection.Target as ICollection;
						CollectionsTracker.collections[foundCollection] = collection.Count;
					}
				}
				Log.Message("Tracking " + CollectionsTracker.collections.Count + " collections.");
			}
		}

		public static void LogGrownCollections()
		{
			if (!MemoryTracker.AnythingTracked)
			{
				Log.Message("No objects tracked, memory tracker markup may not be applied.");
			}
			else
			{
				CollectionsTracker.collections.RemoveAll((KeyValuePair<WeakReference, int> kvp) => !kvp.Key.IsAlive || ((ICollection)kvp.Key.Target).Count <= kvp.Value);
				MemoryTracker.LogObjectHoldPathsFor(from kvp in CollectionsTracker.collections
				select kvp.Key, delegate(WeakReference elem)
				{
					ICollection collection = elem.Target as ICollection;
					return collection.Count - CollectionsTracker.collections[elem];
				});
				CollectionsTracker.collections.Clear();
			}
		}
	}
}
