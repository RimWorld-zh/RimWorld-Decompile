using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F4B RID: 3915
	public static class GenTicks
	{
		// Token: 0x17000F3F RID: 3903
		// (get) Token: 0x06005EB1 RID: 24241 RVA: 0x00303590 File Offset: 0x00301990
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

		// Token: 0x17000F40 RID: 3904
		// (get) Token: 0x06005EB2 RID: 24242 RVA: 0x00303600 File Offset: 0x00301A00
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

		// Token: 0x17000F41 RID: 3905
		// (get) Token: 0x06005EB3 RID: 24243 RVA: 0x0030363C File Offset: 0x00301A3C
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

		// Token: 0x06005EB4 RID: 24244 RVA: 0x00303728 File Offset: 0x00301B28
		public static float TicksToSeconds(this int numTicks)
		{
			return (float)numTicks / 60f;
		}

		// Token: 0x06005EB5 RID: 24245 RVA: 0x00303748 File Offset: 0x00301B48
		public static int SecondsToTicks(this float numSeconds)
		{
			return Mathf.RoundToInt(60f * numSeconds);
		}

		// Token: 0x06005EB6 RID: 24246 RVA: 0x0030376C File Offset: 0x00301B6C
		public static string TicksToSecondsString(this int numTicks)
		{
			return numTicks.TicksToSeconds().ToString("F1") + " " + "SecondsLower".Translate();
		}

		// Token: 0x06005EB7 RID: 24247 RVA: 0x003037A8 File Offset: 0x00301BA8
		public static string SecondsToTicksString(this float numSeconds)
		{
			return numSeconds.SecondsToTicks().ToString();
		}

		// Token: 0x04003E33 RID: 15923
		public const int TicksPerRealSecond = 60;

		// Token: 0x04003E34 RID: 15924
		public const int TickRareInterval = 250;

		// Token: 0x04003E35 RID: 15925
		public const int TickLongInterval = 2000;
	}
}
