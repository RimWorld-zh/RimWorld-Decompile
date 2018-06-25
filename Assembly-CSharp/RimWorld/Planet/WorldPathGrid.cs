using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldPathGrid
	{
		public float[] movementDifficulty;

		private int allPathCostsRecalculatedDayOfYear = -1;

		private const float ImpassableMovemenetDificulty = 1000f;

		public const float WinterMovementDifficultyOffset = 2f;

		public const float MaxTempForWinterOffset = 5f;

		public WorldPathGrid()
		{
			this.ResetPathGrid();
		}

		private static int DayOfYearAt0Long
		{
			get
			{
				return GenDate.DayOfYear((long)GenTicks.TicksAbs, 0f);
			}
		}

		public void ResetPathGrid()
		{
			this.movementDifficulty = new float[Find.WorldGrid.TilesCount];
		}

		public void WorldPathGridTick()
		{
			if (this.allPathCostsRecalculatedDayOfYear != WorldPathGrid.DayOfYearAt0Long)
			{
				this.RecalculateAllPerceivedPathCosts();
			}
		}

		public bool Passable(int tile)
		{
			return Find.WorldGrid.InBounds(tile) && this.movementDifficulty[tile] < 1000f;
		}

		public bool PassableFast(int tile)
		{
			return this.movementDifficulty[tile] < 1000f;
		}

		public float PerceivedMovementDifficultyAt(int tile)
		{
			return this.movementDifficulty[tile];
		}

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

		public void RecalculateAllPerceivedPathCosts()
		{
			this.RecalculateAllPerceivedPathCosts(null);
			this.allPathCostsRecalculatedDayOfYear = WorldPathGrid.DayOfYearAt0Long;
		}

		public void RecalculateAllPerceivedPathCosts(int? ticksAbs)
		{
			this.allPathCostsRecalculatedDayOfYear = -1;
			for (int i = 0; i < this.movementDifficulty.Length; i++)
			{
				this.RecalculatePerceivedMovementDifficultyAt(i, ticksAbs);
			}
		}

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
