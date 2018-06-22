using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CB8 RID: 3256
	public static class ZoneColorUtility
	{
		// Token: 0x060047DC RID: 18396 RVA: 0x0025D120 File Offset: 0x0025B520
		static ZoneColorUtility()
		{
			foreach (Color color in ZoneColorUtility.GrowingZoneColors())
			{
				Color item = new Color(color.r, color.g, color.b, 0.09f);
				ZoneColorUtility.growingZoneColors.Add(item);
			}
			foreach (Color color2 in ZoneColorUtility.StorageZoneColors())
			{
				Color item2 = new Color(color2.r, color2.g, color2.b, 0.09f);
				ZoneColorUtility.storageZoneColors.Add(item2);
			}
		}

		// Token: 0x060047DD RID: 18397 RVA: 0x0025D238 File Offset: 0x0025B638
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

		// Token: 0x060047DE RID: 18398 RVA: 0x0025D284 File Offset: 0x0025B684
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

		// Token: 0x060047DF RID: 18399 RVA: 0x0025D2D0 File Offset: 0x0025B6D0
		private static IEnumerable<Color> GrowingZoneColors()
		{
			yield return Color.Lerp(new Color(0f, 1f, 0f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(1f, 1f, 0f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(0.5f, 1f, 0f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(1f, 1f, 0.5f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(0.5f, 1f, 0.5f), Color.gray, 0.5f);
			yield break;
		}

		// Token: 0x060047E0 RID: 18400 RVA: 0x0025D2F4 File Offset: 0x0025B6F4
		private static IEnumerable<Color> StorageZoneColors()
		{
			yield return Color.Lerp(new Color(1f, 0f, 0f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(1f, 0f, 1f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(0f, 0f, 1f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(1f, 0f, 0.5f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(0f, 0.5f, 1f), Color.gray, 0.5f);
			yield return Color.Lerp(new Color(0.5f, 0f, 1f), Color.gray, 0.5f);
			yield break;
		}

		// Token: 0x040030BE RID: 12478
		private static List<Color> growingZoneColors = new List<Color>();

		// Token: 0x040030BF RID: 12479
		private static List<Color> storageZoneColors = new List<Color>();

		// Token: 0x040030C0 RID: 12480
		private static int nextGrowingZoneColorIndex = 0;

		// Token: 0x040030C1 RID: 12481
		private static int nextStorageZoneColorIndex = 0;

		// Token: 0x040030C2 RID: 12482
		private const float ZoneOpacity = 0.09f;
	}
}
