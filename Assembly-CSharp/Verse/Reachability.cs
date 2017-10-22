using RimWorld;
using System;
using System.Collections.Generic;
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
				Log.ErrorOnce("Tried to queue null region.", 881121);
			}
			else if (region.reachedIndex == this.reachedIndex)
			{
				Log.ErrorOnce("Region is already reached; you can't open it. Region: " + region.ToString(), 719991);
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
			uint num = this.reachedIndex;
			uint result = num;
			this.reachedIndex = num + 1;
			return result;
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
				Log.ErrorOnce("Called ReachableBetween while working. This should never happen. Suppressing further errors.", 7312233);
				result = false;
			}
			else
			{
				if (traverseParams.pawn != null)
				{
					if (!traverseParams.pawn.Spawned)
					{
						result = false;
						goto IL_03a9;
					}
					if (traverseParams.pawn.Map != this.map)
					{
						Log.Error("Called CanReach() with a pawn spawned not on this map. This means that we can't check his reachability here. Pawn's current map should have been used instead of this one. pawn=" + traverseParams.pawn + " pawn.Map=" + traverseParams.pawn.Map + " map=" + this.map);
						result = false;
						goto IL_03a9;
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
				else
				{
					if (start.InBounds(this.map) && dest.Cell.InBounds(this.map))
					{
						if (peMode == PathEndMode.OnCell || peMode == PathEndMode.Touch || peMode == PathEndMode.ClosestTouch)
						{
							Room room = RegionAndRoomQuery.RoomAtFast(start, this.map, RegionType.Set_Passable);
							if (room != null && room == RegionAndRoomQuery.RoomAtFast(dest.Cell, this.map, RegionType.Set_Passable))
							{
								result = true;
								goto IL_03a9;
							}
						}
						if (traverseParams.mode == TraverseMode.PassAllDestroyableThings)
						{
							TraverseParms traverseParams2 = traverseParams;
							traverseParams2.mode = TraverseMode.PassDoors;
							if (this.CanReach(start, dest, peMode, traverseParams2))
							{
								result = true;
								goto IL_03a9;
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
							switch (peMode)
							{
							case PathEndMode.OnCell:
							{
								Region region = dest.Cell.GetRegion(this.map, RegionType.Set_Passable);
								if (region != null && region.Allows(traverseParams, true))
								{
									this.destRegions.Add(region);
								}
								break;
							}
							case PathEndMode.Touch:
							{
								TouchPathEndModeUtility.AddAllowedAdjacentRegions(dest, traverseParams, this.map, this.destRegions);
								break;
							}
							}
							if (this.destRegions.Count == 0 && traverseParams.mode != TraverseMode.PassAllDestroyableThings)
							{
								this.FinalizeCheck();
								return false;
							}
							this.destRegions.RemoveDuplicates();
							this.openQueue.Clear();
							this.numRegionsOpened = 0;
							this.DetermineStartRegions(start);
							if (this.openQueue.Count == 0 && traverseParams.mode != TraverseMode.PassAllDestroyableThings)
							{
								this.FinalizeCheck();
								return false;
							}
							if (this.startingRegions.Any() && this.destRegions.Any())
							{
								switch (this.GetCachedResult(traverseParams))
								{
								case BoolUnknown.True:
								{
									this.FinalizeCheck();
									return true;
								}
								case BoolUnknown.False:
								{
									this.FinalizeCheck();
									return false;
								}
								}
							}
							if (traverseParams.mode == TraverseMode.PassAllDestroyableThings)
							{
								bool result2 = this.CheckCellBasedReachability(start, dest, peMode, traverseParams);
								this.FinalizeCheck();
								return result2;
							}
							bool result3 = this.CheckRegionBasedReachability(traverseParams);
							this.FinalizeCheck();
							return result3;
						}
						finally
						{
							this.working = false;
						}
					}
					result = false;
				}
			}
			goto IL_03a9;
			IL_03a9:
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
					if (intVec.InBounds(this.map) && this.pathGrid.WalkableFast(intVec))
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

		private BoolUnknown GetCachedResult(TraverseParms traverseParams)
		{
			bool flag = false;
			int num = 0;
			BoolUnknown result;
			while (true)
			{
				if (num < this.startingRegions.Count)
				{
					for (int i = 0; i < this.destRegions.Count; i++)
					{
						if (this.destRegions[i] == this.startingRegions[num])
							goto IL_0030;
						switch (this.cache.CachedResultFor(this.startingRegions[num].Room, this.destRegions[i].Room, traverseParams))
						{
						case BoolUnknown.True:
							goto IL_006e;
						case BoolUnknown.Unknown:
						{
							flag = true;
							break;
						}
						}
					}
					num++;
					continue;
				}
				result = (BoolUnknown)((!flag) ? 1 : 2);
				break;
				IL_0030:
				result = BoolUnknown.True;
				break;
				IL_006e:
				result = BoolUnknown.True;
				break;
			}
			return result;
		}

		private bool CheckRegionBasedReachability(TraverseParms traverseParams)
		{
			bool result;
			while (true)
			{
				Region region2;
				if (this.openQueue.Count > 0)
				{
					Region region = this.openQueue.Dequeue();
					for (int i = 0; i < region.links.Count; i++)
					{
						RegionLink regionLink = region.links[i];
						for (int j = 0; j < 2; j++)
						{
							region2 = regionLink.regions[j];
							if (region2 != null && region2.reachedIndex != this.reachedIndex && region2.type.Passable() && region2.Allows(traverseParams, false))
							{
								if (this.destRegions.Contains(region2))
									goto IL_008a;
								this.QueueNewOpenRegion(region2);
							}
						}
					}
					continue;
				}
				for (int k = 0; k < this.startingRegions.Count; k++)
				{
					for (int l = 0; l < this.destRegions.Count; l++)
					{
						this.cache.AddCachedResult(this.startingRegions[k].Room, this.destRegions[l].Room, traverseParams, false);
					}
				}
				result = false;
				break;
				IL_008a:
				for (int m = 0; m < this.startingRegions.Count; m++)
				{
					this.cache.AddCachedResult(this.startingRegions[m].Room, region2.Room, traverseParams, true);
				}
				result = true;
				break;
			}
			return result;
		}

		private bool CheckCellBasedReachability(IntVec3 start, LocalTargetInfo dest, PathEndMode peMode, TraverseParms traverseParams)
		{
			IntVec3 foundCell = IntVec3.Invalid;
			Region[] directRegionGrid = this.regionGrid.DirectGrid;
			PathGrid pathGrid = this.map.pathGrid;
			CellIndices cellIndices = this.map.cellIndices;
			this.map.floodFiller.FloodFill(start, (Predicate<IntVec3>)delegate(IntVec3 c)
			{
				int num = cellIndices.CellToIndex(c);
				bool result3;
				if (traverseParams.mode == TraverseMode.PassAllDestroyableThings)
				{
					if (!pathGrid.WalkableFast(num))
					{
						Building edifice = c.GetEdifice(this.map);
						if (edifice != null && PathFinder.IsDestroyable(edifice))
						{
							goto IL_008c;
						}
						result3 = false;
						goto IL_00bb;
					}
				}
				else
				{
					Log.ErrorOnce("Do not use this method for other modes than PassAllDestroyableThings!", 938476762);
					if (!pathGrid.WalkableFast(num))
					{
						result3 = false;
						goto IL_00bb;
					}
				}
				goto IL_008c;
				IL_00bb:
				return result3;
				IL_008c:
				Region region = directRegionGrid[num];
				result3 = ((byte)((region == null || region.Allows(traverseParams, false)) ? 1 : 0) != 0);
				goto IL_00bb;
			}, (Func<IntVec3, bool>)delegate(IntVec3 c)
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
			}, 2147483647, false, null);
			bool result;
			if (foundCell.IsValid)
			{
				Region validRegionAt = this.regionGrid.GetValidRegionAt(foundCell);
				if (validRegionAt != null)
				{
					for (int i = 0; i < this.startingRegions.Count; i++)
					{
						this.cache.AddCachedResult(this.startingRegions[i].Room, validRegionAt.Room, traverseParams, true);
					}
				}
				result = true;
			}
			else
			{
				for (int j = 0; j < this.startingRegions.Count; j++)
				{
					for (int k = 0; k < this.destRegions.Count; k++)
					{
						this.cache.AddCachedResult(this.startingRegions[j].Room, this.destRegions[k].Room, traverseParams, false);
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
						goto IL_0093;
				}
				TraverseParms traverseParams = TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false);
				if (faction == Faction.OfPlayer)
				{
					List<Building> allBuildingsColonist = this.map.listerBuildings.allBuildingsColonist;
					for (int j = 0; j < allBuildingsColonist.Count; j++)
					{
						if (this.CanReach(c, (Thing)allBuildingsColonist[j], PathEndMode.Touch, traverseParams))
							goto IL_00f9;
					}
				}
				else
				{
					List<Thing> list2 = this.map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
					for (int k = 0; k < list2.Count; k++)
					{
						if (list2[k].Faction == faction && this.CanReach(c, list2[k], PathEndMode.Touch, traverseParams))
							goto IL_016a;
					}
				}
				result = this.CanReachBiggestMapEdgeRoom(c);
			}
			goto IL_0194;
			IL_0194:
			return result;
			IL_0093:
			result = true;
			goto IL_0194;
			IL_00f9:
			result = true;
			goto IL_0194;
			IL_016a:
			result = true;
			goto IL_0194;
		}

		public bool CanReachBiggestMapEdgeRoom(IntVec3 c)
		{
			Room room = null;
			for (int i = 0; i < this.map.regionGrid.allRooms.Count; i++)
			{
				Room room2 = this.map.regionGrid.allRooms[i];
				if (room2.TouchesMapEdge && (room == null || room2.RegionCount > room.RegionCount))
				{
					room = room2;
				}
			}
			return room != null && this.CanReach(c, room.Regions[0].AnyCell, PathEndMode.OnCell, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false));
		}

		public bool CanReachMapEdge(IntVec3 c, TraverseParms traverseParms)
		{
			bool result;
			if (traverseParms.pawn != null)
			{
				if (!traverseParms.pawn.Spawned)
				{
					result = false;
					goto IL_0120;
				}
				if (traverseParms.pawn.Map != this.map)
				{
					Log.Error("Called CanReachMapEdge() with a pawn spawned not on this map. This means that we can't check his reachability here. Pawn's current map should have been used instead of this one. pawn=" + traverseParms.pawn + " pawn.Map=" + traverseParms.pawn.Map + " map=" + this.map);
					result = false;
					goto IL_0120;
				}
			}
			Region region = c.GetRegion(this.map, RegionType.Set_Passable);
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
				RegionEntryPredicate entryCondition = (RegionEntryPredicate)((Region from, Region r) => r.Allows(traverseParms, false));
				bool foundReg = false;
				RegionProcessor regionProcessor = (RegionProcessor)delegate(Region r)
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
			goto IL_0120;
			IL_0120:
			return result;
		}

		public bool CanReachUnfogged(IntVec3 c, TraverseParms traverseParms)
		{
			bool result;
			if (traverseParms.pawn != null)
			{
				if (!traverseParms.pawn.Spawned)
				{
					result = false;
					goto IL_0140;
				}
				if (traverseParms.pawn.Map != this.map)
				{
					Log.Error("Called CanReachUnfogged() with a pawn spawned not on this map. This means that we can't check his reachability here. Pawn's current map should have been used instead of this one. pawn=" + traverseParms.pawn + " pawn.Map=" + traverseParms.pawn.Map + " map=" + this.map);
					result = false;
					goto IL_0140;
				}
			}
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
					RegionEntryPredicate entryCondition = (RegionEntryPredicate)((Region from, Region r) => r.Allows(traverseParms, false));
					bool foundReg = false;
					RegionProcessor regionProcessor = (RegionProcessor)delegate(Region r)
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
			goto IL_0140;
			IL_0140:
			return result;
		}
	}
}
