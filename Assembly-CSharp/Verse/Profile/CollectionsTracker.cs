using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Profile
{
	// Token: 0x02000D64 RID: 3428
	[HasDebugOutput]
	public static class CollectionsTracker
	{
		// Token: 0x06004CB8 RID: 19640 RVA: 0x0027EE34 File Offset: 0x0027D234
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

		// Token: 0x06004CB9 RID: 19641 RVA: 0x0027EEFC File Offset: 0x0027D2FC
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

		// Token: 0x0400332B RID: 13099
		private static Dictionary<WeakReference, int> collections = new Dictionary<WeakReference, int>();
	}
}
