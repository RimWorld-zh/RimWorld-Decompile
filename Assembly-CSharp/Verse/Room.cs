using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C9A RID: 3226
	public sealed class Room
	{
		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x060046B8 RID: 18104 RVA: 0x00253EB0 File Offset: 0x002522B0
		public Map Map
		{
			get
			{
				return ((int)this.mapIndex >= 0) ? Find.Maps[(int)this.mapIndex] : null;
			}
		}

		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x060046B9 RID: 18105 RVA: 0x00253EEC File Offset: 0x002522EC
		public RegionType RegionType
		{
			get
			{
				return (!this.regions.Any<Region>()) ? RegionType.None : this.regions[0].type;
			}
		}

		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x060046BA RID: 18106 RVA: 0x00253F28 File Offset: 0x00252328
		public List<Region> Regions
		{
			get
			{
				return this.regions;
			}
		}

		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x060046BB RID: 18107 RVA: 0x00253F44 File Offset: 0x00252344
		public int RegionCount
		{
			get
			{
				return this.regions.Count;
			}
		}

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x060046BC RID: 18108 RVA: 0x00253F64 File Offset: 0x00252364
		public bool IsHuge
		{
			get
			{
				return this.regions.Count > 60;
			}
		}

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x060046BD RID: 18109 RVA: 0x00253F88 File Offset: 0x00252388
		public bool Dereferenced
		{
			get
			{
				return this.regions.Count == 0;
			}
		}

		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x060046BE RID: 18110 RVA: 0x00253FAC File Offset: 0x002523AC
		public bool TouchesMapEdge
		{
			get
			{
				return this.numRegionsTouchingMapEdge > 0;
			}
		}

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x060046BF RID: 18111 RVA: 0x00253FCC File Offset: 0x002523CC
		public float Temperature
		{
			get
			{
				return this.Group.Temperature;
			}
		}

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x060046C0 RID: 18112 RVA: 0x00253FEC File Offset: 0x002523EC
		public bool UsesOutdoorTemperature
		{
			get
			{
				return this.Group.UsesOutdoorTemperature;
			}
		}

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x060046C1 RID: 18113 RVA: 0x0025400C File Offset: 0x0025240C
		// (set) Token: 0x060046C2 RID: 18114 RVA: 0x00254028 File Offset: 0x00252428
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

		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x060046C3 RID: 18115 RVA: 0x0025407C File Offset: 0x0025247C
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

		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x060046C4 RID: 18116 RVA: 0x002540E8 File Offset: 0x002524E8
		public int OpenRoofCount
		{
			get
			{
				return this.OpenRoofCountStopAt(int.MaxValue);
			}
		}

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x060046C5 RID: 18117 RVA: 0x00254108 File Offset: 0x00252508
		public bool PsychologicallyOutdoors
		{
			get
			{
				return this.OpenRoofCountStopAt(300) >= 300 || (this.Group.AnyRoomTouchesMapEdge && (float)this.OpenRoofCount / (float)this.CellCount >= 0.5f);
			}
		}

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x060046C6 RID: 18118 RVA: 0x0025416C File Offset: 0x0025256C
		public bool OutdoorsForWork
		{
			get
			{
				return this.OpenRoofCountStopAt(101) > 100 || (float)this.OpenRoofCount > (float)this.CellCount * 0.25f;
			}
		}

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x060046C7 RID: 18119 RVA: 0x002541B4 File Offset: 0x002525B4
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

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x060046C8 RID: 18120 RVA: 0x00254294 File Offset: 0x00252694
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

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x060046C9 RID: 18121 RVA: 0x002542C0 File Offset: 0x002526C0
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

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x060046CA RID: 18122 RVA: 0x002542EC File Offset: 0x002526EC
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

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x060046CB RID: 18123 RVA: 0x00254318 File Offset: 0x00252718
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

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x060046CC RID: 18124 RVA: 0x00254344 File Offset: 0x00252744
		public bool Fogged
		{
			get
			{
				return this.regions.Count != 0 && this.regions[0].AnyCell.Fogged(this.Map);
			}
		}

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x060046CD RID: 18125 RVA: 0x0025438C File Offset: 0x0025278C
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

		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x060046CE RID: 18126 RVA: 0x00254448 File Offset: 0x00252848
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

		// Token: 0x060046CF RID: 18127 RVA: 0x00254474 File Offset: 0x00252874
		public static Room MakeNew(Map map)
		{
			Room room = new Room();
			room.mapIndex = (sbyte)map.Index;
			room.ID = Room.nextRoomID;
			Room.nextRoomID++;
			return room;
		}

		// Token: 0x060046D0 RID: 18128 RVA: 0x002544B4 File Offset: 0x002528B4
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

		// Token: 0x060046D1 RID: 18129 RVA: 0x00254550 File Offset: 0x00252950
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

		// Token: 0x060046D2 RID: 18130 RVA: 0x00254609 File Offset: 0x00252A09
		public void Notify_MyMapRemoved()
		{
			this.mapIndex = -1;
		}

		// Token: 0x060046D3 RID: 18131 RVA: 0x00254614 File Offset: 0x00252A14
		public void Notify_ContainedThingSpawnedOrDespawned(Thing th)
		{
			if (th.def.category != ThingCategory.Mote && th.def.category != ThingCategory.Projectile && th.def.category != ThingCategory.Ethereal && th.def.category != ThingCategory.Pawn)
			{
				this.statsAndRoleDirty = true;
			}
		}

		// Token: 0x060046D4 RID: 18132 RVA: 0x00254671 File Offset: 0x00252A71
		public void Notify_TerrainChanged()
		{
			this.statsAndRoleDirty = true;
		}

		// Token: 0x060046D5 RID: 18133 RVA: 0x0025467B File Offset: 0x00252A7B
		public void Notify_BedTypeChanged()
		{
			this.statsAndRoleDirty = true;
		}

		// Token: 0x060046D6 RID: 18134 RVA: 0x00254685 File Offset: 0x00252A85
		public void Notify_RoofChanged()
		{
			this.cachedOpenRoofCount = -1;
			this.cachedOpenRoofState = null;
			this.Group.Notify_RoofChanged();
		}

		// Token: 0x060046D7 RID: 18135 RVA: 0x002546A4 File Offset: 0x00252AA4
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

		// Token: 0x060046D8 RID: 18136 RVA: 0x00254824 File Offset: 0x00252C24
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

		// Token: 0x060046D9 RID: 18137 RVA: 0x00254894 File Offset: 0x00252C94
		public float GetStat(RoomStatDef roomStat)
		{
			if (this.statsAndRoleDirty)
			{
				this.UpdateRoomStatsAndRole();
			}
			float result;
			if (this.stats == null)
			{
				result = roomStat.defaultScore;
			}
			else
			{
				result = this.stats[roomStat];
			}
			return result;
		}

		// Token: 0x060046DA RID: 18138 RVA: 0x002548E0 File Offset: 0x00252CE0
		public RoomStatScoreStage GetStatScoreStage(RoomStatDef stat)
		{
			return stat.GetScoreStage(this.GetStat(stat));
		}

		// Token: 0x060046DB RID: 18139 RVA: 0x00254904 File Offset: 0x00252D04
		public void DrawFieldEdges()
		{
			Room.fields.Clear();
			Room.fields.AddRange(this.Cells);
			Color color = (!this.isPrisonCell) ? Room.NonPrisonFieldColor : Room.PrisonFieldColor;
			color.a = Pulser.PulseBrightness(1f, 0.6f);
			GenDraw.DrawFieldEdges(Room.fields, color);
			Room.fields.Clear();
		}

		// Token: 0x060046DC RID: 18140 RVA: 0x00254974 File Offset: 0x00252D74
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

		// Token: 0x060046DD RID: 18141 RVA: 0x00254A40 File Offset: 0x00252E40
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

		// Token: 0x060046DE RID: 18142 RVA: 0x00254B4C File Offset: 0x00252F4C
		internal void DebugDraw()
		{
			int hashCode = this.GetHashCode();
			foreach (IntVec3 c in this.Cells)
			{
				CellRenderer.RenderCell(c, (float)hashCode * 0.01f);
			}
		}

		// Token: 0x060046DF RID: 18143 RVA: 0x00254BB8 File Offset: 0x00252FB8
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

		// Token: 0x060046E0 RID: 18144 RVA: 0x00254CE8 File Offset: 0x002530E8
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

		// Token: 0x060046E1 RID: 18145 RVA: 0x00254D80 File Offset: 0x00253180
		public override int GetHashCode()
		{
			return Gen.HashCombineInt(this.ID, 1538478890);
		}

		// Token: 0x0400301F RID: 12319
		public sbyte mapIndex = -1;

		// Token: 0x04003020 RID: 12320
		private RoomGroup groupInt;

		// Token: 0x04003021 RID: 12321
		private List<Region> regions = new List<Region>();

		// Token: 0x04003022 RID: 12322
		public int ID = -16161616;

		// Token: 0x04003023 RID: 12323
		public int lastChangeTick = -1;

		// Token: 0x04003024 RID: 12324
		private int numRegionsTouchingMapEdge;

		// Token: 0x04003025 RID: 12325
		private int cachedOpenRoofCount = -1;

		// Token: 0x04003026 RID: 12326
		private IEnumerator<IntVec3> cachedOpenRoofState = null;

		// Token: 0x04003027 RID: 12327
		public bool isPrisonCell;

		// Token: 0x04003028 RID: 12328
		private int cachedCellCount = -1;

		// Token: 0x04003029 RID: 12329
		private bool statsAndRoleDirty = true;

		// Token: 0x0400302A RID: 12330
		private DefMap<RoomStatDef, float> stats = new DefMap<RoomStatDef, float>();

		// Token: 0x0400302B RID: 12331
		private RoomRoleDef role;

		// Token: 0x0400302C RID: 12332
		public int newOrReusedRoomGroupIndex = -1;

		// Token: 0x0400302D RID: 12333
		private static int nextRoomID;

		// Token: 0x0400302E RID: 12334
		private const int RegionCountHuge = 60;

		// Token: 0x0400302F RID: 12335
		private const int MaxRegionsToAssignRoomRole = 36;

		// Token: 0x04003030 RID: 12336
		private static readonly Color PrisonFieldColor = new Color(1f, 0.7f, 0.2f);

		// Token: 0x04003031 RID: 12337
		private static readonly Color NonPrisonFieldColor = new Color(0.3f, 0.3f, 1f);

		// Token: 0x04003032 RID: 12338
		private HashSet<Room> uniqueNeighborsSet = new HashSet<Room>();

		// Token: 0x04003033 RID: 12339
		private List<Room> uniqueNeighbors = new List<Room>();

		// Token: 0x04003034 RID: 12340
		private HashSet<Thing> uniqueContainedThingsSet = new HashSet<Thing>();

		// Token: 0x04003035 RID: 12341
		private List<Thing> uniqueContainedThings = new List<Thing>();

		// Token: 0x04003036 RID: 12342
		private static List<IntVec3> fields = new List<IntVec3>();
	}
}
