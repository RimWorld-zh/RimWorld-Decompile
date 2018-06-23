using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Verse.Profile
{
	// Token: 0x02000D60 RID: 3424
	[HasDebugOutput]
	public static class CollectionsTracker
	{
		// Token: 0x04003334 RID: 13108
		private static Dictionary<WeakReference, int> collections = new Dictionary<WeakReference, int>();

		// Token: 0x06004CCB RID: 19659 RVA: 0x002803C4 File Offset: 0x0027E7C4
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

		// Token: 0x06004CCC RID: 19660 RVA: 0x0028048C File Offset: 0x0027E88C
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
	}
}
