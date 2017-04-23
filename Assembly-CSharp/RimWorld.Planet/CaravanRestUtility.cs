using System;
using UnityEngine;

namespace RimWorld.Planet
{
	public static class CaravanRestUtility
	{
		public const float WakeUpHour = 6f;

		public const float RestStartHour = 22f;

		public static bool RestingNowAt(int tile)
		{
			float num = GenLocalDate.HourFloat(tile);
			return num < 6f || num > 22f;
		}

		public static int LeftRestTicksAt(int tile)
		{
			if (!CaravanRestUtility.RestingNowAt(tile))
			{
				return 0;
			}
			float num = GenLocalDate.HourFloat(tile);
			if (num < 6f)
			{
				return Mathf.CeilToInt((6f - num) * 2500f);
			}
			return Mathf.CeilToInt((24f - num + 6f) * 2500f);
		}

		public static int LeftNonRestTicksAt(int tile)
		{
			if (CaravanRestUtility.RestingNowAt(tile))
			{
				return 0;
			}
			float num = GenLocalDate.HourFloat(tile);
			return Mathf.CeilToInt((22f - num) * 2500f);
		}

		public static float DayPercentNotRestingAt(int tile)
		{
			return Mathf.InverseLerp(6f, 22f, GenLocalDate.HourFloat(tile));
		}
	}
}
