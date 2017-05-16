using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Verse.AI
{
	public class PathFinder
	{
		internal struct CostNode
		{
			public int index;

			public int cost;

			public CostNode(int index, int cost)
			{
				this.index = index;
				this.cost = cost;
			}
		}

		private struct PathFinderNodeFast
		{
			public int knownCost;

			public int heuristicCost;

			public int parentIndex;

			public int costNodeCost;

			public ushort status;
		}

		internal class CostNodeComparer : IComparer<PathFinder.CostNode>
		{
			public int Compare(PathFinder.CostNode a, PathFinder.CostNode b)
			{
				int cost = a.cost;
				int cost2 = b.cost;
				if (cost > cost2)
				{
					return 1;
				}
				if (cost < cost2)
				{
					return -1;
				}
				return 0;
			}
		}

		public const int DefaultMoveTicksCardinal = 13;

		private const int DefaultMoveTicksDiagonal = 18;

		private const int SearchLimit = 160000;

		private const int Cost_DoorToBash = 300;

		private const int Cost_BlockedWall = 60;

		private const float Cost_BlockedWallPerHitPoint = 0.1f;

		public const int Cost_OutsideAllowedArea = 600;

		private const int Cost_PawnCollision = 100;

		private const float NonRegionBasedHeuristicStrengthAnimal = 1.75f;

		private Map map;

		private FastPriorityQueue<PathFinder.CostNode> openList;

		private PathFinder.PathFinderNodeFast[] calcGrid;

		private List<int> disallowedCornerIndices = new List<int>(4);

		private ushort statusOpenValue = 1;

		private ushort statusClosedValue = 2;

		private RegionCostCalculatorWrapper regionCostCalculator;

		private int mapSizeX;

		private int mapSizeZ;

		private PathGrid pathGrid;

		private Building[] edificeGrid;

		private CellIndices cellIndices;

		private static readonly sbyte[] Directions = new sbyte[]
		{
			0,
			1,
			0,
			-1,
			1,
			1,
			-1,
			-1,
			-1,
			0,
			1,
			0,
			-1,
			1,
			1,
			-1
		};

		private static readonly SimpleCurve NonRegionBasedHeuristicStrengthHuman_DistanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(40f, 1f),
				true
			},
			{
				new CurvePoint(120f, 2.8f),
				true
			}
		};

		private readonly SimpleCurve RegionHeuristicWeight_CurProgressPct = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1.04f),
				true
			},
			{
				new CurvePoint(0.5f, 1.08f),
				true
			},
			{
				new CurvePoint(1f, 1.16f),
				true
			}
		};

		private static readonly SimpleCurve RegionHeuristicWeight_NodesOpened = new SimpleCurve
		{
			{
				new CurvePoint(18f, 0.25f),
				true
			},
			{
				new CurvePoint(30f, 1f),
				true
			},
			{
				new CurvePoint(1000f, 1f),
				true
			},
			{
				new CurvePoint(1500f, 1.3f),
				true
			},
			{
				new CurvePoint(7500f, 4f),
				true
			},
			{
				new CurvePoint(15000f, 5.5f),
				true
			},
			{
				new CurvePoint(25000f, 30f),
				true
			},
			{
				new CurvePoint(35000f, 100f),
				true
			}
		};

		public PathFinder(Map map)
		{
			this.map = map;
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.calcGrid = new PathFinder.PathFinderNodeFast[this.mapSizeX * this.mapSizeZ];
			this.openList = new FastPriorityQueue<PathFinder.CostNode>(new PathFinder.CostNodeComparer());
			this.regionCostCalculator = new RegionCostCalculatorWrapper(map);
		}

		public PawnPath FindPath(IntVec3 start, LocalTargetInfo dest, Pawn pawn, PathEndMode peMode = PathEndMode.OnCell)
		{
			bool flag = false;
			if (pawn != null && pawn.CurJob != null && pawn.CurJob.canBash)
			{
				flag = true;
			}
			bool canBash = flag;
			return this.FindPath(start, dest, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, canBash), peMode);
		}

		public PawnPath FindPath(IntVec3 start, LocalTargetInfo dest, TraverseParms traverseParms, PathEndMode peMode = PathEndMode.OnCell)
		{
			if (DebugSettings.pathThroughWalls)
			{
				traverseParms.mode = TraverseMode.PassAllDestroyableThings;
			}
			Pawn pawn = traverseParms.pawn;
			if (pawn != null && pawn.Map != this.map)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to FindPath for pawn which is spawned in another map. His map PathFinder should have been used, not this one. pawn=",
					pawn,
					" pawn.Map=",
					pawn.Map,
					" map=",
					this.map
				}));
				return PawnPath.NotFound;
			}
			if (!start.IsValid)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to FindPath with invalid start ",
					start,
					", pawn= ",
					pawn
				}));
				return PawnPath.NotFound;
			}
			if (!dest.IsValid)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to FindPath with invalid dest ",
					dest,
					", pawn= ",
					pawn
				}));
				return PawnPath.NotFound;
			}
			if (traverseParms.mode == TraverseMode.ByPawn)
			{
				if (!pawn.CanReach(dest, peMode, Danger.Deadly, traverseParms.canBash, traverseParms.mode))
				{
					return PawnPath.NotFound;
				}
			}
			else if (!this.map.reachability.CanReach(start, dest, peMode, traverseParms))
			{
				return PawnPath.NotFound;
			}
			this.PfProfilerBeginSample(string.Concat(new object[]
			{
				"FindPath for ",
				pawn,
				" from ",
				start,
				" to ",
				dest,
				(!dest.HasThing) ? string.Empty : (" at " + dest.Cell)
			}));
			this.cellIndices = this.map.cellIndices;
			this.pathGrid = this.map.pathGrid;
			this.edificeGrid = this.map.edificeGrid.InnerArray;
			int x = dest.Cell.x;
			int z = dest.Cell.z;
			int num = this.cellIndices.CellToIndex(start);
			int num2 = this.cellIndices.CellToIndex(dest.Cell);
			ByteGrid byteGrid = (pawn == null) ? null : pawn.GetAvoidGrid();
			bool flag = traverseParms.mode == TraverseMode.PassAllDestroyableThings;
			bool flag2 = !flag;
			CellRect cellRect = this.CalculateDestinationRect(dest, peMode);
			bool flag3 = cellRect.Width == 1 && cellRect.Height == 1;
			int[] array = this.map.pathGrid.pathGrid;
			EdificeGrid edificeGrid = this.map.edificeGrid;
			int num3 = 0;
			Area allowedArea = this.GetAllowedArea(pawn);
			bool flag4 = pawn != null && PawnUtility.ShouldCollideWithPawns(pawn);
			bool flag5 = !flag && start.GetRegion(this.map, RegionType.Set_Passable) != null;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			float num7 = (!flag5) ? this.DetermineHeuristicStrength(pawn, start, dest) : 1f;
			int num8;
			int num9;
			if (pawn != null)
			{
				num8 = pawn.TicksPerMoveCardinal;
				num9 = pawn.TicksPerMoveDiagonal;
			}
			else
			{
				num8 = 13;
				num9 = 18;
			}
			this.CalculateAndAddDisallowedCorners(traverseParms, peMode, cellRect);
			if (flag5)
			{
				this.regionCostCalculator.Init(cellRect, traverseParms, num8, num9, byteGrid, allowedArea, this.disallowedCornerIndices);
				int pathCostFromDestToRegion = this.regionCostCalculator.GetPathCostFromDestToRegion(this.map.cellIndices.CellToIndex(start));
				int num10 = pathCostFromDestToRegion + num8 * 50;
				this.RegionHeuristicWeight_CurProgressPct[1] = new CurvePoint((float)(num10 / 2), this.RegionHeuristicWeight_CurProgressPct[1].y);
				this.RegionHeuristicWeight_CurProgressPct[2] = new CurvePoint((float)num10, this.RegionHeuristicWeight_CurProgressPct[2].y);
			}
			this.statusOpenValue += 2;
			this.statusClosedValue += 2;
			if (this.statusClosedValue >= 65435)
			{
				this.ResetStatuses();
			}
			this.calcGrid[num].knownCost = 0;
			this.calcGrid[num].heuristicCost = 0;
			this.calcGrid[num].costNodeCost = 0;
			this.calcGrid[num].parentIndex = num;
			this.calcGrid[num].status = this.statusOpenValue;
			this.openList.Clear();
			this.openList.Push(new PathFinder.CostNode(num, 0));
			while (true)
			{
				this.PfProfilerBeginSample("Open cell");
				if (this.openList.Count <= 0)
				{
					break;
				}
				num5 += this.openList.Count;
				num6++;
				PathFinder.CostNode costNode = this.openList.Pop();
				if (costNode.cost != this.calcGrid[costNode.index].costNodeCost)
				{
					this.PfProfilerEndSample();
				}
				else
				{
					num = costNode.index;
					if (this.calcGrid[num].status == this.statusClosedValue)
					{
						this.PfProfilerEndSample();
					}
					else
					{
						IntVec3 c = this.cellIndices.IndexToCell(num);
						ushort num11 = (ushort)c.x;
						ushort num12 = (ushort)c.z;
						if (DebugViewSettings.drawPaths)
						{
							this.DebugFlash(c, (float)this.calcGrid[num].knownCost / 1500f, this.calcGrid[num].knownCost.ToString());
						}
						if (flag3)
						{
							if (num == num2)
							{
								goto Block_27;
							}
						}
						else if (cellRect.Contains(c) && !this.disallowedCornerIndices.Contains(num))
						{
							goto Block_29;
						}
						if (num3 > 160000)
						{
							goto Block_30;
						}
						this.PfProfilerEndSample();
						this.PfProfilerBeginSample("Neighbor consideration");
						for (int i = 0; i < 8; i++)
						{
							ushort num13 = (ushort)((int)num11 + (int)PathFinder.Directions[i]);
							ushort num14 = (ushort)((int)num12 + (int)PathFinder.Directions[i + 8]);
							IntVec3 intVec = new IntVec3((int)num13, 0, (int)num14);
							int num15 = this.cellIndices.CellToIndex((int)num13, (int)num14);
							if ((int)num13 >= this.mapSizeX || (int)num14 >= this.mapSizeZ)
							{
								this.DebugFlash(intVec, 0.75f, "oob");
							}
							else if (this.calcGrid[num15].status != this.statusClosedValue || flag5)
							{
								int num16 = 0;
								bool flag6 = false;
								if (!this.pathGrid.WalkableFast(intVec))
								{
									if (!flag)
									{
										this.DebugFlash(intVec, 0.22f, "walk");
										goto IL_D34;
									}
									flag6 = true;
									num16 += 60;
									Building building = edificeGrid[intVec];
									if (building == null)
									{
										goto IL_D34;
									}
									if (!PathFinder.IsDestroyable(building))
									{
										goto IL_D34;
									}
									num16 += (int)((float)building.HitPoints * 0.1f);
								}
								if (i > 3)
								{
									switch (i)
									{
									case 4:
										if (this.BlocksDiagonalMovement((int)num11, (int)(num12 - 1)))
										{
											if (!flag || !flag2)
											{
												this.DebugFlash(new IntVec3((int)num11, 0, (int)(num12 - 1)), 0.9f, "corn");
												goto IL_D34;
											}
											num16 += 60;
										}
										if (this.BlocksDiagonalMovement((int)(num11 + 1), (int)num12))
										{
											if (!flag || !flag2)
											{
												this.DebugFlash(new IntVec3((int)(num11 + 1), 0, (int)num12), 0.9f, "corn");
												goto IL_D34;
											}
											num16 += 60;
										}
										break;
									case 5:
										if (this.BlocksDiagonalMovement((int)num11, (int)(num12 + 1)))
										{
											if (!flag || !flag2)
											{
												this.DebugFlash(new IntVec3((int)num11, 0, (int)(num12 + 1)), 0.9f, "corn");
												goto IL_D34;
											}
											num16 += 60;
										}
										if (this.BlocksDiagonalMovement((int)(num11 + 1), (int)num12))
										{
											if (!flag || !flag2)
											{
												this.DebugFlash(new IntVec3((int)(num11 + 1), 0, (int)num12), 0.9f, "corn");
												goto IL_D34;
											}
											num16 += 60;
										}
										break;
									case 6:
										if (this.BlocksDiagonalMovement((int)num11, (int)(num12 + 1)))
										{
											if (!flag || !flag2)
											{
												this.DebugFlash(new IntVec3((int)num11, 0, (int)(num12 + 1)), 0.9f, "corn");
												goto IL_D34;
											}
											num16 += 60;
										}
										if (this.BlocksDiagonalMovement((int)(num11 - 1), (int)num12))
										{
											if (!flag || !flag2)
											{
												this.DebugFlash(new IntVec3((int)(num11 - 1), 0, (int)num12), 0.9f, "corn");
												goto IL_D34;
											}
											num16 += 60;
										}
										break;
									case 7:
										if (this.BlocksDiagonalMovement((int)num11, (int)(num12 - 1)))
										{
											if (!flag || !flag2)
											{
												this.DebugFlash(new IntVec3((int)num11, 0, (int)(num12 - 1)), 0.9f, "corn");
												goto IL_D34;
											}
											num16 += 60;
										}
										if (this.BlocksDiagonalMovement((int)(num11 - 1), (int)num12))
										{
											if (!flag || !flag2)
											{
												this.DebugFlash(new IntVec3((int)(num11 - 1), 0, (int)num12), 0.9f, "corn");
												goto IL_D34;
											}
											num16 += 60;
										}
										break;
									}
								}
								int num17 = (i <= 3) ? num8 : num9;
								num17 += num16;
								if (!flag6)
								{
									num17 += array[num15];
								}
								if (byteGrid != null)
								{
									num17 += (int)(byteGrid[num15] * 8);
								}
								if (allowedArea != null && !allowedArea[intVec])
								{
									num17 += 600;
								}
								if (flag4 && PawnUtility.AnyPawnBlockingPathAt(intVec, pawn, false, false))
								{
									num17 += 100;
								}
								Building building2 = this.edificeGrid[this.cellIndices.CellToIndex((int)num13, (int)num14)];
								if (building2 != null)
								{
									this.PfProfilerBeginSample("Edifices");
									int buildingCost = PathFinder.GetBuildingCost(building2, traverseParms, pawn);
									if (buildingCost == 2147483647)
									{
										this.PfProfilerEndSample();
										goto IL_D34;
									}
									num17 += buildingCost;
									this.PfProfilerEndSample();
								}
								int num18 = num17 + this.calcGrid[num].knownCost;
								ushort status = this.calcGrid[num15].status;
								if (status == this.statusClosedValue || status == this.statusOpenValue)
								{
									int num19 = 0;
									if (status == this.statusClosedValue)
									{
										num19 = num8;
									}
									if (this.calcGrid[num15].knownCost <= num18 + num19)
									{
										goto IL_D34;
									}
								}
								if (status != this.statusClosedValue && status != this.statusOpenValue)
								{
									if (flag5)
									{
										this.calcGrid[num15].heuristicCost = this.regionCostCalculator.GetPathCostFromDestToRegion(num15);
									}
									else
									{
										int dx = Math.Abs((int)num13 - x);
										int dz = Math.Abs((int)num14 - z);
										int num20 = GenMath.OctileDistance(dx, dz, num8, num9);
										this.calcGrid[num15].heuristicCost = Mathf.RoundToInt((float)num20 * num7);
									}
								}
								float num21 = (!flag5) ? 1f : (this.RegionHeuristicWeight_CurProgressPct.Evaluate((float)this.calcGrid[num15].heuristicCost) * PathFinder.RegionHeuristicWeight_NodesOpened.Evaluate((float)num4));
								int num22 = num18 + Mathf.CeilToInt((float)this.calcGrid[num15].heuristicCost * num21);
								this.calcGrid[num15].parentIndex = num;
								this.calcGrid[num15].knownCost = num18;
								this.calcGrid[num15].status = this.statusOpenValue;
								this.calcGrid[num15].costNodeCost = num22;
								num4++;
								this.openList.Push(new PathFinder.CostNode(num15, num22));
							}
							IL_D34:;
						}
						this.PfProfilerEndSample();
						num3++;
						this.calcGrid[num].status = this.statusClosedValue;
					}
				}
			}
			string text = (pawn == null || pawn.CurJob == null) ? "null" : pawn.CurJob.ToString();
			string text2 = (pawn == null || pawn.Faction == null) ? "null" : pawn.Faction.ToString();
			Log.Warning(string.Concat(new object[]
			{
				pawn,
				" pathing from ",
				start,
				" to ",
				dest,
				" ran out of cells to process.\nJob:",
				text,
				"\nFaction: ",
				text2
			}));
			this.DebugDrawRichData();
			this.PfProfilerEndSample();
			return PawnPath.NotFound;
			Block_27:
			this.PfProfilerEndSample();
			return this.FinalizedPath(num);
			Block_29:
			this.PfProfilerEndSample();
			return this.FinalizedPath(num);
			Block_30:
			Log.Warning(string.Concat(new object[]
			{
				pawn,
				" pathing from ",
				start,
				" to ",
				dest,
				" hit search limit of ",
				160000,
				" cells."
			}));
			this.DebugDrawRichData();
			this.PfProfilerEndSample();
			return PawnPath.NotFound;
		}

		public static int GetBuildingCost(Building b, TraverseParms traverseParms, Pawn pawn)
		{
			Building_Door building_Door = b as Building_Door;
			if (building_Door != null)
			{
				switch (traverseParms.mode)
				{
				case TraverseMode.ByPawn:
					if (!traverseParms.canBash && building_Door.IsForbiddenToPass(pawn))
					{
						if (DebugViewSettings.drawPaths)
						{
							PathFinder.DebugFlash(b.Position, b.Map, 0.77f, "forbid");
						}
						return 2147483647;
					}
					if (!building_Door.FreePassage)
					{
						if (building_Door.PawnCanOpen(pawn))
						{
							return building_Door.TicksToOpenNow;
						}
						if (traverseParms.canBash)
						{
							return 300;
						}
						if (DebugViewSettings.drawPaths)
						{
							PathFinder.DebugFlash(b.Position, b.Map, 0.34f, "cant pass");
						}
						return 2147483647;
					}
					break;
				case TraverseMode.NoPassClosedDoors:
					if (!building_Door.FreePassage)
					{
						return 2147483647;
					}
					break;
				}
			}
			else if (pawn != null)
			{
				return (int)b.PathFindCostFor(pawn);
			}
			return 0;
		}

		public static bool IsDestroyable(Thing th)
		{
			return th.def.useHitPoints && th.def.destroyable;
		}

		private bool BlocksDiagonalMovement(int x, int z)
		{
			return PathFinder.BlocksDiagonalMovement(x, z, this.map);
		}

		public static bool BlocksDiagonalMovement(int x, int z, Map map)
		{
			return PathFinder.BlocksDiagonalMovement(map.cellIndices.CellToIndex(x, z), map);
		}

		public static bool BlocksDiagonalMovement(int index, Map map)
		{
			return !map.pathGrid.WalkableFast(index) || map.edificeGrid[index] is Building_Door;
		}

		private void DebugFlash(IntVec3 c, float colorPct, string str)
		{
			PathFinder.DebugFlash(c, this.map, colorPct, str);
		}

		private static void DebugFlash(IntVec3 c, Map map, float colorPct, string str)
		{
			if (DebugViewSettings.drawPaths)
			{
				map.debugDrawer.FlashCell(c, colorPct, str);
			}
		}

		private PawnPath FinalizedPath(int finalIndex)
		{
			PawnPath emptyPawnPath = this.map.pawnPathPool.GetEmptyPawnPath();
			int num = finalIndex;
			while (true)
			{
				PathFinder.PathFinderNodeFast pathFinderNodeFast = this.calcGrid[num];
				int parentIndex = pathFinderNodeFast.parentIndex;
				emptyPawnPath.AddNode(this.map.cellIndices.IndexToCell(num));
				if (num == parentIndex)
				{
					break;
				}
				num = parentIndex;
			}
			emptyPawnPath.SetupFound((float)this.calcGrid[finalIndex].knownCost);
			this.PfProfilerEndSample();
			return emptyPawnPath;
		}

		private void ResetStatuses()
		{
			int num = this.calcGrid.Length;
			for (int i = 0; i < num; i++)
			{
				this.calcGrid[i].status = 0;
			}
			this.statusOpenValue = 1;
			this.statusClosedValue = 2;
		}

		[Conditional("PFPROFILE")]
		private void PfProfilerBeginSample(string s)
		{
			ProfilerThreadCheck.BeginSample(s);
		}

		[Conditional("PFPROFILE")]
		private void PfProfilerEndSample()
		{
			ProfilerThreadCheck.EndSample();
		}

		private void DebugDrawRichData()
		{
			if (DebugViewSettings.drawPaths)
			{
				while (this.openList.Count > 0)
				{
					int index = this.openList.Pop().index;
					IntVec3 c = new IntVec3(index % this.mapSizeX, 0, index / this.mapSizeX);
					this.map.debugDrawer.FlashCell(c, 0f, "open");
				}
			}
		}

		private float DetermineHeuristicStrength(Pawn pawn, IntVec3 start, LocalTargetInfo dest)
		{
			if (pawn != null && pawn.RaceProps.Animal)
			{
				return 1.75f;
			}
			float lengthHorizontal = (start - dest.Cell).LengthHorizontal;
			return (float)Mathf.RoundToInt(PathFinder.NonRegionBasedHeuristicStrengthHuman_DistanceCurve.Evaluate(lengthHorizontal));
		}

		private CellRect CalculateDestinationRect(LocalTargetInfo dest, PathEndMode peMode)
		{
			CellRect result;
			if (!dest.HasThing || peMode == PathEndMode.OnCell)
			{
				result = CellRect.SingleCell(dest.Cell);
			}
			else
			{
				result = dest.Thing.OccupiedRect();
			}
			if (peMode == PathEndMode.Touch)
			{
				result = result.ExpandedBy(1);
			}
			return result;
		}

		private Area GetAllowedArea(Pawn pawn)
		{
			if (pawn != null && pawn.playerSettings != null && !pawn.Drafted)
			{
				return pawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap;
			}
			return null;
		}

		private void CalculateAndAddDisallowedCorners(TraverseParms traverseParms, PathEndMode peMode, CellRect destinationRect)
		{
			this.disallowedCornerIndices.Clear();
			if (peMode == PathEndMode.Touch)
			{
				int minX = destinationRect.minX;
				int minZ = destinationRect.minZ;
				int maxX = destinationRect.maxX;
				int maxZ = destinationRect.maxZ;
				if (!this.IsCornerTouchAllowed(minX + 1, minZ + 1, minX + 1, minZ, minX, minZ + 1))
				{
					this.disallowedCornerIndices.Add(this.map.cellIndices.CellToIndex(minX, minZ));
				}
				if (!this.IsCornerTouchAllowed(minX + 1, maxZ - 1, minX + 1, maxZ, minX, maxZ - 1))
				{
					this.disallowedCornerIndices.Add(this.map.cellIndices.CellToIndex(minX, maxZ));
				}
				if (!this.IsCornerTouchAllowed(maxX - 1, maxZ - 1, maxX - 1, maxZ, maxX, maxZ - 1))
				{
					this.disallowedCornerIndices.Add(this.map.cellIndices.CellToIndex(maxX, maxZ));
				}
				if (!this.IsCornerTouchAllowed(maxX - 1, minZ + 1, maxX - 1, minZ, maxX, minZ + 1))
				{
					this.disallowedCornerIndices.Add(this.map.cellIndices.CellToIndex(maxX, minZ));
				}
			}
		}

		private bool IsCornerTouchAllowed(int cornerX, int cornerZ, int adjCardinal1X, int adjCardinal1Z, int adjCardinal2X, int adjCardinal2Z)
		{
			return TouchPathEndModeUtility.IsCornerTouchAllowed(cornerX, cornerZ, adjCardinal1X, adjCardinal1Z, adjCardinal2X, adjCardinal2Z, this.map);
		}
	}
}
