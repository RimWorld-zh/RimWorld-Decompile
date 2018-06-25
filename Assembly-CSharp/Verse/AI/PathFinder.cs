using System;
using System.Collections.Generic;
using System.Diagnostics;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000A8F RID: 2703
	public class PathFinder
	{
		// Token: 0x040025CD RID: 9677
		private Map map;

		// Token: 0x040025CE RID: 9678
		private FastPriorityQueue<PathFinder.CostNode> openList;

		// Token: 0x040025CF RID: 9679
		private PathFinder.PathFinderNodeFast[] calcGrid;

		// Token: 0x040025D0 RID: 9680
		private ushort statusOpenValue = 1;

		// Token: 0x040025D1 RID: 9681
		private ushort statusClosedValue = 2;

		// Token: 0x040025D2 RID: 9682
		private RegionCostCalculatorWrapper regionCostCalculator;

		// Token: 0x040025D3 RID: 9683
		private int mapSizeX;

		// Token: 0x040025D4 RID: 9684
		private int mapSizeZ;

		// Token: 0x040025D5 RID: 9685
		private PathGrid pathGrid;

		// Token: 0x040025D6 RID: 9686
		private Building[] edificeGrid;

		// Token: 0x040025D7 RID: 9687
		private List<Blueprint>[] blueprintGrid;

		// Token: 0x040025D8 RID: 9688
		private CellIndices cellIndices;

		// Token: 0x040025D9 RID: 9689
		private List<int> disallowedCornerIndices = new List<int>(4);

		// Token: 0x040025DA RID: 9690
		public const int DefaultMoveTicksCardinal = 13;

		// Token: 0x040025DB RID: 9691
		private const int DefaultMoveTicksDiagonal = 18;

		// Token: 0x040025DC RID: 9692
		private const int SearchLimit = 160000;

		// Token: 0x040025DD RID: 9693
		private static readonly int[] Directions = new int[]
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

		// Token: 0x040025DE RID: 9694
		private const int Cost_DoorToBash = 300;

		// Token: 0x040025DF RID: 9695
		private const int Cost_BlockedWall = 70;

		// Token: 0x040025E0 RID: 9696
		private const float Cost_BlockedWallPerHitPoint = 0.11f;

		// Token: 0x040025E1 RID: 9697
		private const int Cost_BlockedDoor = 50;

		// Token: 0x040025E2 RID: 9698
		private const float Cost_BlockedDoorPerHitPoint = 0.11f;

		// Token: 0x040025E3 RID: 9699
		public const int Cost_OutsideAllowedArea = 600;

		// Token: 0x040025E4 RID: 9700
		private const int Cost_PawnCollision = 175;

		// Token: 0x040025E5 RID: 9701
		private const int NodesToOpenBeforeRegionBasedPathing_NonColonist = 2000;

		// Token: 0x040025E6 RID: 9702
		private const int NodesToOpenBeforeRegionBasedPathing_Colonist = 100000;

		// Token: 0x040025E7 RID: 9703
		private const float NonRegionBasedHeuristicStrengthAnimal = 1.75f;

		// Token: 0x040025E8 RID: 9704
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

		// Token: 0x040025E9 RID: 9705
		private static readonly SimpleCurve RegionHeuristicWeightByNodesOpened = new SimpleCurve
		{
			{
				new CurvePoint(0f, 5f),
				true
			},
			{
				new CurvePoint(4000f, 5f),
				true
			},
			{
				new CurvePoint(5000f, 500f),
				true
			}
		};

		// Token: 0x06003C00 RID: 15360 RVA: 0x001FAE8C File Offset: 0x001F928C
		public PathFinder(Map map)
		{
			this.map = map;
			this.mapSizeX = map.Size.x;
			this.mapSizeZ = map.Size.z;
			this.calcGrid = new PathFinder.PathFinderNodeFast[this.mapSizeX * this.mapSizeZ];
			this.openList = new FastPriorityQueue<PathFinder.CostNode>(new PathFinder.CostNodeComparer());
			this.regionCostCalculator = new RegionCostCalculatorWrapper(map);
		}

		// Token: 0x06003C01 RID: 15361 RVA: 0x001FAF20 File Offset: 0x001F9320
		public PawnPath FindPath(IntVec3 start, LocalTargetInfo dest, Pawn pawn, PathEndMode peMode = PathEndMode.OnCell)
		{
			bool flag = false;
			if (pawn != null && pawn.CurJob != null && pawn.CurJob.canBash)
			{
				flag = true;
			}
			Danger maxDanger = Danger.Deadly;
			bool canBash = flag;
			return this.FindPath(start, dest, TraverseParms.For(pawn, maxDanger, TraverseMode.ByPawn, canBash), peMode);
		}

		// Token: 0x06003C02 RID: 15362 RVA: 0x001FAF78 File Offset: 0x001F9378
		public PawnPath FindPath(IntVec3 start, LocalTargetInfo dest, TraverseParms traverseParms, PathEndMode peMode = PathEndMode.OnCell)
		{
			if (DebugSettings.pathThroughWalls)
			{
				traverseParms.mode = TraverseMode.PassAllDestroyableThings;
			}
			Pawn pawn = traverseParms.pawn;
			PawnPath notFound;
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
				}), false);
				notFound = PawnPath.NotFound;
			}
			else if (!start.IsValid)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to FindPath with invalid start ",
					start,
					", pawn= ",
					pawn
				}), false);
				notFound = PawnPath.NotFound;
			}
			else if (!dest.IsValid)
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to FindPath with invalid dest ",
					dest,
					", pawn= ",
					pawn
				}), false);
				notFound = PawnPath.NotFound;
			}
			else
			{
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
					(!dest.HasThing) ? "" : (" at " + dest.Cell)
				}));
				this.cellIndices = this.map.cellIndices;
				this.pathGrid = this.map.pathGrid;
				this.edificeGrid = this.map.edificeGrid.InnerArray;
				this.blueprintGrid = this.map.blueprintGrid.InnerArray;
				int x = dest.Cell.x;
				int z = dest.Cell.z;
				int num = this.cellIndices.CellToIndex(start);
				int num2 = this.cellIndices.CellToIndex(dest.Cell);
				ByteGrid byteGrid = (pawn == null) ? null : pawn.GetAvoidGrid();
				bool flag = traverseParms.mode == TraverseMode.PassAllDestroyableThings || traverseParms.mode == TraverseMode.PassAllDestroyableThingsNotWater;
				bool flag2 = traverseParms.mode != TraverseMode.NoPassClosedDoorsOrWater && traverseParms.mode != TraverseMode.PassAllDestroyableThingsNotWater;
				bool flag3 = !flag;
				CellRect cellRect = this.CalculateDestinationRect(dest, peMode);
				bool flag4 = cellRect.Width == 1 && cellRect.Height == 1;
				int[] array = this.map.pathGrid.pathGrid;
				TerrainDef[] topGrid = this.map.terrainGrid.topGrid;
				EdificeGrid edificeGrid = this.map.edificeGrid;
				int num3 = 0;
				int num4 = 0;
				Area allowedArea = this.GetAllowedArea(pawn);
				bool flag5 = pawn != null && PawnUtility.ShouldCollideWithPawns(pawn);
				bool flag6 = true && DebugViewSettings.drawPaths;
				bool flag7 = !flag && start.GetRegion(this.map, RegionType.Set_Passable) != null && flag2;
				bool flag8 = !flag || !flag3;
				bool flag9 = false;
				bool flag10 = pawn != null && pawn.Drafted;
				bool flag11 = pawn != null && pawn.IsColonist;
				int num5 = (!flag11) ? 2000 : 100000;
				int num6 = 0;
				int num7 = 0;
				float num8 = this.DetermineHeuristicStrength(pawn, start, dest);
				int num9;
				int num10;
				if (pawn != null)
				{
					num9 = pawn.TicksPerMoveCardinal;
					num10 = pawn.TicksPerMoveDiagonal;
				}
				else
				{
					num9 = 13;
					num10 = 18;
				}
				this.CalculateAndAddDisallowedCorners(traverseParms, peMode, cellRect);
				this.InitStatusesAndPushStartNode(ref num, start);
				for (;;)
				{
					this.PfProfilerBeginSample("Open cell");
					if (this.openList.Count <= 0)
					{
						break;
					}
					num6 += this.openList.Count;
					num7++;
					PathFinder.CostNode costNode = this.openList.Pop();
					num = costNode.index;
					if (costNode.cost != this.calcGrid[num].costNodeCost)
					{
						this.PfProfilerEndSample();
					}
					else if (this.calcGrid[num].status == this.statusClosedValue)
					{
						this.PfProfilerEndSample();
					}
					else
					{
						IntVec3 c = this.cellIndices.IndexToCell(num);
						int x2 = c.x;
						int z2 = c.z;
						if (flag6)
						{
							this.DebugFlash(c, (float)this.calcGrid[num].knownCost / 1500f, this.calcGrid[num].knownCost.ToString());
						}
						if (flag4)
						{
							if (num == num2)
							{
								goto Block_32;
							}
						}
						else if (cellRect.Contains(c) && !this.disallowedCornerIndices.Contains(num))
						{
							goto Block_34;
						}
						if (num3 > 160000)
						{
							goto Block_35;
						}
						this.PfProfilerEndSample();
						this.PfProfilerBeginSample("Neighbor consideration");
						for (int i = 0; i < 8; i++)
						{
							uint num11 = (uint)(x2 + PathFinder.Directions[i]);
							uint num12 = (uint)(z2 + PathFinder.Directions[i + 8]);
							if ((ulong)num11 < (ulong)((long)this.mapSizeX) && (ulong)num12 < (ulong)((long)this.mapSizeZ))
							{
								int num13 = (int)num11;
								int num14 = (int)num12;
								int num15 = this.cellIndices.CellToIndex(num13, num14);
								if (this.calcGrid[num15].status != this.statusClosedValue || flag9)
								{
									int num16 = 0;
									bool flag12 = false;
									if (flag2 || !new IntVec3(num13, 0, num14).GetTerrain(this.map).HasTag("Water"))
									{
										if (!this.pathGrid.WalkableFast(num15))
										{
											if (!flag)
											{
												if (flag6)
												{
													this.DebugFlash(new IntVec3(num13, 0, num14), 0.22f, "walk");
												}
												goto IL_DCD;
											}
											flag12 = true;
											num16 += 70;
											Building building = edificeGrid[num15];
											if (building == null)
											{
												goto IL_DCD;
											}
											if (!PathFinder.IsDestroyable(building))
											{
												goto IL_DCD;
											}
											num16 += (int)((float)building.HitPoints * 0.11f);
										}
										if (i > 3)
										{
											switch (i)
											{
											case 4:
												if (this.BlocksDiagonalMovement(num - this.mapSizeX))
												{
													if (flag8)
													{
														if (flag6)
														{
															this.DebugFlash(new IntVec3(x2, 0, z2 - 1), 0.9f, "corn");
														}
														goto IL_DCD;
													}
													num16 += 70;
												}
												if (this.BlocksDiagonalMovement(num + 1))
												{
													if (flag8)
													{
														if (flag6)
														{
															this.DebugFlash(new IntVec3(x2 + 1, 0, z2), 0.9f, "corn");
														}
														goto IL_DCD;
													}
													num16 += 70;
												}
												break;
											case 5:
												if (this.BlocksDiagonalMovement(num + this.mapSizeX))
												{
													if (flag8)
													{
														if (flag6)
														{
															this.DebugFlash(new IntVec3(x2, 0, z2 + 1), 0.9f, "corn");
														}
														goto IL_DCD;
													}
													num16 += 70;
												}
												if (this.BlocksDiagonalMovement(num + 1))
												{
													if (flag8)
													{
														if (flag6)
														{
															this.DebugFlash(new IntVec3(x2 + 1, 0, z2), 0.9f, "corn");
														}
														goto IL_DCD;
													}
													num16 += 70;
												}
												break;
											case 6:
												if (this.BlocksDiagonalMovement(num + this.mapSizeX))
												{
													if (flag8)
													{
														if (flag6)
														{
															this.DebugFlash(new IntVec3(x2, 0, z2 + 1), 0.9f, "corn");
														}
														goto IL_DCD;
													}
													num16 += 70;
												}
												if (this.BlocksDiagonalMovement(num - 1))
												{
													if (flag8)
													{
														if (flag6)
														{
															this.DebugFlash(new IntVec3(x2 - 1, 0, z2), 0.9f, "corn");
														}
														goto IL_DCD;
													}
													num16 += 70;
												}
												break;
											case 7:
												if (this.BlocksDiagonalMovement(num - this.mapSizeX))
												{
													if (flag8)
													{
														if (flag6)
														{
															this.DebugFlash(new IntVec3(x2, 0, z2 - 1), 0.9f, "corn");
														}
														goto IL_DCD;
													}
													num16 += 70;
												}
												if (this.BlocksDiagonalMovement(num - 1))
												{
													if (flag8)
													{
														if (flag6)
														{
															this.DebugFlash(new IntVec3(x2 - 1, 0, z2), 0.9f, "corn");
														}
														goto IL_DCD;
													}
													num16 += 70;
												}
												break;
											}
										}
										int num17 = (i <= 3) ? num9 : num10;
										num17 += num16;
										if (!flag12)
										{
											num17 += array[num15];
											if (flag10)
											{
												num17 += topGrid[num15].extraDraftedPerceivedPathCost;
											}
											else
											{
												num17 += topGrid[num15].extraNonDraftedPerceivedPathCost;
											}
										}
										if (byteGrid != null)
										{
											num17 += (int)(byteGrid[num15] * 8);
										}
										if (allowedArea != null && !allowedArea[num15])
										{
											num17 += 600;
										}
										if (flag5 && PawnUtility.AnyPawnBlockingPathAt(new IntVec3(num13, 0, num14), pawn, false, false, true))
										{
											num17 += 175;
										}
										Building building2 = this.edificeGrid[num15];
										if (building2 != null)
										{
											this.PfProfilerBeginSample("Edifices");
											int buildingCost = PathFinder.GetBuildingCost(building2, traverseParms, pawn);
											if (buildingCost == 2147483647)
											{
												this.PfProfilerEndSample();
												goto IL_DCD;
											}
											num17 += buildingCost;
											this.PfProfilerEndSample();
										}
										List<Blueprint> list = this.blueprintGrid[num15];
										if (list != null)
										{
											this.PfProfilerBeginSample("Blueprints");
											int num18 = 0;
											for (int j = 0; j < list.Count; j++)
											{
												num18 = Mathf.Max(num18, PathFinder.GetBlueprintCost(list[j], pawn));
											}
											if (num18 == 2147483647)
											{
												this.PfProfilerEndSample();
												goto IL_DCD;
											}
											num17 += num18;
											this.PfProfilerEndSample();
										}
										int num19 = num17 + this.calcGrid[num].knownCost;
										ushort status = this.calcGrid[num15].status;
										if (status == this.statusClosedValue || status == this.statusOpenValue)
										{
											int num20 = 0;
											if (status == this.statusClosedValue)
											{
												num20 = num9;
											}
											if (this.calcGrid[num15].knownCost <= num19 + num20)
											{
												goto IL_DCD;
											}
										}
										if (status != this.statusClosedValue && status != this.statusOpenValue)
										{
											if (flag9)
											{
												this.calcGrid[num15].heuristicCost = Mathf.RoundToInt((float)this.regionCostCalculator.GetPathCostFromDestToRegion(num15) * PathFinder.RegionHeuristicWeightByNodesOpened.Evaluate((float)num4));
											}
											else
											{
												int dx = Math.Abs(num13 - x);
												int dz = Math.Abs(num14 - z);
												int num21 = GenMath.OctileDistance(dx, dz, num9, num10);
												this.calcGrid[num15].heuristicCost = Mathf.RoundToInt((float)num21 * num8);
											}
										}
										int num22 = num19 + this.calcGrid[num15].heuristicCost;
										this.calcGrid[num15].parentIndex = num;
										this.calcGrid[num15].knownCost = num19;
										this.calcGrid[num15].status = this.statusOpenValue;
										this.calcGrid[num15].costNodeCost = num22;
										num4++;
										this.openList.Push(new PathFinder.CostNode(num15, num22));
									}
								}
							}
							IL_DCD:;
						}
						this.PfProfilerEndSample();
						num3++;
						this.calcGrid[num].status = this.statusClosedValue;
						if (num4 >= num5 && flag7 && !flag9)
						{
							flag9 = true;
							this.regionCostCalculator.Init(cellRect, traverseParms, num9, num10, byteGrid, allowedArea, flag10, this.disallowedCornerIndices);
							this.InitStatusesAndPushStartNode(ref num, start);
							num4 = 0;
							num3 = 0;
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
				}), false);
				this.DebugDrawRichData();
				this.PfProfilerEndSample();
				return PawnPath.NotFound;
				Block_32:
				this.PfProfilerEndSample();
				PawnPath result = this.FinalizedPath(num);
				this.PfProfilerEndSample();
				return result;
				Block_34:
				this.PfProfilerEndSample();
				PawnPath result2 = this.FinalizedPath(num);
				this.PfProfilerEndSample();
				return result2;
				Block_35:
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
				}), false);
				this.DebugDrawRichData();
				this.PfProfilerEndSample();
				notFound = PawnPath.NotFound;
			}
			return notFound;
		}

		// Token: 0x06003C03 RID: 15363 RVA: 0x001FBDD4 File Offset: 0x001FA1D4
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
						return int.MaxValue;
					}
					if (building_Door.FreePassage)
					{
						return 0;
					}
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
					return int.MaxValue;
				case TraverseMode.PassDoors:
					if (building_Door.FreePassage)
					{
						return 0;
					}
					if (pawn != null && building_Door.PawnCanOpen(pawn) && !building_Door.IsForbiddenToPass(pawn))
					{
						return building_Door.TicksToOpenNow;
					}
					return 150;
				case TraverseMode.NoPassClosedDoors:
				case TraverseMode.NoPassClosedDoorsOrWater:
					if (building_Door.FreePassage)
					{
						return 0;
					}
					return int.MaxValue;
				case TraverseMode.PassAllDestroyableThings:
				case TraverseMode.PassAllDestroyableThingsNotWater:
					if (building_Door.FreePassage)
					{
						return 0;
					}
					if (pawn != null && building_Door.PawnCanOpen(pawn) && !building_Door.IsForbiddenToPass(pawn))
					{
						return building_Door.TicksToOpenNow;
					}
					return 50 + (int)((float)building_Door.HitPoints * 0.11f);
				}
			}
			else if (pawn != null)
			{
				return (int)b.PathFindCostFor(pawn);
			}
			return 0;
		}

		// Token: 0x06003C04 RID: 15364 RVA: 0x001FBFB4 File Offset: 0x001FA3B4
		public static int GetBlueprintCost(Blueprint b, Pawn pawn)
		{
			int result;
			if (pawn != null)
			{
				result = (int)b.PathFindCostFor(pawn);
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06003C05 RID: 15365 RVA: 0x001FBFE0 File Offset: 0x001FA3E0
		public static bool IsDestroyable(Thing th)
		{
			return th.def.useHitPoints && th.def.destroyable;
		}

		// Token: 0x06003C06 RID: 15366 RVA: 0x001FC014 File Offset: 0x001FA414
		private bool BlocksDiagonalMovement(int x, int z)
		{
			return PathFinder.BlocksDiagonalMovement(x, z, this.map);
		}

		// Token: 0x06003C07 RID: 15367 RVA: 0x001FC038 File Offset: 0x001FA438
		private bool BlocksDiagonalMovement(int index)
		{
			return PathFinder.BlocksDiagonalMovement(index, this.map);
		}

		// Token: 0x06003C08 RID: 15368 RVA: 0x001FC05C File Offset: 0x001FA45C
		public static bool BlocksDiagonalMovement(int x, int z, Map map)
		{
			return PathFinder.BlocksDiagonalMovement(map.cellIndices.CellToIndex(x, z), map);
		}

		// Token: 0x06003C09 RID: 15369 RVA: 0x001FC084 File Offset: 0x001FA484
		public static bool BlocksDiagonalMovement(int index, Map map)
		{
			return !map.pathGrid.WalkableFast(index) || map.edificeGrid[index] is Building_Door;
		}

		// Token: 0x06003C0A RID: 15370 RVA: 0x001FC0CF File Offset: 0x001FA4CF
		private void DebugFlash(IntVec3 c, float colorPct, string str)
		{
			PathFinder.DebugFlash(c, this.map, colorPct, str);
		}

		// Token: 0x06003C0B RID: 15371 RVA: 0x001FC0E0 File Offset: 0x001FA4E0
		private static void DebugFlash(IntVec3 c, Map map, float colorPct, string str)
		{
			map.debugDrawer.FlashCell(c, colorPct, str, 50);
		}

		// Token: 0x06003C0C RID: 15372 RVA: 0x001FC0F4 File Offset: 0x001FA4F4
		private PawnPath FinalizedPath(int finalIndex)
		{
			PawnPath emptyPawnPath = this.map.pawnPathPool.GetEmptyPawnPath();
			int num = finalIndex;
			for (;;)
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
			return emptyPawnPath;
		}

		// Token: 0x06003C0D RID: 15373 RVA: 0x001FC180 File Offset: 0x001FA580
		private void InitStatusesAndPushStartNode(ref int curIndex, IntVec3 start)
		{
			this.statusOpenValue += 2;
			this.statusClosedValue += 2;
			if (this.statusClosedValue >= 65435)
			{
				this.ResetStatuses();
			}
			curIndex = this.cellIndices.CellToIndex(start);
			this.calcGrid[curIndex].knownCost = 0;
			this.calcGrid[curIndex].heuristicCost = 0;
			this.calcGrid[curIndex].costNodeCost = 0;
			this.calcGrid[curIndex].parentIndex = curIndex;
			this.calcGrid[curIndex].status = this.statusOpenValue;
			this.openList.Clear();
			this.openList.Push(new PathFinder.CostNode(curIndex, 0));
		}

		// Token: 0x06003C0E RID: 15374 RVA: 0x001FC254 File Offset: 0x001FA654
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

		// Token: 0x06003C0F RID: 15375 RVA: 0x001FC29F File Offset: 0x001FA69F
		[Conditional("PFPROFILE")]
		private void PfProfilerBeginSample(string s)
		{
		}

		// Token: 0x06003C10 RID: 15376 RVA: 0x001FC2A2 File Offset: 0x001FA6A2
		[Conditional("PFPROFILE")]
		private void PfProfilerEndSample()
		{
		}

		// Token: 0x06003C11 RID: 15377 RVA: 0x001FC2A8 File Offset: 0x001FA6A8
		private void DebugDrawRichData()
		{
			if (DebugViewSettings.drawPaths)
			{
				while (this.openList.Count > 0)
				{
					int index = this.openList.Pop().index;
					IntVec3 c = new IntVec3(index % this.mapSizeX, 0, index / this.mapSizeX);
					this.map.debugDrawer.FlashCell(c, 0f, "open", 50);
				}
			}
		}

		// Token: 0x06003C12 RID: 15378 RVA: 0x001FC324 File Offset: 0x001FA724
		private float DetermineHeuristicStrength(Pawn pawn, IntVec3 start, LocalTargetInfo dest)
		{
			float result;
			if (pawn != null && pawn.RaceProps.Animal)
			{
				result = 1.75f;
			}
			else
			{
				float lengthHorizontal = (start - dest.Cell).LengthHorizontal;
				result = (float)Mathf.RoundToInt(PathFinder.NonRegionBasedHeuristicStrengthHuman_DistanceCurve.Evaluate(lengthHorizontal));
			}
			return result;
		}

		// Token: 0x06003C13 RID: 15379 RVA: 0x001FC384 File Offset: 0x001FA784
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

		// Token: 0x06003C14 RID: 15380 RVA: 0x001FC3DC File Offset: 0x001FA7DC
		private Area GetAllowedArea(Pawn pawn)
		{
			Area result;
			if (pawn != null && pawn.playerSettings != null && !pawn.Drafted && ForbidUtility.CaresAboutForbidden(pawn, true))
			{
				Area area = pawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap;
				if (area != null && area.TrueCount <= 0)
				{
					area = null;
				}
				result = area;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06003C15 RID: 15381 RVA: 0x001FC444 File Offset: 0x001FA844
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

		// Token: 0x06003C16 RID: 15382 RVA: 0x001FC560 File Offset: 0x001FA960
		private bool IsCornerTouchAllowed(int cornerX, int cornerZ, int adjCardinal1X, int adjCardinal1Z, int adjCardinal2X, int adjCardinal2Z)
		{
			return TouchPathEndModeUtility.IsCornerTouchAllowed(cornerX, cornerZ, adjCardinal1X, adjCardinal1Z, adjCardinal2X, adjCardinal2Z, this.map);
		}

		// Token: 0x02000A90 RID: 2704
		internal struct CostNode
		{
			// Token: 0x040025EA RID: 9706
			public int index;

			// Token: 0x040025EB RID: 9707
			public int cost;

			// Token: 0x06003C18 RID: 15384 RVA: 0x001FC636 File Offset: 0x001FAA36
			public CostNode(int index, int cost)
			{
				this.index = index;
				this.cost = cost;
			}
		}

		// Token: 0x02000A91 RID: 2705
		private struct PathFinderNodeFast
		{
			// Token: 0x040025EC RID: 9708
			public int knownCost;

			// Token: 0x040025ED RID: 9709
			public int heuristicCost;

			// Token: 0x040025EE RID: 9710
			public int parentIndex;

			// Token: 0x040025EF RID: 9711
			public int costNodeCost;

			// Token: 0x040025F0 RID: 9712
			public ushort status;
		}

		// Token: 0x02000A92 RID: 2706
		internal class CostNodeComparer : IComparer<PathFinder.CostNode>
		{
			// Token: 0x06003C1A RID: 15386 RVA: 0x001FC650 File Offset: 0x001FAA50
			public int Compare(PathFinder.CostNode a, PathFinder.CostNode b)
			{
				return a.cost.CompareTo(b.cost);
			}
		}
	}
}
