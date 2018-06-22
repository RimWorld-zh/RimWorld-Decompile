using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000541 RID: 1345
	public class WorldPathGrid
	{
		// Token: 0x06001925 RID: 6437 RVA: 0x000DB120 File Offset: 0x000D9520
		public WorldPathGrid()
		{
			this.ResetPathGrid();
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06001926 RID: 6438 RVA: 0x000DB138 File Offset: 0x000D9538
		private static int DayOfYearAt0Long
		{
			get
			{
				return GenDate.DayOfYear((long)GenTicks.TicksAbs, 0f);
			}
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x000DB15D File Offset: 0x000D955D
		public void ResetPathGrid()
		{
			this.movementDifficulty = new float[Find.WorldGrid.TilesCount];
		}

		// Token: 0x06001928 RID: 6440 RVA: 0x000DB175 File Offset: 0x000D9575
		public void WorldPathGridTick()
		{
			if (this.allPathCostsRecalculatedDayOfYear != WorldPathGrid.DayOfYearAt0Long)
			{
				this.RecalculateAllPerceivedPathCosts();
			}
		}

		// Token: 0x06001929 RID: 6441 RVA: 0x000DB190 File Offset: 0x000D9590
		public bool Passable(int tile)
		{
			return Find.WorldGrid.InBounds(tile) && this.movementDifficulty[tile] < 1000f;
		}

		// Token: 0x0600192A RID: 6442 RVA: 0x000DB1CC File Offset: 0x000D95CC
		public bool PassableFast(int tile)
		{
			return this.movementDifficulty[tile] < 1000f;
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x000DB1F0 File Offset: 0x000D95F0
		public float PerceivedMovementDifficultyAt(int tile)
		{
			return this.movementDifficulty[tile];
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x000DB210 File Offset: 0x000D9610
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

		// Token: 0x0600192D RID: 6445 RVA: 0x000DB264 File Offset: 0x000D9664
		public void RecalculateAllPerceivedPathCosts()
		{
			this.RecalculateAllPerceivedPathCosts(null);
			this.allPathCostsRecalculatedDayOfYear = WorldPathGrid.DayOfYearAt0Long;
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x000DB28C File Offset: 0x000D968C
		public void RecalculateAllPerceivedPathCosts(int? ticksAbs)
		{
			this.allPathCostsRecalculatedDayOfYear = -1;
			for (int i = 0; i < this.movementDifficulty.Length; i++)
			{
				this.RecalculatePerceivedMovementDifficultyAt(i, ticksAbs);
			}
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x000DB2C4 File Offset: 0x000D96C4
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

		// Token: 0x06001930 RID: 6448 RVA: 0x000DB40C File Offset: 0x000D980C
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

		// Token: 0x06001931 RID: 6449 RVA: 0x000DB534 File Offset: 0x000D9934
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

		// Token: 0x06001932 RID: 6450 RVA: 0x000DB590 File Offset: 0x000D9990
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

		// Token: 0x04000EC7 RID: 3783
		public float[] movementDifficulty;

		// Token: 0x04000EC8 RID: 3784
		private int allPathCostsRecalculatedDayOfYear = -1;

		// Token: 0x04000EC9 RID: 3785
		private const float ImpassableMovemenetDificulty = 1000f;

		// Token: 0x04000ECA RID: 3786
		public const float WinterMovementDifficultyOffset = 2f;

		// Token: 0x04000ECB RID: 3787
		public const float MaxTempForWinterOffset = 5f;
	}
}
