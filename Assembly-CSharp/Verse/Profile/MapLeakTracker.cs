using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Profile
{
	// Token: 0x02000D65 RID: 3429
	[HasDebugOutput]
	internal static class MapLeakTracker
	{
		// Token: 0x06004CBE RID: 19646 RVA: 0x0027F047 File Offset: 0x0027D447
		public static void AddReference(Map element)
		{
			MapLeakTracker.references.Add(new WeakReference<Map>(element));
		}

		// Token: 0x06004CBF RID: 19647 RVA: 0x0027F05C File Offset: 0x0027D45C
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

		// Token: 0x06004CC0 RID: 19648 RVA: 0x0027F2A8 File Offset: 0x0027D6A8
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

		// Token: 0x0400332F RID: 13103
		private static List<WeakReference<Map>> references = new List<WeakReference<Map>>();

		// Token: 0x04003330 RID: 13104
		private static List<WeakReference<Map>> referencesFlagged = new List<WeakReference<Map>>();

		// Token: 0x04003331 RID: 13105
		private static float lastUpdateSecond = 0f;

		// Token: 0x04003332 RID: 13106
		private static int lastUpdateTick = 0;

		// Token: 0x04003333 RID: 13107
		private static bool gcSinceLastUpdate = false;

		// Token: 0x04003334 RID: 13108
		private static long gcUsedLastFrame = 0L;

		// Token: 0x04003335 RID: 13109
		private const float TimeBetweenUpdateRealtimeSeconds = 60f;

		// Token: 0x04003336 RID: 13110
		private const float TimeBetweenUpdateGameDays = 1f;
	}
}
