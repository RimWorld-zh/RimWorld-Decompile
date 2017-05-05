using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Verse
{
	public static class ZoneColorUtility
	{
		private const float ZoneOpacity = 0.09f;

		private static List<Color> growingZoneColors;

		private static List<Color> storageZoneColors;

		private static int nextGrowingZoneColorIndex;

		private static int nextStorageZoneColorIndex;

		static ZoneColorUtility()
		{
			ZoneColorUtility.growingZoneColors = new List<Color>();
			ZoneColorUtility.storageZoneColors = new List<Color>();
			ZoneColorUtility.nextGrowingZoneColorIndex = 0;
			ZoneColorUtility.nextStorageZoneColorIndex = 0;
			foreach (Color current in ZoneColorUtility.GrowingZoneColors())
			{
				Color item = new Color(current.r, current.g, current.b, 0.09f);
				ZoneColorUtility.growingZoneColors.Add(item);
			}
			foreach (Color current2 in ZoneColorUtility.StorageZoneColors())
			{
				Color item2 = new Color(current2.r, current2.g, current2.b, 0.09f);
				ZoneColorUtility.storageZoneColors.Add(item2);
			}
		}

		public static Color NextGrowingZoneColor()
		{
			Color result = ZoneColorUtility.growingZoneColors[ZoneColorUtility.nextGrowingZoneColorIndex];
			ZoneColorUtility.nextGrowingZoneColorIndex++;
			if (ZoneColorUtility.nextGrowingZoneColorIndex >= ZoneColorUtility.growingZoneColors.Count)
			{
				ZoneColorUtility.nextGrowingZoneColorIndex = 0;
			}
			return result;
		}

		public static Color NextStorageZoneColor()
		{
			Color result = ZoneColorUtility.storageZoneColors[ZoneColorUtility.nextStorageZoneColorIndex];
			ZoneColorUtility.nextStorageZoneColorIndex++;
			if (ZoneColorUtility.nextStorageZoneColorIndex >= ZoneColorUtility.storageZoneColors.Count)
			{
				ZoneColorUtility.nextStorageZoneColorIndex = 0;
			}
			return result;
		}

		[DebuggerHidden]
		private static IEnumerable<Color> GrowingZoneColors()
		{
			ZoneColorUtility.<GrowingZoneColors>c__Iterator208 <GrowingZoneColors>c__Iterator = new ZoneColorUtility.<GrowingZoneColors>c__Iterator208();
			ZoneColorUtility.<GrowingZoneColors>c__Iterator208 expr_07 = <GrowingZoneColors>c__Iterator;
			expr_07.$PC = -2;
			return expr_07;
		}

		[DebuggerHidden]
		private static IEnumerable<Color> StorageZoneColors()
		{
			ZoneColorUtility.<StorageZoneColors>c__Iterator209 <StorageZoneColors>c__Iterator = new ZoneColorUtility.<StorageZoneColors>c__Iterator209();
			ZoneColorUtility.<StorageZoneColors>c__Iterator209 expr_07 = <StorageZoneColors>c__Iterator;
			expr_07.$PC = -2;
			return expr_07;
		}
	}
}
