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
			return (num != 1.0) ? ((num2 != 1.0) ? GenMath.MaxBy(Season.Spring, by, Season.Summer, by2, Season.Fall, by3, Season.Winter, by4) : Season.PermanentWinter) : Season.PermanentSummer;
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
			Twelfth result;
			if (latitude >= 0.0)
			{
				switch (season)
				{
				case Season.Spring:
				{
					result = Twelfth.First;
					goto IL_00bd;
				}
				case Season.Summer:
				{
					result = Twelfth.Fourth;
					goto IL_00bd;
				}
				case Season.Fall:
				{
					result = Twelfth.Seventh;
					goto IL_00bd;
				}
				case Season.Winter:
				{
					result = Twelfth.Tenth;
					goto IL_00bd;
				}
				case Season.PermanentSummer:
				{
					result = Twelfth.First;
					goto IL_00bd;
				}
				case Season.PermanentWinter:
				{
					result = Twelfth.First;
					goto IL_00bd;
				}
				}
			}
			else
			{
				switch (season)
				{
				case Season.Fall:
				{
					result = Twelfth.First;
					goto IL_00bd;
				}
				case Season.Winter:
				{
					result = Twelfth.Fourth;
					goto IL_00bd;
				}
				case Season.Spring:
				{
					result = Twelfth.Seventh;
					goto IL_00bd;
				}
				case Season.Summer:
				{
					result = Twelfth.Tenth;
					goto IL_00bd;
				}
				case Season.PermanentSummer:
				{
					result = Twelfth.First;
					goto IL_00bd;
				}
				case Season.PermanentWinter:
				{
					result = Twelfth.First;
					goto IL_00bd;
				}
				}
			}
			result = Twelfth.Undefined;
			goto IL_00bd;
			IL_00bd:
			return result;
		}

		public static Twelfth GetMiddleTwelfth(this Season season, float latitude)
		{
			Twelfth result;
			if (latitude >= 0.0)
			{
				switch (season)
				{
				case Season.Spring:
				{
					result = Twelfth.Second;
					goto IL_00bd;
				}
				case Season.Summer:
				{
					result = Twelfth.Fifth;
					goto IL_00bd;
				}
				case Season.Fall:
				{
					result = Twelfth.Eighth;
					goto IL_00bd;
				}
				case Season.Winter:
				{
					result = Twelfth.Eleventh;
					goto IL_00bd;
				}
				case Season.PermanentSummer:
				{
					result = Twelfth.Sixth;
					goto IL_00bd;
				}
				case Season.PermanentWinter:
				{
					result = Twelfth.Sixth;
					goto IL_00bd;
				}
				}
			}
			else
			{
				switch (season)
				{
				case Season.Fall:
				{
					result = Twelfth.Second;
					goto IL_00bd;
				}
				case Season.Winter:
				{
					result = Twelfth.Fifth;
					goto IL_00bd;
				}
				case Season.Spring:
				{
					result = Twelfth.Eighth;
					goto IL_00bd;
				}
				case Season.Summer:
				{
					result = Twelfth.Eleventh;
					goto IL_00bd;
				}
				case Season.PermanentSummer:
				{
					result = Twelfth.Sixth;
					goto IL_00bd;
				}
				case Season.PermanentWinter:
				{
					result = Twelfth.Sixth;
					goto IL_00bd;
				}
				}
			}
			result = Twelfth.Undefined;
			goto IL_00bd;
			IL_00bd:
			return result;
		}

		public static Season GetPreviousSeason(this Season season)
		{
			Season result;
			switch (season)
			{
			case Season.Undefined:
			{
				result = Season.Undefined;
				break;
			}
			case Season.Spring:
			{
				result = Season.Winter;
				break;
			}
			case Season.Summer:
			{
				result = Season.Spring;
				break;
			}
			case Season.Fall:
			{
				result = Season.Summer;
				break;
			}
			case Season.Winter:
			{
				result = Season.Fall;
				break;
			}
			case Season.PermanentSummer:
			{
				result = Season.PermanentSummer;
				break;
			}
			case Season.PermanentWinter:
			{
				result = Season.PermanentWinter;
				break;
			}
			default:
			{
				result = Season.Undefined;
				break;
			}
			}
			return result;
		}

		public static float GetMiddleYearPct(this Season season, float latitude)
		{
			return (float)((season != 0) ? season.GetMiddleTwelfth(latitude).GetMiddleYearPct() : 0.5);
		}

		public static string Label(this Season season)
		{
			string result;
			switch (season)
			{
			case Season.Spring:
			{
				result = "SeasonSpring".Translate();
				break;
			}
			case Season.Summer:
			{
				result = "SeasonSummer".Translate();
				break;
			}
			case Season.Fall:
			{
				result = "SeasonFall".Translate();
				break;
			}
			case Season.Winter:
			{
				result = "SeasonWinter".Translate();
				break;
			}
			case Season.PermanentSummer:
			{
				result = "SeasonPermanentSummer".Translate();
				break;
			}
			case Season.PermanentWinter:
			{
				result = "SeasonPermanentWinter".Translate();
				break;
			}
			default:
			{
				result = "Unknown season";
				break;
			}
			}
			return result;
		}

		public static string LabelCap(this Season season)
		{
			return season.Label().CapitalizeFirst();
		}

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
					Twelfth twelfth = (Twelfth)(byte)i;
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
