using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld;
using UnityEngine;

namespace Verse
{
	public sealed class Room
	{
		public sbyte mapIndex = -1;

		private RoomGroup groupInt;

		private List<Region> regions = new List<Region>();

		public int ID = -16161616;

		public int lastChangeTick = -1;

		private int numRegionsTouchingMapEdge;

		private int cachedOpenRoofCount = -1;

		private IEnumerator<IntVec3> cachedOpenRoofState = null;

		public bool isPrisonCell;

		private int cachedCellCount = -1;

		private bool statsAndRoleDirty = true;

		private DefMap<RoomStatDef, float> stats = new DefMap<RoomStatDef, float>();

		private RoomRoleDef role;

		public int newOrReusedRoomGroupIndex = -1;

		private static int nextRoomID;

		private const int RegionCountHuge = 60;

		private const int MaxRegionsToAssignRoomRole = 36;

		private static readonly Color PrisonFieldColor = new Color(1f, 0.7f, 0.2f);

		private static readonly Color NonPrisonFieldColor = new Color(0.3f, 0.3f, 1f);

		private HashSet<Room> uniqueNeighborsSet = new HashSet<Room>();

		private List<Room> uniqueNeighbors = new List<Room>();

		private HashSet<Thing> uniqueContainedThingsSet = new HashSet<Thing>();

		private List<Thing> uniqueContainedThings = new List<Thing>();

		private static List<IntVec3> fields = new List<IntVec3>();

		[CompilerGenerated]
		private static Func<RoomStatDef, float> <>f__am$cache0;

		public Room()
		{
		}

		public Map Map
		{
			get
			{
				return ((int)this.mapIndex >= 0) ? Find.Maps[(int)this.mapIndex] : null;
			}
		}

		public RegionType RegionType
		{
			get
			{
				return (!this.regions.Any<Region>()) ? RegionType.None : this.regions[0].type;
			}
		}

		public List<Region> Regions
		{
			get
			{
				return this.regions;
			}
		}

		public int RegionCount
		{
			get
			{
				return this.regions.Count;
			}
		}

		public bool IsHuge
		{
			get
			{
				return this.regions.Count > 60;
			}
		}

		public bool Dereferenced
		{
			get
			{
				return this.regions.Count == 0;
			}
		}

		public bool TouchesMapEdge
		{
			get
			{
				return this.numRegionsTouchingMapEdge > 0;
			}
		}

		public float Temperature
		{
			get
			{
				return this.Group.Temperature;
			}
		}

		public bool UsesOutdoorTemperature
		{
			get
			{
				return this.Group.UsesOutdoorTemperature;
			}
		}

		public RoomGroup Group
		{
			get
			{
				return this.groupInt;
			}
			set
			{
				if (value != this.groupInt)
				{
					if (this.groupInt != null)
					{
						this.groupInt.RemoveRoom(this);
					}
					this.groupInt = value;
					if (this.groupInt != null)
					{
						this.groupInt.AddRoom(this);
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
					this.cachedCellCount = 0;
					for (int i = 0; i < this.regions.Count; i++)
					{
						this.cachedCellCount += this.regions[i].CellCount;
					}
				}
				return this.cachedCellCount;
			}
		}

		public int OpenRoofCount
		{
			get
			{
				return this.OpenRoofCountStopAt(int.MaxValue);
			}
		}

		public bool PsychologicallyOutdoors
		{
			get
			{
				return this.OpenRoofCountStopAt(300) >= 300 || (this.Group.AnyRoomTouchesMapEdge && (float)this.OpenRoofCount / (float)this.CellCount >= 0.5f);
			}
		}

		public bool OutdoorsForWork
		{
			get
			{
				return this.OpenRoofCountStopAt(101) > 100 || (float)this.OpenRoofCount > (float)this.CellCount * 0.25f;
			}
		}

		public List<Room> Neighbors
		{
			get
			{
				this.uniqueNeighborsSet.Clear();
				this.uniqueNeighbors.Clear();
				for (int i = 0; i < this.regions.Count; i++)
				{
					foreach (Region region in this.regions[i].Neighbors)
					{
						if (this.uniqueNeighborsSet.Add(region.Room) && region.Room != this)
						{
							this.uniqueNeighbors.Add(region.Room);
						}
					}
				}
				this.uniqueNeighborsSet.Clear();
				return this.uniqueNeighbors;
			}
		}

		public IEnumerable<IntVec3> Cells
		{
			get
			{
				for (int i = 0; i < this.regions.Count; i++)
				{
					foreach (IntVec3 c in this.regions[i].Cells)
					{
						yield return c;
					}
				}
				yield break;
			}
		}

		public IEnumerable<IntVec3> BorderCells
		{
			get
			{
				foreach (IntVec3 c in this.Cells)
				{
					for (int i = 0; i < 8; i++)
					{
						IntVec3 prospective = c + GenAdj.AdjacentCells[i];
						Region region = (c + GenAdj.AdjacentCells[i]).GetRegion(this.Map, RegionType.Set_Passable);
						if (region == null || region.Room != this)
						{
							yield return prospective;
						}
					}
				}
				yield break;
			}
		}

		public IEnumerable<Pawn> Owners
		{
			get
			{
				if (this.TouchesMapEdge)
				{
					yield break;
				}
				if (this.IsHuge)
				{
					yield break;
				}
				if (this.Role != RoomRoleDefOf.Bedroom && this.Role != RoomRoleDefOf.PrisonCell && this.Role != RoomRoleDefOf.Barracks && this.Role != RoomRoleDefOf.PrisonBarracks)
				{
					yield break;
				}
				Pawn firstOwner = null;
				Pawn secondOwner = null;
				foreach (Building_Bed building_Bed in this.ContainedBeds)
				{
					if (building_Bed.def.building.bed_humanlike)
					{
						for (int i = 0; i < building_Bed.owners.Count; i++)
						{
							if (firstOwner == null)
							{
								firstOwner = building_Bed.owners[i];
							}
							else
							{
								if (secondOwner != null)
								{
									yield break;
								}
								secondOwner = building_Bed.owners[i];
							}
						}
					}
				}
				if (firstOwner != null)
				{
					if (secondOwner == null)
					{
						yield return firstOwner;
					}
					else if (LovePartnerRelationUtility.LovePartnerRelationExists(firstOwner, secondOwner))
					{
						yield return firstOwner;
						yield return secondOwner;
					}
				}
				yield break;
			}
		}

		public IEnumerable<Building_Bed> ContainedBeds
		{
			get
			{
				List<Thing> things = this.ContainedAndAdjacentThings;
				for (int i = 0; i < things.Count; i++)
				{
					Building_Bed bed = things[i] as Building_Bed;
					if (bed != null)
					{
						yield return bed;
					}
				}
				yield break;
			}
		}

		public bool Fogged
		{
			get
			{
				return this.regions.Count != 0 && this.regions[0].AnyCell.Fogged(this.Map);
			}
		}

		public List<Thing> ContainedAndAdjacentThings
		{
			get
			{
				this.uniqueContainedThingsSet.Clear();
				this.uniqueContainedThings.Clear();
				for (int i = 0; i < this.regions.Count; i++)
				{
					List<Thing> allThings = this.regions[i].ListerThings.AllThings;
					if (allThings != null)
					{
						for (int j = 0; j < allThings.Count; j++)
						{
							Thing item = allThings[j];
							if (this.uniqueContainedThingsSet.Add(item))
							{
								this.uniqueContainedThings.Add(item);
							}
						}
					}
				}
				this.uniqueContainedThingsSet.Clear();
				return this.uniqueContainedThings;
			}
		}

		public RoomRoleDef Role
		{
			get
			{
				if (this.statsAndRoleDirty)
				{
					this.UpdateRoomStatsAndRole();
				}
				return this.role;
			}
		}

		public static Room MakeNew(Map map)
		{
			Room room = new Room();
			room.mapIndex = (sbyte)map.Index;
			room.ID = Room.nextRoomID;
			Room.nextRoomID++;
			return room;
		}

		public void AddRegion(Region r)
		{
			if (this.regions.Contains(r))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to add the same region twice to Room. region=",
					r,
					", room=",
					this
				}), false);
			}
			else
			{
				this.regions.Add(r);
				if (r.touchesMapEdge)
				{
					this.numRegionsTouchingMapEdge++;
				}
				if (this.regions.Count == 1)
				{
					this.Map.regionGrid.allRooms.Add(this);
				}
			}
		}

		public void RemoveRegion(Region r)
		{
			if (!this.regions.Contains(r))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to remove region from Room but this region is not here. region=",
					r,
					", room=",
					this
				}), false);
			}
			else
			{
				this.regions.Remove(r);
				if (r.touchesMapEdge)
				{
					this.numRegionsTouchingMapEdge--;
				}
				if (this.regions.Count == 0)
				{
					this.Group = null;
					this.cachedOpenRoofCount = -1;
					this.cachedOpenRoofState = null;
					this.statsAndRoleDirty = true;
					this.Map.regionGrid.allRooms.Remove(this);
				}
			}
		}

		public void Notify_MyMapRemoved()
		{
			this.mapIndex = -1;
		}

		public void Notify_ContainedThingSpawnedOrDespawned(Thing th)
		{
			if (th.def.category != ThingCategory.Mote && th.def.category != ThingCategory.Projectile && th.def.category != ThingCategory.Ethereal && th.def.category != ThingCategory.Pawn)
			{
				this.statsAndRoleDirty = true;
			}
		}

		public void Notify_TerrainChanged()
		{
			this.statsAndRoleDirty = true;
		}

		public void Notify_BedTypeChanged()
		{
			this.statsAndRoleDirty = true;
		}

		public void Notify_RoofChanged()
		{
			this.cachedOpenRoofCount = -1;
			this.cachedOpenRoofState = null;
			this.Group.Notify_RoofChanged();
		}

		public void Notify_RoomShapeOrContainedBedsChanged()
		{
			this.cachedCellCount = -1;
			this.cachedOpenRoofCount = -1;
			this.cachedOpenRoofState = null;
			if (Current.ProgramState == ProgramState.Playing && !this.Fogged)
			{
				this.Map.autoBuildRoofAreaSetter.TryGenerateAreaFor(this);
			}
			this.isPrisonCell = false;
			if (Building_Bed.RoomCanBePrisonCell(this))
			{
				List<Thing> containedAndAdjacentThings = this.ContainedAndAdjacentThings;
				for (int i = 0; i < containedAndAdjacentThings.Count; i++)
				{
					Building_Bed building_Bed = containedAndAdjacentThings[i] as Building_Bed;
					if (building_Bed != null && building_Bed.ForPrisoners)
					{
						this.isPrisonCell = true;
						break;
					}
				}
			}
			List<Thing> list = this.Map.listerThings.ThingsOfDef(ThingDefOf.NutrientPasteDispenser);
			for (int j = 0; j < list.Count; j++)
			{
				list[j].Notify_ColorChanged();
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (this.isPrisonCell)
				{
					foreach (Building_Bed building_Bed2 in this.ContainedBeds)
					{
						building_Bed2.ForPrisoners = true;
					}
				}
			}
			this.lastChangeTick = Find.TickManager.TicksGame;
			this.statsAndRoleDirty = true;
			FacilitiesUtility.NotifyFacilitiesAboutChangedLOSBlockers(this.regions);
		}

		public void DecrementMapIndex()
		{
			if ((int)this.mapIndex <= 0)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to decrement map index for room ",
					this.ID,
					", but mapIndex=",
					this.mapIndex
				}), false);
			}
			else
			{
				this.mapIndex = (sbyte)((int)this.mapIndex - 1);
			}
		}

		public float GetStat(RoomStatDef roomStat)
		{
			if (this.statsAndRoleDirty)
			{
				this.UpdateRoomStatsAndRole();
			}
			float result;
			if (this.stats == null)
			{
				result = roomStat.roomlessScore;
			}
			else
			{
				result = this.stats[roomStat];
			}
			return result;
		}

		public RoomStatScoreStage GetStatScoreStage(RoomStatDef stat)
		{
			return stat.GetScoreStage(this.GetStat(stat));
		}

		public void DrawFieldEdges()
		{
			Room.fields.Clear();
			Room.fields.AddRange(this.Cells);
			Color color = (!this.isPrisonCell) ? Room.NonPrisonFieldColor : Room.PrisonFieldColor;
			color.a = Pulser.PulseBrightness(1f, 0.6f);
			GenDraw.DrawFieldEdges(Room.fields, color);
			Room.fields.Clear();
		}

		public int OpenRoofCountStopAt(int threshold)
		{
			if (this.cachedOpenRoofCount == -1 && this.cachedOpenRoofState == null)
			{
				this.cachedOpenRoofCount = 0;
				this.cachedOpenRoofState = this.Cells.GetEnumerator();
			}
			if (this.cachedOpenRoofCount < threshold && this.cachedOpenRoofState != null)
			{
				RoofGrid roofGrid = this.Map.roofGrid;
				while (this.cachedOpenRoofCount < threshold && this.cachedOpenRoofState.MoveNext())
				{
					if (!roofGrid.Roofed(this.cachedOpenRoofState.Current))
					{
						this.cachedOpenRoofCount++;
					}
				}
				if (this.cachedOpenRoofCount < threshold)
				{
					this.cachedOpenRoofState = null;
				}
			}
			return this.cachedOpenRoofCount;
		}

		private void UpdateRoomStatsAndRole()
		{
			this.statsAndRoleDirty = false;
			if (!this.TouchesMapEdge && this.RegionType == RegionType.Normal && this.regions.Count <= 36)
			{
				if (this.stats == null)
				{
					this.stats = new DefMap<RoomStatDef, float>();
				}
				foreach (RoomStatDef roomStatDef in from x in DefDatabase<RoomStatDef>.AllDefs
				orderby x.updatePriority descending
				select x)
				{
					this.stats[roomStatDef] = roomStatDef.Worker.GetScore(this);
				}
				this.role = DefDatabase<RoomRoleDef>.AllDefs.MaxBy((RoomRoleDef x) => x.Worker.GetScore(this));
			}
			else
			{
				this.stats = null;
				this.role = RoomRoleDefOf.None;
			}
		}

		internal void DebugDraw()
		{
			int hashCode = this.GetHashCode();
			foreach (IntVec3 c in this.Cells)
			{
				CellRenderer.RenderCell(c, (float)hashCode * 0.01f);
			}
		}

		internal string DebugString()
		{
			return string.Concat(new object[]
			{
				"Room ID=",
				this.ID,
				"\n  first cell=",
				this.Cells.FirstOrDefault<IntVec3>(),
				"\n  RegionCount=",
				this.RegionCount,
				"\n  RegionType=",
				this.RegionType,
				"\n  CellCount=",
				this.CellCount,
				"\n  OpenRoofCount=",
				this.OpenRoofCount,
				"\n  numRegionsTouchingMapEdge=",
				this.numRegionsTouchingMapEdge,
				"\n  lastChangeTick=",
				this.lastChangeTick,
				"\n  isPrisonCell=",
				this.isPrisonCell,
				"\n  RoomGroup=",
				(this.Group == null) ? "null" : this.Group.ID.ToString()
			});
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Room(roomID=",
				this.ID,
				", first=",
				this.Cells.FirstOrDefault<IntVec3>().ToString(),
				", RegionsCount=",
				this.RegionCount.ToString(),
				", lastChangeTick=",
				this.lastChangeTick,
				")"
			});
		}

		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.ID, 1538478890);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static Room()
		{
		}

		[CompilerGenerated]
		private static float <UpdateRoomStatsAndRole>m__0(RoomStatDef x)
		{
			return x.updatePriority;
		}

		[CompilerGenerated]
		private float <UpdateRoomStatsAndRole>m__1(RoomRoleDef x)
		{
			return x.Worker.GetScore(this);
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal int <i>__1;

			internal IEnumerator<IntVec3> $locvar0;

			internal IntVec3 <c>__2;

			internal Room $this;

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
				bool flag = false;
				switch (num)
				{
				case 0u:
					i = 0;
					break;
				case 1u:
					Block_2:
					try
					{
						switch (num)
						{
						}
						if (enumerator.MoveNext())
						{
							c = enumerator.Current;
							this.$current = c;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator != null)
							{
								enumerator.Dispose();
							}
						}
					}
					i++;
					break;
				default:
					return false;
				}
				if (i < this.regions.Count)
				{
					enumerator = this.regions[i].Cells.GetEnumerator();
					num = 4294967293u;
					goto Block_2;
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
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
				Room.<>c__Iterator0 <>c__Iterator = new Room.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator1 : IEnumerable, IEnumerable<IntVec3>, IEnumerator, IDisposable, IEnumerator<IntVec3>
		{
			internal IEnumerator<IntVec3> $locvar0;

			internal IntVec3 <c>__1;

			internal int <i>__2;

			internal IntVec3 <prospective>__3;

			internal Region <region>__3;

			internal Room $this;

			internal IntVec3 $current;

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
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.Cells.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						IL_10E:
						i++;
						break;
					default:
						goto IL_12A;
					}
					IL_11D:
					if (i < 8)
					{
						prospective = c + GenAdj.AdjacentCells[i];
						region = (c + GenAdj.AdjacentCells[i]).GetRegion(base.Map, RegionType.Set_Passable);
						if (region == null || region.Room != this)
						{
							this.$current = prospective;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_10E;
					}
					IL_12A:
					if (enumerator.MoveNext())
					{
						c = enumerator.Current;
						i = 0;
						goto IL_11D;
					}
				}
				finally
				{
					if (!flag)
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
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
				Room.<>c__Iterator1 <>c__Iterator = new Room.<>c__Iterator1();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator2 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal Pawn <firstOwner>__0;

			internal Pawn <secondOwner>__0;

			internal IEnumerator<Building_Bed> $locvar0;

			internal Room $this;

			internal Pawn $current;

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
					if (base.TouchesMapEdge)
					{
						return false;
					}
					if (base.IsHuge)
					{
						return false;
					}
					if (base.Role != RoomRoleDefOf.Bedroom && base.Role != RoomRoleDefOf.PrisonCell && base.Role != RoomRoleDefOf.Barracks && base.Role != RoomRoleDefOf.PrisonBarracks)
					{
						return false;
					}
					firstOwner = null;
					secondOwner = null;
					enumerator = base.ContainedBeds.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							Building_Bed building_Bed = enumerator.Current;
							if (building_Bed.def.building.bed_humanlike)
							{
								for (int i = 0; i < building_Bed.owners.Count; i++)
								{
									if (firstOwner == null)
									{
										firstOwner = building_Bed.owners[i];
									}
									else
									{
										if (secondOwner != null)
										{
											return false;
										}
										secondOwner = building_Bed.owners[i];
									}
								}
							}
						}
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					if (firstOwner != null)
					{
						if (secondOwner == null)
						{
							this.$current = firstOwner;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							return true;
						}
						if (LovePartnerRelationUtility.LovePartnerRelationExists(firstOwner, secondOwner))
						{
							this.$current = firstOwner;
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
							return true;
						}
					}
					break;
				case 1u:
					break;
				case 2u:
					this.$current = secondOwner;
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				case 3u:
					break;
				default:
					return false;
				}
				this.$PC = -1;
				return false;
			}

			Pawn IEnumerator<Pawn>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.Pawn>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Room.<>c__Iterator2 <>c__Iterator = new Room.<>c__Iterator2();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator3 : IEnumerable, IEnumerable<Building_Bed>, IEnumerator, IDisposable, IEnumerator<Building_Bed>
		{
			internal List<Thing> <things>__0;

			internal int <i>__1;

			internal Building_Bed <bed>__2;

			internal Room $this;

			internal Building_Bed $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator3()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					things = base.ContainedAndAdjacentThings;
					i = 0;
					break;
				case 1u:
					IL_87:
					i++;
					break;
				default:
					return false;
				}
				if (i >= things.Count)
				{
					this.$PC = -1;
				}
				else
				{
					bed = (things[i] as Building_Bed);
					if (bed != null)
					{
						this.$current = bed;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_87;
				}
				return false;
			}

			Building_Bed IEnumerator<Building_Bed>.Current
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.Building_Bed>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Building_Bed> IEnumerable<Building_Bed>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				Room.<>c__Iterator3 <>c__Iterator = new Room.<>c__Iterator3();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}
	}
}
