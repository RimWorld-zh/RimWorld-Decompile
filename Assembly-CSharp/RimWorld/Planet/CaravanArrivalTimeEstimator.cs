using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005D4 RID: 1492
	public static class CaravanArrivalTimeEstimator
	{
		// Token: 0x06001D5E RID: 7518 RVA: 0x000FC7F4 File Offset: 0x000FABF4
		public static int EstimatedTicksToArrive(Caravan caravan, bool allowCaching)
		{
			int result;
			if (allowCaching && caravan == CaravanArrivalTimeEstimator.cachedForCaravan && caravan.pather.Destination == CaravanArrivalTimeEstimator.cachedForDest && Find.TickManager.TicksGame - CaravanArrivalTimeEstimator.cacheTicks < 100)
			{
				result = CaravanArrivalTimeEstimator.cachedResult;
			}
			else
			{
				int to;
				int num;
				if (!caravan.Spawned || !caravan.pather.Moving || caravan.pather.curPath == null)
				{
					to = -1;
					num = 0;
				}
				else
				{
					to = caravan.pather.Destination;
					num = CaravanArrivalTimeEstimator.EstimatedTicksToArrive(caravan.Tile, to, caravan.pather.curPath, caravan.pather.nextTileCostLeft, caravan.TicksPerMove, Find.TickManager.TicksAbs);
				}
				if (allowCaching)
				{
					CaravanArrivalTimeEstimator.cacheTicks = Find.TickManager.TicksGame;
					CaravanArrivalTimeEstimator.cachedForCaravan = caravan;
					CaravanArrivalTimeEstimator.cachedForDest = to;
					CaravanArrivalTimeEstimator.cachedResult = num;
				}
				result = num;
			}
			return result;
		}

		// Token: 0x06001D5F RID: 7519 RVA: 0x000FC8F4 File Offset: 0x000FACF4
		public static int EstimatedTicksToArrive(int from, int to, Caravan caravan)
		{
			int result;
			using (WorldPath worldPath = Find.WorldPathFinder.FindPath(from, to, caravan, null))
			{
				if (!worldPath.Found)
				{
					result = 0;
				}
				else
				{
					result = CaravanArrivalTimeEstimator.EstimatedTicksToArrive(from, to, worldPath, 0f, (caravan == null) ? 3500 : caravan.TicksPerMove, Find.TickManager.TicksAbs);
				}
			}
			return result;
		}

		// Token: 0x06001D60 RID: 7520 RVA: 0x000FC978 File Offset: 0x000FAD78
		public static int EstimatedTicksToArrive(int from, int to, WorldPath path, float nextTileCostLeft, int caravanTicksPerMove, int curTicksAbs)
		{
			CaravanArrivalTimeEstimator.tmpTicksToArrive.Clear();
			CaravanArrivalTimeEstimator.EstimatedTicksToArriveToEvery(from, to, path, nextTileCostLeft, caravanTicksPerMove, curTicksAbs, CaravanArrivalTimeEstimator.tmpTicksToArrive);
			return CaravanArrivalTimeEstimator.EstimatedTicksToArrive(to, CaravanArrivalTimeEstimator.tmpTicksToArrive);
		}

		// Token: 0x06001D61 RID: 7521 RVA: 0x000FC9B4 File Offset: 0x000FADB4
		public static void EstimatedTicksToArriveToEvery(int from, int to, WorldPath path, float nextTileCostLeft, int caravanTicksPerMove, int curTicksAbs, List<Pair<int, int>> outTicksToArrive)
		{
			outTicksToArrive.Clear();
			outTicksToArrive.Add(new Pair<int, int>(from, 0));
			if (from == to)
			{
				outTicksToArrive.Add(new Pair<int, int>(to, 0));
			}
			else
			{
				int num = 0;
				int num2 = from;
				int num3 = 0;
				int num4 = Mathf.CeilToInt(20000f) - 1;
				int num5 = 60000 - num4;
				int num6 = 0;
				int num7 = 0;
				int num9;
				if (CaravanRestUtility.WouldBeRestingAt(from, (long)curTicksAbs))
				{
					if (Caravan_PathFollower.IsValidFinalPushDestination(to) && (path.Peek(0) == to || (nextTileCostLeft <= 0f && path.NodesLeftCount >= 2 && path.Peek(1) == to)))
					{
						float costToMove = CaravanArrivalTimeEstimator.GetCostToMove(nextTileCostLeft, path.Peek(0) == to, curTicksAbs, num, caravanTicksPerMove, from, to);
						int num8 = Mathf.CeilToInt(costToMove / 1f);
						if (num8 <= 10000)
						{
							num += num8;
							outTicksToArrive.Add(new Pair<int, int>(to, num));
							return;
						}
					}
					num += CaravanRestUtility.LeftRestTicksAt(from, (long)curTicksAbs);
					num9 = num5;
				}
				else
				{
					num9 = CaravanRestUtility.LeftNonRestTicksAt(from, (long)curTicksAbs);
				}
				for (;;)
				{
					num7++;
					if (num7 >= 10000)
					{
						break;
					}
					if (num6 <= 0)
					{
						if (num2 == to)
						{
							goto Block_10;
						}
						bool firstInPath = num3 == 0;
						int num10 = num2;
						num2 = path.Peek(num3);
						num3++;
						outTicksToArrive.Add(new Pair<int, int>(num10, num));
						float costToMove2 = CaravanArrivalTimeEstimator.GetCostToMove(nextTileCostLeft, firstInPath, curTicksAbs, num, caravanTicksPerMove, num10, num2);
						num6 = Mathf.CeilToInt(costToMove2 / 1f);
					}
					if (num9 < num6)
					{
						num += num9;
						num6 -= num9;
						if (num2 == to && num6 <= 10000 && Caravan_PathFollower.IsValidFinalPushDestination(to))
						{
							goto Block_14;
						}
						num += num4;
						num9 = num5;
					}
					else
					{
						num += num6;
						num9 -= num6;
						num6 = 0;
					}
				}
				Log.ErrorOnce("Could not calculate estimated ticks to arrive. Too many iterations.", 1837451324, false);
				outTicksToArrive.Add(new Pair<int, int>(to, num));
				return;
				Block_10:
				outTicksToArrive.Add(new Pair<int, int>(to, num));
				return;
				Block_14:
				num += num6;
				outTicksToArrive.Add(new Pair<int, int>(to, num));
			}
		}

		// Token: 0x06001D62 RID: 7522 RVA: 0x000FCBE8 File Offset: 0x000FAFE8
		private static float GetCostToMove(float initialNextTileCostLeft, bool firstInPath, int initialTicksAbs, int curResult, int caravanTicksPerMove, int curTile, int nextTile)
		{
			float result;
			if (firstInPath)
			{
				result = initialNextTileCostLeft;
			}
			else
			{
				int value = initialTicksAbs + curResult;
				result = (float)Caravan_PathFollower.CostToMove(caravanTicksPerMove, curTile, nextTile, new int?(value), false, null, null);
			}
			return result;
		}

		// Token: 0x06001D63 RID: 7523 RVA: 0x000FCC28 File Offset: 0x000FB028
		public static int EstimatedTicksToArrive(int destinationTile, List<Pair<int, int>> estimatedTicksToArriveToEvery)
		{
			int result;
			if (destinationTile == -1)
			{
				result = 0;
			}
			else
			{
				for (int i = 0; i < estimatedTicksToArriveToEvery.Count; i++)
				{
					if (destinationTile == estimatedTicksToArriveToEvery[i].First)
					{
						return estimatedTicksToArriveToEvery[i].Second;
					}
				}
				result = 0;
			}
			return result;
		}

		// Token: 0x06001D64 RID: 7524 RVA: 0x000FCC90 File Offset: 0x000FB090
		public static int TileIllBeInAt(int ticksAbs, List<Pair<int, int>> estimatedTicksToArriveToEvery, int ticksAbsUsedToCalculateEstimatedTicksToArriveToEvery)
		{
			int result;
			if (!estimatedTicksToArriveToEvery.Any<Pair<int, int>>())
			{
				result = -1;
			}
			else
			{
				for (int i = estimatedTicksToArriveToEvery.Count - 1; i >= 0; i--)
				{
					int num = ticksAbsUsedToCalculateEstimatedTicksToArriveToEvery + estimatedTicksToArriveToEvery[i].Second;
					if (ticksAbs >= num)
					{
						return estimatedTicksToArriveToEvery[i].First;
					}
				}
				result = estimatedTicksToArriveToEvery[0].First;
			}
			return result;
		}

		// Token: 0x04001171 RID: 4465
		private static int cacheTicks = -1;

		// Token: 0x04001172 RID: 4466
		private static Caravan cachedForCaravan;

		// Token: 0x04001173 RID: 4467
		private static int cachedForDest = -1;

		// Token: 0x04001174 RID: 4468
		private static int cachedResult = -1;

		// Token: 0x04001175 RID: 4469
		private const int CacheDuration = 100;

		// Token: 0x04001176 RID: 4470
		private const int MaxIterations = 10000;

		// Token: 0x04001177 RID: 4471
		private static List<Pair<int, int>> tmpTicksToArrive = new List<Pair<int, int>>();
	}
}
