using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000909 RID: 2313
	public static class GenLocalDate
	{
		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x060035CE RID: 13774 RVA: 0x001CEFE0 File Offset: 0x001CD3E0
		private static int TicksAbs
		{
			get
			{
				return GenTicks.TicksAbs;
			}
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x001CEFFC File Offset: 0x001CD3FC
		public static int DayOfYear(Map map)
		{
			return GenLocalDate.DayOfYear(map.Tile);
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x001CF01C File Offset: 0x001CD41C
		public static int HourOfDay(Map map)
		{
			return GenLocalDate.HourOfDay(map.Tile);
		}

		// Token: 0x060035D1 RID: 13777 RVA: 0x001CF03C File Offset: 0x001CD43C
		public static int DayOfTwelfth(Map map)
		{
			return GenLocalDate.DayOfTwelfth(map.Tile);
		}

		// Token: 0x060035D2 RID: 13778 RVA: 0x001CF05C File Offset: 0x001CD45C
		public static Twelfth Twelfth(Map map)
		{
			return GenLocalDate.Twelfth(map.Tile);
		}

		// Token: 0x060035D3 RID: 13779 RVA: 0x001CF07C File Offset: 0x001CD47C
		public static Season Season(Map map)
		{
			return GenLocalDate.Season(map.Tile);
		}

		// Token: 0x060035D4 RID: 13780 RVA: 0x001CF09C File Offset: 0x001CD49C
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

		// Token: 0x060035D5 RID: 13781 RVA: 0x001CF0D4 File Offset: 0x001CD4D4
		public static int DayOfSeason(Map map)
		{
			return GenLocalDate.DayOfSeason(map.Tile);
		}

		// Token: 0x060035D6 RID: 13782 RVA: 0x001CF0F4 File Offset: 0x001CD4F4
		public static int DayOfQuadrum(Map map)
		{
			return GenLocalDate.DayOfQuadrum(map.Tile);
		}

		// Token: 0x060035D7 RID: 13783 RVA: 0x001CF114 File Offset: 0x001CD514
		public static float DayPercent(Map map)
		{
			return GenLocalDate.DayPercent(map.Tile);
		}

		// Token: 0x060035D8 RID: 13784 RVA: 0x001CF134 File Offset: 0x001CD534
		public static float YearPercent(Map map)
		{
			return GenLocalDate.YearPercent(map.Tile);
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x001CF154 File Offset: 0x001CD554
		public static int HourInteger(Map map)
		{
			return GenLocalDate.HourInteger(map.Tile);
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x001CF174 File Offset: 0x001CD574
		public static float HourFloat(Map map)
		{
			return GenLocalDate.HourFloat(map.Tile);
		}

		// Token: 0x060035DB RID: 13787 RVA: 0x001CF194 File Offset: 0x001CD594
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

		// Token: 0x060035DC RID: 13788 RVA: 0x001CF1CC File Offset: 0x001CD5CC
		public static int HourOfDay(Thing thing)
		{
			return GenDate.HourOfDay((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035DD RID: 13789 RVA: 0x001CF1F4 File Offset: 0x001CD5F4
		public static int DayOfTwelfth(Thing thing)
		{
			return GenDate.DayOfTwelfth((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x001CF21C File Offset: 0x001CD61C
		public static Twelfth Twelfth(Thing thing)
		{
			return GenDate.Twelfth((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x001CF244 File Offset: 0x001CD644
		public static Season Season(Thing thing)
		{
			return GenDate.Season((long)GenLocalDate.TicksAbs, GenLocalDate.LocationForDate(thing));
		}

		// Token: 0x060035E0 RID: 13792 RVA: 0x001CF26C File Offset: 0x001CD66C
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

		// Token: 0x060035E1 RID: 13793 RVA: 0x001CF2A8 File Offset: 0x001CD6A8
		public static int DayOfSeason(Thing thing)
		{
			return GenDate.DayOfSeason((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x001CF2D0 File Offset: 0x001CD6D0
		public static int DayOfQuadrum(Thing thing)
		{
			return GenDate.DayOfQuadrum((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x001CF2F8 File Offset: 0x001CD6F8
		public static float DayPercent(Thing thing)
		{
			return GenDate.DayPercent((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E4 RID: 13796 RVA: 0x001CF320 File Offset: 0x001CD720
		public static float YearPercent(Thing thing)
		{
			return GenDate.YearPercent((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x001CF348 File Offset: 0x001CD748
		public static int HourInteger(Thing thing)
		{
			return GenDate.HourInteger((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x001CF370 File Offset: 0x001CD770
		public static float HourFloat(Thing thing)
		{
			return GenDate.HourFloat((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E7 RID: 13799 RVA: 0x001CF398 File Offset: 0x001CD798
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

		// Token: 0x060035E8 RID: 13800 RVA: 0x001CF3E0 File Offset: 0x001CD7E0
		public static int HourOfDay(int tile)
		{
			return GenDate.HourOfDay((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035E9 RID: 13801 RVA: 0x001CF414 File Offset: 0x001CD814
		public static int DayOfTwelfth(int tile)
		{
			return GenDate.DayOfTwelfth((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035EA RID: 13802 RVA: 0x001CF448 File Offset: 0x001CD848
		public static Twelfth Twelfth(int tile)
		{
			return GenDate.Twelfth((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035EB RID: 13803 RVA: 0x001CF47C File Offset: 0x001CD87C
		public static Season Season(int tile)
		{
			return GenDate.Season((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile));
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x001CF4A8 File Offset: 0x001CD8A8
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

		// Token: 0x060035ED RID: 13805 RVA: 0x001CF4F4 File Offset: 0x001CD8F4
		public static int DayOfSeason(int tile)
		{
			return GenDate.DayOfSeason((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x001CF528 File Offset: 0x001CD928
		public static int DayOfQuadrum(int tile)
		{
			return GenDate.DayOfQuadrum((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035EF RID: 13807 RVA: 0x001CF55C File Offset: 0x001CD95C
		public static float DayPercent(int tile)
		{
			return GenDate.DayPercent((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x001CF590 File Offset: 0x001CD990
		public static float YearPercent(int tile)
		{
			return GenDate.YearPercent((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x001CF5C4 File Offset: 0x001CD9C4
		public static int HourInteger(int tile)
		{
			return GenDate.HourInteger((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x001CF5F8 File Offset: 0x001CD9F8
		public static float HourFloat(int tile)
		{
			return GenDate.HourFloat((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x001CF62C File Offset: 0x001CDA2C
		private static float LongitudeForDate(Thing thing)
		{
			return GenLocalDate.LocationForDate(thing).x;
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x001CF650 File Offset: 0x001CDA50
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
