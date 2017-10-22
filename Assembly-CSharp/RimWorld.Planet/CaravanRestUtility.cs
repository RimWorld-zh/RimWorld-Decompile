using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public static class CaravanRestUtility
	{
		public const float WakeUpHour = 6f;

		public const float RestStartHour = 22f;

		public static bool RestingNowAt(int tile)
		{
			return CaravanRestUtility.WouldBeRestingAt(tile, GenTicks.TicksAbs);
		}

		public static bool WouldBeRestingAt(int tile, long ticksAbs)
		{
			Vector2 vector = Find.WorldGrid.LongLatOf(tile);
			float num = GenDate.HourFloat(ticksAbs, vector.x);
			return num < 6.0 || num > 22.0;
		}

		public static int LeftRestTicksAt(int tile)
		{
			return CaravanRestUtility.LeftRestTicksAt(tile, GenTicks.TicksAbs);
		}

		public static int LeftRestTicksAt(int tile, long ticksAbs)
		{
			int result;
			if (!CaravanRestUtility.WouldBeRestingAt(tile, ticksAbs))
			{
				result = 0;
			}
			else
			{
				Vector2 vector = Find.WorldGrid.LongLatOf(tile);
				float num = GenDate.HourFloat(ticksAbs, vector.x);
				result = ((!(num < 6.0)) ? Mathf.CeilToInt((float)((24.0 - num + 6.0) * 2500.0)) : Mathf.CeilToInt((float)((6.0 - num) * 2500.0)));
			}
			return result;
		}

		public static int LeftNonRestTicksAt(int tile)
		{
			return CaravanRestUtility.LeftNonRestTicksAt(tile, GenTicks.TicksAbs);
		}

		public static int LeftNonRestTicksAt(int tile, long ticksAbs)
		{
			int result;
			if (CaravanRestUtility.WouldBeRestingAt(tile, ticksAbs))
			{
				result = 0;
			}
			else
			{
				Vector2 vector = Find.WorldGrid.LongLatOf(tile);
				float num = GenDate.HourFloat(ticksAbs, vector.x);
				result = Mathf.CeilToInt((float)((22.0 - num) * 2500.0));
			}
			return result;
		}

		public static float DayPercentNotRestingAt(int tile)
		{
			return Mathf.InverseLerp(6f, 22f, GenLocalDate.HourFloat(tile));
		}
	}
}
