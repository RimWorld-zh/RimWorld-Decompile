using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class GenCelestial
	{
		public struct LightInfo
		{
			public Vector2 vector;

			public float intensity;
		}

		public enum LightType
		{
			Shadow,
			LightingSun,
			LightingMoon
		}

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
			return GenCelestial.CelestialSunGlow(map, Find.TickManager.TicksAbs);
		}

		public static float CelestialSunGlow(Map map, int ticksAbs)
		{
			Vector2 vector = Find.WorldGrid.LongLatOf(map.Tile);
			return GenCelestial.CelestialSunGlowPercent(vector.y, GenDate.DayOfYear(ticksAbs, vector.x), GenDate.DayPercent(ticksAbs, vector.x));
		}

		public static float CurShadowStrength(Map map)
		{
			return Mathf.Clamp01((float)(Mathf.Abs((float)(GenCelestial.CurCelestialSunGlow(map) - 0.60000002384185791)) / 0.15000000596046448));
		}

		public static LightInfo GetLightSourceInfo(Map map, LightType type)
		{
			float num = GenLocalDate.DayPercent(map);
			bool flag;
			float intensity;
			switch (type)
			{
			case LightType.Shadow:
				flag = GenCelestial.IsDaytime(GenCelestial.CurCelestialSunGlow(map));
				intensity = GenCelestial.CurShadowStrength(map);
				break;
			case LightType.LightingSun:
				flag = true;
				intensity = Mathf.Clamp01((float)((GenCelestial.CurCelestialSunGlow(map) - 0.60000002384185791 + 0.20000000298023224) / 0.15000000596046448));
				break;
			case LightType.LightingMoon:
				flag = false;
				intensity = Mathf.Clamp01((float)((0.0 - (GenCelestial.CurCelestialSunGlow(map) - 0.60000002384185791 - 0.20000000298023224)) / 0.15000000596046448));
				break;
			default:
				Log.ErrorOnce("Invalid light type requested", 64275614);
				flag = true;
				intensity = 0f;
				break;
			}
			float t;
			float num2;
			float num3;
			if (flag)
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
			float num4 = Mathf.LerpUnclamped((float)(0.0 - num3), num3, t);
			float y = (float)(num2 - 2.5 * (num4 * num4 / 100.0));
			LightInfo result = default(LightInfo);
			result.vector = new Vector2(num4, y);
			result.intensity = intensity;
			return result;
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
			for (int i = -90; i <= 90; i += 10)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Sun visibility percents for latitude " + i + ", for each hour of each day of the year");
				stringBuilder.AppendLine("---------------------------------------");
				stringBuilder.Append("Day/hr".PadRight(6));
				for (int j = 0; j < 24; j += 2)
				{
					stringBuilder.Append((j.ToString() + "h").PadRight(6));
				}
				stringBuilder.AppendLine();
				for (int k = 0; k < 60; k += 5)
				{
					stringBuilder.Append(k.ToString().PadRight(6));
					for (int l = 0; l < 24; l += 2)
					{
						stringBuilder.Append(GenCelestial.CelestialSunGlowPercent((float)i, k, (float)((float)l / 24.0)).ToString("F3").PadRight(6));
					}
					stringBuilder.AppendLine();
				}
				Log.Message(stringBuilder.ToString());
			}
		}
	}
}
