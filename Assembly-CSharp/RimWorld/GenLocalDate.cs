using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000909 RID: 2313
	public static class GenLocalDate
	{
		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x060035CE RID: 13774 RVA: 0x001CF2B4 File Offset: 0x001CD6B4
		private static int TicksAbs
		{
			get
			{
				return GenTicks.TicksAbs;
			}
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x001CF2D0 File Offset: 0x001CD6D0
		public static int DayOfYear(Map map)
		{
			return GenLocalDate.DayOfYear(map.Tile);
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x001CF2F0 File Offset: 0x001CD6F0
		public static int HourOfDay(Map map)
		{
			return GenLocalDate.HourOfDay(map.Tile);
		}

		// Token: 0x060035D1 RID: 13777 RVA: 0x001CF310 File Offset: 0x001CD710
		public static int DayOfTwelfth(Map map)
		{
			return GenLocalDate.DayOfTwelfth(map.Tile);
		}

		// Token: 0x060035D2 RID: 13778 RVA: 0x001CF330 File Offset: 0x001CD730
		public static Twelfth Twelfth(Map map)
		{
			return GenLocalDate.Twelfth(map.Tile);
		}

		// Token: 0x060035D3 RID: 13779 RVA: 0x001CF350 File Offset: 0x001CD750
		public static Season Season(Map map)
		{
			return GenLocalDate.Season(map.Tile);
		}

		// Token: 0x060035D4 RID: 13780 RVA: 0x001CF370 File Offset: 0x001CD770
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

		// Token: 0x060035D5 RID: 13781 RVA: 0x001CF3A8 File Offset: 0x001CD7A8
		public static int DayOfSeason(Map map)
		{
			return GenLocalDate.DayOfSeason(map.Tile);
		}

		// Token: 0x060035D6 RID: 13782 RVA: 0x001CF3C8 File Offset: 0x001CD7C8
		public static int DayOfQuadrum(Map map)
		{
			return GenLocalDate.DayOfQuadrum(map.Tile);
		}

		// Token: 0x060035D7 RID: 13783 RVA: 0x001CF3E8 File Offset: 0x001CD7E8
		public static float DayPercent(Map map)
		{
			return GenLocalDate.DayPercent(map.Tile);
		}

		// Token: 0x060035D8 RID: 13784 RVA: 0x001CF408 File Offset: 0x001CD808
		public static float YearPercent(Map map)
		{
			return GenLocalDate.YearPercent(map.Tile);
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x001CF428 File Offset: 0x001CD828
		public static int HourInteger(Map map)
		{
			return GenLocalDate.HourInteger(map.Tile);
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x001CF448 File Offset: 0x001CD848
		public static float HourFloat(Map map)
		{
			return GenLocalDate.HourFloat(map.Tile);
		}

		// Token: 0x060035DB RID: 13787 RVA: 0x001CF468 File Offset: 0x001CD868
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

		// Token: 0x060035DC RID: 13788 RVA: 0x001CF4A0 File Offset: 0x001CD8A0
		public static int HourOfDay(Thing thing)
		{
			return GenDate.HourOfDay((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035DD RID: 13789 RVA: 0x001CF4C8 File Offset: 0x001CD8C8
		public static int DayOfTwelfth(Thing thing)
		{
			return GenDate.DayOfTwelfth((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x001CF4F0 File Offset: 0x001CD8F0
		public static Twelfth Twelfth(Thing thing)
		{
			return GenDate.Twelfth((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x001CF518 File Offset: 0x001CD918
		public static Season Season(Thing thing)
		{
			return GenDate.Season((long)GenLocalDate.TicksAbs, GenLocalDate.LocationForDate(thing));
		}

		// Token: 0x060035E0 RID: 13792 RVA: 0x001CF540 File Offset: 0x001CD940
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

		// Token: 0x060035E1 RID: 13793 RVA: 0x001CF57C File Offset: 0x001CD97C
		public static int DayOfSeason(Thing thing)
		{
			return GenDate.DayOfSeason((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x001CF5A4 File Offset: 0x001CD9A4
		public static int DayOfQuadrum(Thing thing)
		{
			return GenDate.DayOfQuadrum((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x001CF5CC File Offset: 0x001CD9CC
		public static float DayPercent(Thing thing)
		{
			return GenDate.DayPercent((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E4 RID: 13796 RVA: 0x001CF5F4 File Offset: 0x001CD9F4
		public static float YearPercent(Thing thing)
		{
			return GenDate.YearPercent((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x001CF61C File Offset: 0x001CDA1C
		public static int HourInteger(Thing thing)
		{
			return GenDate.HourInteger((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x001CF644 File Offset: 0x001CDA44
		public static float HourFloat(Thing thing)
		{
			return GenDate.HourFloat((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E7 RID: 13799 RVA: 0x001CF66C File Offset: 0x001CDA6C
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

		// Token: 0x060035E8 RID: 13800 RVA: 0x001CF6B4 File Offset: 0x001CDAB4
		public static int HourOfDay(int tile)
		{
			return GenDate.HourOfDay((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035E9 RID: 13801 RVA: 0x001CF6E8 File Offset: 0x001CDAE8
		public static int DayOfTwelfth(int tile)
		{
			return GenDate.DayOfTwelfth((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035EA RID: 13802 RVA: 0x001CF71C File Offset: 0x001CDB1C
		public static Twelfth Twelfth(int tile)
		{
			return GenDate.Twelfth((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035EB RID: 13803 RVA: 0x001CF750 File Offset: 0x001CDB50
		public static Season Season(int tile)
		{
			return GenDate.Season((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile));
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x001CF77C File Offset: 0x001CDB7C
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

		// Token: 0x060035ED RID: 13805 RVA: 0x001CF7C8 File Offset: 0x001CDBC8
		public static int DayOfSeason(int tile)
		{
			return GenDate.DayOfSeason((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x001CF7FC File Offset: 0x001CDBFC
		public static int DayOfQuadrum(int tile)
		{
			return GenDate.DayOfQuadrum((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035EF RID: 13807 RVA: 0x001CF830 File Offset: 0x001CDC30
		public static float DayPercent(int tile)
		{
			return GenDate.DayPercent((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x001CF864 File Offset: 0x001CDC64
		public static float YearPercent(int tile)
		{
			return GenDate.YearPercent((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x001CF898 File Offset: 0x001CDC98
		public static int HourInteger(int tile)
		{
			return GenDate.HourInteger((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x001CF8CC File Offset: 0x001CDCCC
		public static float HourFloat(int tile)
		{
			return GenDate.HourFloat((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x001CF900 File Offset: 0x001CDD00
		private static float LongitudeForDate(Thing thing)
		{
			return GenLocalDate.LocationForDate(thing).x;
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x001CF924 File Offset: 0x001CDD24
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
