using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Verse.Profile
{
	[HasDebugOutput]
	public static class CollectionsTracker
	{
		private static Dictionary<WeakReference, int> collections = new Dictionary<WeakReference, int>();

		[CompilerGenerated]
		private static Predicate<KeyValuePair<WeakReference, int>> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<KeyValuePair<WeakReference, int>, WeakReference> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<WeakReference, int> <>f__am$cache2;

		[DebugOutput]
		private static void GrownCollectionsStart()
		{
			if (!MemoryTracker.AnythingTracked)
			{
				Log.Message("No objects tracked, memory tracker markup may not be applied.", false);
			}
			else
			{
				CollectionsTracker.collections.Clear();
				foreach (WeakReference weakReference in MemoryTracker.FoundCollections)
				{
					if (weakReference.IsAlive)
					{
						ICollection collection = weakReference.Target as ICollection;
						CollectionsTracker.collections[weakReference] = collection.Count;
					}
				}
				Log.Message("Tracking " + CollectionsTracker.collections.Count + " collections.", false);
			}
		}

		[DebugOutput]
		private static void GrownCollectionsLog()
		{
			if (!MemoryTracker.AnythingTracked)
			{
				Log.Message("No objects tracked, memory tracker markup may not be applied.", false);
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

		// Note: this type is marked as 'beforefieldinit'.
		static CollectionsTracker()
		{
		}

		[CompilerGenerated]
		private static bool <GrownCollectionsLog>m__0(KeyValuePair<WeakReference, int> kvp)
		{
			return !kvp.Key.IsAlive || ((ICollection)kvp.Key.Target).Count <= kvp.Value;
		}

		[CompilerGenerated]
		private static WeakReference <GrownCollectionsLog>m__1(KeyValuePair<WeakReference, int> kvp)
		{
			return kvp.Key;
		}

		[CompilerGenerated]
		private static int <GrownCollectionsLog>m__2(WeakReference elem)
		{
			ICollection collection = elem.Target as ICollection;
			return collection.Count - CollectionsTracker.collections[elem];
		}
	}
}
