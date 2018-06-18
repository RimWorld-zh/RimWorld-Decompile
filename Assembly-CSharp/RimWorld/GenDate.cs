using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200090A RID: 2314
	public static class GenDate
	{
		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x060035A5 RID: 13733 RVA: 0x001CDEB0 File Offset: 0x001CC2B0
		private static int TicksGame
		{
			get
			{
				return Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x060035A6 RID: 13734 RVA: 0x001CDED0 File Offset: 0x001CC2D0
		public static int DaysPassed
		{
			get
			{
				return GenDate.DaysPassedAt(GenDate.TicksGame);
			}
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x060035A7 RID: 13735 RVA: 0x001CDEF0 File Offset: 0x001CC2F0
		public static float DaysPassedFloat
		{
			get
			{
				return (float)GenDate.TicksGame / 60000f;
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x060035A8 RID: 13736 RVA: 0x001CDF14 File Offset: 0x001CC314
		public static int TwelfthsPassed
		{
			get
			{
				return GenDate.TwelfthsPassedAt(GenDate.TicksGame);
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x060035A9 RID: 13737 RVA: 0x001CDF34 File Offset: 0x001CC334
		public static float TwelfthsPassedFloat
		{
			get
			{
				return (float)GenDate.TicksGame / 300000f;
			}
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x060035AA RID: 13738 RVA: 0x001CDF58 File Offset: 0x001CC358
		public static int YearsPassed
		{
			get
			{
				return GenDate.YearsPassedAt(GenDate.TicksGame);
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x060035AB RID: 13739 RVA: 0x001CDF78 File Offset: 0x001CC378
		public static float YearsPassedFloat
		{
			get
			{
				return (float)GenDate.TicksGame / 3600000f;
			}
		}

		// Token: 0x060035AC RID: 13740 RVA: 0x001CDF9C File Offset: 0x001CC39C
		public static int TickAbsToGame(int absTick)
		{
			return absTick - Find.TickManager.gameStartAbsTick;
		}

		// Token: 0x060035AD RID: 13741 RVA: 0x001CDFC0 File Offset: 0x001CC3C0
		public static int TickGameToAbs(int gameTick)
		{
			return gameTick + Find.TickManager.gameStartAbsTick;
		}

		// Token: 0x060035AE RID: 13742 RVA: 0x001CDFE4 File Offset: 0x001CC3E4
		public static int DaysPassedAt(int gameTicks)
		{
			return Mathf.FloorToInt((float)gameTicks / 60000f);
		}

		// Token: 0x060035AF RID: 13743 RVA: 0x001CE008 File Offset: 0x001CC408
		public static int TwelfthsPassedAt(int gameTicks)
		{
			return Mathf.FloorToInt((float)gameTicks / 300000f);
		}

		// Token: 0x060035B0 RID: 13744 RVA: 0x001CE02C File Offset: 0x001CC42C
		public static int YearsPassedAt(int gameTicks)
		{
			return Mathf.FloorToInt((float)gameTicks / 3600000f);
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x001CE050 File Offset: 0x001CC450
		private static long LocalTicksOffsetFromLongitude(float longitude)
		{
			return (long)GenDate.TimeZoneAt(longitude) * 2500L;
		}

		// Token: 0x060035B2 RID: 13746 RVA: 0x001CE074 File Offset: 0x001CC474
		public static int HourOfDay(long absTicks, float longitude)
		{
			long x = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
			return GenMath.PositiveModRemap(x, 2500, 24);
		}

		// Token: 0x060035B3 RID: 13747 RVA: 0x001CE0A0 File Offset: 0x001CC4A0
		public static int DayOfTwelfth(long absTicks, float longitude)
		{
			long x = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
			return GenMath.PositiveModRemap(x, 60000, 5);
		}

		// Token: 0x060035B4 RID: 13748 RVA: 0x001CE0CC File Offset: 0x001CC4CC
		public static int DayOfYear(long absTicks, float longitude)
		{
			long x = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
			return GenMath.PositiveModRemap(x, 60000, 60);
		}

		// Token: 0x060035B5 RID: 13749 RVA: 0x001CE0F8 File Offset: 0x001CC4F8
		public static Twelfth Twelfth(long absTicks, float longitude)
		{
			long x = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
			return (Twelfth)GenMath.PositiveModRemap(x, 300000, 12);
		}

		// Token: 0x060035B6 RID: 13750 RVA: 0x001CE124 File Offset: 0x001CC524
		public static Season Season(long absTicks, Vector2 longLat)
		{
			return GenDate.Season(absTicks, longLat.y, longLat.x);
		}

		// Token: 0x060035B7 RID: 13751 RVA: 0x001CE150 File Offset: 0x001CC550
		public static Season Season(long absTicks, float latitude, float longitude)
		{
			float yearPct = GenDate.YearPercent(absTicks, longitude);
			return SeasonUtility.GetReportedSeason(yearPct, latitude);
		}

		// Token: 0x060035B8 RID: 13752 RVA: 0x001CE174 File Offset: 0x001CC574
		public static Quadrum Quadrum(long absTicks, float longitude)
		{
			Twelfth twelfth = GenDate.Twelfth(absTicks, longitude);
			return twelfth.GetQuadrum();
		}

		// Token: 0x060035B9 RID: 13753 RVA: 0x001CE198 File Offset: 0x001CC598
		public static int Year(long absTicks, float longitude)
		{
			long num = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
			return 5500 + Mathf.FloorToInt((float)num / 3600000f);
		}

		// Token: 0x060035BA RID: 13754 RVA: 0x001CE1CC File Offset: 0x001CC5CC
		public static int DayOfSeason(long absTicks, float longitude)
		{
			int num = GenDate.DayOfYear(absTicks, longitude);
			return (num - (int)(SeasonUtility.FirstSeason.GetFirstTwelfth(0f) * RimWorld.Twelfth.Sixth)) % 15;
		}

		// Token: 0x060035BB RID: 13755 RVA: 0x001CE200 File Offset: 0x001CC600
		public static int DayOfQuadrum(long absTicks, float longitude)
		{
			int num = GenDate.DayOfYear(absTicks, longitude);
			return (num - (int)(QuadrumUtility.FirstQuadrum.GetFirstTwelfth() * RimWorld.Twelfth.Sixth)) % 15;
		}

		// Token: 0x060035BC RID: 13756 RVA: 0x001CE230 File Offset: 0x001CC630
		public static float DayPercent(long absTicks, float longitude)
		{
			long x = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
			int num = (int)GenMath.PositiveMod(x, 60000L);
			if (num == 0)
			{
				num = 1;
			}
			return (float)num / 60000f;
		}

		// Token: 0x060035BD RID: 13757 RVA: 0x001CE26C File Offset: 0x001CC66C
		public static float YearPercent(long absTicks, float longitude)
		{
			long x = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
			int num = (int)GenMath.PositiveMod(x, 3600000L);
			return (float)num / 3600000f;
		}

		// Token: 0x060035BE RID: 13758 RVA: 0x001CE2A0 File Offset: 0x001CC6A0
		public static int HourInteger(long absTicks, float longitude)
		{
			long x = absTicks + GenDate.LocalTicksOffsetFromLongitude(longitude);
			return GenMath.PositiveModRemap(x, 2500, 24);
		}

		// Token: 0x060035BF RID: 13759 RVA: 0x001CE2CC File Offset: 0x001CC6CC
		public static float HourFloat(long absTicks, float longitude)
		{
			return GenDate.DayPercent(absTicks, longitude) * 24f;
		}

		// Token: 0x060035C0 RID: 13760 RVA: 0x001CE2F0 File Offset: 0x001CC6F0
		public static string DateFullStringAt(long absTicks, Vector2 location)
		{
			int num = GenDate.DayOfSeason(absTicks, location.x) + 1;
			string text = Find.ActiveLanguageWorker.OrdinalNumber(num);
			return "FullDate".Translate(new object[]
			{
				text,
				GenDate.Quadrum(absTicks, location.x).Label(),
				GenDate.Year(absTicks, location.x),
				num
			});
		}

		// Token: 0x060035C1 RID: 13761 RVA: 0x001CE368 File Offset: 0x001CC768
		public static string DateReadoutStringAt(long absTicks, Vector2 location)
		{
			int num = GenDate.DayOfSeason(absTicks, location.x) + 1;
			string text = Find.ActiveLanguageWorker.OrdinalNumber(num);
			return "DateReadout".Translate(new object[]
			{
				text,
				GenDate.Quadrum(absTicks, location.x).Label(),
				GenDate.Year(absTicks, location.x),
				num
			});
		}

		// Token: 0x060035C2 RID: 13762 RVA: 0x001CE3E0 File Offset: 0x001CC7E0
		public static string SeasonDateStringAt(long absTicks, Vector2 longLat)
		{
			int num = GenDate.DayOfSeason(absTicks, longLat.x) + 1;
			string text = Find.ActiveLanguageWorker.OrdinalNumber(num);
			return "SeasonFullDate".Translate(new object[]
			{
				text,
				GenDate.Season(absTicks, longLat).Label(),
				num
			});
		}

		// Token: 0x060035C3 RID: 13763 RVA: 0x001CE440 File Offset: 0x001CC840
		public static string SeasonDateStringAt(Twelfth twelfth, Vector2 longLat)
		{
			return GenDate.SeasonDateStringAt((long)((int)twelfth * 300000 + 1), longLat);
		}

		// Token: 0x060035C4 RID: 13764 RVA: 0x001CE468 File Offset: 0x001CC868
		public static string QuadrumDateStringAt(long absTicks, float longitude)
		{
			int num = GenDate.DayOfQuadrum(absTicks, longitude) + 1;
			string text = Find.ActiveLanguageWorker.OrdinalNumber(num);
			return "SeasonFullDate".Translate(new object[]
			{
				text,
				GenDate.Quadrum(absTicks, longitude).Label(),
				num
			});
		}

		// Token: 0x060035C5 RID: 13765 RVA: 0x001CE4C0 File Offset: 0x001CC8C0
		public static string QuadrumDateStringAt(Quadrum quadrum)
		{
			return GenDate.QuadrumDateStringAt((long)((int)quadrum * 900000 + 1), 0f);
		}

		// Token: 0x060035C6 RID: 13766 RVA: 0x001CE4EC File Offset: 0x001CC8EC
		public static string QuadrumDateStringAt(Twelfth twelfth)
		{
			return GenDate.QuadrumDateStringAt((long)((int)twelfth * 300000 + 1), 0f);
		}

		// Token: 0x060035C7 RID: 13767 RVA: 0x001CE518 File Offset: 0x001CC918
		public static float TicksToDays(this int numTicks)
		{
			return (float)numTicks / 60000f;
		}

		// Token: 0x060035C8 RID: 13768 RVA: 0x001CE538 File Offset: 0x001CC938
		public static string ToStringTicksToDays(this int numTicks, string format = "F1")
		{
			string text = numTicks.TicksToDays().ToString(format);
			string result;
			if (text == "1")
			{
				result = "Period1Day".Translate();
			}
			else
			{
				result = text + " " + "DaysLower".Translate();
			}
			return result;
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x001CE594 File Offset: 0x001CC994
		public static string ToStringTicksToPeriod(this int numTicks)
		{
			string result;
			if (numTicks < 2500 && (numTicks < 600 || Math.Round((double)((float)numTicks / 2500f), 1) == 0.0))
			{
				int num = Mathf.RoundToInt((float)numTicks / 60f);
				if (num == 1)
				{
					result = "Period1Second".Translate();
				}
				else
				{
					result = "PeriodSeconds".Translate(new object[]
					{
						num
					});
				}
			}
			else if (numTicks < 60000)
			{
				if (numTicks < 2500)
				{
					string text = ((float)numTicks / 2500f).ToString("0.#");
					if (text == "1")
					{
						result = "Period1Hour".Translate();
					}
					else
					{
						result = "PeriodHours".Translate(new object[]
						{
							text
						});
					}
				}
				else
				{
					int num2 = Mathf.RoundToInt((float)numTicks / 2500f);
					if (num2 == 1)
					{
						result = "Period1Hour".Translate();
					}
					else
					{
						result = "PeriodHours".Translate(new object[]
						{
							num2
						});
					}
				}
			}
			else if (numTicks < 3600000)
			{
				string text2 = ((float)numTicks / 60000f).ToStringDecimalIfSmall();
				if (text2 == "1")
				{
					result = "Period1Day".Translate();
				}
				else
				{
					result = "PeriodDays".Translate(new object[]
					{
						text2
					});
				}
			}
			else
			{
				string text3 = ((float)numTicks / 3600000f).ToStringDecimalIfSmall();
				if (text3 == "1")
				{
					result = "Period1Year".Translate();
				}
				else
				{
					result = "PeriodYears".Translate(new object[]
					{
						text3
					});
				}
			}
			return result;
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x001CE770 File Offset: 0x001CCB70
		public static string ToStringTicksToPeriodVerbose(this int numTicks, bool allowHours = true, bool allowQuadrums = true)
		{
			string result;
			if (numTicks < 0)
			{
				result = "0";
			}
			else
			{
				int num;
				int num2;
				int num3;
				float num4;
				numTicks.TicksToPeriod(out num, out num2, out num3, out num4);
				if (!allowQuadrums)
				{
					num3 += 15 * num2;
					num2 = 0;
				}
				if (num > 0)
				{
					string text;
					if (num == 1)
					{
						text = "Period1Year".Translate();
					}
					else
					{
						text = "PeriodYears".Translate(new object[]
						{
							num
						});
					}
					if (num2 > 0)
					{
						text += ", ";
						if (num2 == 1)
						{
							text += "Period1Quadrum".Translate();
						}
						else
						{
							text += "PeriodQuadrums".Translate(new object[]
							{
								num2
							});
						}
					}
					result = text;
				}
				else if (num2 > 0)
				{
					string text2;
					if (num2 == 1)
					{
						text2 = "Period1Quadrum".Translate();
					}
					else
					{
						text2 = "PeriodQuadrums".Translate(new object[]
						{
							num2
						});
					}
					if (num3 > 0)
					{
						text2 += ", ";
						if (num3 == 1)
						{
							text2 += "Period1Day".Translate();
						}
						else
						{
							text2 += "PeriodDays".Translate(new object[]
							{
								num3
							});
						}
					}
					result = text2;
				}
				else if (num3 > 0)
				{
					string text3;
					if (num3 == 1)
					{
						text3 = "Period1Day".Translate();
					}
					else
					{
						text3 = "PeriodDays".Translate(new object[]
						{
							num3
						});
					}
					int num5 = (int)num4;
					if (allowHours && num5 > 0)
					{
						text3 += ", ";
						if (num5 == 1)
						{
							text3 += "Period1Hour".Translate();
						}
						else
						{
							text3 += "PeriodHours".Translate(new object[]
							{
								num5
							});
						}
					}
					result = text3;
				}
				else if (allowHours)
				{
					if (num4 > 1f)
					{
						int num6 = Mathf.RoundToInt(num4);
						if (num6 == 1)
						{
							result = "Period1Hour".Translate();
						}
						else
						{
							result = "PeriodHours".Translate(new object[]
							{
								num6
							});
						}
					}
					else if (Math.Round((double)num4, 1) == 1.0)
					{
						result = "Period1Hour".Translate();
					}
					else
					{
						result = "PeriodHours".Translate(new object[]
						{
							num4.ToString("0.#")
						});
					}
				}
				else
				{
					result = "PeriodDays".Translate(new object[]
					{
						0
					});
				}
			}
			return result;
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x001CEA50 File Offset: 0x001CCE50
		public static string ToStringTicksToPeriodVague(this int numTicks, bool vagueMin = true, bool vagueMax = true)
		{
			string result;
			if (vagueMax && numTicks > 36000000)
			{
				result = "OverADecade".Translate();
			}
			else if (vagueMin && numTicks < 60000)
			{
				result = "LessThanADay".Translate();
			}
			else
			{
				result = numTicks.ToStringTicksToPeriod();
			}
			return result;
		}

		// Token: 0x060035CC RID: 13772 RVA: 0x001CEAAD File Offset: 0x001CCEAD
		public static void TicksToPeriod(this int numTicks, out int years, out int quadrums, out int days, out float hoursFloat)
		{
			((long)numTicks).TicksToPeriod(out years, out quadrums, out days, out hoursFloat);
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x001CEABC File Offset: 0x001CCEBC
		public static void TicksToPeriod(this long numTicks, out int years, out int quadrums, out int days, out float hoursFloat)
		{
			if (numTicks < 0L)
			{
				Log.ErrorOnce("Tried to calculate period for negative ticks", 12841103, false);
			}
			years = (int)(numTicks / 3600000L);
			long num = numTicks - (long)years * 3600000L;
			quadrums = (int)(num / 900000L);
			num -= (long)quadrums * 900000L;
			days = (int)(num / 60000L);
			num -= (long)days * 60000L;
			hoursFloat = (float)num / 2500f;
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x001CEB38 File Offset: 0x001CCF38
		public static string ToStringApproxAge(this float yearsFloat)
		{
			string result;
			if (yearsFloat >= 1f)
			{
				result = ((int)yearsFloat).ToStringCached();
			}
			else
			{
				int num = (int)(yearsFloat * 3600000f);
				num = Mathf.Min(num, 3599999);
				int num2;
				int num3;
				int num4;
				float num5;
				num.TicksToPeriod(out num2, out num3, out num4, out num5);
				if (num2 > 0)
				{
					if (num2 == 1)
					{
						result = "Period1Year".Translate();
					}
					else
					{
						result = "PeriodYears".Translate(new object[]
						{
							num2
						});
					}
				}
				else if (num3 > 0)
				{
					if (num3 == 1)
					{
						result = "Period1Quadrum".Translate();
					}
					else
					{
						result = "PeriodQuadrums".Translate(new object[]
						{
							num3
						});
					}
				}
				else if (num4 > 0)
				{
					if (num4 == 1)
					{
						result = "Period1Day".Translate();
					}
					else
					{
						result = "PeriodDays".Translate(new object[]
						{
							num4
						});
					}
				}
				else
				{
					int num6 = (int)num5;
					if (num6 == 1)
					{
						result = "Period1Hour".Translate();
					}
					else
					{
						result = "PeriodHours".Translate(new object[]
						{
							num6
						});
					}
				}
			}
			return result;
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x001CEC7C File Offset: 0x001CD07C
		public static int TimeZoneAt(float longitude)
		{
			return Mathf.RoundToInt(GenDate.TimeZoneFloatAt(longitude));
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x001CEC9C File Offset: 0x001CD09C
		public static float TimeZoneFloatAt(float longitude)
		{
			return longitude / 15f;
		}

		// Token: 0x04001D0D RID: 7437
		public const int TicksPerDay = 60000;

		// Token: 0x04001D0E RID: 7438
		public const int HoursPerDay = 24;

		// Token: 0x04001D0F RID: 7439
		public const int DaysPerTwelfth = 5;

		// Token: 0x04001D10 RID: 7440
		public const int TwelfthsPerYear = 12;

		// Token: 0x04001D11 RID: 7441
		public const int GameStartHourOfDay = 6;

		// Token: 0x04001D12 RID: 7442
		public const int TicksPerTwelfth = 300000;

		// Token: 0x04001D13 RID: 7443
		public const int TicksPerSeason = 900000;

		// Token: 0x04001D14 RID: 7444
		public const int TicksPerQuadrum = 900000;

		// Token: 0x04001D15 RID: 7445
		public const int TicksPerYear = 3600000;

		// Token: 0x04001D16 RID: 7446
		public const int DaysPerYear = 60;

		// Token: 0x04001D17 RID: 7447
		public const int DaysPerSeason = 15;

		// Token: 0x04001D18 RID: 7448
		public const int DaysPerQuadrum = 15;

		// Token: 0x04001D19 RID: 7449
		public const int TicksPerHour = 2500;

		// Token: 0x04001D1A RID: 7450
		public const float TimeZoneWidth = 15f;

		// Token: 0x04001D1B RID: 7451
		public const int DefaultStartingYear = 5500;
	}
}
