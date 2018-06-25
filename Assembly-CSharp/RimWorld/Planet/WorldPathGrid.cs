using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000543 RID: 1347
	public class WorldPathGrid
	{
		// Token: 0x04000ECB RID: 3787
		public float[] movementDifficulty;

		// Token: 0x04000ECC RID: 3788
		private int allPathCostsRecalculatedDayOfYear = -1;

		// Token: 0x04000ECD RID: 3789
		private const float ImpassableMovemenetDificulty = 1000f;

		// Token: 0x04000ECE RID: 3790
		public const float WinterMovementDifficultyOffset = 2f;

		// Token: 0x04000ECF RID: 3791
		public const float MaxTempForWinterOffset = 5f;

		// Token: 0x06001928 RID: 6440 RVA: 0x000DB4D8 File Offset: 0x000D98D8
		public WorldPathGrid()
		{
			this.ResetPathGrid();
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06001929 RID: 6441 RVA: 0x000DB4F0 File Offset: 0x000D98F0
		private static int DayOfYearAt0Long
		{
			get
			{
				return GenDate.DayOfYear((long)GenTicks.TicksAbs, 0f);
			}
		}

		// Token: 0x0600192A RID: 6442 RVA: 0x000DB515 File Offset: 0x000D9915
		public void ResetPathGrid()
		{
			this.movementDifficulty = new float[Find.WorldGrid.TilesCount];
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x000DB52D File Offset: 0x000D992D
		public void WorldPathGridTick()
		{
			if (this.allPathCostsRecalculatedDayOfYear != WorldPathGrid.DayOfYearAt0Long)
			{
				this.RecalculateAllPerceivedPathCosts();
			}
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x000DB548 File Offset: 0x000D9948
		public bool Passable(int tile)
		{
			return Find.WorldGrid.InBounds(tile) && this.movementDifficulty[tile] < 1000f;
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x000DB584 File Offset: 0x000D9984
		public bool PassableFast(int tile)
		{
			return this.movementDifficulty[tile] < 1000f;
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x000DB5A8 File Offset: 0x000D99A8
		public float PerceivedMovementDifficultyAt(int tile)
		{
			return this.movementDifficulty[tile];
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x000DB5C8 File Offset: 0x000D99C8
		public void RecalculatePerceivedMovementDifficultyAt(int tile, int? ticksAbs = null)
		{
			if (Find.WorldGrid.InBounds(tile))
			{
				bool flag = this.PassableFast(tile);
				this.movementDifficulty[tile] = WorldPathGrid.CalculatedMovementDifficultyAt(tile, true, ticksAbs, null);
				if (flag != this.PassableFast(tile))
				{
					Find.WorldReachability.ClearCache();
				}
			}
		}

		// Token: 0x06001930 RID: 6448 RVA: 0x000DB61C File Offset: 0x000D9A1C
		public void RecalculateAllPerceivedPathCosts()
		{
			this.RecalculateAllPerceivedPathCosts(null);
			this.allPathCostsRecalculatedDayOfYear = WorldPathGrid.DayOfYearAt0Long;
		}

		// Token: 0x06001931 RID: 6449 RVA: 0x000DB644 File Offset: 0x000D9A44
		public void RecalculateAllPerceivedPathCosts(int? ticksAbs)
		{
			this.allPathCostsRecalculatedDayOfYear = -1;
			for (int i = 0; i < this.movementDifficulty.Length; i++)
			{
				this.RecalculatePerceivedMovementDifficultyAt(i, ticksAbs);
			}
		}

		// Token: 0x06001932 RID: 6450 RVA: 0x000DB67C File Offset: 0x000D9A7C
		public static float CalculatedMovementDifficultyAt(int tile, bool perceivedStatic, int? ticksAbs = null, StringBuilder explanation = null)
		{
			Tile tile2 = Find.WorldGrid[tile];
			if (explanation != null && explanation.Length > 0)
			{
				explanation.AppendLine();
			}
			float result;
			if (tile2.biome.impassable || tile2.hilliness == Hilliness.Impassable)
			{
				if (explanation != null)
				{
					explanation.Append("Impassable".Translate());
				}
				result = 1000f;
			}
			else
			{
				float num = 0f;
				num += tile2.biome.movementDifficulty;
				if (explanation != null)
				{
					explanation.Append(tile2.biome.LabelCap + ": " + tile2.biome.movementDifficulty.ToStringWithSign("0.#"));
				}
				float num2 = WorldPathGrid.HillinessMovementDifficultyOffset(tile2.hilliness);
				num += num2;
				if (explanation != null && num2 != 0f)
				{
					explanation.AppendLine();
					explanation.Append(tile2.hilliness.GetLabelCap() + ": " + num2.ToStringWithSign("0.#"));
				}
				num += WorldPathGrid.GetCurrentWinterMovementDifficultyOffset(tile, new int?((ticksAbs == null) ? GenTicks.TicksAbs : ticksAbs.Value), explanation);
				result = num;
			}
			return result;
		}

		// Token: 0x06001933 RID: 6451 RVA: 0x000DB7C4 File Offset: 0x000D9BC4
		public static float GetCurrentWinterMovementDifficultyOffset(int tile, int? ticksAbs = null, StringBuilder explanation = null)
		{
			if (ticksAbs == null)
			{
				ticksAbs = new int?(GenTicks.TicksAbs);
			}
			Vector2 vector = Find.WorldGrid.LongLatOf(tile);
			float yearPct = GenDate.YearPercent((long)ticksAbs.Value, vector.x);
			float num;
			float num2;
			float num3;
			float num4;
			float num5;
			float num6;
			SeasonUtility.GetSeason(yearPct, vector.y, out num, out num2, out num3, out num4, out num5, out num6);
			float num7 = num4 + num6;
			num7 *= Mathf.InverseLerp(5f, 0f, GenTemperature.GetTemperatureFromSeasonAtTile(ticksAbs.Value, tile));
			float result;
			if (num7 > 0.01f)
			{
				float num8 = 2f * num7;
				if (explanation != null)
				{
					explanation.AppendLine();
					explanation.Append("Winter".Translate());
					if (num7 < 0.999f)
					{
						explanation.Append(" (" + num7.ToStringPercent("F0") + ")");
					}
					explanation.Append(": ");
					explanation.Append(num8.ToStringWithSign("0.#"));
				}
				result = num8;
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x06001934 RID: 6452 RVA: 0x000DB8EC File Offset: 0x000D9CEC
		public static bool WillWinterEverAffectMovementDifficulty(int tile)
		{
			int ticksAbs = GenTicks.TicksAbs;
			for (int i = 0; i < 3600000; i += 60000)
			{
				int absTick = ticksAbs + i;
				float temperatureFromSeasonAtTile = GenTemperature.GetTemperatureFromSeasonAtTile(absTick, tile);
				if (temperatureFromSeasonAtTile < 5f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001935 RID: 6453 RVA: 0x000DB948 File Offset: 0x000D9D48
		private static float HillinessMovementDifficultyOffset(Hilliness hilliness)
		{
			float result;
			switch (hilliness)
			{
			case Hilliness.Flat:
				result = 0f;
				break;
			case Hilliness.SmallHills:
				result = 0.5f;
				break;
			case Hilliness.LargeHills:
				result = 1.5f;
				break;
			case Hilliness.Mountainous:
				result = 3f;
				break;
			case Hilliness.Impassable:
				result = 1000f;
				break;
			default:
				result = 0f;
				break;
			}
			return result;
		}
	}
}
