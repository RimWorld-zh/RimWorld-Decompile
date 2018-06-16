using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200090B RID: 2315
	public static class GenLocalDate
	{
		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x060035CF RID: 13775 RVA: 0x001CEBF0 File Offset: 0x001CCFF0
		private static int TicksAbs
		{
			get
			{
				return GenTicks.TicksAbs;
			}
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x001CEC0C File Offset: 0x001CD00C
		public static int DayOfYear(Map map)
		{
			return GenLocalDate.DayOfYear(map.Tile);
		}

		// Token: 0x060035D1 RID: 13777 RVA: 0x001CEC2C File Offset: 0x001CD02C
		public static int HourOfDay(Map map)
		{
			return GenLocalDate.HourOfDay(map.Tile);
		}

		// Token: 0x060035D2 RID: 13778 RVA: 0x001CEC4C File Offset: 0x001CD04C
		public static int DayOfTwelfth(Map map)
		{
			return GenLocalDate.DayOfTwelfth(map.Tile);
		}

		// Token: 0x060035D3 RID: 13779 RVA: 0x001CEC6C File Offset: 0x001CD06C
		public static Twelfth Twelfth(Map map)
		{
			return GenLocalDate.Twelfth(map.Tile);
		}

		// Token: 0x060035D4 RID: 13780 RVA: 0x001CEC8C File Offset: 0x001CD08C
		public static Season Season(Map map)
		{
			return GenLocalDate.Season(map.Tile);
		}

		// Token: 0x060035D5 RID: 13781 RVA: 0x001CECAC File Offset: 0x001CD0AC
		public static int Year(Map map)
		{
			int result;
			if (Current.ProgramState != ProgramState.Playing)
			{
				result = 5500;
			}
			else
			{
				result = GenLocalDate.Year(map.Tile);
			}
			return result;
		}

		// Token: 0x060035D6 RID: 13782 RVA: 0x001CECE4 File Offset: 0x001CD0E4
		public static int DayOfSeason(Map map)
		{
			return GenLocalDate.DayOfSeason(map.Tile);
		}

		// Token: 0x060035D7 RID: 13783 RVA: 0x001CED04 File Offset: 0x001CD104
		public static int DayOfQuadrum(Map map)
		{
			return GenLocalDate.DayOfQuadrum(map.Tile);
		}

		// Token: 0x060035D8 RID: 13784 RVA: 0x001CED24 File Offset: 0x001CD124
		public static float DayPercent(Map map)
		{
			return GenLocalDate.DayPercent(map.Tile);
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x001CED44 File Offset: 0x001CD144
		public static float YearPercent(Map map)
		{
			return GenLocalDate.YearPercent(map.Tile);
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x001CED64 File Offset: 0x001CD164
		public static int HourInteger(Map map)
		{
			return GenLocalDate.HourInteger(map.Tile);
		}

		// Token: 0x060035DB RID: 13787 RVA: 0x001CED84 File Offset: 0x001CD184
		public static float HourFloat(Map map)
		{
			return GenLocalDate.HourFloat(map.Tile);
		}

		// Token: 0x060035DC RID: 13788 RVA: 0x001CEDA4 File Offset: 0x001CD1A4
		public static int DayOfYear(Thing thing)
		{
			int result;
			if (Current.ProgramState == ProgramState.Playing)
			{
				result = GenDate.DayOfYear((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x060035DD RID: 13789 RVA: 0x001CEDDC File Offset: 0x001CD1DC
		public static int HourOfDay(Thing thing)
		{
			return GenDate.HourOfDay((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x001CEE04 File Offset: 0x001CD204
		public static int DayOfTwelfth(Thing thing)
		{
			return GenDate.DayOfTwelfth((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x001CEE2C File Offset: 0x001CD22C
		public static Twelfth Twelfth(Thing thing)
		{
			return GenDate.Twelfth((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E0 RID: 13792 RVA: 0x001CEE54 File Offset: 0x001CD254
		public static Season Season(Thing thing)
		{
			return GenDate.Season((long)GenLocalDate.TicksAbs, GenLocalDate.LocationForDate(thing));
		}

		// Token: 0x060035E1 RID: 13793 RVA: 0x001CEE7C File Offset: 0x001CD27C
		public static int Year(Thing thing)
		{
			int result;
			if (Current.ProgramState != ProgramState.Playing)
			{
				result = 5500;
			}
			else
			{
				result = GenDate.Year((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
			}
			return result;
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x001CEEB8 File Offset: 0x001CD2B8
		public static int DayOfSeason(Thing thing)
		{
			return GenDate.DayOfSeason((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x001CEEE0 File Offset: 0x001CD2E0
		public static int DayOfQuadrum(Thing thing)
		{
			return GenDate.DayOfQuadrum((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E4 RID: 13796 RVA: 0x001CEF08 File Offset: 0x001CD308
		public static float DayPercent(Thing thing)
		{
			return GenDate.DayPercent((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x001CEF30 File Offset: 0x001CD330
		public static float YearPercent(Thing thing)
		{
			return GenDate.YearPercent((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x001CEF58 File Offset: 0x001CD358
		public static int HourInteger(Thing thing)
		{
			return GenDate.HourInteger((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E7 RID: 13799 RVA: 0x001CEF80 File Offset: 0x001CD380
		public static float HourFloat(Thing thing)
		{
			return GenDate.HourFloat((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E8 RID: 13800 RVA: 0x001CEFA8 File Offset: 0x001CD3A8
		public static int DayOfYear(int tile)
		{
			int result;
			if (Current.ProgramState == ProgramState.Playing)
			{
				result = GenDate.DayOfYear((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x060035E9 RID: 13801 RVA: 0x001CEFF0 File Offset: 0x001CD3F0
		public static int HourOfDay(int tile)
		{
			return GenDate.HourOfDay((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035EA RID: 13802 RVA: 0x001CF024 File Offset: 0x001CD424
		public static int DayOfTwelfth(int tile)
		{
			return GenDate.DayOfTwelfth((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035EB RID: 13803 RVA: 0x001CF058 File Offset: 0x001CD458
		public static Twelfth Twelfth(int tile)
		{
			return GenDate.Twelfth((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x001CF08C File Offset: 0x001CD48C
		public static Season Season(int tile)
		{
			return GenDate.Season((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile));
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x001CF0B8 File Offset: 0x001CD4B8
		public static int Year(int tile)
		{
			int result;
			if (Current.ProgramState != ProgramState.Playing)
			{
				result = 5500;
			}
			else
			{
				result = GenDate.Year((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
			}
			return result;
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x001CF104 File Offset: 0x001CD504
		public static int DayOfSeason(int tile)
		{
			return GenDate.DayOfSeason((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035EF RID: 13807 RVA: 0x001CF138 File Offset: 0x001CD538
		public static int DayOfQuadrum(int tile)
		{
			return GenDate.DayOfQuadrum((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x001CF16C File Offset: 0x001CD56C
		public static float DayPercent(int tile)
		{
			return GenDate.DayPercent((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x001CF1A0 File Offset: 0x001CD5A0
		public static float YearPercent(int tile)
		{
			return GenDate.YearPercent((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x001CF1D4 File Offset: 0x001CD5D4
		public static int HourInteger(int tile)
		{
			return GenDate.HourInteger((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x001CF208 File Offset: 0x001CD608
		public static float HourFloat(int tile)
		{
			return GenDate.HourFloat((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x001CF23C File Offset: 0x001CD63C
		private static float LongitudeForDate(Thing thing)
		{
			return GenLocalDate.LocationForDate(thing).x;
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x001CF260 File Offset: 0x001CD660
		private static Vector2 LocationForDate(Thing thing)
		{
			int tile = thing.Tile;
			Vector2 result;
			if (tile >= 0)
			{
				result = Find.WorldGrid.LongLatOf(tile);
			}
			else
			{
				result = Vector2.zero;
			}
			return result;
		}
	}
}
