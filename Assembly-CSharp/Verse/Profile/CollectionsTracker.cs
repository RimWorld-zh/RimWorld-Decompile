using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Profile
{
	// Token: 0x02000D63 RID: 3427
	[HasDebugOutput]
	public static class CollectionsTracker
	{
		// Token: 0x06004CB6 RID: 19638 RVA: 0x0027EE14 File Offset: 0x0027D214
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

		// Token: 0x06004CB7 RID: 19639 RVA: 0x0027EEDC File Offset: 0x0027D2DC
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

		// Token: 0x04003329 RID: 13097
		private static Dictionary<WeakReference, int> collections = new Dictionary<WeakReference, int>();
	}
}
