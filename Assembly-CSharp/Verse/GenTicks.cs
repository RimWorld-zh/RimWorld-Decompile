using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F4F RID: 3919
	public static class GenTicks
	{
		// Token: 0x04003E36 RID: 15926
		public const int TicksPerRealSecond = 60;

		// Token: 0x04003E37 RID: 15927
		public const int TickRareInterval = 250;

		// Token: 0x04003E38 RID: 15928
		public const int TickLongInterval = 2000;

		// Token: 0x17000F3E RID: 3902
		// (get) Token: 0x06005EBB RID: 24251 RVA: 0x00303C10 File Offset: 0x00302010
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

		// Token: 0x17000F3F RID: 3903
		// (get) Token: 0x06005EBC RID: 24252 RVA: 0x00303C80 File Offset: 0x00302080
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

		// Token: 0x17000F40 RID: 3904
		// (get) Token: 0x06005EBD RID: 24253 RVA: 0x00303CBC File Offset: 0x003020BC
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

		// Token: 0x06005EBE RID: 24254 RVA: 0x00303DA8 File Offset: 0x003021A8
		public static float TicksToSeconds(this int numTicks)
		{
			return (float)numTicks / 60f;
		}

		// Token: 0x06005EBF RID: 24255 RVA: 0x00303DC8 File Offset: 0x003021C8
		public static int SecondsToTicks(this float numSeconds)
		{
			return Mathf.RoundToInt(60f * numSeconds);
		}

		// Token: 0x06005EC0 RID: 24256 RVA: 0x00303DEC File Offset: 0x003021EC
		public static string TicksToSecondsString(this int numTicks)
		{
			return numTicks.TicksToSeconds().ToString("F1") + " " + "SecondsLower".Translate();
		}

		// Token: 0x06005EC1 RID: 24257 RVA: 0x00303E28 File Offset: 0x00302228
		public static string SecondsToTicksString(this float numSeconds)
		{
			return numSeconds.SecondsToTicks().ToString();
		}
	}
}
