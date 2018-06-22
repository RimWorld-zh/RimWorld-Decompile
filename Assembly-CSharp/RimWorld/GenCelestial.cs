using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000903 RID: 2307
	public static class GenCelestial
	{
		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06003591 RID: 13713 RVA: 0x001CDA70 File Offset: 0x001CBE70
		private static int TicksAbsForSunPosInWorldSpace
		{
			get
			{
				int result;
				if (Current.ProgramState != ProgramState.Entry)
				{
					result = GenTicks.TicksAbs;
				}
				else
				{
					int startingTile = Find.GameInitData.startingTile;
					float longitude = (startingTile < 0) ? 0f : Find.WorldGrid.LongLatOf(startingTile).x;
					result = Mathf.RoundToInt(2500f * (12f - GenDate.TimeZoneFloatAt(longitude)));
				}
				return result;
			}
		}

		// Token: 0x06003592 RID: 13714 RVA: 0x001CDAE4 File Offset: 0x001CBEE4
		public static float CurCelestialSunGlow(Map map)
		{
			return GenCelestial.CelestialSunGlow(map, Find.TickManager.TicksAbs);
		}

		// Token: 0x06003593 RID: 13715 RVA: 0x001CDB0C File Offset: 0x001CBF0C
		public static float CelestialSunGlow(Map map, int ticksAbs)
		{
			Vector2 vector = Find.WorldGrid.LongLatOf(map.Tile);
			return GenCelestial.CelestialSunGlowPercent(vector.y, GenDate.DayOfYear((long)ticksAbs, vector.x), GenDate.DayPercent((long)ticksAbs, vector.x));
		}

		// Token: 0x06003594 RID: 13716 RVA: 0x001CDB5C File Offset: 0x001CBF5C
		public static float CurShadowStrength(Map map)
		{
			return Mathf.Clamp01(Mathf.Abs(GenCelestial.CurCelestialSunGlow(map) - 0.6f) / 0.15f);
		}

		// Token: 0x06003595 RID: 13717 RVA: 0x001CDB90 File Offset: 0x001CBF90
		public static GenCelestial.LightInfo GetLightSourceInfo(Map map, GenCelestial.LightType type)
		{
			float num = GenLocalDate.DayPercent(map);
			bool flag;
			float intensity;
			if (type == GenCelestial.LightType.Shadow)
			{
				flag = GenCelestial.IsDaytime(GenCelestial.CurCelestialSunGlow(map));
				intensity = GenCelestial.CurShadowStrength(map);
			}
			else if (type == GenCelestial.LightType.LightingSun)
			{
				flag = true;
				intensity = Mathf.Clamp01((GenCelestial.CurCelestialSunGlow(map) - 0.6f + 0.2f) / 0.15f);
			}
			else if (type == GenCelestial.LightType.LightingMoon)
			{
				flag = false;
				intensity = Mathf.Clamp01(-(GenCelestial.CurCelestialSunGlow(map) - 0.6f - 0.2f) / 0.15f);
			}
			else
			{
				Log.ErrorOnce("Invalid light type requested", 64275614, false);
				flag = true;
				intensity = 0f;
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
				if (num > 0.5f)
				{
					t = Mathf.InverseLerp(0.5f, 1f, num) * 0.5f;
				}
				else
				{
					t = 0.5f + Mathf.InverseLerp(0f, 0.5f, num) * 0.5f;
				}
				num2 = -0.9f;
				num3 = 15f;
			}
			float num4 = Mathf.LerpUnclamped(-num3, num3, t);
			float y = num2 - 2.5f * (num4 * num4 / 100f);
			return new GenCelestial.LightInfo
			{
				vector = new Vector2(num4, y),
				intensity = intensity
			};
		}

		// Token: 0x06003596 RID: 13718 RVA: 0x001CDCFC File Offset: 0x001CC0FC
		public static Vector3 CurSunPositionInWorldSpace()
		{
			int ticksAbsForSunPosInWorldSpace = GenCelestial.TicksAbsForSunPosInWorldSpace;
			return GenCelestial.SunPositionUnmodified((float)GenDate.DayOfYear((long)ticksAbsForSunPosInWorldSpace, 0f), GenDate.DayPercent((long)ticksAbsForSunPosInWorldSpace, 0f), new Vector3(0f, 0f, -1f));
		}

		// Token: 0x06003597 RID: 13719 RVA: 0x001CDD4C File Offset: 0x001CC14C
		public static bool IsDaytime(float glow)
		{
			return glow > 0.6f;
		}

		// Token: 0x06003598 RID: 13720 RVA: 0x001CDD6C File Offset: 0x001CC16C
		private static Vector3 SunPosition(float latitude, int dayOfYear, float dayPercent)
		{
			Vector3 target = GenCelestial.SurfaceNormal(latitude);
			Vector3 current = GenCelestial.SunPositionUnmodified((float)dayOfYear, dayPercent, new Vector3(1f, 0f, 0f));
			current = Vector3.RotateTowards(current, target, 0.331612557f, 9999999f);
			float num = Mathf.InverseLerp(60f, 0f, Mathf.Abs(latitude));
			if (num > 0f)
			{
				current = Vector3.RotateTowards(current, target, 6.28318548f * (17f * num / 360f), 9999999f);
			}
			return current.normalized;
		}

		// Token: 0x06003599 RID: 13721 RVA: 0x001CDE00 File Offset: 0x001CC200
		private static Vector3 SunPositionUnmodified(float dayOfYear, float dayPercent, Vector3 initialSunPos)
		{
			Vector3 point = initialSunPos * 100f;
			float num = dayOfYear / 60f;
			float f = num * 3.14159274f * 2f;
			float num2 = -Mathf.Cos(f);
			point.y += num2 * 20f;
			float angle = (dayPercent - 0.5f) * 360f;
			point = Quaternion.AngleAxis(angle, Vector3.up) * point;
			return point.normalized;
		}

		// Token: 0x0600359A RID: 13722 RVA: 0x001CDE80 File Offset: 0x001CC280
		private static float CelestialSunGlowPercent(float latitude, int dayOfYear, float dayPercent)
		{
			Vector3 vector = GenCelestial.SurfaceNormal(latitude);
			Vector3 rhs = GenCelestial.SunPosition(latitude, dayOfYear, dayPercent);
			float value = Vector3.Dot(vector.normalized, rhs);
			float value2 = Mathf.InverseLerp(0f, 0.7f, value);
			return Mathf.Clamp01(value2);
		}

		// Token: 0x0600359B RID: 13723 RVA: 0x001CDED0 File Offset: 0x001CC2D0
		public static float AverageGlow(float latitude, int dayOfYear)
		{
			float num = 0f;
			for (int i = 0; i < 24; i++)
			{
				num += GenCelestial.CelestialSunGlowPercent(latitude, dayOfYear, (float)i / 24f);
			}
			return num / 24f;
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x001CDF1C File Offset: 0x001CC31C
		private static Vector3 SurfaceNormal(float latitude)
		{
			Vector3 vector = new Vector3(1f, 0f, 0f);
			vector = Quaternion.AngleAxis(latitude, new Vector3(0f, 0f, 1f)) * vector;
			return vector;
		}

		// Token: 0x0600359D RID: 13725 RVA: 0x001CDF6C File Offset: 0x001CC36C
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
						stringBuilder.Append(GenCelestial.CelestialSunGlowPercent((float)i, k, (float)l / 24f).ToString("F3").PadRight(6));
					}
					stringBuilder.AppendLine();
				}
				Log.Message(stringBuilder.ToString(), false);
			}
		}

		// Token: 0x04001D01 RID: 7425
		public const float ShadowMaxLengthDay = 15f;

		// Token: 0x04001D02 RID: 7426
		public const float ShadowMaxLengthNight = 15f;

		// Token: 0x04001D03 RID: 7427
		private const float ShadowGlowLerpSpan = 0.15f;

		// Token: 0x04001D04 RID: 7428
		private const float ShadowDayNightThreshold = 0.6f;

		// Token: 0x02000904 RID: 2308
		public struct LightInfo
		{
			// Token: 0x04001D05 RID: 7429
			public Vector2 vector;

			// Token: 0x04001D06 RID: 7430
			public float intensity;
		}

		// Token: 0x02000905 RID: 2309
		public enum LightType
		{
			// Token: 0x04001D08 RID: 7432
			Shadow,
			// Token: 0x04001D09 RID: 7433
			LightingSun,
			// Token: 0x04001D0A RID: 7434
			LightingMoon
		}
	}
}
