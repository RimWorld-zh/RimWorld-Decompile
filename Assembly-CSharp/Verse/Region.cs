using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Verse
{
	public sealed class Region
	{
		public RegionType type = RegionType.Normal;

		public int id = -1;

		public sbyte mapIndex = (sbyte)(-1);

		private Room roomInt;

		public List<RegionLink> links = new List<RegionLink>();

		public CellRect extentsClose;

		public CellRect extentsLimit;

		public Building_Door portal;

		private int precalculatedHashCode;

		public bool touchesMapEdge = false;

		private int cachedCellCount = -1;

		public bool valid = true;

		private ListerThings listerThings = new ListerThings(ListerThingsUse.Region);

		public uint[] closedIndex = new uint[RegionTraverser.NumWorkers];

		public uint reachedIndex = 0u;

		public int newRegionGroupIndex = -1;

		private Dictionary<Area, AreaOverlap> cachedAreaOverlaps = null;

		public int mark;

		private List<KeyValuePair<Pawn, Danger>> cachedDangers = new List<KeyValuePair<Pawn, Danger>>();

		private int cachedDangersForFrame;

		private int debug_makeTick = -1000;

		private int debug_lastTraverseTick = -1000;

		private static int nextId = 1;

		public const int GridSize = 12;

		public Map Map
		{
			get
			{
				return (this.mapIndex >= 0) ? Find.Maps[this.mapIndex] : null;
			}
		}

		public IEnumerable<IntVec3> Cells
		{
			get
			{
				RegionGrid regions = this.Map.regionGrid;
				for (int z = this.extentsClose.minZ; z <= this.extentsClose.maxZ; z++)
				{
					for (int x = this.extentsClose.minX; x <= this.extentsClose.maxX; x++)
					{
						IntVec3 c = new IntVec3(x, 0, z);
						if (regions.GetRegionAt_NoRebuild_InvalidAllowed(c) == this)
						{
							yield return c;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
			}
		}

		public int CellCount
		{
			get
			{
				if (this.cachedCellCount == -1)
				{
					this.cachedCellCount = this.Cells.Count();
				}
				return this.cachedCellCount;
			}
		}

		public IEnumerable<Region> Neighbors
		{
			get
			{
				for (int li = 0; li < this.links.Count; li++)
				{
					RegionLink link = this.links[li];
					for (int ri = 0; ri < 2; ri++)
					{
						if (link.regions[ri] != null && link.regions[ri] != this && link.regions[ri].valid)
						{
							yield return link.regions[ri];
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
			}
		}

		public IEnumerable<Region> NeighborsOfSameType
		{
			get
			{
				for (int li = 0; li < this.links.Count; li++)
				{
					RegionLink link = this.links[li];
					for (int ri = 0; ri < 2; ri++)
					{
						if (link.regions[ri] != null && link.regions[ri] != this && link.regions[ri].type == this.type && link.regions[ri].valid)
						{
							yield return link.regions[ri];
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
			}
		}

		public Room Room
		{
			get
			{
				return this.roomInt;
			}
			set
			{
				if (value != this.roomInt)
				{
					if (this.roomInt != null)
					{
						this.roomInt.RemoveRegion(this);
					}
					this.roomInt = value;
					if (this.roomInt != null)
					{
						this.roomInt.AddRegion(this);
					}
				}
			}
		}

		public IntVec3 RandomCell
		{
			get
			{
				Map map = this.Map;
				CellIndices cellIndices = map.cellIndices;
				Region[] directGrid = map.regionGrid.DirectGrid;
				int num = 0;
				IntVec3 result;
				while (true)
				{
					if (num < 1000)
					{
						IntVec3 randomCell = this.extentsClose.RandomCell;
						if (directGrid[cellIndices.CellToIndex(randomCell)] == this)
						{
							result = randomCell;
							break;
						}
						num++;
						continue;
					}
					result = this.AnyCell;
					break;
				}
				return result;
			}
		}

		public IntVec3 AnyCell
		{
			get
			{
				Map map = this.Map;
				CellIndices cellIndices = map.cellIndices;
				Region[] directGrid = map.regionGrid.DirectGrid;
				CellRect.CellRectIterator iterator = this.extentsClose.GetIterator();
				IntVec3 result;
				while (true)
				{
					if (!iterator.Done())
					{
						IntVec3 current = iterator.Current;
						if (directGrid[cellIndices.CellToIndex(current)] == this)
						{
							result = current;
							break;
						}
						iterator.MoveNext();
						continue;
					}
					Log.Error("Couldn't find any cell in region " + this.ToString());
					result = this.extentsClose.RandomCell;
					break;
				}
				return result;
			}
		}

		public string DebugString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("id: " + this.id);
				stringBuilder.AppendLine("mapIndex: " + this.mapIndex);
				stringBuilder.AppendLine("links count: " + this.links.Count);
				foreach (RegionLink link in this.links)
				{
					stringBuilder.AppendLine("  --" + link.ToString());
				}
				stringBuilder.AppendLine("valid: " + this.valid.ToString());
				stringBuilder.AppendLine("makeTick: " + this.debug_makeTick);
				stringBuilder.AppendLine("roomID: " + ((this.Room == null) ? "null room!" : this.Room.ID.ToString()));
				stringBuilder.AppendLine("extentsClose: " + this.extentsClose);
				stringBuilder.AppendLine("extentsLimit: " + this.extentsLimit);
				stringBuilder.AppendLine("ListerThings:");
				if (this.listerThings.AllThings != null)
				{
					for (int i = 0; i < this.listerThings.AllThings.Count; i++)
					{
						stringBuilder.AppendLine("  --" + this.listerThings.AllThings[i]);
					}
				}
				return stringBuilder.ToString();
			}
		}

		public bool DebugIsNew
		{
			get
			{
				return this.debug_makeTick > Find.TickManager.TicksGame - 60;
			}
		}

		public ListerThings ListerThings
		{
			get
			{
				return this.listerThings;
			}
		}

		private Region()
		{
		}

		public static Region MakeNewUnfilled(IntVec3 root, Map map)
		{
			Region region = new Region();
			region.debug_makeTick = Find.TickManager.TicksGame;
			region.id = Region.nextId;
			Region.nextId++;
			region.mapIndex = (sbyte)map.Index;
			region.precalculatedHashCode = Gen.HashCombineInt(region.id, 1295813358);
			region.extentsClose.minX = root.x;
			region.extentsClose.maxX = root.x;
			region.extentsClose.minZ = root.z;
			region.extentsClose.maxZ = root.z;
			region.extentsLimit.minX = root.x - root.x % 12;
			region.extentsLimit.maxX = root.x + 12 - (root.x + 12) % 12 - 1;
			region.extentsLimit.minZ = root.z - root.z % 12;
			region.extentsLimit.maxZ = root.z + 12 - (root.z + 12) % 12 - 1;
			region.extentsLimit.ClipInsideMap(map);
			return region;
		}

		public bool Allows(TraverseParms tp, bool isDestination)
		{
			bool result;
			if (tp.mode != TraverseMode.PassAllDestroyableThings && !this.type.Passable())
			{
				result = false;
			}
			else
			{
				if ((int)tp.maxDanger < 3 && tp.pawn != null)
				{
					Danger danger = this.DangerFor(tp.pawn);
					if (isDestination || danger == Danger.Deadly)
					{
						Region region = tp.pawn.GetRegion(RegionType.Set_All);
						if ((region == null || (int)danger > (int)region.DangerFor(tp.pawn)) && (int)danger > (int)tp.maxDanger)
						{
							result = false;
							goto IL_01ac;
						}
					}
				}
				switch (tp.mode)
				{
				case TraverseMode.ByPawn:
				{
					if (this.portal != null)
					{
						ByteGrid avoidGrid = tp.pawn.GetAvoidGrid();
						result = ((avoidGrid == null || avoidGrid[this.portal.Position] != 255) && ((!tp.pawn.HostileTo(this.portal)) ? (this.portal.CanPhysicallyPass(tp.pawn) && !this.portal.IsForbiddenToPass(tp.pawn)) : (this.portal.CanPhysicallyPass(tp.pawn) || tp.canBash)));
					}
					else
					{
						result = true;
					}
					break;
				}
				case TraverseMode.NoPassClosedDoors:
				{
					result = (this.portal == null || this.portal.FreePassage);
					break;
				}
				case TraverseMode.PassDoors:
				{
					result = true;
					break;
				}
				case TraverseMode.PassAllDestroyableThings:
				{
					result = true;
					break;
				}
				default:
				{
					throw new NotImplementedException();
				}
				}
			}
			goto IL_01ac;
			IL_01ac:
			return result;
		}

		public Danger DangerFor(Pawn p)
		{
			int i;
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (this.cachedDangersForFrame != Time.frameCount)
				{
					this.cachedDangers.Clear();
					this.cachedDangersForFrame = Time.frameCount;
				}
				else
				{
					for (i = 0; i < this.cachedDangers.Count; i++)
					{
						if (this.cachedDangers[i].Key == p)
							goto IL_005d;
					}
				}
			}
			Room room = this.Room;
			float temperature = room.Temperature;
			FloatRange floatRange = p.SafeTemperatureRange();
			Danger danger = (Danger)(floatRange.Includes(temperature) ? 1 : ((!floatRange.ExpandedBy(80f).Includes(temperature)) ? 3 : 2));
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.cachedDangers.Add(new KeyValuePair<Pawn, Danger>(p, danger));
			}
			Danger result = danger;
			goto IL_010d;
			IL_010d:
			return result;
			IL_005d:
			result = this.cachedDangers[i].Value;
			goto IL_010d;
		}

		public AreaOverlap OverlapWith(Area a)
		{
			AreaOverlap result;
			if (a.TrueCount == 0)
			{
				result = AreaOverlap.None;
			}
			else if (this.Map != a.Map)
			{
				result = AreaOverlap.None;
			}
			else
			{
				if (this.cachedAreaOverlaps == null)
				{
					this.cachedAreaOverlaps = new Dictionary<Area, AreaOverlap>();
				}
				AreaOverlap areaOverlap = default(AreaOverlap);
				if (!this.cachedAreaOverlaps.TryGetValue(a, out areaOverlap))
				{
					int num = 0;
					int num2 = 0;
					foreach (IntVec3 cell in this.Cells)
					{
						num2++;
						if (a[cell])
						{
							num++;
						}
					}
					areaOverlap = (AreaOverlap)((num != 0) ? ((num == num2) ? 1 : 2) : 0);
					this.cachedAreaOverlaps.Add(a, areaOverlap);
				}
				result = areaOverlap;
			}
			return result;
		}

		public void Notify_AreaChanged(Area a)
		{
			if (this.cachedAreaOverlaps != null && this.cachedAreaOverlaps.ContainsKey(a))
			{
				this.cachedAreaOverlaps.Remove(a);
			}
		}

		public void DecrementMapIndex()
		{
			if (this.mapIndex <= 0)
			{
				Log.Warning("Tried to decrement map index for region " + this.id + ", but mapIndex=" + this.mapIndex);
			}
			else
			{
				this.mapIndex = (sbyte)(this.mapIndex - 1);
			}
		}

		public void Notify_MyMapRemoved()
		{
			this.mapIndex = (sbyte)(-1);
		}

		public override string ToString()
		{
			string str = (this.portal == null) ? "null" : this.portal.ToString();
			return "Region(id=" + this.id + ", mapIndex=" + this.mapIndex + ", center=" + this.extentsClose.CenterCell + ", links=" + this.links.Count + ", cells=" + this.CellCount + ((this.portal == null) ? null : (", portal=" + str)) + ")";
		}

		public void DebugDraw()
		{
			if (DebugViewSettings.drawRegionTraversal && Find.TickManager.TicksGame < this.debug_lastTraverseTick + 60)
			{
				float a = (float)(1.0 - (float)(Find.TickManager.TicksGame - this.debug_lastTraverseTick) / 60.0);
				GenDraw.DrawFieldEdges(this.Cells.ToList(), new Color(0f, 0f, 1f, a));
			}
		}

		public void DebugDrawMouseover()
		{
			int num = Mathf.RoundToInt((float)(Time.realtimeSinceStartup * 2.0)) % 2;
			if (DebugViewSettings.drawRegions)
			{
				Color color = this.valid ? ((!this.DebugIsNew) ? Color.green : Color.yellow) : Color.red;
				GenDraw.DrawFieldEdges(this.Cells.ToList(), color);
				foreach (Region neighbor in this.Neighbors)
				{
					GenDraw.DrawFieldEdges(neighbor.Cells.ToList(), Color.grey);
				}
			}
			if (DebugViewSettings.drawRegionLinks)
			{
				foreach (RegionLink link in this.links)
				{
					if (num == 1)
					{
						foreach (IntVec3 cell in link.span.Cells)
						{
							CellRenderer.RenderCell(cell, DebugSolidColorMats.MaterialOf(Color.magenta));
						}
					}
				}
			}
			if (DebugViewSettings.drawRegionThings)
			{
				foreach (Thing allThing in this.listerThings.AllThings)
				{
					CellRenderer.RenderSpot(allThing.TrueCenter(), (float)((float)(allThing.thingIDNumber % 256) / 256.0));
				}
			}
		}

		public void Debug_Notify_Traversed()
		{
			this.debug_lastTraverseTick = Find.TickManager.TicksGame;
		}

		public override int GetHashCode()
		{
			return this.precalculatedHashCode;
		}

		public override bool Equals(object obj)
		{
			bool result;
			if (obj == null)
			{
				result = false;
			}
			else
			{
				Region region = obj as Region;
				result = (region != null && region.id == this.id);
			}
			return result;
		}
	}
}
