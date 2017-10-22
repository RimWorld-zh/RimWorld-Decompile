using RimWorld.BaseGen;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class GenStep_Roads : GenStep
	{
		private struct NeededRoad
		{
			public float angle;

			public RoadDef road;
		}

		private struct DrawCommand
		{
			public RoadDef roadDef;

			public Action action;
		}

		private struct DistanceElement
		{
			public float fromRoad;

			public float alongPath;

			public bool touched;
		}

		private const float CurveControlPointDistance = 4f;

		private const int CurveSampleMultiplier = 4;

		private readonly float[] endcapSamples = new float[5]
		{
			0.75f,
			0.8f,
			0.85f,
			0.9f,
			0.95f
		};

		public override void Generate(Map map)
		{
			List<NeededRoad> neededRoads = this.CalculateNeededRoads(map);
			if (neededRoads.Count != 0)
			{
				List<DrawCommand> list = new List<DrawCommand>();
				DeepProfiler.Start("RebuildAllRegions");
				map.regionAndRoomUpdater.RebuildAllRegionsAndRooms();
				DeepProfiler.End();
				TerrainDef rockDef = BaseGenUtility.RegionalRockTerrainDef(map.Tile, false);
				IntVec3 intVec = CellFinderLoose.TryFindCentralCell(map, 3, 10, null);
				RoadDef bestRoadType = (from rd in DefDatabase<RoadDef>.AllDefs
				where neededRoads.Count((Func<NeededRoad, bool>)((NeededRoad nr) => nr.road == rd)) >= 2
				select rd).MaxByWithFallback((Func<RoadDef, int>)((RoadDef rd) => rd.priority), (RoadDef)null);
				if (bestRoadType != null)
				{
					NeededRoad neededRoad = neededRoads[neededRoads.FindIndex((Predicate<NeededRoad>)((NeededRoad nr) => nr.road == bestRoadType))];
					neededRoads.RemoveAt(neededRoads.FindIndex((Predicate<NeededRoad>)((NeededRoad nr) => nr.road == bestRoadType)));
					NeededRoad neededRoad2 = neededRoads[neededRoads.FindIndex((Predicate<NeededRoad>)((NeededRoad nr) => nr.road == bestRoadType))];
					neededRoads.RemoveAt(neededRoads.FindIndex((Predicate<NeededRoad>)((NeededRoad nr) => nr.road == bestRoadType)));
					RoadPathingDef pathingMode = neededRoad.road.pathingMode;
					IntVec3 intVec2 = this.FindRoadExitCell(map, neededRoad.angle, intVec, ref pathingMode);
					IntVec3 end = this.FindRoadExitCell(map, neededRoad2.angle, intVec2, ref pathingMode);
					Action action = this.PrepDrawRoad(map, rockDef, intVec2, end, neededRoad.road, pathingMode, out intVec);
					list.Add(new DrawCommand
					{
						action = action,
						roadDef = bestRoadType
					});
				}
				foreach (NeededRoad item in neededRoads)
				{
					NeededRoad current = item;
					RoadPathingDef pathingMode2 = current.road.pathingMode;
					IntVec3 intVec3 = this.FindRoadExitCell(map, current.angle, intVec, ref pathingMode2);
					if (!(intVec3 == IntVec3.Invalid))
					{
						list.Add(new DrawCommand
						{
							action = this.PrepDrawRoad(map, rockDef, intVec, intVec3, current.road, pathingMode2),
							roadDef = current.road
						});
					}
				}
				foreach (DrawCommand item2 in from dc in list
				orderby dc.roadDef.priority
				select dc)
				{
					DrawCommand current2 = item2;
					if ((object)current2.action != null)
					{
						current2.action();
					}
				}
			}
		}

		private List<NeededRoad> CalculateNeededRoads(Map map)
		{
			List<int> list = new List<int>();
			Find.WorldGrid.GetTileNeighbors(map.Tile, list);
			List<NeededRoad> list2 = new List<NeededRoad>();
			foreach (int item in list)
			{
				RoadDef roadDef = Find.WorldGrid.GetRoadDef(map.Tile, item, true);
				if (roadDef != null)
				{
					list2.Add(new NeededRoad
					{
						angle = Find.WorldGrid.GetHeadingFromTo(map.Tile, item),
						road = roadDef
					});
				}
			}
			if (list2.Count > 1)
			{
				Vector3 vector = Vector3.zero;
				foreach (NeededRoad item2 in list2)
				{
					NeededRoad current2 = item2;
					vector += Vector3Utility.HorizontalVectorFromAngle(current2.angle);
				}
				vector /= (float)(-list2.Count);
				vector += Rand.UnitVector3 * 1f / 6f;
				vector.y = 0f;
				for (int i = 0; i < list2.Count; i++)
				{
					List<NeededRoad> obj = list2;
					int index = i;
					NeededRoad value = default(NeededRoad);
					NeededRoad neededRoad = list2[i];
					value.angle = (Vector3Utility.HorizontalVectorFromAngle(neededRoad.angle) + vector).AngleFlat();
					NeededRoad neededRoad2 = list2[i];
					value.road = neededRoad2.road;
					obj[index] = value;
				}
			}
			return list2;
		}

		private IntVec3 FindRoadExitCell(Map map, float angle, IntVec3 crossroads, ref RoadPathingDef pathingDef)
		{
			Predicate<IntVec3> tileValidator = (Predicate<IntVec3>)delegate(IntVec3 pos)
			{
				foreach (IntVec3 item in GenRadial.RadialCellsAround(pos, 8f, true))
				{
					if (item.InBounds(map) && item.GetTerrain(map).HasTag("Water"))
					{
						return false;
					}
				}
				return true;
			};
			float validAngleSpan = 10f;
			IntVec3 result;
			while (true)
			{
				IntVec3 intVec = default(IntVec3);
				if (validAngleSpan < 90.0)
				{
					Predicate<IntVec3> angleValidator = (Predicate<IntVec3>)((IntVec3 pos) => GenGeo.AngleDifferenceBetween((pos - map.Center).AngleFlat, angle) < validAngleSpan);
					if (CellFinder.TryFindRandomEdgeCellWith((Predicate<IntVec3>)((IntVec3 x) => angleValidator(x) && tileValidator(x) && map.reachability.CanReach(crossroads, x, PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false))), map, 0f, out intVec))
					{
						result = intVec;
						break;
					}
					validAngleSpan += 10f;
					continue;
				}
				if (pathingDef == RoadPathingDefOf.Avoid)
				{
					pathingDef = RoadPathingDefOf.Bulldoze;
				}
				float validAngleSpan2;
				for (validAngleSpan2 = 10f; validAngleSpan2 < 90.0; validAngleSpan2 += 10f)
				{
					Predicate<IntVec3> angleValidator2 = (Predicate<IntVec3>)((IntVec3 pos) => GenGeo.AngleDifferenceBetween((pos - map.Center).AngleFlat, angle) < validAngleSpan2);
					if (CellFinder.TryFindRandomEdgeCellWith((Predicate<IntVec3>)((IntVec3 x) => angleValidator2(x) && tileValidator(x) && map.reachability.CanReach(crossroads, x, PathEndMode.OnCell, TraverseParms.For(TraverseMode.PassAllDestroyableThings, Danger.Deadly, false))), map, 0f, out intVec))
						goto IL_0137;
				}
				Log.Error(string.Format("Can't find exit from map from {0} to angle {1}", crossroads, angle));
				result = IntVec3.Invalid;
				break;
				IL_0137:
				result = intVec;
				break;
			}
			return result;
		}

		private Action PrepDrawRoad(Map map, TerrainDef rockDef, IntVec3 start, IntVec3 end, RoadDef roadDef, RoadPathingDef pathingDef)
		{
			IntVec3 intVec = default(IntVec3);
			return this.PrepDrawRoad(map, rockDef, start, end, roadDef, pathingDef, out intVec);
		}

		private Action PrepDrawRoad(Map map, TerrainDef rockDef, IntVec3 start, IntVec3 end, RoadDef roadDef, RoadPathingDef pathingDef, out IntVec3 centerpoint)
		{
			centerpoint = IntVec3.Invalid;
			PawnPath pawnPath = map.pathFinder.FindPath(start, end, TraverseParms.For(TraverseMode.NoPassClosedDoorsOrWater, Danger.Deadly, false), PathEndMode.OnCell);
			if (pawnPath == PawnPath.NotFound)
			{
				pawnPath = map.pathFinder.FindPath(start, end, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), PathEndMode.OnCell);
			}
			if (pawnPath == PawnPath.NotFound)
			{
				pawnPath = map.pathFinder.FindPath(start, end, TraverseParms.For(TraverseMode.PassAllDestroyableThingsNotWater, Danger.Deadly, false), PathEndMode.OnCell);
			}
			if (pawnPath == PawnPath.NotFound)
			{
				pawnPath = map.pathFinder.FindPath(start, end, TraverseParms.For(TraverseMode.PassAllDestroyableThings, Danger.Deadly, false), PathEndMode.OnCell);
			}
			Action result;
			if (pawnPath == PawnPath.NotFound)
			{
				result = null;
			}
			else
			{
				List<IntVec3> list = this.RefinePath(pawnPath.NodesReversed, map);
				pawnPath.ReleaseToPool();
				IntVec3 size = map.Size;
				int x = size.x;
				IntVec3 size2 = map.Size;
				DistanceElement[,] distance = new DistanceElement[x, size2.z];
				int count = list.Count;
				int centerpointIndex = Mathf.RoundToInt(Rand.Range(0.3f, 0.7f) * (float)count);
				int num = Mathf.Max(1, GenMath.RoundRandom((float)count / (float)roadDef.tilesPerSegment));
				for (int num2 = 0; num2 < num; num2++)
				{
					int pathStartIndex = Mathf.RoundToInt((float)(count - 1) / (float)num * (float)num2);
					int pathEndIndex = Mathf.RoundToInt((float)(count - 1) / (float)num * (float)(num2 + 1));
					this.DrawCurveSegment(distance, list, pathStartIndex, pathEndIndex, pathingDef, map, centerpointIndex, ref centerpoint);
				}
				result = (Action)delegate()
				{
					this.ApplyDistanceField(distance, map, rockDef, roadDef, pathingDef);
				};
			}
			return result;
		}

		private void DrawCurveSegment(DistanceElement[,] distance, List<IntVec3> path, int pathStartIndex, int pathEndIndex, RoadPathingDef pathing, Map map, int centerpointIndex, ref IntVec3 centerpoint)
		{
			if (pathStartIndex == pathEndIndex)
			{
				Log.ErrorOnce("Zero-length segment drawn in road routine", 78187971);
			}
			else
			{
				GenMath.BezierCubicControls bcc = this.GenerateBezierControls(path, pathStartIndex, pathEndIndex);
				List<Vector3> list = new List<Vector3>();
				int num = (pathEndIndex - pathStartIndex) * 4;
				for (int num2 = 0; num2 <= num; num2++)
				{
					list.Add(GenMath.BezierCubicEvaluate((float)num2 / (float)num, bcc));
				}
				int num3 = 0;
				for (int num4 = pathStartIndex; num4 <= pathEndIndex; num4++)
				{
					if (num4 > 0 && num4 <= path.Count && path[num4].InBounds(map) && path[num4].GetTerrain(map).HasTag("Water"))
					{
						num3++;
					}
				}
				if (pathStartIndex + 1 < pathEndIndex)
				{
					int num5 = 0;
					while (num5 < list.Count)
					{
						IntVec3 intVec = list[num5].ToIntVec3();
						bool flag = intVec.InBounds(map) && intVec.Impassable(map);
						int num6 = 0;
						int num7 = 0;
						while (num7 < GenAdj.CardinalDirections.Length && !flag)
						{
							IntVec3 c = intVec + GenAdj.CardinalDirections[num7];
							if (c.InBounds(map))
							{
								flag |= (pathing == RoadPathingDefOf.Avoid && c.Impassable(map));
								if (c.GetTerrain(map).HasTag("Water"))
								{
									num6++;
								}
								if (flag)
									break;
							}
							num7++;
						}
						if (!flag && !((float)num6 > (float)num3 * 1.5 + 2.0))
						{
							num5++;
							continue;
						}
						this.DrawCurveSegment(distance, path, pathStartIndex, (pathStartIndex + pathEndIndex) / 2, pathing, map, centerpointIndex, ref centerpoint);
						this.DrawCurveSegment(distance, path, (pathStartIndex + pathEndIndex) / 2, pathEndIndex, pathing, map, centerpointIndex, ref centerpoint);
						return;
					}
				}
				for (int i = 0; i < list.Count; i++)
				{
					Vector3 vector = list[i];
					float x = vector.x;
					Vector3 vector2 = list[i];
					this.FillDistanceField(distance, x, vector2.z, GenMath.LerpDouble(0f, (float)(list.Count - 1), (float)pathStartIndex, (float)pathEndIndex, (float)i), 10f);
				}
				if (centerpointIndex >= pathStartIndex && centerpointIndex < pathEndIndex)
				{
					centerpointIndex = Mathf.Clamp(Mathf.RoundToInt(GenMath.LerpDouble((float)pathStartIndex, (float)pathEndIndex, 0f, (float)list.Count, (float)centerpointIndex)), 0, list.Count - 1);
					centerpoint = list[centerpointIndex].ToIntVec3();
				}
			}
		}

		private GenMath.BezierCubicControls GenerateBezierControls(List<IntVec3> path, int pathStartIndex, int pathEndIndex)
		{
			int index = Mathf.Max(0, pathStartIndex - (pathEndIndex - pathStartIndex));
			int index2 = Mathf.Min(path.Count - 1, pathEndIndex - (pathStartIndex - pathEndIndex));
			return new GenMath.BezierCubicControls
			{
				w0 = path[pathStartIndex].ToVector3Shifted(),
				w1 = path[pathStartIndex].ToVector3Shifted() + (path[pathEndIndex] - path[index]).ToVector3().normalized * 4f,
				w2 = path[pathEndIndex].ToVector3Shifted() + (path[pathStartIndex] - path[index2]).ToVector3().normalized * 4f,
				w3 = path[pathEndIndex].ToVector3Shifted()
			};
		}

		private void ApplyDistanceField(DistanceElement[,] distance, Map map, TerrainDef rockDef, RoadDef roadDef, RoadPathingDef pathingDef)
		{
			int num = 0;
			while (true)
			{
				int num2 = num;
				IntVec3 size = map.Size;
				if (num2 < size.x)
				{
					int num3 = 0;
					while (true)
					{
						int num4 = num3;
						IntVec3 size2 = map.Size;
						if (num4 < size2.z)
						{
							DistanceElement distanceElement = distance[num, num3];
							if (distanceElement.touched)
							{
								float b = Mathf.Abs((float)(distanceElement.fromRoad + Rand.Value - 0.5));
								for (int i = 0; i < roadDef.roadGenSteps.Count; i++)
								{
									RoadDefGenStep roadDefGenStep = roadDef.roadGenSteps[i];
									float x = Mathf.LerpUnclamped(distanceElement.fromRoad, b, roadDefGenStep.antialiasingMultiplier);
									float num5 = roadDefGenStep.chancePerPositionCurve.Evaluate(x);
									if (!(num5 <= 0.0) && (roadDefGenStep.periodicSpacing == 0 || !(distanceElement.alongPath / (float)roadDefGenStep.periodicSpacing % 1.0 * (float)roadDefGenStep.periodicSpacing >= 1.0)))
									{
										IntVec3 position = new IntVec3(num, 0, num3);
										if (Rand.Value < num5)
										{
											roadDefGenStep.Place(map, position, rockDef);
										}
									}
								}
							}
							num3++;
							continue;
						}
						break;
					}
					num++;
					continue;
				}
				break;
			}
		}

		private void FillDistanceField(DistanceElement[,] field, float cx, float cz, float alongPath, float radius)
		{
			int num = Mathf.Clamp(Mathf.FloorToInt(cx - radius), 0, field.GetLength(0) - 1);
			int num2 = Mathf.Clamp(Mathf.FloorToInt(cx + radius), 0, field.GetLength(0) - 1);
			int num3 = Mathf.Clamp(Mathf.FloorToInt(cz - radius), 0, field.GetLength(1) - 1);
			int num4 = Mathf.Clamp(Mathf.FloorToInt(cz + radius), 0, field.GetLength(1) - 1);
			for (int num5 = num; num5 <= num2; num5++)
			{
				float num6 = (float)(((float)num5 + 0.5 - cx) * ((float)num5 + 0.5 - cx));
				for (int num7 = num3; num7 <= num4; num7++)
				{
					float num8 = (float)(((float)num7 + 0.5 - cz) * ((float)num7 + 0.5 - cz));
					float num9 = Mathf.Sqrt(num6 + num8);
					float fromRoad = field[num5, num7].fromRoad;
					if (!field[num5, num7].touched || num9 < fromRoad)
					{
						field[num5, num7].fromRoad = num9;
						field[num5, num7].alongPath = alongPath;
					}
					field[num5, num7].touched = true;
				}
			}
		}

		private List<IntVec3> RefinePath(List<IntVec3> input, Map map)
		{
			List<IntVec3> list = this.RefineEndcap(input, map);
			list.Reverse();
			return this.RefineEndcap(list, map);
		}

		private List<IntVec3> RefineEndcap(List<IntVec3> input, Map map)
		{
			float[] array = new float[this.endcapSamples.Length];
			for (int i = 0; i < this.endcapSamples.Length; i++)
			{
				int index = Mathf.RoundToInt((float)input.Count * this.endcapSamples[i]);
				PawnPath pawnPath = map.pathFinder.FindPath(input[index], input[input.Count - 1], TraverseParms.For(TraverseMode.NoPassClosedDoorsOrWater, Danger.Deadly, false), PathEndMode.OnCell);
				if (pawnPath == PawnPath.NotFound)
				{
					pawnPath = map.pathFinder.FindPath(input[index], input[input.Count - 1], TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), PathEndMode.OnCell);
				}
				if (pawnPath == PawnPath.NotFound)
				{
					pawnPath = map.pathFinder.FindPath(input[index], input[input.Count - 1], TraverseParms.For(TraverseMode.PassAllDestroyableThingsNotWater, Danger.Deadly, false), PathEndMode.OnCell);
				}
				if (pawnPath == PawnPath.NotFound)
				{
					pawnPath = map.pathFinder.FindPath(input[index], input[input.Count - 1], TraverseParms.For(TraverseMode.PassAllDestroyableThings, Danger.Deadly, false), PathEndMode.OnCell);
				}
				if (pawnPath != null && pawnPath != PawnPath.NotFound)
				{
					array[i] = pawnPath.TotalCost;
				}
				pawnPath.ReleaseToPool();
			}
			float num = 0f;
			int num2 = 0;
			IntVec3 start = IntVec3.Invalid;
			for (int j = 0; j < 2; j++)
			{
				IntVec3 facingCell = new Rot4(j).FacingCell;
				IntVec3 intVec = input[input.Count - 1];
				bool flag = true;
				if (Mathf.Abs(intVec.x * facingCell.x) > 5)
				{
					int num3 = intVec.x * facingCell.x;
					IntVec3 size = map.Size;
					if (Mathf.Abs(num3 - size.x) > 5)
					{
						flag = false;
					}
				}
				if (Mathf.Abs(intVec.z * facingCell.z) > 5)
				{
					int num4 = intVec.z * facingCell.z;
					IntVec3 size2 = map.Size;
					if (Mathf.Abs(num4 - size2.z) > 5)
					{
						flag = false;
					}
				}
				if (flag)
				{
					for (int k = 0; k < this.endcapSamples.Length; k++)
					{
						if (array[k] != 0.0)
						{
							int num5 = Mathf.RoundToInt((float)input.Count * this.endcapSamples[k]);
							IntVec3 intVec2;
							IntVec3 intVec3 = intVec2 = input[num5];
							if (facingCell.x != 0)
							{
								intVec2.x = intVec.x;
							}
							else if (facingCell.z != 0)
							{
								intVec2.z = intVec.z;
							}
							PawnPath pawnPath2 = map.pathFinder.FindPath(input[num5], input[input.Count - 1], TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), PathEndMode.OnCell);
							if (pawnPath2 == PawnPath.NotFound)
							{
								pawnPath2 = map.pathFinder.FindPath(input[num5], input[input.Count - 1], TraverseParms.For(TraverseMode.PassAllDestroyableThings, Danger.Deadly, false), PathEndMode.OnCell);
							}
							if (pawnPath2 != PawnPath.NotFound)
							{
								float num6 = array[k] / pawnPath2.TotalCost;
								if (num6 > num)
								{
									num = num6;
									num2 = num5;
									start = intVec2;
								}
								pawnPath2.ReleaseToPool();
							}
						}
					}
				}
			}
			input = new List<IntVec3>(input);
			if ((double)num > 1.75)
			{
				using (PawnPath pawnPath3 = map.pathFinder.FindPath(start, input[num2], TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), PathEndMode.OnCell))
				{
					input.RemoveRange(num2, input.Count - num2);
					input.AddRange(pawnPath3.NodesReversed);
				}
			}
			return input;
		}
	}
}
