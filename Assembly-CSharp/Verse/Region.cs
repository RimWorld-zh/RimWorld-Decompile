using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public sealed class Region
	{
		public RegionType type = RegionType.Normal;

		public int id = -1;

		public sbyte mapIndex = -1;

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

		private float cachedBaseDesiredPlantsCount;

		private int cachedBaseDesiredPlantsCountForTick = -999999;

		private int debug_makeTick = -1000;

		private int debug_lastTraverseTick = -1000;

		private static int nextId = 1;

		public const int GridSize = 12;

		private Region()
		{
		}

		public Map Map
		{
			get
			{
				return ((int)this.mapIndex >= 0) ? Find.Maps[(int)this.mapIndex] : null;
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
						}
					}
				}
				yield break;
			}
		}

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

		public void Notify_MyMapRemoved()
		{
			this.listerThings.Clear();
			this.mapIndex = -1;
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static Region()
		{
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal RegionGrid <regions>__0;

			internal int <z>__1;

			internal int <x>__2;

			internal IntVec3 <c>__3;

			internal Region $this;

			internal IntVec3 $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					regions = base.Map.regionGrid;
					z = this.extentsClose.minZ;
					goto IL_FD;
				case 1u:
					IL_C4:
					x++;
					break;
				default:
					return false;
				}
				IL_D3:
				if (x > this.extentsClose.maxX)
				{
					z++;
				}
				else
				{
					c = new IntVec3(x, 0, z);
					if (regions.GetRegionAt_NoRebuild_InvalidAllowed(c) == this)
					{
						this.$current = c;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_C4;
				}
				IL_FD:
				if (z <= this.extentsClose.maxZ)
				{
					x = this.extentsClose.minX;
					goto IL_D3;
				}
				this.$PC = -1;
				return false;
			}

			IntVec3 IEnumerator<IntVec3>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.IntVec3>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<IntVec3> IEnumerable<IntVec3>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Region.<>c__Iterator0 <>c__Iterator = new Region.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator1 : IEnumerable, IEnumerable<Region>, IEnumerator, IDisposable, IEnumerator<Region>
		{
			internal int <li>__1;

			internal RegionLink <link>__2;

			internal int <ri>__3;

			internal Region $this;

			internal Region $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					li = 0;
					goto IL_100;
				case 1u:
					break;
				default:
					return false;
				}
				IL_D6:
				ri++;
				IL_E5:
				if (ri >= 2)
				{
					li++;
				}
				else
				{
					if (link.regions[ri] != null && link.regions[ri] != this && link.regions[ri].valid)
					{
						this.$current = link.regions[ri];
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_D6;
				}
				IL_100:
				if (li < this.links.Count)
				{
					link = this.links[li];
					ri = 0;
					goto IL_E5;
				}
				this.$PC = -1;
				return false;
			}

			Region IEnumerator<Region>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Region>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Region> IEnumerable<Region>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Region.<>c__Iterator1 <>c__Iterator = new Region.<>c__Iterator1();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator2 : IEnumerable, IEnumerable<Region>, IEnumerator, IDisposable, IEnumerator<Region>
		{
			internal int <li>__1;

			internal RegionLink <link>__2;

			internal int <ri>__3;

			internal Region $this;

			internal Region $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator2()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					li = 0;
					goto IL_127;
				case 1u:
					break;
				default:
					return false;
				}
				IL_FD:
				ri++;
				IL_10C:
				if (ri >= 2)
				{
					li++;
				}
				else
				{
					if (link.regions[ri] != null && link.regions[ri] != this && link.regions[ri].type == this.type && link.regions[ri].valid)
					{
						this.$current = link.regions[ri];
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_FD;
				}
				IL_127:
				if (li < this.links.Count)
				{
					link = this.links[li];
					ri = 0;
					goto IL_10C;
				}
				this.$PC = -1;
				return false;
			}

			Region IEnumerator<Region>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Region>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Region> IEnumerable<Region>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Region.<>c__Iterator2 <>c__Iterator = new Region.<>c__Iterator2();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}
	}
}
