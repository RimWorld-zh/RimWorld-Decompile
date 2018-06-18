using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200090B RID: 2315
	public static class GenLocalDate
	{
		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x060035D1 RID: 13777 RVA: 0x001CECB8 File Offset: 0x001CD0B8
		private static int TicksAbs
		{
			get
			{
				return GenTicks.TicksAbs;
			}
		}

		// Token: 0x060035D2 RID: 13778 RVA: 0x001CECD4 File Offset: 0x001CD0D4
		public static int DayOfYear(Map map)
		{
			return GenLocalDate.DayOfYear(map.Tile);
		}

		// Token: 0x060035D3 RID: 13779 RVA: 0x001CECF4 File Offset: 0x001CD0F4
		public static int HourOfDay(Map map)
		{
			return GenLocalDate.HourOfDay(map.Tile);
		}

		// Token: 0x060035D4 RID: 13780 RVA: 0x001CED14 File Offset: 0x001CD114
		public static int DayOfTwelfth(Map map)
		{
			return GenLocalDate.DayOfTwelfth(map.Tile);
		}

		// Token: 0x060035D5 RID: 13781 RVA: 0x001CED34 File Offset: 0x001CD134
		public static Twelfth Twelfth(Map map)
		{
			return GenLocalDate.Twelfth(map.Tile);
		}

		// Token: 0x060035D6 RID: 13782 RVA: 0x001CED54 File Offset: 0x001CD154
		public static Season Season(Map map)
		{
			return GenLocalDate.Season(map.Tile);
		}

		// Token: 0x060035D7 RID: 13783 RVA: 0x001CED74 File Offset: 0x001CD174
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

		// Token: 0x060035D8 RID: 13784 RVA: 0x001CEDAC File Offset: 0x001CD1AC
		public static int DayOfSeason(Map map)
		{
			return GenLocalDate.DayOfSeason(map.Tile);
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x001CEDCC File Offset: 0x001CD1CC
		public static int DayOfQuadrum(Map map)
		{
			return GenLocalDate.DayOfQuadrum(map.Tile);
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x001CEDEC File Offset: 0x001CD1EC
		public static float DayPercent(Map map)
		{
			return GenLocalDate.DayPercent(map.Tile);
		}

		// Token: 0x060035DB RID: 13787 RVA: 0x001CEE0C File Offset: 0x001CD20C
		public static float YearPercent(Map map)
		{
			return GenLocalDate.YearPercent(map.Tile);
		}

		// Token: 0x060035DC RID: 13788 RVA: 0x001CEE2C File Offset: 0x001CD22C
		public static int HourInteger(Map map)
		{
			return GenLocalDate.HourInteger(map.Tile);
		}

		// Token: 0x060035DD RID: 13789 RVA: 0x001CEE4C File Offset: 0x001CD24C
		public static float HourFloat(Map map)
		{
			return GenLocalDate.HourFloat(map.Tile);
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x001CEE6C File Offset: 0x001CD26C
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

		// Token: 0x060035DF RID: 13791 RVA: 0x001CEEA4 File Offset: 0x001CD2A4
		public static int HourOfDay(Thing thing)
		{
			return GenDate.HourOfDay((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E0 RID: 13792 RVA: 0x001CEECC File Offset: 0x001CD2CC
		public static int DayOfTwelfth(Thing thing)
		{
			return GenDate.DayOfTwelfth((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E1 RID: 13793 RVA: 0x001CEEF4 File Offset: 0x001CD2F4
		public static Twelfth Twelfth(Thing thing)
		{
			return GenDate.Twelfth((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x001CEF1C File Offset: 0x001CD31C
		public static Season Season(Thing thing)
		{
			return GenDate.Season((long)GenLocalDate.TicksAbs, GenLocalDate.LocationForDate(thing));
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x001CEF44 File Offset: 0x001CD344
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

		// Token: 0x060035E4 RID: 13796 RVA: 0x001CEF80 File Offset: 0x001CD380
		public static int DayOfSeason(Thing thing)
		{
			return GenDate.DayOfSeason((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x001CEFA8 File Offset: 0x001CD3A8
		public static int DayOfQuadrum(Thing thing)
		{
			return GenDate.DayOfQuadrum((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x001CEFD0 File Offset: 0x001CD3D0
		public static float DayPercent(Thing thing)
		{
			return GenDate.DayPercent((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E7 RID: 13799 RVA: 0x001CEFF8 File Offset: 0x001CD3F8
		public static float YearPercent(Thing thing)
		{
			return GenDate.YearPercent((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E8 RID: 13800 RVA: 0x001CF020 File Offset: 0x001CD420
		public static int HourInteger(Thing thing)
		{
			return GenDate.HourInteger((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E9 RID: 13801 RVA: 0x001CF048 File Offset: 0x001CD448
		public static float HourFloat(Thing thing)
		{
			return GenDate.HourFloat((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035EA RID: 13802 RVA: 0x001CF070 File Offset: 0x001CD470
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

		// Token: 0x060035EB RID: 13803 RVA: 0x001CF0B8 File Offset: 0x001CD4B8
		public static int HourOfDay(int tile)
		{
			return GenDate.HourOfDay((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x001CF0EC File Offset: 0x001CD4EC
		public static int DayOfTwelfth(int tile)
		{
			return GenDate.DayOfTwelfth((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x001CF120 File Offset: 0x001CD520
		public static Twelfth Twelfth(int tile)
		{
			return GenDate.Twelfth((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x001CF154 File Offset: 0x001CD554
		public static Season Season(int tile)
		{
			return GenDate.Season((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile));
		}

		// Token: 0x060035EF RID: 13807 RVA: 0x001CF180 File Offset: 0x001CD580
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

		// Token: 0x060035F0 RID: 13808 RVA: 0x001CF1CC File Offset: 0x001CD5CC
		public static int DayOfSeason(int tile)
		{
			return GenDate.DayOfSeason((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x001CF200 File Offset: 0x001CD600
		public static int DayOfQuadrum(int tile)
		{
			return GenDate.DayOfQuadrum((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x001CF234 File Offset: 0x001CD634
		public static float DayPercent(int tile)
		{
			return GenDate.DayPercent((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x001CF268 File Offset: 0x001CD668
		public static float YearPercent(int tile)
		{
			return GenDate.YearPercent((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x001CF29C File Offset: 0x001CD69C
		public static int HourInteger(int tile)
		{
			return GenDate.HourInteger((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x001CF2D0 File Offset: 0x001CD6D0
		public static float HourFloat(int tile)
		{
			return GenDate.HourFloat((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035F6 RID: 13814 RVA: 0x001CF304 File Offset: 0x001CD704
		private static float LongitudeForDate(Thing thing)
		{
			return GenLocalDate.LocationForDate(thing).x;
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x001CF328 File Offset: 0x001CD728
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
