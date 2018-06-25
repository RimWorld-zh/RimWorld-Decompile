using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Profile
{
	// Token: 0x02000D64 RID: 3428
	[HasDebugOutput]
	internal static class MapLeakTracker
	{
		// Token: 0x0400333F RID: 13119
		private static List<WeakReference<Map>> references = new List<WeakReference<Map>>();

		// Token: 0x04003340 RID: 13120
		private static List<WeakReference<Map>> referencesFlagged = new List<WeakReference<Map>>();

		// Token: 0x04003341 RID: 13121
		private static float lastUpdateSecond = 0f;

		// Token: 0x04003342 RID: 13122
		private static int lastUpdateTick = 0;

		// Token: 0x04003343 RID: 13123
		private static bool gcSinceLastUpdate = false;

		// Token: 0x04003344 RID: 13124
		private static long gcUsedLastFrame = 0L;

		// Token: 0x04003345 RID: 13125
		private const float TimeBetweenUpdateRealtimeSeconds = 60f;

		// Token: 0x04003346 RID: 13126
		private const float TimeBetweenUpdateGameDays = 1f;

		// Token: 0x06004CD5 RID: 19669 RVA: 0x002809E3 File Offset: 0x0027EDE3
		public static void AddReference(Map element)
		{
			MapLeakTracker.references.Add(new WeakReference<Map>(element));
		}

		// Token: 0x06004CD6 RID: 19670 RVA: 0x002809F8 File Offset: 0x0027EDF8
		public static void Update()
		{
			if (Current.Game != null && Find.TickManager.TicksGame < MapLeakTracker.lastUpdateTick)
			{
				MapLeakTracker.lastUpdateTick = Find.TickManager.TicksGame;
			}
			long totalMemory = GC.GetTotalMemory(false);
			if (totalMemory < MapLeakTracker.gcUsedLastFrame)
			{
				MapLeakTracker.gcSinceLastUpdate = true;
			}
			MapLeakTracker.gcUsedLastFrame = totalMemory;
			if (MapLeakTracker.lastUpdateSecond + 60f <= Time.time)
			{
				if (Current.Game == null || (float)MapLeakTracker.lastUpdateTick + 60000f <= (float)Find.TickManager.TicksGame)
				{
					if (MapLeakTracker.gcSinceLastUpdate)
					{
						MapLeakTracker.lastUpdateSecond = Time.time;
						if (Current.Game != null)
						{
							MapLeakTracker.lastUpdateTick = Find.TickManager.TicksGame;
						}
						MapLeakTracker.gcSinceLastUpdate = false;
						foreach (WeakReference<Map> weakReference in MapLeakTracker.referencesFlagged)
						{
							Map map = weakReference.Target;
							if (map != null && (Find.Maps == null || !Find.Maps.Contains(map)))
							{
								Log.Error(string.Format("Memory leak detected: map {0} is still live when it shouldn't be; this map will not be mentioned again", map.ToStringSafe<Map>()), false);
								MapLeakTracker.references.RemoveAll((WeakReference<Map> liveref) => liveref.Target == map);
							}
						}
						MapLeakTracker.referencesFlagged.Clear();
						foreach (WeakReference<Map> weakReference2 in MapLeakTracker.references)
						{
							Map target = weakReference2.Target;
							if (target != null && (Find.Maps == null || !Find.Maps.Contains(target)))
							{
								MapLeakTracker.referencesFlagged.Add(weakReference2);
							}
						}
						MapLeakTracker.references.RemoveAll((WeakReference<Map> liveref) => !liveref.IsAlive);
						MapLeakTracker.gcUsedLastFrame = GC.GetTotalMemory(false);
					}
				}
			}
		}

		// Token: 0x06004CD7 RID: 19671 RVA: 0x00280C44 File Offset: 0x0027F044
		[DebugOutput]
		[Category("System")]
		public static void ForceLeakCheck()
		{
			GC.Collect();
			foreach (WeakReference<Map> weakReference in MapLeakTracker.references)
			{
				Map target = weakReference.Target;
				if (target != null && (Find.Maps == null || !Find.Maps.Contains(target)))
				{
					Log.Error(string.Format("Memory leak detected: map {0} is still live when it shouldn't be", target.ToStringSafe<Map>()), false);
				}
			}
		}
	}
}
