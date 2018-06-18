using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F4B RID: 3915
	public static class GenTicks
	{
		// Token: 0x17000F3B RID: 3899
		// (get) Token: 0x06005E89 RID: 24201 RVA: 0x00301554 File Offset: 0x002FF954
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

		// Token: 0x17000F3C RID: 3900
		// (get) Token: 0x06005E8A RID: 24202 RVA: 0x003015C4 File Offset: 0x002FF9C4
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

		// Token: 0x17000F3D RID: 3901
		// (get) Token: 0x06005E8B RID: 24203 RVA: 0x00301600 File Offset: 0x002FFA00
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

		// Token: 0x06005E8C RID: 24204 RVA: 0x003016EC File Offset: 0x002FFAEC
		public static float TicksToSeconds(this int numTicks)
		{
			return (float)numTicks / 60f;
		}

		// Token: 0x06005E8D RID: 24205 RVA: 0x0030170C File Offset: 0x002FFB0C
		public static int SecondsToTicks(this float numSeconds)
		{
			return Mathf.RoundToInt(60f * numSeconds);
		}

		// Token: 0x06005E8E RID: 24206 RVA: 0x00301730 File Offset: 0x002FFB30
		public static string TicksToSecondsString(this int numTicks)
		{
			return numTicks.TicksToSeconds().ToString("F1") + " " + "SecondsLower".Translate();
		}

		// Token: 0x06005E8F RID: 24207 RVA: 0x0030176C File Offset: 0x002FFB6C
		public static string SecondsToTicksString(this float numSeconds)
		{
			return numSeconds.SecondsToTicks().ToString();
		}

		// Token: 0x04003E21 RID: 15905
		public const int TicksPerRealSecond = 60;

		// Token: 0x04003E22 RID: 15906
		public const int TickRareInterval = 250;

		// Token: 0x04003E23 RID: 15907
		public const int TickLongInterval = 2000;
	}
}
