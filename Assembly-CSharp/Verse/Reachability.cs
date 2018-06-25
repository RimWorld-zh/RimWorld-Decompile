using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse.AI;

namespace Verse
{
	public class Reachability
	{
		private Map map;

		private Queue<Region> openQueue = new Queue<Region>();

		private List<Region> startingRegions = new List<Region>();

		private List<Region> destRegions = new List<Region>();

		private uint reachedIndex = 1u;

		private int numRegionsOpened;

		private bool working = false;

		private ReachabilityCache cache = new ReachabilityCache();

		private PathGrid pathGrid;

		private RegionGrid regionGrid;

		public Reachability(Map map)
		{
			this.map = map;
		}

		public void ClearCache()
		{
			if (this.cache.Count > 0)
			{
				this.cache.Clear();
			}
		}

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

		private uint NewReachedIndex()
		{
			return this.reachedIndex++;
		}

		private void FinalizeCheck()
		{
			this.working = false;
		}

		public bool CanReachNonLocal(IntVec3 start, TargetInfo dest, PathEndMode peMode, TraverseMode traverseMode, Danger maxDanger)
		{
			return (dest.Map == null || dest.Map == this.map) && this.CanReach(start, (LocalTargetInfo)dest, peMode, traverseMode, maxDanger);
		}

		public bool CanReachNonLocal(IntVec3 start, TargetInfo dest, PathEndMode peMode, TraverseParms traverseParams)
		{
			return (dest.Map == null || dest.Map == this.map) && this.CanReach(start, (LocalTargetInfo)dest, peMode, traverseParams);
		}

		public bool CanReach(IntVec3 start, LocalTargetInfo dest, PathEndMode peMode, TraverseMode traverseMode, Danger maxDanger)
		{
			return this.CanReach(start, dest, peMode, TraverseParms.For(traverseMode, maxDanger, false));
		}

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

		public bool CanReachColony(IntVec3 c)
		{
			return this.CanReachFactionBase(c, Faction.OfPlayer);
		}

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

		private bool CanUseCache(TraverseMode mode)
		{
			return mode != TraverseMode.PassAllDestroyableThingsNotWater && mode != TraverseMode.NoPassClosedDoorsOrWater;
		}

		[CompilerGenerated]
		private sealed class <CheckCellBasedReachability>c__AnonStorey0
		{
			internal CellIndices cellIndices;

			internal TraverseParms traverseParams;

			internal PathGrid pathGrid;

			internal Region[] directRegionGrid;

			internal LocalTargetInfo dest;

			internal PathEndMode peMode;

			internal IntVec3 foundCell;

			internal Reachability $this;

			public <CheckCellBasedReachability>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 c)
			{
				int num = this.cellIndices.CellToIndex(c);
				if (this.traverseParams.mode == TraverseMode.PassAllDestroyableThingsNotWater || this.traverseParams.mode == TraverseMode.NoPassClosedDoorsOrWater)
				{
					if (c.GetTerrain(this.$this.map).IsWater)
					{
						return false;
					}
				}
				if (this.traverseParams.mode == TraverseMode.PassAllDestroyableThings || this.traverseParams.mode == TraverseMode.PassAllDestroyableThingsNotWater)
				{
					if (!this.pathGrid.WalkableFast(num))
					{
						Building edifice = c.GetEdifice(this.$this.map);
						if (edifice == null || !PathFinder.IsDestroyable(edifice))
						{
							return false;
						}
					}
				}
				else if (this.traverseParams.mode != TraverseMode.NoPassClosedDoorsOrWater)
				{
					Log.ErrorOnce("Do not use this method for non-cell based modes!", 938476762, false);
					if (!this.pathGrid.WalkableFast(num))
					{
						return false;
					}
				}
				Region region = this.directRegionGrid[num];
				return region == null || region.Allows(this.traverseParams, false);
			}

			internal bool <>m__1(IntVec3 c)
			{
				bool result;
				if (ReachabilityImmediate.CanReachImmediate(c, this.dest, this.$this.map, this.peMode, this.traverseParams.pawn))
				{
					this.foundCell = c;
					result = true;
				}
				else
				{
					result = false;
				}
				return result;
			}
		}

		[CompilerGenerated]
		private sealed class <CanReachMapEdge>c__AnonStorey1
		{
			internal TraverseParms traverseParms;

			internal bool foundReg;

			public <CanReachMapEdge>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Region from, Region r)
			{
				return r.Allows(this.traverseParms, false);
			}

			internal bool <>m__1(Region r)
			{
				bool result;
				if (r.Room.TouchesMapEdge)
				{
					this.foundReg = true;
					result = true;
				}
				else
				{
					result = false;
				}
				return result;
			}
		}

		[CompilerGenerated]
		private sealed class <CanReachUnfogged>c__AnonStorey2
		{
			internal TraverseParms traverseParms;

			internal bool foundReg;

			internal Reachability $this;

			public <CanReachUnfogged>c__AnonStorey2()
			{
			}

			internal bool <>m__0(Region from, Region r)
			{
				return r.Allows(this.traverseParms, false);
			}

			internal bool <>m__1(Region r)
			{
				bool result;
				if (!r.AnyCell.Fogged(this.$this.map))
				{
					this.foundReg = true;
					result = true;
				}
				else
				{
					result = false;
				}
				return result;
			}
		}
	}
}
