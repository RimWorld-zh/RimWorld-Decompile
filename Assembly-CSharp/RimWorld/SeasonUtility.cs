using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class SeasonUtility
	{
		private const float HemisphereLerpDistance = 5f;

		private const float SeasonYearPctLerpDistance = 0.085f;

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

		public static Season FirstSeason
		{
			get
			{
				return Season.Spring;
			}
		}

		public static Season GetReportedSeason(float yearPct, float latitude)
		{
			float by = default(float);
			float by2 = default(float);
			float by3 = default(float);
			float by4 = default(float);
			float num = default(float);
			float num2 = default(float);
			SeasonUtility.GetSeason(yearPct, latitude, out by, out by2, out by3, out by4, out num, out num2);
			if (num == 1.0)
			{
				return Season.PermanentSummer;
			}
			if (num2 == 1.0)
			{
				return Season.PermanentWinter;
			}
			return GenMath.MaxBy(Season.Spring, by, Season.Summer, by2, Season.Fall, by3, Season.Winter, by4);
		}

		public static Season GetDominantSeason(float yearPct, float latitude)
		{
			float by = default(float);
			float by2 = default(float);
			float by3 = default(float);
			float by4 = default(float);
			float by5 = default(float);
			float by6 = default(float);
			SeasonUtility.GetSeason(yearPct, latitude, out by, out by2, out by3, out by4, out by5, out by6);
			return GenMath.MaxBy(Season.Spring, by, Season.Summer, by2, Season.Fall, by3, Season.Winter, by4, Season.PermanentSummer, by5, Season.PermanentWinter, by6);
		}

		public static void GetSeason(float yearPct, float latitude, out float spring, out float summer, out float fall, out float winter, out float permanentSummer, out float permanentWinter)
		{
			yearPct = Mathf.Clamp01(yearPct);
			float num = default(float);
			float num2 = default(float);
			float num3 = default(float);
			LatitudeSectionUtility.GetLatitudeSection(latitude, out num, out num2, out num3);
			float num4 = default(float);
			float num5 = default(float);
			float num6 = default(float);
			float num7 = default(float);
			SeasonUtility.GetSeasonalAreaSeason(yearPct, out num4, out num5, out num6, out num7, true);
			float num8 = default(float);
			float num9 = default(float);
			float num10 = default(float);
			float num11 = default(float);
			SeasonUtility.GetSeasonalAreaSeason(yearPct, out num8, out num9, out num10, out num11, false);
			float num12 = Mathf.InverseLerp(-2.5f, 2.5f, latitude);
			float num13 = (float)(num12 * num4 + (1.0 - num12) * num8);
			float num14 = (float)(num12 * num5 + (1.0 - num12) * num9);
			float num15 = (float)(num12 * num6 + (1.0 - num12) * num10);
			float num16 = (float)(num12 * num7 + (1.0 - num12) * num11);
			spring = num13 * num2;
			summer = num14 * num2;
			fall = num15 * num2;
			winter = num16 * num2;
			permanentSummer = num;
			permanentWinter = num3;
		}

		private static void GetSeasonalAreaSeason(float yearPct, out float spring, out float summer, out float fall, out float winter, bool northernHemisphere)
		{
			yearPct = Mathf.Clamp01(yearPct);
			float x = (float)((!northernHemisphere) ? ((yearPct + 0.5) % 1.0) : yearPct);
			float num = SeasonUtility.SeasonalAreaSeasons.Evaluate(x);
			if (num <= 1.0)
			{
				winter = (float)(1.0 - num);
				spring = num;
				summer = 0f;
				fall = 0f;
			}
			else if (num <= 2.0)
			{
				spring = (float)(1.0 - (num - 1.0));
				summer = (float)(num - 1.0);
				fall = 0f;
				winter = 0f;
			}
			else if (num <= 3.0)
			{
				summer = (float)(1.0 - (num - 2.0));
				fall = (float)(num - 2.0);
				spring = 0f;
				winter = 0f;
			}
			else if (num <= 4.0)
			{
				fall = (float)(1.0 - (num - 3.0));
				winter = (float)(num - 3.0);
				spring = 0f;
				summer = 0f;
			}
			else
			{
				winter = (float)(1.0 - (num - 4.0));
				spring = (float)(num - 4.0);
				summer = 0f;
				fall = 0f;
			}
		}

		public static Twelfth GetFirstTwelfth(this Season season, float latitude)
		{
			if (latitude >= 0.0)
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
				case Season.Fall:
					return Twelfth.First;
				case Season.Winter:
					return Twelfth.Fourth;
				case Season.Spring:
					return Twelfth.Seventh;
				case Season.Summer:
					return Twelfth.Tenth;
				case Season.PermanentSummer:
					return Twelfth.First;
				case Season.PermanentWinter:
					return Twelfth.First;
				}
			}
			return Twelfth.Undefined;
		}

		public static Twelfth GetMiddleTwelfth(this Season season, float latitude)
		{
			if (latitude >= 0.0)
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
				case Season.Fall:
					return Twelfth.Second;
				case Season.Winter:
					return Twelfth.Fifth;
				case Season.Spring:
					return Twelfth.Eighth;
				case Season.Summer:
					return Twelfth.Eleventh;
				case Season.PermanentSummer:
					return Twelfth.Sixth;
				case Season.PermanentWinter:
					return Twelfth.Sixth;
				}
			}
			return Twelfth.Undefined;
		}

		public static Season GetPreviousSeason(this Season season)
		{
			switch (season)
			{
			case Season.Undefined:
				return Season.Undefined;
			case Season.Spring:
				return Season.Winter;
			case Season.Summer:
				return Season.Spring;
			case Season.Fall:
				return Season.Summer;
			case Season.Winter:
				return Season.Fall;
			case Season.PermanentSummer:
				return Season.PermanentSummer;
			case Season.PermanentWinter:
				return Season.PermanentWinter;
			default:
				return Season.Undefined;
			}
		}

		public static float GetMiddleYearPct(this Season season, float latitude)
		{
			if (season == Season.Undefined)
			{
				return 0.5f;
			}
			return season.GetMiddleTwelfth(latitude).GetMiddleYearPct();
		}

		public static string Label(this Season season)
		{
			switch (season)
			{
			case Season.Spring:
				return "SeasonSpring".Translate();
			case Season.Summer:
				return "SeasonSummer".Translate();
			case Season.Fall:
				return "SeasonFall".Translate();
			case Season.Winter:
				return "SeasonWinter".Translate();
			case Season.PermanentSummer:
				return "SeasonPermanentSummer".Translate();
			case Season.PermanentWinter:
				return "SeasonPermanentWinter".Translate();
			default:
				return "Unknown season";
			}
		}

		public static string LabelCap(this Season season)
		{
			return season.Label().CapitalizeFirst();
		}

		public static string SeasonsRangeLabel(List<Twelfth> twelfths, Vector2 longLat)
		{
			if (twelfths.Count == 0)
			{
				return string.Empty;
			}
			if (twelfths.Count == 12)
			{
				return "WholeYear".Translate();
			}
			string text = string.Empty;
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
			return text;
		}

		private static string SeasonsContinuousRangeLabel(List<Twelfth> twelfths, Twelfth rootTwelfth, Vector2 longLat)
		{
			Twelfth leftMostTwelfth = TwelfthUtility.GetLeftMostTwelfth(twelfths, rootTwelfth);
			Twelfth rightMostTwelfth = TwelfthUtility.GetRightMostTwelfth(twelfths, rootTwelfth);
			Twelfth twelfth = leftMostTwelfth;
			while (twelfth != rightMostTwelfth)
			{
				if (twelfths.Contains(twelfth))
				{
					twelfths.Remove(twelfth);
					twelfth = TwelfthUtility.TwelfthAfter(twelfth);
					continue;
				}
				Log.Error("Twelfths doesn't contain " + twelfth + " (" + leftMostTwelfth + ".." + rightMostTwelfth + ")");
				break;
			}
			twelfths.Remove(rightMostTwelfth);
			return GenDate.SeasonDateStringAt(leftMostTwelfth, longLat) + " - " + GenDate.SeasonDateStringAt(rightMostTwelfth, longLat);
		}
	}
}
