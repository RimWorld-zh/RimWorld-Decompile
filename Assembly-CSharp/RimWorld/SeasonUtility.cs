using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200090D RID: 2317
	public static class SeasonUtility
	{
		// Token: 0x04001D2E RID: 7470
		private const float HemisphereLerpDistance = 5f;

		// Token: 0x04001D2F RID: 7471
		private const float SeasonYearPctLerpDistance = 0.085f;

		// Token: 0x04001D30 RID: 7472
		private static readonly SimpleCurve SeasonalAreaSeasons = new SimpleCurve
		{
			{
				new CurvePoint(-0.0425f, 0f),
				true
			},
			{
				new CurvePoint(0.0425f, 1f),
				true
			},
			{
				new CurvePoint(0.2075f, 1f),
				true
			},
			{
				new CurvePoint(0.2925f, 2f),
				true
			},
			{
				new CurvePoint(0.4575f, 2f),
				true
			},
			{
				new CurvePoint(0.5425f, 3f),
				true
			},
			{
				new CurvePoint(0.7075f, 3f),
				true
			},
			{
				new CurvePoint(0.7925f, 4f),
				true
			},
			{
				new CurvePoint(0.9575f, 4f),
				true
			},
			{
				new CurvePoint(1.0425f, 5f),
				true
			}
		};

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x060035FD RID: 13821 RVA: 0x001CFBCC File Offset: 0x001CDFCC
		public static Season FirstSeason
		{
			get
			{
				return Season.Spring;
			}
		}

		// Token: 0x060035FE RID: 13822 RVA: 0x001CFBE4 File Offset: 0x001CDFE4
		public static Season GetReportedSeason(float yearPct, float latitude)
		{
			float by;
			float by2;
			float by3;
			float by4;
			float num;
			float num2;
			SeasonUtility.GetSeason(yearPct, latitude, out by, out by2, out by3, out by4, out num, out num2);
			Season result;
			if (num == 1f)
			{
				result = Season.PermanentSummer;
			}
			else if (num2 == 1f)
			{
				result = Season.PermanentWinter;
			}
			else
			{
				result = GenMath.MaxBy<Season>(Season.Spring, by, Season.Summer, by2, Season.Fall, by3, Season.Winter, by4);
			}
			return result;
		}

		// Token: 0x060035FF RID: 13823 RVA: 0x001CFC44 File Offset: 0x001CE044
		public static Season GetDominantSeason(float yearPct, float latitude)
		{
			float by;
			float by2;
			float by3;
			float by4;
			float by5;
			float by6;
			SeasonUtility.GetSeason(yearPct, latitude, out by, out by2, out by3, out by4, out by5, out by6);
			return GenMath.MaxBy<Season>(Season.Spring, by, Season.Summer, by2, Season.Fall, by3, Season.Winter, by4, Season.PermanentSummer, by5, Season.PermanentWinter, by6);
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x001CFC84 File Offset: 0x001CE084
		public static void GetSeason(float yearPct, float latitude, out float spring, out float summer, out float fall, out float winter, out float permanentSummer, out float permanentWinter)
		{
			yearPct = Mathf.Clamp01(yearPct);
			float num;
			float num2;
			float num3;
			LatitudeSectionUtility.GetLatitudeSection(latitude, out num, out num2, out num3);
			float num4;
			float num5;
			float num6;
			float num7;
			SeasonUtility.GetSeasonalAreaSeason(yearPct, out num4, out num5, out num6, out num7, true);
			float num8;
			float num9;
			float num10;
			float num11;
			SeasonUtility.GetSeasonalAreaSeason(yearPct, out num8, out num9, out num10, out num11, false);
			float num12 = Mathf.InverseLerp(-2.5f, 2.5f, latitude);
			float num13 = num12 * num4 + (1f - num12) * num8;
			float num14 = num12 * num5 + (1f - num12) * num9;
			float num15 = num12 * num6 + (1f - num12) * num10;
			float num16 = num12 * num7 + (1f - num12) * num11;
			spring = num13 * num2;
			summer = num14 * num2;
			fall = num15 * num2;
			winter = num16 * num2;
			permanentSummer = num;
			permanentWinter = num3;
		}

		// Token: 0x06003601 RID: 13825 RVA: 0x001CFD44 File Offset: 0x001CE144
		private static void GetSeasonalAreaSeason(float yearPct, out float spring, out float summer, out float fall, out float winter, bool northernHemisphere)
		{
			yearPct = Mathf.Clamp01(yearPct);
			float x = (!northernHemisphere) ? ((yearPct + 0.5f) % 1f) : yearPct;
			float num = SeasonUtility.SeasonalAreaSeasons.Evaluate(x);
			if (num <= 1f)
			{
				winter = 1f - num;
				spring = num;
				summer = 0f;
				fall = 0f;
			}
			else if (num <= 2f)
			{
				spring = 1f - (num - 1f);
				summer = num - 1f;
				fall = 0f;
				winter = 0f;
			}
			else if (num <= 3f)
			{
				summer = 1f - (num - 2f);
				fall = num - 2f;
				spring = 0f;
				winter = 0f;
			}
			else if (num <= 4f)
			{
				fall = 1f - (num - 3f);
				winter = num - 3f;
				spring = 0f;
				summer = 0f;
			}
			else
			{
				winter = 1f - (num - 4f);
				spring = num - 4f;
				summer = 0f;
				fall = 0f;
			}
		}

		// Token: 0x06003602 RID: 13826 RVA: 0x001CFE84 File Offset: 0x001CE284
		public static Twelfth GetFirstTwelfth(this Season season, float latitude)
		{
			if (latitude >= 0f)
			{
				switch (season)
				{
				case Season.Spring:
					return Twelfth.First;
				case Season.Summer:
					return Twelfth.Fourth;
				case Season.Fall:
					return Twelfth.Seventh;
				case Season.Winter:
					return Twelfth.Tenth;
				case Season.PermanentSummer:
					return Twelfth.First;
				case Season.PermanentWinter:
					return Twelfth.First;
				}
			}
			else
			{
				switch (season)
				{
				case Season.Spring:
					return Twelfth.Seventh;
				case Season.Summer:
					return Twelfth.Tenth;
				case Season.Fall:
					return Twelfth.First;
				case Season.Winter:
					return Twelfth.Fourth;
				case Season.PermanentSummer:
					return Twelfth.First;
				case Season.PermanentWinter:
					return Twelfth.First;
				}
			}
			return Twelfth.Undefined;
		}

		// Token: 0x06003603 RID: 13827 RVA: 0x001CFF50 File Offset: 0x001CE350
		public static Twelfth GetMiddleTwelfth(this Season season, float latitude)
		{
			if (latitude >= 0f)
			{
				switch (season)
				{
				case Season.Spring:
					return Twelfth.Second;
				case Season.Summer:
					return Twelfth.Fifth;
				case Season.Fall:
					return Twelfth.Eighth;
				case Season.Winter:
					return Twelfth.Eleventh;
				case Season.PermanentSummer:
					return Twelfth.Sixth;
				case Season.PermanentWinter:
					return Twelfth.Sixth;
				}
			}
			else
			{
				switch (season)
				{
				case Season.Spring:
					return Twelfth.Eighth;
				case Season.Summer:
					return Twelfth.Eleventh;
				case Season.Fall:
					return Twelfth.Second;
				case Season.Winter:
					return Twelfth.Fifth;
				case Season.PermanentSummer:
					return Twelfth.Sixth;
				case Season.PermanentWinter:
					return Twelfth.Sixth;
				}
			}
			return Twelfth.Undefined;
		}

		// Token: 0x06003604 RID: 13828 RVA: 0x001D001C File Offset: 0x001CE41C
		public static Season GetPreviousSeason(this Season season)
		{
			Season result;
			switch (season)
			{
			case Season.Undefined:
				result = Season.Undefined;
				break;
			case Season.Spring:
				result = Season.Winter;
				break;
			case Season.Summer:
				result = Season.Spring;
				break;
			case Season.Fall:
				result = Season.Summer;
				break;
			case Season.Winter:
				result = Season.Fall;
				break;
			case Season.PermanentSummer:
				result = Season.PermanentSummer;
				break;
			case Season.PermanentWinter:
				result = Season.PermanentWinter;
				break;
			default:
				result = Season.Undefined;
				break;
			}
			return result;
		}

		// Token: 0x06003605 RID: 13829 RVA: 0x001D008C File Offset: 0x001CE48C
		public static float GetMiddleYearPct(this Season season, float latitude)
		{
			float result;
			if (season == Season.Undefined)
			{
				result = 0.5f;
			}
			else
			{
				result = season.GetMiddleTwelfth(latitude).GetMiddleYearPct();
			}
			return result;
		}

		// Token: 0x06003606 RID: 13830 RVA: 0x001D00C0 File Offset: 0x001CE4C0
		public static string Label(this Season season)
		{
			string result;
			switch (season)
			{
			case Season.Spring:
				result = "SeasonSpring".Translate();
				break;
			case Season.Summer:
				result = "SeasonSummer".Translate();
				break;
			case Season.Fall:
				result = "SeasonFall".Translate();
				break;
			case Season.Winter:
				result = "SeasonWinter".Translate();
				break;
			case Season.PermanentSummer:
				result = "SeasonPermanentSummer".Translate();
				break;
			case Season.PermanentWinter:
				result = "SeasonPermanentWinter".Translate();
				break;
			default:
				result = "Unknown season";
				break;
			}
			return result;
		}

		// Token: 0x06003607 RID: 13831 RVA: 0x001D0160 File Offset: 0x001CE560
		public static string LabelCap(this Season season)
		{
			return season.Label().CapitalizeFirst();
		}

		// Token: 0x06003608 RID: 13832 RVA: 0x001D0180 File Offset: 0x001CE580
		public static string SeasonsRangeLabel(List<Twelfth> twelfths, Vector2 longLat)
		{
			string result;
			if (twelfths.Count == 0)
			{
				result = "";
			}
			else if (twelfths.Count == 12)
			{
				result = "WholeYear".Translate();
			}
			else
			{
				string text = "";
				for (int i = 0; i < 12; i++)
				{
					Twelfth twelfth = (Twelfth)i;
					if (twelfths.Contains(twelfth))
					{
						if (!text.NullOrEmpty())
						{
							text += ", ";
						}
						text += SeasonUtility.SeasonsContinuousRangeLabel(twelfths, twelfth, longLat);
					}
				}
				result = text;
			}
			return result;
		}

		// Token: 0x06003609 RID: 13833 RVA: 0x001D021C File Offset: 0x001CE61C
		private static string SeasonsContinuousRangeLabel(List<Twelfth> twelfths, Twelfth rootTwelfth, Vector2 longLat)
		{
			Twelfth leftMostTwelfth = TwelfthUtility.GetLeftMostTwelfth(twelfths, rootTwelfth);
			Twelfth rightMostTwelfth = TwelfthUtility.GetRightMostTwelfth(twelfths, rootTwelfth);
			for (Twelfth twelfth = leftMostTwelfth; twelfth != rightMostTwelfth; twelfth = TwelfthUtility.TwelfthAfter(twelfth))
			{
				if (!twelfths.Contains(twelfth))
				{
					Log.Error(string.Concat(new object[]
					{
						"Twelfths doesn't contain ",
						twelfth,
						" (",
						leftMostTwelfth,
						"..",
						rightMostTwelfth,
						")"
					}), false);
					break;
				}
				twelfths.Remove(twelfth);
			}
			twelfths.Remove(rightMostTwelfth);
			return GenDate.SeasonDateStringAt(leftMostTwelfth, longLat) + " - " + GenDate.SeasonDateStringAt(rightMostTwelfth, longLat);
		}
	}
}
