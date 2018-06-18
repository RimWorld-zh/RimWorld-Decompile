using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C88 RID: 3208
	public sealed class Region
	{
		// Token: 0x0600462D RID: 17965 RVA: 0x0024F028 File Offset: 0x0024D428
		private Region()
		{
		}

		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x0600462E RID: 17966 RVA: 0x0024F0D0 File Offset: 0x0024D4D0
		public Map Map
		{
			get
			{
				return ((int)this.mapIndex >= 0) ? Find.Maps[(int)this.mapIndex] : null;
			}
		}

		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x0600462F RID: 17967 RVA: 0x0024F10C File Offset: 0x0024D50C
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
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x06004630 RID: 17968 RVA: 0x0024F138 File Offset: 0x0024D538
		public int CellCount
		{
			get
			{
				if (this.cachedCellCount == -1)
				{
					this.cachedCellCount = this.Cells.Count<IntVec3>();
				}
				return this.cachedCellCount;
			}
		}

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x06004631 RID: 17969 RVA: 0x0024F174 File Offset: 0x0024D574
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
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x06004632 RID: 17970 RVA: 0x0024F1A0 File Offset: 0x0024D5A0
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
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x06004633 RID: 17971 RVA: 0x0024F1CC File Offset: 0x0024D5CC
		// (set) Token: 0x06004634 RID: 17972 RVA: 0x0024F1E8 File Offset: 0x0024D5E8
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

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x06004635 RID: 17973 RVA: 0x0024F23C File Offset: 0x0024D63C
		public IntVec3 RandomCell
		{
			get
			{
				Map map = this.Map;
				CellIndices cellIndices = map.cellIndices;
				Region[] directGrid = map.regionGrid.DirectGrid;
				for (int i = 0; i < 1000; i++)
				{
					IntVec3 randomCell = this.extentsClose.RandomCell;
					if (directGrid[cellIndices.CellToIndex(randomCell)] == this)
					{
						return randomCell;
					}
				}
				return this.AnyCell;
			}
		}

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x06004636 RID: 17974 RVA: 0x0024F2B4 File Offset: 0x0024D6B4
		public IntVec3 AnyCell
		{
			get
			{
				Map map = this.Map;
				CellIndices cellIndices = map.cellIndices;
				Region[] directGrid = map.regionGrid.DirectGrid;
				CellRect.CellRectIterator iterator = this.extentsClose.GetIterator();
				while (!iterator.Done())
				{
					IntVec3 intVec = iterator.Current;
					if (directGrid[cellIndices.CellToIndex(intVec)] == this)
					{
						return intVec;
					}
					iterator.MoveNext();
				}
				Log.Error("Couldn't find any cell in region " + this.ToString(), false);
				return this.extentsClose.RandomCell;
			}
		}

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x06004637 RID: 17975 RVA: 0x0024F350 File Offset: 0x0024D750
		public string DebugString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("id: " + this.id);
				stringBuilder.AppendLine("mapIndex: " + this.mapIndex);
				stringBuilder.AppendLine("links count: " + this.links.Count);
				foreach (RegionLink regionLink in this.links)
				{
					stringBuilder.AppendLine("  --" + regionLink.ToString());
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

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x06004638 RID: 17976 RVA: 0x0024F544 File Offset: 0x0024D944
		public bool DebugIsNew
		{
			get
			{
				return this.debug_makeTick > Find.TickManager.TicksGame - 60;
			}
		}

		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x06004639 RID: 17977 RVA: 0x0024F570 File Offset: 0x0024D970
		public ListerThings ListerThings
		{
			get
			{
				return this.listerThings;
			}
		}

		// Token: 0x0600463A RID: 17978 RVA: 0x0024F58C File Offset: 0x0024D98C
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

		// Token: 0x0600463B RID: 17979 RVA: 0x0024F6CC File Offset: 0x0024DACC
		public bool Allows(TraverseParms tp, bool isDestination)
		{
			bool result;
			if (tp.mode != TraverseMode.PassAllDestroyableThings && tp.mode != TraverseMode.PassAllDestroyableThingsNotWater && !this.type.Passable())
			{
				result = false;
			}
			else
			{
				if (tp.maxDanger < Danger.Deadly && tp.pawn != null)
				{
					Danger danger = this.DangerFor(tp.pawn);
					if (isDestination || danger == Danger.Deadly)
					{
						Region region = tp.pawn.GetRegion(RegionType.Set_All);
						if ((region == null || danger > region.DangerFor(tp.pawn)) && danger > tp.maxDanger)
						{
							return false;
						}
					}
				}
				switch (tp.mode)
				{
				case TraverseMode.ByPawn:
					if (this.portal != null)
					{
						ByteGrid avoidGrid = tp.pawn.GetAvoidGrid();
						if (avoidGrid != null && avoidGrid[this.portal.Position] == 255)
						{
							result = false;
						}
						else if (tp.pawn.HostileTo(this.portal))
						{
							result = (this.portal.CanPhysicallyPass(tp.pawn) || tp.canBash);
						}
						else
						{
							result = (this.portal.CanPhysicallyPass(tp.pawn) && !this.portal.IsForbiddenToPass(tp.pawn));
						}
					}
					else
					{
						result = true;
					}
					break;
				case TraverseMode.PassDoors:
					result = true;
					break;
				case TraverseMode.NoPassClosedDoors:
					result = (this.portal == null || this.portal.FreePassage);
					break;
				case TraverseMode.PassAllDestroyableThings:
					result = true;
					break;
				case TraverseMode.NoPassClosedDoorsOrWater:
					result = (this.portal == null || this.portal.FreePassage);
					break;
				case TraverseMode.PassAllDestroyableThingsNotWater:
					result = true;
					break;
				default:
					throw new NotImplementedException();
				}
			}
			return result;
		}

		// Token: 0x0600463C RID: 17980 RVA: 0x0024F8C4 File Offset: 0x0024DCC4
		public Danger DangerFor(Pawn p)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (this.cachedDangersForFrame != Time.frameCount)
				{
					this.cachedDangers.Clear();
					this.cachedDangersForFrame = Time.frameCount;
				}
				else
				{
					for (int i = 0; i < this.cachedDangers.Count; i++)
					{
						if (this.cachedDangers[i].Key == p)
						{
							return this.cachedDangers[i].Value;
						}
					}
				}
			}
			Room room = this.Room;
			float temperature = room.Temperature;
			FloatRange floatRange = p.SafeTemperatureRange();
			Danger danger;
			if (floatRange.Includes(temperature))
			{
				danger = Danger.None;
			}
			else if (floatRange.ExpandedBy(80f).Includes(temperature))
			{
				danger = Danger.Some;
			}
			else
			{
				danger = Danger.Deadly;
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.cachedDangers.Add(new KeyValuePair<Pawn, Danger>(p, danger));
			}
			return danger;
		}

		// Token: 0x0600463D RID: 17981 RVA: 0x0024F9E0 File Offset: 0x0024DDE0
		public float GetBaseDesiredPlantsCount(bool allowCache = true)
		{
			int ticksGame = Find.TickManager.TicksGame;
			float result;
			if (allowCache && ticksGame - this.cachedBaseDesiredPlantsCountForTick < 2500)
			{
				result = this.cachedBaseDesiredPlantsCount;
			}
			else
			{
				this.cachedBaseDesiredPlantsCount = 0f;
				Map map = this.Map;
				foreach (IntVec3 c in this.Cells)
				{
					this.cachedBaseDesiredPlantsCount += map.wildPlantSpawner.GetBaseDesiredPlantsCountAt(c);
				}
				this.cachedBaseDesiredPlantsCountForTick = ticksGame;
				result = this.cachedBaseDesiredPlantsCount;
			}
			return result;
		}

		// Token: 0x0600463E RID: 17982 RVA: 0x0024FAAC File Offset: 0x0024DEAC
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
				AreaOverlap areaOverlap;
				if (!this.cachedAreaOverlaps.TryGetValue(a, out areaOverlap))
				{
					int num = 0;
					int num2 = 0;
					foreach (IntVec3 c in this.Cells)
					{
						num2++;
						if (a[c])
						{
							num++;
						}
					}
					if (num == 0)
					{
						areaOverlap = AreaOverlap.None;
					}
					else if (num == num2)
					{
						areaOverlap = AreaOverlap.Entire;
					}
					else
					{
						areaOverlap = AreaOverlap.Partial;
					}
					this.cachedAreaOverlaps.Add(a, areaOverlap);
				}
				result = areaOverlap;
			}
			return result;
		}

		// Token: 0x0600463F RID: 17983 RVA: 0x0024FBA8 File Offset: 0x0024DFA8
		public void Notify_AreaChanged(Area a)
		{
			if (this.cachedAreaOverlaps != null)
			{
				if (this.cachedAreaOverlaps.ContainsKey(a))
				{
					this.cachedAreaOverlaps.Remove(a);
				}
			}
		}

		// Token: 0x06004640 RID: 17984 RVA: 0x0024FBDC File Offset: 0x0024DFDC
		public void DecrementMapIndex()
		{
			if ((int)this.mapIndex <= 0)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to decrement map index for region ",
					this.id,
					", but mapIndex=",
					this.mapIndex
				}), false);
			}
			else
			{
				this.mapIndex = (sbyte)((int)this.mapIndex - 1);
			}
		}

		// Token: 0x06004641 RID: 17985 RVA: 0x0024FC4A File Offset: 0x0024E04A
		public void Notify_MyMapRemoved()
		{
			this.listerThings.Clear();
			this.mapIndex = -1;
		}

		// Token: 0x06004642 RID: 17986 RVA: 0x0024FC60 File Offset: 0x0024E060
		public override string ToString()
		{
			string str;
			if (this.portal != null)
			{
				str = this.portal.ToString();
			}
			else
			{
				str = "null";
			}
			return string.Concat(new object[]
			{
				"Region(id=",
				this.id,
				", mapIndex=",
				this.mapIndex,
				", center=",
				this.extentsClose.CenterCell,
				", links=",
				this.links.Count,
				", cells=",
				this.CellCount,
				(this.portal == null) ? null : (", portal=" + str),
				")"
			});
		}

		// Token: 0x06004643 RID: 17987 RVA: 0x0024FD48 File Offset: 0x0024E148
		public void DebugDraw()
		{
			if (DebugViewSettings.drawRegionTraversal)
			{
				if (Find.TickManager.TicksGame < this.debug_lastTraverseTick + 60)
				{
					float a = 1f - (float)(Find.TickManager.TicksGame - this.debug_lastTraverseTick) / 60f;
					GenDraw.DrawFieldEdges(this.Cells.ToList<IntVec3>(), new Color(0f, 0f, 1f, a));
				}
			}
		}

		// Token: 0x06004644 RID: 17988 RVA: 0x0024FDC0 File Offset: 0x0024E1C0
		public void DebugDrawMouseover()
		{
			int num = Mathf.RoundToInt(Time.realtimeSinceStartup * 2f) % 2;
			if (DebugViewSettings.drawRegions)
			{
				Color color;
				if (!this.valid)
				{
					color = Color.red;
				}
				else if (this.DebugIsNew)
				{
					color = Color.yellow;
				}
				else
				{
					color = Color.green;
				}
				GenDraw.DrawFieldEdges(this.Cells.ToList<IntVec3>(), color);
				foreach (Region region in this.Neighbors)
				{
					GenDraw.DrawFieldEdges(region.Cells.ToList<IntVec3>(), Color.grey);
				}
			}
			if (DebugViewSettings.drawRegionLinks)
			{
				foreach (RegionLink regionLink in this.links)
				{
					if (num == 1)
					{
						foreach (IntVec3 c in regionLink.span.Cells)
						{
							CellRenderer.RenderCell(c, DebugSolidColorMats.MaterialOf(Color.magenta));
						}
					}
				}
			}
			if (DebugViewSettings.drawRegionThings)
			{
				foreach (Thing thing in this.listerThings.AllThings)
				{
					CellRenderer.RenderSpot(thing.TrueCenter(), (float)(thing.thingIDNumber % 256) / 256f);
				}
			}
		}

		// Token: 0x06004645 RID: 17989 RVA: 0x0024FFC8 File Offset: 0x0024E3C8
		public void Debug_Notify_Traversed()
		{
			this.debug_lastTraverseTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06004646 RID: 17990 RVA: 0x0024FFDC File Offset: 0x0024E3DC
		public override int GetHashCode()
		{
			return this.precalculatedHashCode;
		}

		// Token: 0x06004647 RID: 17991 RVA: 0x0024FFF8 File Offset: 0x0024E3F8
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

		// Token: 0x04002FCA RID: 12234
		public RegionType type = RegionType.Normal;

		// Token: 0x04002FCB RID: 12235
		public int id = -1;

		// Token: 0x04002FCC RID: 12236
		public sbyte mapIndex = -1;

		// Token: 0x04002FCD RID: 12237
		private Room roomInt;

		// Token: 0x04002FCE RID: 12238
		public List<RegionLink> links = new List<RegionLink>();

		// Token: 0x04002FCF RID: 12239
		public CellRect extentsClose;

		// Token: 0x04002FD0 RID: 12240
		public CellRect extentsLimit;

		// Token: 0x04002FD1 RID: 12241
		public Building_Door portal;

		// Token: 0x04002FD2 RID: 12242
		private int precalculatedHashCode;

		// Token: 0x04002FD3 RID: 12243
		public bool touchesMapEdge = false;

		// Token: 0x04002FD4 RID: 12244
		private int cachedCellCount = -1;

		// Token: 0x04002FD5 RID: 12245
		public bool valid = true;

		// Token: 0x04002FD6 RID: 12246
		private ListerThings listerThings = new ListerThings(ListerThingsUse.Region);

		// Token: 0x04002FD7 RID: 12247
		public uint[] closedIndex = new uint[RegionTraverser.NumWorkers];

		// Token: 0x04002FD8 RID: 12248
		public uint reachedIndex = 0u;

		// Token: 0x04002FD9 RID: 12249
		public int newRegionGroupIndex = -1;

		// Token: 0x04002FDA RID: 12250
		private Dictionary<Area, AreaOverlap> cachedAreaOverlaps = null;

		// Token: 0x04002FDB RID: 12251
		public int mark;

		// Token: 0x04002FDC RID: 12252
		private List<KeyValuePair<Pawn, Danger>> cachedDangers = new List<KeyValuePair<Pawn, Danger>>();

		// Token: 0x04002FDD RID: 12253
		private int cachedDangersForFrame;

		// Token: 0x04002FDE RID: 12254
		private float cachedBaseDesiredPlantsCount;

		// Token: 0x04002FDF RID: 12255
		private int cachedBaseDesiredPlantsCountForTick = -999999;

		// Token: 0x04002FE0 RID: 12256
		private int debug_makeTick = -1000;

		// Token: 0x04002FE1 RID: 12257
		private int debug_lastTraverseTick = -1000;

		// Token: 0x04002FE2 RID: 12258
		private static int nextId = 1;

		// Token: 0x04002FE3 RID: 12259
		public const int GridSize = 12;
	}
}
