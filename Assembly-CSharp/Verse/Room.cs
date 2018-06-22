using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C96 RID: 3222
	public sealed class Room
	{
		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x060046BF RID: 18111 RVA: 0x00255278 File Offset: 0x00253678
		public Map Map
		{
			get
			{
				return ((int)this.mapIndex >= 0) ? Find.Maps[(int)this.mapIndex] : null;
			}
		}

		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x060046C0 RID: 18112 RVA: 0x002552B4 File Offset: 0x002536B4
		public RegionType RegionType
		{
			get
			{
				return (!this.regions.Any<Region>()) ? RegionType.None : this.regions[0].type;
			}
		}

		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x060046C1 RID: 18113 RVA: 0x002552F0 File Offset: 0x002536F0
		public List<Region> Regions
		{
			get
			{
				return this.regions;
			}
		}

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x060046C2 RID: 18114 RVA: 0x0025530C File Offset: 0x0025370C
		public int RegionCount
		{
			get
			{
				return this.regions.Count;
			}
		}

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x060046C3 RID: 18115 RVA: 0x0025532C File Offset: 0x0025372C
		public bool IsHuge
		{
			get
			{
				return this.regions.Count > 60;
			}
		}

		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x060046C4 RID: 18116 RVA: 0x00255350 File Offset: 0x00253750
		public bool Dereferenced
		{
			get
			{
				return this.regions.Count == 0;
			}
		}

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x060046C5 RID: 18117 RVA: 0x00255374 File Offset: 0x00253774
		public bool TouchesMapEdge
		{
			get
			{
				return this.numRegionsTouchingMapEdge > 0;
			}
		}

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x060046C6 RID: 18118 RVA: 0x00255394 File Offset: 0x00253794
		public float Temperature
		{
			get
			{
				return this.Group.Temperature;
			}
		}

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x060046C7 RID: 18119 RVA: 0x002553B4 File Offset: 0x002537B4
		public bool UsesOutdoorTemperature
		{
			get
			{
				return this.Group.UsesOutdoorTemperature;
			}
		}

		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x060046C8 RID: 18120 RVA: 0x002553D4 File Offset: 0x002537D4
		// (set) Token: 0x060046C9 RID: 18121 RVA: 0x002553F0 File Offset: 0x002537F0
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

		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x060046CA RID: 18122 RVA: 0x00255444 File Offset: 0x00253844
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

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x060046CB RID: 18123 RVA: 0x002554B0 File Offset: 0x002538B0
		public int OpenRoofCount
		{
			get
			{
				return this.OpenRoofCountStopAt(int.MaxValue);
			}
		}

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x060046CC RID: 18124 RVA: 0x002554D0 File Offset: 0x002538D0
		public bool PsychologicallyOutdoors
		{
			get
			{
				return this.OpenRoofCountStopAt(300) >= 300 || (this.Group.AnyRoomTouchesMapEdge && (float)this.OpenRoofCount / (float)this.CellCount >= 0.5f);
			}
		}

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x060046CD RID: 18125 RVA: 0x00255534 File Offset: 0x00253934
		public bool OutdoorsForWork
		{
			get
			{
				return this.OpenRoofCountStopAt(101) > 100 || (float)this.OpenRoofCount > (float)this.CellCount * 0.25f;
			}
		}

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x060046CE RID: 18126 RVA: 0x0025557C File Offset: 0x0025397C
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

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x060046CF RID: 18127 RVA: 0x0025565C File Offset: 0x00253A5C
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

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x060046D0 RID: 18128 RVA: 0x00255688 File Offset: 0x00253A88
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

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x060046D1 RID: 18129 RVA: 0x002556B4 File Offset: 0x00253AB4
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

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x060046D2 RID: 18130 RVA: 0x002556E0 File Offset: 0x00253AE0
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

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x060046D3 RID: 18131 RVA: 0x0025570C File Offset: 0x00253B0C
		public bool Fogged
		{
			get
			{
				return this.regions.Count != 0 && this.regions[0].AnyCell.Fogged(this.Map);
			}
		}

		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x060046D4 RID: 18132 RVA: 0x00255754 File Offset: 0x00253B54
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

		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x060046D5 RID: 18133 RVA: 0x00255810 File Offset: 0x00253C10
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

		// Token: 0x060046D6 RID: 18134 RVA: 0x0025583C File Offset: 0x00253C3C
		public static Room MakeNew(Map map)
		{
			Room room = new Room();
			room.mapIndex = (sbyte)map.Index;
			room.ID = Room.nextRoomID;
			Room.nextRoomID++;
			return room;
		}

		// Token: 0x060046D7 RID: 18135 RVA: 0x0025587C File Offset: 0x00253C7C
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

		// Token: 0x060046D8 RID: 18136 RVA: 0x00255918 File Offset: 0x00253D18
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

		// Token: 0x060046D9 RID: 18137 RVA: 0x002559D1 File Offset: 0x00253DD1
		public void Notify_MyMapRemoved()
		{
			this.mapIndex = -1;
		}

		// Token: 0x060046DA RID: 18138 RVA: 0x002559DC File Offset: 0x00253DDC
		public void Notify_ContainedThingSpawnedOrDespawned(Thing th)
		{
			if (th.def.category != ThingCategory.Mote && th.def.category != ThingCategory.Projectile && th.def.category != ThingCategory.Ethereal && th.def.category != ThingCategory.Pawn)
			{
				this.statsAndRoleDirty = true;
			}
		}

		// Token: 0x060046DB RID: 18139 RVA: 0x00255A39 File Offset: 0x00253E39
		public void Notify_TerrainChanged()
		{
			this.statsAndRoleDirty = true;
		}

		// Token: 0x060046DC RID: 18140 RVA: 0x00255A43 File Offset: 0x00253E43
		public void Notify_BedTypeChanged()
		{
			this.statsAndRoleDirty = true;
		}

		// Token: 0x060046DD RID: 18141 RVA: 0x00255A4D File Offset: 0x00253E4D
		public void Notify_RoofChanged()
		{
			this.cachedOpenRoofCount = -1;
			this.cachedOpenRoofState = null;
			this.Group.Notify_RoofChanged();
		}

		// Token: 0x060046DE RID: 18142 RVA: 0x00255A6C File Offset: 0x00253E6C
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

		// Token: 0x060046DF RID: 18143 RVA: 0x00255BEC File Offset: 0x00253FEC
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

		// Token: 0x060046E0 RID: 18144 RVA: 0x00255C5C File Offset: 0x0025405C
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

		// Token: 0x060046E1 RID: 18145 RVA: 0x00255CA8 File Offset: 0x002540A8
		public RoomStatScoreStage GetStatScoreStage(RoomStatDef stat)
		{
			return stat.GetScoreStage(this.GetStat(stat));
		}

		// Token: 0x060046E2 RID: 18146 RVA: 0x00255CCC File Offset: 0x002540CC
		public void DrawFieldEdges()
		{
			Room.fields.Clear();
			Room.fields.AddRange(this.Cells);
			Color color = (!this.isPrisonCell) ? Room.NonPrisonFieldColor : Room.PrisonFieldColor;
			color.a = Pulser.PulseBrightness(1f, 0.6f);
			GenDraw.DrawFieldEdges(Room.fields, color);
			Room.fields.Clear();
		}

		// Token: 0x060046E3 RID: 18147 RVA: 0x00255D3C File Offset: 0x0025413C
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

		// Token: 0x060046E4 RID: 18148 RVA: 0x00255E08 File Offset: 0x00254208
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

		// Token: 0x060046E5 RID: 18149 RVA: 0x00255F14 File Offset: 0x00254314
		internal void DebugDraw()
		{
			int hashCode = this.GetHashCode();
			foreach (IntVec3 c in this.Cells)
			{
				CellRenderer.RenderCell(c, (float)hashCode * 0.01f);
			}
		}

		// Token: 0x060046E6 RID: 18150 RVA: 0x00255F80 File Offset: 0x00254380
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

		// Token: 0x060046E7 RID: 18151 RVA: 0x002560B0 File Offset: 0x002544B0
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

		// Token: 0x060046E8 RID: 18152 RVA: 0x00256148 File Offset: 0x00254548
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.ID, 1538478890);
		}

		// Token: 0x04003028 RID: 12328
		public sbyte mapIndex = -1;

		// Token: 0x04003029 RID: 12329
		private RoomGroup groupInt;

		// Token: 0x0400302A RID: 12330
		private List<Region> regions = new List<Region>();

		// Token: 0x0400302B RID: 12331
		public int ID = -16161616;

		// Token: 0x0400302C RID: 12332
		public int lastChangeTick = -1;

		// Token: 0x0400302D RID: 12333
		private int numRegionsTouchingMapEdge;

		// Token: 0x0400302E RID: 12334
		private int cachedOpenRoofCount = -1;

		// Token: 0x0400302F RID: 12335
		private IEnumerator<IntVec3> cachedOpenRoofState = null;

		// Token: 0x04003030 RID: 12336
		public bool isPrisonCell;

		// Token: 0x04003031 RID: 12337
		private int cachedCellCount = -1;

		// Token: 0x04003032 RID: 12338
		private bool statsAndRoleDirty = true;

		// Token: 0x04003033 RID: 12339
		private DefMap<RoomStatDef, float> stats = new DefMap<RoomStatDef, float>();

		// Token: 0x04003034 RID: 12340
		private RoomRoleDef role;

		// Token: 0x04003035 RID: 12341
		public int newOrReusedRoomGroupIndex = -1;

		// Token: 0x04003036 RID: 12342
		private static int nextRoomID;

		// Token: 0x04003037 RID: 12343
		private const int RegionCountHuge = 60;

		// Token: 0x04003038 RID: 12344
		private const int MaxRegionsToAssignRoomRole = 36;

		// Token: 0x04003039 RID: 12345
		private static readonly Color PrisonFieldColor = new Color(1f, 0.7f, 0.2f);

		// Token: 0x0400303A RID: 12346
		private static readonly Color NonPrisonFieldColor = new Color(0.3f, 0.3f, 1f);

		// Token: 0x0400303B RID: 12347
		private HashSet<Room> uniqueNeighborsSet = new HashSet<Room>();

		// Token: 0x0400303C RID: 12348
		private List<Room> uniqueNeighbors = new List<Room>();

		// Token: 0x0400303D RID: 12349
		private HashSet<Thing> uniqueContainedThingsSet = new HashSet<Thing>();

		// Token: 0x0400303E RID: 12350
		private List<Thing> uniqueContainedThings = new List<Thing>();

		// Token: 0x0400303F RID: 12351
		private static List<IntVec3> fields = new List<IntVec3>();
	}
}
