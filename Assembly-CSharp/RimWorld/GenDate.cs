using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class GenDate
	{
		public const int TicksPerDay = 60000;

		public const int HoursPerDay = 24;

		public const int DaysPerTwelfth = 5;

		public const int TwelfthsPerYear = 12;

		public const int GameStartHourOfDay = 6;

		public const int TicksPerTwelfth = 300000;

		public const int TicksPerSeason = 900000;

		public const int TicksPerQuadrum = 900000;

		public const int TicksPerYear = 3600000;

		public const int DaysPerYear = 60;

		public const int DaysPerSeason = 15;

		public const int DaysPerQuadrum = 15;

		public const int TicksPerHour = 2500;

		public const float TimeZoneWidth = 15f;

		public const int DefaultStartingYear = 5500;

		private static int TicksGame
		{
			get
			{
				return Find.TickManager.TicksGame;
			}
		}

		public static int DaysPassed
		{
			get
			{
				return GenDate.DaysPassedAt(GenDate.TicksGame);
			}
		}

		public static float DaysPassedFloat
		{
			get
			{
				return (float)((float)GenDate.TicksGame / 60000.0);
			}
		}

		public static int TwelfthsPassed
		{
			get
			{
				return GenDate.TwelfthsPassedAt(GenDate.TicksGame);
			}
		}

		public static float TwelfthsPassedFloat
		{
			get
			{
				return (float)((float)GenDate.TicksGame / 300000.0);
			}
		}

		public static int YearsPassed
		{
			get
			{
				return GenDate.YearsPassedAt(GenDate.TicksGame);
			}
		}

		public static float YearsPassedFloat
		{
			get
			{
				return (float)((float)GenDate.TicksGame / 3600000.0);
			}
		}

		public static int TickAbsToGame(int absTick)
		{
			return absTick - Find.TickManager.gameStartAbsTick;
		}

		public static int TickGameToAbs(int gameTick)
		{
			return gameTick + Find.TickManager.gameStartAbsTick;
		}

		public static int DaysPassedAt(int gameticks)
		{
			return gameticks / 60000;
		}

		public static int TwelfthsPassedAt(int gameticks)
		{
			return gameticks / 300000;
		}

		public static int YearsPassedAt(int gameTicks)
		{
			return gameTicks / 3600000;
		}

		private static long LocalTicksOffsetFromLongitude(float longitude)
		{
			return (long)GenDate.TimeZoneAt(longitude) * 2500L;
		}

		public static int HourOfDay(long absTicks, float longitude)
		{
			long num = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
			return (int)(num % 60000 / 2500);
		}

		public static int DayOfTwelfth(long absTicks, float longitude)
		{
			long num = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
			int num2 = (int)(num / 60000 % 5);
			if (num2 < 0)
			{
				num2 += 5;
			}
			return num2;
		}

		public static int DayOfYear(long absTicks, float longitude)
		{
			long num = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
			int num2 = (int)(num / 60000) % 60;
			if (num2 < 0)
			{
				num2 += 60;
			}
			return num2;
		}

		public static Twelfth Twelfth(long absTicks, float longitude)
		{
			long num = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
			int num2 = (int)(num / 300000 % 12);
			if (num2 < 0)
			{
				num2 += 12;
			}
			return (Twelfth)(byte)num2;
		}

		public static Season Season(long absTicks, Vector2 longLat)
		{
			return GenDate.Season(absTicks, longLat.y, longLat.x);
		}

		public static Season Season(long absTicks, float latitude, float longitude)
		{
			float yearPct = GenDate.YearPercent(absTicks, longitude);
			return SeasonUtility.GetReportedSeason(yearPct, latitude);
		}

		public static Quadrum Quadrum(long absTicks, float longitude)
		{
			Twelfth twelfth = GenDate.Twelfth(absTicks, longitude);
			return twelfth.GetQuadrum();
		}

		public static int Year(long absTicks, float longitude)
		{
			long num = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
			int num2 = (int)(num / 3600000);
			if (num < 0)
			{
				num2--;
			}
			return 5500 + num2;
		}

		public static int DayOfSeason(long absTicks, float longitude)
		{
			int num = GenDate.DayOfYear(absTicks, longitude);
			return (num - (int)SeasonUtility.FirstSeason.GetFirstTwelfth(0f) * 5) % 15;
		}

		public static int DayOfQuadrum(long absTicks, float longitude)
		{
			int num = GenDate.DayOfYear(absTicks, longitude);
			return (num - (int)QuadrumUtility.FirstQuadrum.GetFirstTwelfth() * 5) % 15;
		}

		public static float DayPercent(long absTicks, float longitude)
		{
			long x = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
			int num = (int)GenMath.PositiveMod(x, 60000L);
			if (num == 0)
			{
				num = 1;
			}
			return (float)((float)num / 60000.0);
		}

		public static float YearPercent(long absTicks, float longitude)
		{
			long x = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
			int num = (int)GenMath.PositiveMod(x, 3600000L);
			return (float)((float)num / 3600000.0);
		}

		public static int HourInteger(long absTicks, float longitude)
		{
			long x = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
			int num = (int)GenMath.PositiveMod(x, 60000L);
			return num / 2500;
		}

		public static float HourFloat(long absTicks, float longitude)
		{
			return (float)(GenDate.DayPercent(absTicks, longitude) * 24.0);
		}

		public static string DateFullStringAt(long absTicks, Vector2 location)
		{
			int num = GenDate.DayOfSeason(absTicks, location.x) + 1;
			string text = Find.ActiveLanguageWorker.OrdinalNumber(num);
			return "FullDate".Translate(text, GenDate.Quadrum(absTicks, location.x).Label(), GenDate.Year(absTicks, location.x), num);
		}

		public static string DateReadoutStringAt(long absTicks, Vector2 location)
		{
			int num = GenDate.DayOfSeason(absTicks, location.x) + 1;
			string text = Find.ActiveLanguageWorker.OrdinalNumber(num);
			return "DateReadout".Translate(text, GenDate.Quadrum(absTicks, location.x).Label(), GenDate.Year(absTicks, location.x), num);
		}

		public static string SeasonDateStringAt(long absTicks, Vector2 longLat)
		{
			int num = GenDate.DayOfSeason(absTicks, longLat.x) + 1;
			string text = Find.ActiveLanguageWorker.OrdinalNumber(num);
			return "SeasonFullDate".Translate(text, GenDate.Season(absTicks, longLat).Label(), num);
		}

		public static string SeasonDateStringAt(Twelfth twelfth, Vector2 longLat)
		{
			return GenDate.SeasonDateStringAt((int)twelfth * 300000 + 1, longLat);
		}

		public static string QuadrumDateStringAt(long absTicks, float longitude)
		{
			int num = GenDate.DayOfQuadrum(absTicks, longitude) + 1;
			string text = Find.ActiveLanguageWorker.OrdinalNumber(num);
			return "SeasonFullDate".Translate(text, GenDate.Quadrum(absTicks, longitude).Label(), num);
		}

		public static string QuadrumDateStringAt(Quadrum quadrum)
		{
			return GenDate.QuadrumDateStringAt((int)quadrum * 900000 + 1, 0f);
		}

		public static string QuadrumDateStringAt(Twelfth twelfth)
		{
			return GenDate.QuadrumDateStringAt((int)twelfth * 300000 + 1, 0f);
		}

		public static float TicksToDays(this int numTicks)
		{
			return (float)((float)numTicks / 60000.0);
		}

		public static string ToStringTicksToDays(this int numTicks, string format = "F1")
		{
			return numTicks.TicksToDays().ToString(format) + " " + "DaysLower".Translate();
		}

		public static string ToStringTicksToPeriod(this int numTicks, bool allowHours = true, bool hoursMax1DecimalPlace = false, bool allowQuadrums = true)
		{
			if (numTicks < 0)
			{
				return "0";
			}
			int num = default(int);
			int num2 = default(int);
			int num3 = default(int);
			float num4 = default(float);
			numTicks.TicksToPeriod(out num, out num2, out num3, out num4);
			if (!allowQuadrums)
			{
				num3 += 15 * num2;
				num2 = 0;
			}
			if (num > 0)
			{
				string text = (num != 1) ? "PeriodYears".Translate(num) : "Period1Year".Translate();
				if (num2 > 0)
				{
					text += ", ";
					text = ((num2 != 1) ? (text + "PeriodQuadrums".Translate(num2)) : (text + "Period1Quadrum".Translate()));
				}
				return text;
			}
			if (num2 > 0)
			{
				string text2 = (num2 != 1) ? "PeriodQuadrums".Translate(num2) : "Period1Quadrum".Translate();
				if (num3 > 0)
				{
					text2 += ", ";
					text2 = ((num3 != 1) ? (text2 + "PeriodDays".Translate(num3)) : (text2 + "Period1Day".Translate()));
				}
				return text2;
			}
			if (num3 > 0)
			{
				string text3 = (num3 != 1) ? "PeriodDays".Translate(num3) : "Period1Day".Translate();
				int num5 = (int)num4;
				if (allowHours && num5 > 0)
				{
					text3 += ", ";
					text3 = ((num5 != 1) ? (text3 + "PeriodHours".Translate(num5)) : (text3 + "Period1Hour".Translate()));
				}
				return text3;
			}
			if (allowHours)
			{
				if (hoursMax1DecimalPlace)
				{
					if (num4 > 1.0)
					{
						int num6 = Mathf.RoundToInt(num4);
						if (num6 == 1)
						{
							return "Period1Hour".Translate();
						}
						return "PeriodHours".Translate(num6);
					}
					if (Math.Round((double)num4, 1) == 1.0)
					{
						return "Period1Hour".Translate();
					}
					return "PeriodHours".Translate(num4.ToString("0.#"));
				}
				if (Math.Round((double)num4, 2) == 1.0)
				{
					return "Period1Hour".Translate();
				}
				return "PeriodHours".Translate(num4.ToStringDecimalIfSmall());
			}
			return "LessThanADay".Translate();
		}

		public static string ToStringTicksToPeriodVagueMax(this int numTicks)
		{
			if (numTicks > 36000000)
			{
				return "OverADecade".Translate();
			}
			return numTicks.ToStringTicksToPeriod(false, false, true);
		}

		public static void TicksToPeriod(this int numTicks, out int years, out int quadrums, out int days, out float hoursFloat)
		{
			((long)numTicks).TicksToPeriod(out years, out quadrums, out days, out hoursFloat);
		}

		public static void TicksToPeriod(this long numTicks, out int years, out int quadrums, out int days, out float hoursFloat)
		{
			years = (int)(numTicks / 3600000);
			long num = numTicks - (long)years * 3600000L;
			quadrums = (int)(num / 900000);
			num -= (long)quadrums * 900000L;
			days = (int)(num / 60000);
			num -= (long)days * 60000L;
			hoursFloat = (float)((float)num / 2500.0);
		}

		public static string ToStringApproxAge(this float yearsFloat)
		{
			if (yearsFloat >= 1.0)
			{
				return ((int)yearsFloat).ToStringCached();
			}
			int a = (int)(yearsFloat * 3600000.0);
			a = Mathf.Min(a, 3599999);
			int num = default(int);
			int num2 = default(int);
			int num3 = default(int);
			float num4 = default(float);
			a.TicksToPeriod(out num, out num2, out num3, out num4);
			if (num > 0)
			{
				if (num == 1)
				{
					return "Period1Year".Translate();
				}
				return "PeriodYears".Translate(num);
			}
			if (num2 > 0)
			{
				if (num2 == 1)
				{
					return "Period1Quadrum".Translate();
				}
				return "PeriodQuadrums".Translate(num2);
			}
			if (num3 > 0)
			{
				if (num3 == 1)
				{
					return "Period1Day".Translate();
				}
				return "PeriodDays".Translate(num3);
			}
			int num5 = (int)num4;
			if (num5 == 1)
			{
				return "Period1Hour".Translate();
			}
			return "PeriodHours".Translate(num5);
		}

		public static int TimeZoneAt(float longitude)
		{
			return Mathf.RoundToInt(GenDate.TimeZoneFloatAt(longitude));
		}

		public static float TimeZoneFloatAt(float longitude)
		{
			return (float)(longitude / 15.0);
		}
	}
}
