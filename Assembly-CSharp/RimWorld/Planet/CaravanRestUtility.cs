using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005E0 RID: 1504
	public static class CaravanRestUtility
	{
		// Token: 0x06001DC5 RID: 7621 RVA: 0x00100BF8 File Offset: 0x000FEFF8
		public static bool RestingNowAt(int tile)
		{
			return CaravanRestUtility.WouldBeRestingAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x00100C1C File Offset: 0x000FF01C
		public static bool WouldBeRestingAt(int tile, long ticksAbs)
		{
			float num = GenDate.HourFloat(ticksAbs, Find.WorldGrid.LongLatOf(tile).x);
			return num < 6f || num > 22f;
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x00100C64 File Offset: 0x000FF064
		public static int LeftRestTicksAt(int tile)
		{
			return CaravanRestUtility.LeftRestTicksAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x06001DC8 RID: 7624 RVA: 0x00100C88 File Offset: 0x000FF088
		public static int LeftRestTicksAt(int tile, long ticksAbs)
		{
			int result;
			if (!CaravanRestUtility.WouldBeRestingAt(tile, ticksAbs))
			{
				result = 0;
			}
			else
			{
				float num = GenDate.HourFloat(ticksAbs, Find.WorldGrid.LongLatOf(tile).x);
				if (num < 6f)
				{
					result = Mathf.CeilToInt((6f - num) * 2500f);
				}
				else
				{
					result = Mathf.CeilToInt((24f - num + 6f) * 2500f);
				}
			}
			return result;
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x00100D08 File Offset: 0x000FF108
		public static int LeftNonRestTicksAt(int tile)
		{
			return CaravanRestUtility.LeftNonRestTicksAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x00100D2C File Offset: 0x000FF12C
		public static int LeftNonRestTicksAt(int tile, long ticksAbs)
		{
			int result;
			if (CaravanRestUtility.WouldBeRestingAt(tile, ticksAbs))
			{
				result = 0;
			}
			else
			{
				float num = GenDate.HourFloat(ticksAbs, Find.WorldGrid.LongLatOf(tile).x);
				result = Mathf.CeilToInt((22f - num) * 2500f);
			}
			return result;
		}

		// Token: 0x040011A1 RID: 4513
		public const float WakeUpHour = 6f;

		// Token: 0x040011A2 RID: 4514
		public const float RestStartHour = 22f;
	}
}
