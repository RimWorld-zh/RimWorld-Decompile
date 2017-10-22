using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class GenCelestial
	{
		public const float ShadowMaxLengthDay = 15f;

		public const float ShadowMaxLengthNight = 15f;

		private const float ShadowGlowLerpSpan = 0.15f;

		private const float ShadowDayNightThreshold = 0.6f;

		private static int TicksAbsForSunPosInWorldSpace
		{
			get
			{
				if (Current.ProgramState != 0)
				{
					return GenTicks.TicksAbs;
				}
				int startingTile = Find.GameInitData.startingTile;
				double num;
				if (startingTile >= 0)
				{
					Vector2 vector = Find.WorldGrid.LongLatOf(startingTile);
					num = vector.x;
				}
				else
				{
					num = 0.0;
				}
				float longitude = (float)num;
				return Mathf.RoundToInt((float)(2500.0 * (12.0 - GenDate.TimeZoneFloatAt(longitude))));
			}
		}

		public static float CurCelestialSunGlow(Map map)
		{
			Vector2 vector = Find.WorldGrid.LongLatOf(map.Tile);
			return GenCelestial.CelestialSunGlowPercent(vector.y, GenLocalDate.DayOfYear(map), GenLocalDate.DayPercent(map));
		}

		public static float CurShadowStrength(Map map)
		{
			return Mathf.Clamp01((float)(Mathf.Abs((float)(GenCelestial.CurCelestialSunGlow(map) - 0.60000002384185791)) / 0.15000000596046448));
		}

		public static Vector2 CurShadowVector(Map map)
		{
			float num = GenLocalDate.DayPercent(map);
			float t;
			float num2;
			float num3;
			if (GenCelestial.IsDaytime(GenCelestial.CurCelestialSunGlow(map)))
			{
				t = num;
				num2 = -1.5f;
				num3 = 15f;
			}
			else
			{
				t = (float)((!(num > 0.5)) ? (0.5 + Mathf.InverseLerp(0f, 0.5f, num) * 0.5) : (Mathf.InverseLerp(0.5f, 1f, num) * 0.5));
				num2 = -0.9f;
				num3 = 15f;
			}
			float num4 = Mathf.Lerp(num3, (float)(0.0 - num3), t);
			float y = (float)(num2 - 2.5 * (num4 * num4 / 100.0));
			return new Vector2(num4, y);
		}

		public static Vector3 CurSunPositionInWorldSpace()
		{
			int ticksAbsForSunPosInWorldSpace = GenCelestial.TicksAbsForSunPosInWorldSpace;
			return GenCelestial.SunPositionUnmodified((float)GenDate.DayOfYear(ticksAbsForSunPosInWorldSpace, 0f), GenDate.DayPercent(ticksAbsForSunPosInWorldSpace, 0f), new Vector3(0f, 0f, -1f));
		}

		public static bool IsDaytime(float glow)
		{
			return glow > 0.60000002384185791;
		}

		private static Vector3 SunPosition(float latitude, int dayOfYear, float dayPercent)
		{
			Vector3 target = GenCelestial.SurfaceNormal(latitude);
			Vector3 current = GenCelestial.SunPositionUnmodified((float)dayOfYear, dayPercent, new Vector3(1f, 0f, 0f));
			current = Vector3.RotateTowards(current, target, 0.331612557f, 9999999f);
			float num = Mathf.InverseLerp(60f, 0f, Mathf.Abs(latitude));
			if (num > 0.0)
			{
				current = Vector3.RotateTowards(current, target, (float)(6.2831854820251465 * (17.0 * num / 360.0)), 9999999f);
			}
			return current.normalized;
		}

		private static Vector3 SunPositionUnmodified(float dayOfYear, float dayPercent, Vector3 initialSunPos)
		{
			Vector3 point = initialSunPos * 100f;
			float num = (float)(dayOfYear / 60.0);
			float f = (float)(num * 3.1415927410125732 * 2.0);
			float num2 = (float)(0.0 - Mathf.Cos(f));
			point.y += (float)(num2 * 20.0);
			float angle = (float)((dayPercent - 0.5) * 360.0);
			point = Quaternion.AngleAxis(angle, Vector3.up) * point;
			return point.normalized;
		}

		private static float CelestialSunGlowPercent(float latitude, int dayOfYear, float dayPercent)
		{
			Vector3 vector = GenCelestial.SurfaceNormal(latitude);
			Vector3 rhs = GenCelestial.SunPosition(latitude, dayOfYear, dayPercent);
			float value = Vector3.Dot(vector.normalized, rhs);
			float value2 = Mathf.InverseLerp(0f, 0.7f, value);
			return Mathf.Clamp01(value2);
		}

		private static Vector3 SurfaceNormal(float latitude)
		{
			Vector3 point = new Vector3(1f, 0f, 0f);
			return Quaternion.AngleAxis(latitude, new Vector3(0f, 0f, 1f)) * point;
		}

		public static void LogSunGlowForYear()
		{
			for (int num = -90; num <= 90; num += 10)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Sun visibility percents for latitude " + num + ", for each hour of each day of the year");
				stringBuilder.AppendLine("---------------------------------------");
				stringBuilder.Append("Day/hr".PadRight(6));
				for (int num2 = 0; num2 < 24; num2 += 2)
				{
					stringBuilder.Append((num2.ToString() + "h").PadRight(6));
				}
				stringBuilder.AppendLine();
				for (int num3 = 0; num3 < 60; num3 += 5)
				{
					stringBuilder.Append(num3.ToString().PadRight(6));
					for (int num4 = 0; num4 < 24; num4 += 2)
					{
						stringBuilder.Append(GenCelestial.CelestialSunGlowPercent((float)num, num3, (float)((float)num4 / 24.0)).ToString("F3").PadRight(6));
					}
					stringBuilder.AppendLine();
				}
				Log.Message(stringBuilder.ToString());
			}
		}
	}
}
