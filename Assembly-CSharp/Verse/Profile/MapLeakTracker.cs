using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse.Profile
{
	[HasDebugOutput]
	internal static class MapLeakTracker
	{
		private static List<WeakReference<Map>> references = new List<WeakReference<Map>>();

		private static List<WeakReference<Map>> referencesFlagged = new List<WeakReference<Map>>();

		private static float lastUpdateSecond = 0f;

		private static int lastUpdateTick = 0;

		private static bool gcSinceLastUpdate = false;

		private static long gcUsedLastFrame = 0L;

		private const float TimeBetweenUpdateRealtimeSeconds = 60f;

		private const float TimeBetweenUpdateGameDays = 1f;

		[CompilerGenerated]
		private static Predicate<WeakReference<Map>> <>f__am$cache0;

		public static void AddReference(Map element)
		{
			MapLeakTracker.references.Add(new WeakReference<Map>(element));
		}

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

		[Category("System")]
		[DebugOutput]
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

		// Note: this type is marked as 'beforefieldinit'.
		static MapLeakTracker()
		{
		}

		[CompilerGenerated]
		private static bool <Update>m__0(WeakReference<Map> liveref)
		{
			return !liveref.IsAlive;
		}

		[CompilerGenerated]
		private sealed class <Update>c__AnonStorey0
		{
			internal Map map;

			public <Update>c__AnonStorey0()
			{
			}

			internal bool <>m__0(WeakReference<Map> liveref)
			{
				return liveref.Target == this.map;
			}
		}
	}
}
