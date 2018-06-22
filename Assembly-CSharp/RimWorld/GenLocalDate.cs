using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000907 RID: 2311
	public static class GenLocalDate
	{
		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x060035CA RID: 13770 RVA: 0x001CEEA0 File Offset: 0x001CD2A0
		private static int TicksAbs
		{
			get
			{
				return GenTicks.TicksAbs;
			}
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x001CEEBC File Offset: 0x001CD2BC
		public static int DayOfYear(Map map)
		{
			return GenLocalDate.DayOfYear(map.Tile);
		}

		// Token: 0x060035CC RID: 13772 RVA: 0x001CEEDC File Offset: 0x001CD2DC
		public static int HourOfDay(Map map)
		{
			return GenLocalDate.HourOfDay(map.Tile);
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x001CEEFC File Offset: 0x001CD2FC
		public static int DayOfTwelfth(Map map)
		{
			return GenLocalDate.DayOfTwelfth(map.Tile);
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x001CEF1C File Offset: 0x001CD31C
		public static Twelfth Twelfth(Map map)
		{
			return GenLocalDate.Twelfth(map.Tile);
		}

		// Token: 0x060035CF RID: 13775 RVA: 0x001CEF3C File Offset: 0x001CD33C
		public static Season Season(Map map)
		{
			return GenLocalDate.Season(map.Tile);
		}

		// Token: 0x060035D0 RID: 13776 RVA: 0x001CEF5C File Offset: 0x001CD35C
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

		// Token: 0x060035D1 RID: 13777 RVA: 0x001CEF94 File Offset: 0x001CD394
		public static int DayOfSeason(Map map)
		{
			return GenLocalDate.DayOfSeason(map.Tile);
		}

		// Token: 0x060035D2 RID: 13778 RVA: 0x001CEFB4 File Offset: 0x001CD3B4
		public static int DayOfQuadrum(Map map)
		{
			return GenLocalDate.DayOfQuadrum(map.Tile);
		}

		// Token: 0x060035D3 RID: 13779 RVA: 0x001CEFD4 File Offset: 0x001CD3D4
		public static float DayPercent(Map map)
		{
			return GenLocalDate.DayPercent(map.Tile);
		}

		// Token: 0x060035D4 RID: 13780 RVA: 0x001CEFF4 File Offset: 0x001CD3F4
		public static float YearPercent(Map map)
		{
			return GenLocalDate.YearPercent(map.Tile);
		}

		// Token: 0x060035D5 RID: 13781 RVA: 0x001CF014 File Offset: 0x001CD414
		public static int HourInteger(Map map)
		{
			return GenLocalDate.HourInteger(map.Tile);
		}

		// Token: 0x060035D6 RID: 13782 RVA: 0x001CF034 File Offset: 0x001CD434
		public static float HourFloat(Map map)
		{
			return GenLocalDate.HourFloat(map.Tile);
		}

		// Token: 0x060035D7 RID: 13783 RVA: 0x001CF054 File Offset: 0x001CD454
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

		// Token: 0x060035D8 RID: 13784 RVA: 0x001CF08C File Offset: 0x001CD48C
		public static int HourOfDay(Thing thing)
		{
			return GenDate.HourOfDay((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035D9 RID: 13785 RVA: 0x001CF0B4 File Offset: 0x001CD4B4
		public static int DayOfTwelfth(Thing thing)
		{
			return GenDate.DayOfTwelfth((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035DA RID: 13786 RVA: 0x001CF0DC File Offset: 0x001CD4DC
		public static Twelfth Twelfth(Thing thing)
		{
			return GenDate.Twelfth((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035DB RID: 13787 RVA: 0x001CF104 File Offset: 0x001CD504
		public static Season Season(Thing thing)
		{
			return GenDate.Season((long)GenLocalDate.TicksAbs, GenLocalDate.LocationForDate(thing));
		}

		// Token: 0x060035DC RID: 13788 RVA: 0x001CF12C File Offset: 0x001CD52C
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

		// Token: 0x060035DD RID: 13789 RVA: 0x001CF168 File Offset: 0x001CD568
		public static int DayOfSeason(Thing thing)
		{
			return GenDate.DayOfSeason((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x001CF190 File Offset: 0x001CD590
		public static int DayOfQuadrum(Thing thing)
		{
			return GenDate.DayOfQuadrum((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035DF RID: 13791 RVA: 0x001CF1B8 File Offset: 0x001CD5B8
		public static float DayPercent(Thing thing)
		{
			return GenDate.DayPercent((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E0 RID: 13792 RVA: 0x001CF1E0 File Offset: 0x001CD5E0
		public static float YearPercent(Thing thing)
		{
			return GenDate.YearPercent((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E1 RID: 13793 RVA: 0x001CF208 File Offset: 0x001CD608
		public static int HourInteger(Thing thing)
		{
			return GenDate.HourInteger((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x001CF230 File Offset: 0x001CD630
		public static float HourFloat(Thing thing)
		{
			return GenDate.HourFloat((long)GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x001CF258 File Offset: 0x001CD658
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

		// Token: 0x060035E4 RID: 13796 RVA: 0x001CF2A0 File Offset: 0x001CD6A0
		public static int HourOfDay(int tile)
		{
			return GenDate.HourOfDay((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x001CF2D4 File Offset: 0x001CD6D4
		public static int DayOfTwelfth(int tile)
		{
			return GenDate.DayOfTwelfth((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x001CF308 File Offset: 0x001CD708
		public static Twelfth Twelfth(int tile)
		{
			return GenDate.Twelfth((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035E7 RID: 13799 RVA: 0x001CF33C File Offset: 0x001CD73C
		public static Season Season(int tile)
		{
			return GenDate.Season((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile));
		}

		// Token: 0x060035E8 RID: 13800 RVA: 0x001CF368 File Offset: 0x001CD768
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

		// Token: 0x060035E9 RID: 13801 RVA: 0x001CF3B4 File Offset: 0x001CD7B4
		public static int DayOfSeason(int tile)
		{
			return GenDate.DayOfSeason((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035EA RID: 13802 RVA: 0x001CF3E8 File Offset: 0x001CD7E8
		public static int DayOfQuadrum(int tile)
		{
			return GenDate.DayOfQuadrum((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035EB RID: 13803 RVA: 0x001CF41C File Offset: 0x001CD81C
		public static float DayPercent(int tile)
		{
			return GenDate.DayPercent((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035EC RID: 13804 RVA: 0x001CF450 File Offset: 0x001CD850
		public static float YearPercent(int tile)
		{
			return GenDate.YearPercent((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x001CF484 File Offset: 0x001CD884
		public static int HourInteger(int tile)
		{
			return GenDate.HourInteger((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x001CF4B8 File Offset: 0x001CD8B8
		public static float HourFloat(int tile)
		{
			return GenDate.HourFloat((long)GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile).x);
		}

		// Token: 0x060035EF RID: 13807 RVA: 0x001CF4EC File Offset: 0x001CD8EC
		private static float LongitudeForDate(Thing thing)
		{
			return GenLocalDate.LocationForDate(thing).x;
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x001CF510 File Offset: 0x001CD910
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
