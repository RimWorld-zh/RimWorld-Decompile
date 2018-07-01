using System;
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
				int result;
				if (Current.ProgramState != ProgramState.Playing && Find.GameInitData != null && Find.GameInitData.gameToLoad.NullOrEmpty())
				{
					result = GenTicks.ConfiguredTicksAbsAtGameStart;
				}
				else if (Current.Game != null && Find.TickManager != null)
				{
					result = Find.TickManager.TicksAbs;
				}
				else
				{
					result = 0;
				}
				return result;
			}
		}

		public static int TicksGame
		{
			get
			{
				int result;
				if (Current.Game != null && Find.TickManager != null)
				{
					result = Find.TickManager.TicksGame;
				}
				else
				{
					result = 0;
				}
				return result;
			}
		}

		public static int ConfiguredTicksAbsAtGameStart
		{
			get
			{
				GameInitData gameInitData = Find.GameInitData;
				ConfiguredTicksAbsAtGameStartCache ticksAbsCache = Find.World.ticksAbsCache;
				int num;
				int result;
				if (ticksAbsCache.TryGetCachedValue(gameInitData, out num))
				{
					result = num;
				}
				else
				{
					Vector2 vector;
					if (gameInitData.startingTile >= 0)
					{
						vector = Find.WorldGrid.LongLatOf(gameInitData.startingTile);
					}
					else
					{
						vector = Vector2.zero;
					}
					Twelfth twelfth;
					if (gameInitData.startingSeason != Season.Undefined)
					{
						twelfth = gameInitData.startingSeason.GetFirstTwelfth(vector.y);
					}
					else if (gameInitData.startingTile >= 0)
					{
						twelfth = TwelfthUtility.FindStartingWarmTwelfth(gameInitData.startingTile);
					}
					else
					{
						twelfth = Season.Summer.GetFirstTwelfth(0f);
					}
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
			return (float)numTicks / 60f;
		}

		public static int SecondsToTicks(this float numSeconds)
		{
			return Mathf.RoundToInt(60f * numSeconds);
		}

		public static string ToStringSecondsFromTicks(this int numTicks)
		{
			return numTicks.TicksToSeconds().ToString("F1") + " " + "SecondsLower".Translate();
		}

		public static string ToStringTicksFromSeconds(this float numSeconds)
		{
			return numSeconds.SecondsToTicks().ToString();
		}
	}
}
