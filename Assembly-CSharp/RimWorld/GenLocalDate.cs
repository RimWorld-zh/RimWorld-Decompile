using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class GenLocalDate
	{
		private static int TicksAbs
		{
			get
			{
				return GenTicks.TicksAbs;
			}
		}

		public static int DayOfYear(Map map)
		{
			return GenLocalDate.DayOfYear(map.Tile);
		}

		public static int HourOfDay(Map map)
		{
			return GenLocalDate.HourOfDay(map.Tile);
		}

		public static int DayOfTwelfth(Map map)
		{
			return GenLocalDate.DayOfTwelfth(map.Tile);
		}

		public static Twelfth Twelfth(Map map)
		{
			return GenLocalDate.Twelfth(map.Tile);
		}

		public static Season Season(Map map)
		{
			return GenLocalDate.Season(map.Tile);
		}

		public static int Year(Map map)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return 5500;
			}
			return GenLocalDate.Year(map.Tile);
		}

		public static int DayOfSeason(Map map)
		{
			return GenLocalDate.DayOfSeason(map.Tile);
		}

		public static int DayOfQuadrum(Map map)
		{
			return GenLocalDate.DayOfQuadrum(map.Tile);
		}

		public static float DayPercent(Map map)
		{
			return GenLocalDate.DayPercent(map.Tile);
		}

		public static float YearPercent(Map map)
		{
			return GenLocalDate.YearPercent(map.Tile);
		}

		public static int HourInteger(Map map)
		{
			return GenLocalDate.HourInteger(map.Tile);
		}

		public static float HourFloat(Map map)
		{
			return GenLocalDate.HourFloat(map.Tile);
		}

		public static int DayOfYear(Thing thing)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				return GenDate.DayOfYear(GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
			}
			return 0;
		}

		public static int HourOfDay(Thing thing)
		{
			return GenDate.HourOfDay(GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		public static int DayOfTwelfth(Thing thing)
		{
			return GenDate.DayOfTwelfth(GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		public static Twelfth Twelfth(Thing thing)
		{
			return GenDate.Twelfth(GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		public static Season Season(Thing thing)
		{
			return GenDate.Season(GenLocalDate.TicksAbs, GenLocalDate.LocationForDate(thing));
		}

		public static int Year(Thing thing)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return 5500;
			}
			return GenDate.Year(GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		public static int DayOfSeason(Thing thing)
		{
			return GenDate.DayOfSeason(GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		public static int DayOfQuadrum(Thing thing)
		{
			return GenDate.DayOfQuadrum(GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		public static float DayPercent(Thing thing)
		{
			return GenDate.DayPercent(GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		public static float YearPercent(Thing thing)
		{
			return GenDate.YearPercent(GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		public static int HourInteger(Thing thing)
		{
			return GenDate.HourInteger(GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		public static float HourFloat(Thing thing)
		{
			return GenDate.HourFloat(GenLocalDate.TicksAbs, GenLocalDate.LongitudeForDate(thing));
		}

		public static int DayOfYear(int tile)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				long absTicks = GenLocalDate.TicksAbs;
				Vector2 vector = Find.WorldGrid.LongLatOf(tile);
				return GenDate.DayOfYear(absTicks, vector.x);
			}
			return 0;
		}

		public static int HourOfDay(int tile)
		{
			long absTicks = GenLocalDate.TicksAbs;
			Vector2 vector = Find.WorldGrid.LongLatOf(tile);
			return GenDate.HourOfDay(absTicks, vector.x);
		}

		public static int DayOfTwelfth(int tile)
		{
			long absTicks = GenLocalDate.TicksAbs;
			Vector2 vector = Find.WorldGrid.LongLatOf(tile);
			return GenDate.DayOfTwelfth(absTicks, vector.x);
		}

		public static Twelfth Twelfth(int tile)
		{
			long absTicks = GenLocalDate.TicksAbs;
			Vector2 vector = Find.WorldGrid.LongLatOf(tile);
			return GenDate.Twelfth(absTicks, vector.x);
		}

		public static Season Season(int tile)
		{
			return GenDate.Season(GenLocalDate.TicksAbs, Find.WorldGrid.LongLatOf(tile));
		}

		public static int Year(int tile)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return 5500;
			}
			long absTicks = GenLocalDate.TicksAbs;
			Vector2 vector = Find.WorldGrid.LongLatOf(tile);
			return GenDate.Year(absTicks, vector.x);
		}

		public static int DayOfSeason(int tile)
		{
			long absTicks = GenLocalDate.TicksAbs;
			Vector2 vector = Find.WorldGrid.LongLatOf(tile);
			return GenDate.DayOfSeason(absTicks, vector.x);
		}

		public static int DayOfQuadrum(int tile)
		{
			long absTicks = GenLocalDate.TicksAbs;
			Vector2 vector = Find.WorldGrid.LongLatOf(tile);
			return GenDate.DayOfQuadrum(absTicks, vector.x);
		}

		public static float DayPercent(int tile)
		{
			long absTicks = GenLocalDate.TicksAbs;
			Vector2 vector = Find.WorldGrid.LongLatOf(tile);
			return GenDate.DayPercent(absTicks, vector.x);
		}

		public static float YearPercent(int tile)
		{
			long absTicks = GenLocalDate.TicksAbs;
			Vector2 vector = Find.WorldGrid.LongLatOf(tile);
			return GenDate.YearPercent(absTicks, vector.x);
		}

		public static int HourInteger(int tile)
		{
			long absTicks = GenLocalDate.TicksAbs;
			Vector2 vector = Find.WorldGrid.LongLatOf(tile);
			return GenDate.HourInteger(absTicks, vector.x);
		}

		public static float HourFloat(int tile)
		{
			long absTicks = GenLocalDate.TicksAbs;
			Vector2 vector = Find.WorldGrid.LongLatOf(tile);
			return GenDate.HourFloat(absTicks, vector.x);
		}

		private static float LongitudeForDate(Thing thing)
		{
			Vector2 vector = GenLocalDate.LocationForDate(thing);
			return vector.x;
		}

		private static Vector2 LocationForDate(Thing thing)
		{
			int tile = thing.Tile;
			if (tile >= 0)
			{
				return Find.WorldGrid.LongLatOf(tile);
			}
			return Vector2.zero;
		}
	}
}
