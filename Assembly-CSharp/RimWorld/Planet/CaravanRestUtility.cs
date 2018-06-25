using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005E2 RID: 1506
	public static class CaravanRestUtility
	{
		// Token: 0x040011A5 RID: 4517
		public const float WakeUpHour = 6f;

		// Token: 0x040011A6 RID: 4518
		public const float RestStartHour = 22f;

		// Token: 0x06001DC8 RID: 7624 RVA: 0x00100FB0 File Offset: 0x000FF3B0
		public static bool RestingNowAt(int tile)
		{
			return CaravanRestUtility.WouldBeRestingAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x06001DC9 RID: 7625 RVA: 0x00100FD4 File Offset: 0x000FF3D4
		public static bool WouldBeRestingAt(int tile, long ticksAbs)
		{
			float num = GenDate.HourFloat(ticksAbs, Find.WorldGrid.LongLatOf(tile).x);
			return num < 6f || num > 22f;
		}

		// Token: 0x06001DCA RID: 7626 RVA: 0x0010101C File Offset: 0x000FF41C
		public static int LeftRestTicksAt(int tile)
		{
			return CaravanRestUtility.LeftRestTicksAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x06001DCB RID: 7627 RVA: 0x00101040 File Offset: 0x000FF440
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

		// Token: 0x06001DCC RID: 7628 RVA: 0x001010C0 File Offset: 0x000FF4C0
		public static int LeftNonRestTicksAt(int tile)
		{
			return CaravanRestUtility.LeftNonRestTicksAt(tile, (long)GenTicks.TicksAbs);
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x001010E4 File Offset: 0x000FF4E4
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
	}
}
