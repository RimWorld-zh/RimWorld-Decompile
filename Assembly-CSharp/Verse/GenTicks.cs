using RimWorld;
using UnityEngine;

namespace Verse
{
	public static class GenTicks
	{
		public const int TicksPerRealSecond = 60;

		public const int TickRareInterval = 250;

		public const int TickLongInterval = 2000;

		public static int TicksAbs
		{
			get
			{
				return (Current.ProgramState == ProgramState.Playing || Find.GameInitData == null || !Find.GameInitData.gameToLoad.NullOrEmpty()) ? ((Current.Game != null && Find.TickManager != null) ? Find.TickManager.TicksAbs : 0) : GenTicks.ConfiguredTicksAbsAtGameStart;
			}
		}

		public static int TicksGame
		{
			get
			{
				return (Current.Game != null && Find.TickManager != null) ? Find.TickManager.TicksGame : 0;
			}
		}

		public static int ConfiguredTicksAbsAtGameStart
		{
			get
			{
				GameInitData gameInitData = Find.GameInitData;
				ConfiguredTicksAbsAtGameStartCache ticksAbsCache = Find.World.ticksAbsCache;
				int num = default(int);
				int result;
				if (ticksAbsCache.TryGetCachedValue(gameInitData, out num))
				{
					result = num;
				}
				else
				{
					Vector2 vector = (gameInitData.startingTile < 0) ? Vector2.zero : Find.WorldGrid.LongLatOf(gameInitData.startingTile);
					Twelfth twelfth = (gameInitData.startingSeason == Season.Undefined) ? ((gameInitData.startingTile < 0) ? Season.Summer.GetFirstTwelfth(0f) : TwelfthUtility.FindStartingWarmTwelfth(gameInitData.startingTile)) : gameInitData.startingSeason.GetFirstTwelfth(vector.y);
					int num2 = (24 - GenDate.TimeZoneAt(vector.x)) % 24;
					int num3 = 300000 * (int)twelfth + 2500 * (6 + num2);
					ticksAbsCache.Cache(num3, gameInitData);
					result = num3;
				}
				return result;
			}
		}

		public static float TicksToSeconds(this int numTicks)
		{
			return (float)((float)numTicks / 60.0);
		}

		public static int SecondsToTicks(this float numSeconds)
		{
			return Mathf.RoundToInt((float)(60.0 * numSeconds));
		}

		public static string TicksToSecondsString(this int numTicks)
		{
			return numTicks.TicksToSeconds().ToString("F1") + " " + "SecondsLower".Translate();
		}

		public static string SecondsToTicksString(this float numSeconds)
		{
			return numSeconds.SecondsToTicks().ToString();
		}
	}
}
