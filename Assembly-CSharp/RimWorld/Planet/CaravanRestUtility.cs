using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005E4 RID: 1508
	public static class CaravanRestUtility
	{
		// Token: 0x06001DCE RID: 7630 RVA: 0x00100BA4 File Offset: 0x000FEFA4
		public static bool RestingNowAt(int tile)
		{
			return CaravanRestUtility.WouldBeRestingAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x06001DCF RID: 7631 RVA: 0x00100BC8 File Offset: 0x000FEFC8
		public static bool WouldBeRestingAt(int tile, long ticksAbs)
		{
			float num = GenDate.HourFloat(ticksAbs, Find.WorldGrid.LongLatOf(tile).x);
			return num < 6f || num > 22f;
		}

		// Token: 0x06001DD0 RID: 7632 RVA: 0x00100C10 File Offset: 0x000FF010
		public static int LeftRestTicksAt(int tile)
		{
			return CaravanRestUtility.LeftRestTicksAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x06001DD1 RID: 7633 RVA: 0x00100C34 File Offset: 0x000FF034
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

		// Token: 0x06001DD2 RID: 7634 RVA: 0x00100CB4 File Offset: 0x000FF0B4
		public static int LeftNonRestTicksAt(int tile)
		{
			return CaravanRestUtility.LeftNonRestTicksAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x06001DD3 RID: 7635 RVA: 0x00100CD8 File Offset: 0x000FF0D8
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

		// Token: 0x040011A4 RID: 4516
		public const float WakeUpHour = 6f;

		// Token: 0x040011A5 RID: 4517
		public const float RestStartHour = 22f;
	}
}
