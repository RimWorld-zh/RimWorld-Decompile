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
				if (Current.ProgramState != ProgramState.Playing && Find.GameInitData != null && Find.GameInitData.gameToLoad.NullOrEmpty())
				{
					return GenTicks.ConfiguredTicksAbsAtGameStart;
				}
				if (Current.Game != null && Find.TickManager != null)
				{
					return Find.TickManager.TicksAbs;
				}
				return 0;
			}
		}

		public static int ConfiguredTicksAbsAtGameStart
		{
			get
			{
				GameInitData gameInitData = Find.GameInitData;
				Vector2 vector = (gameInitData.startingTile < 0) ? Vector2.zero : Find.WorldGrid.LongLatOf(gameInitData.startingTile);
				Twelfth twelfth = (gameInitData.startingSeason == Season.Undefined) ? ((gameInitData.startingTile < 0) ? Season.Summer.GetFirstTwelfth(0f) : TwelfthUtility.FindStartingWarmTwelfth(gameInitData.startingTile)) : gameInitData.startingSeason.GetFirstTwelfth(vector.y);
				int num = (24 - GenDate.TimeZoneAt(vector.x)) % 24;
				return 300000 * (int)twelfth + 2500 * (6 + num);
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
