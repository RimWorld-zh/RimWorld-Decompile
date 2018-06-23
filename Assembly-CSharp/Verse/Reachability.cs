using System;
using System.Collections.Generic;
using RimWorld;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000C80 RID: 3200
	public class Reachability
	{
		// Token: 0x04002FC6 RID: 12230
		private Map map;

		// Token: 0x04002FC7 RID: 12231
		private Queue<Region> openQueue = new Queue<Region>();

		// Token: 0x04002FC8 RID: 12232
		private List<Region> startingRegions = new List<Region>();

		// Token: 0x04002FC9 RID: 12233
		private List<Region> destRegions = new List<Region>();

		// Token: 0x04002FCA RID: 12234
		private uint reachedIndex = 1u;

		// Token: 0x04002FCB RID: 12235
		private int numRegionsOpened;

		// Token: 0x04002FCC RID: 12236
		private bool working = false;

		// Token: 0x04002FCD RID: 12237
		private ReachabilityCache cache = new ReachabilityCache();

		// Token: 0x04002FCE RID: 12238
		private PathGrid pathGrid;

		// Token: 0x04002FCF RID: 12239
		private RegionGrid regionGrid;

		// Token: 0x0600461A RID: 17946 RVA: 0x0024EC64 File Offset: 0x0024D064
		public Reachability(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600461B RID: 17947 RVA: 0x0024ECB9 File Offset: 0x0024D0B9
		public void ClearCache()
		{
			if (this.cache.Count > 0)
			{
				this.cache.Clear();
			}
		}

		// Token: 0x0600461C RID: 17948 RVA: 0x0024ECDC File Offset: 0x0024D0DC
		private void QueueNewOpenRegion(Region region)
		{
			if (region == null)
			{
				Log.ErrorOnce("Tried to queue null region.", 881121, false);
			}
			else if (region.reachedIndex == this.reachedIndex)
			{
				Log.ErrorOnce("Region is already reached; you can't open it. Region: " + region.ToString(), 719991, false);
			}
			else
			{
				this.openQueue.Enqueue(region);
				region.reachedIndex = this.reachedIndex;
				this.numRegionsOpened++;
			}
		}

		// Token: 0x0600461D RID: 17949 RVA: 0x0024ED60 File Offset: 0x0024D160
		private uint NewReachedIndex()
		{
			return this.reachedIndex++;
		}

		// Token: 0x0600461E RID: 17950 RVA: 0x0024ED86 File Offset: 0x0024D186
		private void FinalizeCheck()
		{
			this.working = false;
		}

		// Token: 0x0600461F RID: 17951 RVA: 0x0024ED90 File Offset: 0x0024D190
		public bool CanReachNonLocal(IntVec3 start, TargetInfo dest, PathEndMode peMode, TraverseMode traverseMode, Danger maxDanger)
		{
			return (dest.Map == null || dest.Map == this.map) && this.CanReach(start, (LocalTargetInfo)dest, peMode, traverseMode, maxDanger);
		}

		// Token: 0x06004620 RID: 17952 RVA: 0x0024EDDC File Offset: 0x0024D1DC
		public bool CanReachNonLocal(IntVec3 start, TargetInfo dest, PathEndMode peMode, TraverseParms traverseParams)
		{
			return (dest.Map == null || dest.Map == this.map) && this.CanReach(start, (LocalTargetInfo)dest, peMode, traverseParams);
		}

		// Token: 0x06004621 RID: 17953 RVA: 0x0024EE28 File Offset: 0x0024D228
		public bool CanReach(IntVec3 start, LocalTargetInfo dest, PathEndMode peMode, TraverseMode traverseMode, Danger maxDanger)
		{
			return this.CanReach(start, dest, peMode, TraverseParms.For(traverseMode, maxDanger, false));
		}

		// Token: 0x06004622 RID: 17954 RVA: 0x0024EE50 File Offset: 0x0024D250
		public bool CanReach(IntVec3 start, LocalTargetInfo dest, PathEndMode peMode, TraverseParms traverseParams)
		{
			bool result;
			if (this.working)
			{
				Log.ErrorOnce("Called CanReach() while working. This should never happen. Suppressing further errors.", 7312233, false);
				result = false;
			}
			else
			{
				if (traverseParams.pawn != null)
				{
					if (!traverseParams.pawn.Spawned)
					{
						return false;
					}
					if (traverseParams.pawn.Map != this.map)
					{
						Log.Error(string.Concat(new object[]
						{
							"Called CanReach() with a pawn spawned not on this map. This means that we can't check his reachability here. Pawn's current map should have been used instead of this one. pawn=",
							traverseParams.pawn,
							" pawn.Map=",
							traverseParams.pawn.Map,
							" map=",
							this.map
						}), false);
						return false;
					}
				}
				if (ReachabilityImmediate.CanReachImmediate(start, dest, this.map, peMode, traverseParams.pawn))
				{
					result = true;
				}
				else if (!dest.IsValid)
				{
					result = false;
				}
				else if (dest.HasThing && dest.Thing.Map != this.map)
				{
					result = false;
				}
				else if (!start.InBounds(this.map) || !dest.Cell.InBounds(this.map))
				{
					result = false;
				}
				else
				{
					if (peMode == PathEndMode.OnCell || peMode == PathEndMode.Touch || peMode == PathEndMode.ClosestTouch)
					{
						if (traverseParams.mode != TraverseMode.NoPassClosedDoorsOrWater && traverseParams.mode != TraverseMode.PassAllDestroyableThingsNotWater)
						{
							Room room = RegionAndRoomQuery.RoomAtFast(start, this.map, RegionType.Set_Passable);
							if (room != null && room == RegionAndRoomQuery.RoomAtFast(dest.Cell, this.map, RegionType.Set_Passable))
							{
								return true;
							}
						}
					}
					if (traverseParams.mode == TraverseMode.PassAllDestroyableThings)
					{
						TraverseParms traverseParams2 = traverseParams;
						traverseParams2.mode = TraverseMode.PassDoors;
						if (this.CanReach(start, dest, peMode, traverseParams2))
						{
							return true;
						}
					}
					dest = (LocalTargetInfo)GenPath.ResolvePathMode(traverseParams.pawn, dest.ToTargetInfo(this.map), ref peMode);
					this.working = true;
					try
					{
						this.pathGrid = this.map.pathGrid;
						this.regionGrid = this.map.regionGrid;
						this.reachedIndex += 1u;
						this.destRegions.Clear();
						if (peMode == PathEndMode.OnCell)
						{
							Region region = dest.Cell.GetRegion(this.map, RegionType.Set_Passable);
							if (region != null && region.Allows(traverseParams, true))
							{
								this.destRegions.Add(region);
							}
						}
						else if (peMode == PathEndMode.Touch)
						{
							TouchPathEndModeUtility.AddAllowedAdjacentRegions(dest, traverseParams, this.map, this.destRegions);
						}
						if (this.destRegions.Count == 0 && traverseParams.mode != TraverseMode.PassAllDestroyableThings && traverseParams.mode != TraverseMode.PassAllDestroyableThingsNotWater)
						{
							this.FinalizeCheck();
							result = false;
						}
						else
						{
							this.destRegions.RemoveDuplicates<Region>();
							this.openQueue.Clear();
							this.numRegionsOpened = 0;
							this.DetermineStartRegions(start);
							if (this.openQueue.Count == 0 && traverseParams.mode != TraverseMode.PassAllDestroyableThings && traverseParams.mode != TraverseMode.PassAllDestroyableThingsNotWater)
							{
								this.FinalizeCheck();
								result = false;
							}
							else
							{
								if (this.startingRegions.Any<Region>() && this.destRegions.Any<Region>() && this.CanUseCache(traverseParams.mode))
								{
									BoolUnknown cachedResult = this.GetCachedResult(traverseParams);
									if (cachedResult == BoolUnknown.True)
									{
										this.FinalizeCheck();
										return true;
									}
									if (cachedResult == BoolUnknown.False)
									{
										this.FinalizeCheck();
										return false;
									}
									if (cachedResult != BoolUnknown.Unknown)
									{
									}
								}
								if (traverseParams.mode == TraverseMode.PassAllDestroyableThings || traverseParams.mode == TraverseMode.PassAllDestroyableThingsNotWater || traverseParams.mode == TraverseMode.NoPassClosedDoorsOrWater)
								{
									bool flag = this.CheckCellBasedReachability(start, dest, peMode, traverseParams);
									this.FinalizeCheck();
									result = flag;
								}
								else
								{
									bool flag2 = this.CheckRegionBasedReachability(traverseParams);
									this.FinalizeCheck();
									result = flag2;
								}
							}
						}
					}
					finally
					{
						this.working = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06004623 RID: 17955 RVA: 0x0024F288 File Offset: 0x0024D688
		private void DetermineStartRegions(IntVec3 start)
		{
			this.startingRegions.Clear();
			if (this.pathGrid.WalkableFast(start))
			{
				Region validRegionAt = this.regionGrid.GetValidRegionAt(start);
				this.QueueNewOpenRegion(validRegionAt);
				this.startingRegions.Add(validRegionAt);
			}
			else
			{
				for (int i = 0; i < 8; i++)
				{
					IntVec3 intVec = start + GenAdj.AdjacentCells[i];
					if (intVec.InBounds(this.map))
					{
						if (this.pathGrid.WalkableFast(intVec))
						{
							Region validRegionAt2 = this.regionGrid.GetValidRegionAt(intVec);
							if (validRegionAt2 != null && validRegionAt2.reachedIndex != this.reachedIndex)
							{
								this.QueueNewOpenRegion(validRegionAt2);
								this.startingRegions.Add(validRegionAt2);
							}
						}
					}
				}
			}
		}

		// Token: 0x06004624 RID: 17956 RVA: 0x0024F368 File Offset: 0x0024D768
		private BoolUnknown GetCachedResult(TraverseParms traverseParams)
		{
			bool flag = false;
			for (int i = 0; i < this.startingRegions.Count; i++)
			{
				for (int j = 0; j < this.destRegions.Count; j++)
				{
					if (this.destRegions[j] == this.startingRegions[i])
					{
						return BoolUnknown.True;
					}
					BoolUnknown boolUnknown = this.cache.CachedResultFor(this.startingRegions[i].Room, this.destRegions[j].Room, traverseParams);
					if (boolUnknown == BoolUnknown.True)
					{
						return BoolUnknown.True;
					}
					if (boolUnknown == BoolUnknown.Unknown)
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return BoolUnknown.False;
			}
			return BoolUnknown.Unknown;
		}

		// Token: 0x06004625 RID: 17957 RVA: 0x0024F438 File Offset: 0x0024D838
		private bool CheckRegionBasedReachability(TraverseParms traverseParams)
		{
			while (this.openQueue.Count > 0)
			{
				Region region = this.openQueue.Dequeue();
				for (int i = 0; i < region.links.Count; i++)
				{
					RegionLink regionLink = region.links[i];
					int j = 0;
					while (j < 2)
					{
						Region region2 = regionLink.regions[j];
						if (region2 != null && region2.reachedIndex != this.reachedIndex && region2.type.Passable())
						{
							if (region2.Allows(traverseParams, false))
							{
								if (this.destRegions.Contains(region2))
								{
									for (int k = 0; k < this.startingRegions.Count; k++)
									{
										this.cache.AddCachedResult(this.startingRegions[k].Room, region2.Room, traverseParams, true);
									}
									return true;
								}
								this.QueueNewOpenRegion(region2);
							}
						}
						IL_E5:
						j++;
						continue;
						goto IL_E5;
					}
				}
			}
			for (int l = 0; l < this.startingRegions.Count; l++)
			{
				for (int m = 0; m < this.destRegions.Count; m++)
				{
					this.cache.AddCachedResult(this.startingRegions[l].Room, this.destRegions[m].Room, traverseParams, false);
				}
			}
			return false;
		}

		// Token: 0x06004626 RID: 17958 RVA: 0x0024F5DC File Offset: 0x0024D9DC
		private bool CheckCellBasedReachability(IntVec3 start, LocalTargetInfo dest, PathEndMode peMode, TraverseParms traverseParams)
		{
			IntVec3 foundCell = IntVec3.Invalid;
			Region[] directRegionGrid = this.regionGrid.DirectGrid;
			PathGrid pathGrid = this.map.pathGrid;
			CellIndices cellIndices = this.map.cellIndices;
			this.map.floodFiller.FloodFill(start, delegate(IntVec3 c)
			{
				int num = cellIndices.CellToIndex(c);
				if (traverseParams.mode == TraverseMode.PassAllDestroyableThingsNotWater || traverseParams.mode == TraverseMode.NoPassClosedDoorsOrWater)
				{
					if (c.GetTerrain(this.map).IsWater)
					{
						return false;
					}
				}
				if (traverseParams.mode == TraverseMode.PassAllDestroyableThings || traverseParams.mode == TraverseMode.PassAllDestroyableThingsNotWater)
				{
					if (!pathGrid.WalkableFast(num))
					{
						Building edifice = c.GetEdifice(this.map);
						if (edifice == null || !PathFinder.IsDestroyable(edifice))
						{
							return false;
						}
					}
				}
				else if (traverseParams.mode != TraverseMode.NoPassClosedDoorsOrWater)
				{
					Log.ErrorOnce("Do not use this method for non-cell based modes!", 938476762, false);
					if (!pathGrid.WalkableFast(num))
					{
						return false;
					}
				}
				Region region = directRegionGrid[num];
				return region == null || region.Allows(traverseParams, false);
			}, delegate(IntVec3 c)
			{
				bool result2;
				if (ReachabilityImmediate.CanReachImmediate(c, dest, this.map, peMode, traverseParams.pawn))
				{
					foundCell = c;
					result2 = true;
				}
				else
				{
					result2 = false;
				}
				return result2;
			}, int.MaxValue, false, null);
			bool result;
			if (foundCell.IsValid)
			{
				if (this.CanUseCache(traverseParams.mode))
				{
					Region validRegionAt = this.regionGrid.GetValidRegionAt(foundCell);
					if (validRegionAt != null)
					{
						for (int i = 0; i < this.startingRegions.Count; i++)
						{
							this.cache.AddCachedResult(this.startingRegions[i].Room, validRegionAt.Room, traverseParams, true);
						}
					}
				}
				result = true;
			}
			else
			{
				if (this.CanUseCache(traverseParams.mode))
				{
					for (int j = 0; j < this.startingRegions.Count; j++)
					{
						for (int k = 0; k < this.destRegions.Count; k++)
						{
							this.cache.AddCachedResult(this.startingRegions[j].Room, this.destRegions[k].Room, traverseParams, false);
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06004627 RID: 17959 RVA: 0x0024F7A8 File Offset: 0x0024DBA8
		public bool CanReachColony(IntVec3 c)
		{
			return this.CanReachFactionBase(c, Faction.OfPlayer);
		}

		// Token: 0x06004628 RID: 17960 RVA: 0x0024F7CC File Offset: 0x0024DBCC
		public bool CanReachFactionBase(IntVec3 c, Faction factionBaseFaction)
		{
			bool result;
			if (Current.ProgramState != ProgramState.Playing)
			{
				result = this.CanReach(c, MapGenerator.PlayerStartSpot, PathEndMode.OnCell, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false));
			}
			else if (!c.Walkable(this.map))
			{
				result = false;
			}
			else
			{
				Faction faction = this.map.ParentFaction ?? Faction.OfPlayer;
				List<Pawn> list = this.map.mapPawns.SpawnedPawnsInFaction(faction);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].CanReach(c, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
					{
						return true;
					}
				}
				TraverseParms traverseParams = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false);
				if (faction == Faction.OfPlayer)
				{
					List<Building> allBuildingsColonist = this.map.listerBuildings.allBuildingsColonist;
					for (int j = 0; j < allBuildingsColonist.Count; j++)
					{
						if (this.CanReach(c, allBuildingsColonist[j], PathEndMode.Touch, traverseParams))
						{
							return true;
						}
					}
				}
				else
				{
					List<Thing> list2 = this.map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
					for (int k = 0; k < list2.Count; k++)
					{
						if (list2[k].Faction == faction && this.CanReach(c, list2[k], PathEndMode.Touch, traverseParams))
						{
							return true;
						}
					}
				}
				result = this.CanReachBiggestMapEdgeRoom(c);
			}
			return result;
		}

		// Token: 0x06004629 RID: 17961 RVA: 0x0024F970 File Offset: 0x0024DD70
		public bool CanReachBiggestMapEdgeRoom(IntVec3 c)
		{
			Room room = null;
			for (int i = 0; i < this.map.regionGrid.allRooms.Count; i++)
			{
				Room room2 = this.map.regionGrid.allRooms[i];
				if (room2.TouchesMapEdge)
				{
					if (room == null || room2.RegionCount > room.RegionCount)
					{
						room = room2;
					}
				}
			}
			return room != null && this.CanReach(c, room.Regions[0].AnyCell, PathEndMode.OnCell, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false));
		}

		// Token: 0x0600462A RID: 17962 RVA: 0x0024FA24 File Offset: 0x0024DE24
		public bool CanReachMapEdge(IntVec3 c, TraverseParms traverseParms)
		{
			if (traverseParms.pawn != null)
			{
				if (!traverseParms.pawn.Spawned)
				{
					return false;
				}
				if (traverseParms.pawn.Map != this.map)
				{
					Log.Error(string.Concat(new object[]
					{
						"Called CanReachMapEdge() with a pawn spawned not on this map. This means that we can't check his reachability here. Pawn's current map should have been used instead of this one. pawn=",
						traverseParms.pawn,
						" pawn.Map=",
						traverseParms.pawn.Map,
						" map=",
						this.map
					}), false);
					return false;
				}
			}
			Region region = c.GetRegion(this.map, RegionType.Set_Passable);
			bool result;
			if (region == null)
			{
				result = false;
			}
			else if (region.Room.TouchesMapEdge)
			{
				result = true;
			}
			else
			{
				RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(traverseParms, false);
				bool foundReg = false;
				RegionProcessor regionProcessor = delegate(Region r)
				{
					bool result2;
					if (r.Room.TouchesMapEdge)
					{
						foundReg = true;
						result2 = true;
					}
					else
					{
						result2 = false;
					}
					return result2;
				};
				RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, 9999, RegionType.Set_Passable);
				result = foundReg;
			}
			return result;
		}

		// Token: 0x0600462B RID: 17963 RVA: 0x0024FB54 File Offset: 0x0024DF54
		public bool CanReachUnfogged(IntVec3 c, TraverseParms traverseParms)
		{
			if (traverseParms.pawn != null)
			{
				if (!traverseParms.pawn.Spawned)
				{
					return false;
				}
				if (traverseParms.pawn.Map != this.map)
				{
					Log.Error(string.Concat(new object[]
					{
						"Called CanReachUnfogged() with a pawn spawned not on this map. This means that we can't check his reachability here. Pawn's current map should have been used instead of this one. pawn=",
						traverseParms.pawn,
						" pawn.Map=",
						traverseParms.pawn.Map,
						" map=",
						this.map
					}), false);
					return false;
				}
			}
			bool result;
			if (!c.InBounds(this.map))
			{
				result = false;
			}
			else if (!c.Fogged(this.map))
			{
				result = true;
			}
			else
			{
				Region region = c.GetRegion(this.map, RegionType.Set_Passable);
				if (region == null)
				{
					result = false;
				}
				else
				{
					RegionEntryPredicate entryCondition = (Region from, Region r) => r.Allows(traverseParms, false);
					bool foundReg = false;
					RegionProcessor regionProcessor = delegate(Region r)
					{
						bool result2;
						if (!r.AnyCell.Fogged(this.map))
						{
							foundReg = true;
							result2 = true;
						}
						else
						{
							result2 = false;
						}
						return result2;
					};
					RegionTraverser.BreadthFirstTraverse(region, entryCondition, regionProcessor, 9999, RegionType.Set_Passable);
					result = foundReg;
				}
			}
			return result;
		}

		// Token: 0x0600462C RID: 17964 RVA: 0x0024FCA4 File Offset: 0x0024E0A4
		private bool CanUseCache(TraverseMode mode)
		{
			return mode != TraverseMode.PassAllDestroyableThingsNotWater && mode != TraverseMode.NoPassClosedDoorsOrWater;
		}
	}
}
