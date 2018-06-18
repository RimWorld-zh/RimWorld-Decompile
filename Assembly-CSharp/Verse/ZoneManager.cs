using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CBD RID: 3261
	public sealed class ZoneManager : IExposable
	{
		// Token: 0x060047D9 RID: 18393 RVA: 0x0025C3F0 File Offset: 0x0025A7F0
		public ZoneManager(Map map)
		{
			this.map = map;
			this.zoneGrid = new Zone[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x060047DA RID: 18394 RVA: 0x0025C424 File Offset: 0x0025A824
		public List<Zone> AllZones
		{
			get
			{
				return this.allZones;
			}
		}

		// Token: 0x060047DB RID: 18395 RVA: 0x0025C43F File Offset: 0x0025A83F
		public void ExposeData()
		{
			Scribe_Collections.Look<Zone>(ref this.allZones, "allZones", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateZoneManagerLinks();
				this.RebuildZoneGrid();
			}
		}

		// Token: 0x060047DC RID: 18396 RVA: 0x0025C474 File Offset: 0x0025A874
		private void UpdateZoneManagerLinks()
		{
			for (int i = 0; i < this.allZones.Count; i++)
			{
				this.allZones[i].zoneManager = this;
			}
		}

		// Token: 0x060047DD RID: 18397 RVA: 0x0025C4B4 File Offset: 0x0025A8B4
		private void RebuildZoneGrid()
		{
			CellIndices cellIndices = this.map.cellIndices;
			this.zoneGrid = new Zone[cellIndices.NumGridCells];
			foreach (Zone zone in this.allZones)
			{
				foreach (IntVec3 c in zone)
				{
					this.zoneGrid[cellIndices.CellToIndex(c)] = zone;
				}
			}
		}

		// Token: 0x060047DE RID: 18398 RVA: 0x0025C580 File Offset: 0x0025A980
		public void RegisterZone(Zone newZone)
		{
			this.allZones.Add(newZone);
			newZone.PostRegister();
		}

		// Token: 0x060047DF RID: 18399 RVA: 0x0025C595 File Offset: 0x0025A995
		public void DeregisterZone(Zone oldZone)
		{
			this.allZones.Remove(oldZone);
			oldZone.PostDeregister();
		}

		// Token: 0x060047E0 RID: 18400 RVA: 0x0025C5AB File Offset: 0x0025A9AB
		internal void AddZoneGridCell(Zone zone, IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = zone;
		}

		// Token: 0x060047E1 RID: 18401 RVA: 0x0025C5C7 File Offset: 0x0025A9C7
		internal void ClearZoneGridCell(IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = null;
		}

		// Token: 0x060047E2 RID: 18402 RVA: 0x0025C5E4 File Offset: 0x0025A9E4
		public Zone ZoneAt(IntVec3 c)
		{
			return this.zoneGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x060047E3 RID: 18403 RVA: 0x0025C614 File Offset: 0x0025AA14
		public string NewZoneName(string nameBase)
		{
			for (int i = 1; i <= 1000; i++)
			{
				string cand = nameBase + " " + i;
				if (!this.allZones.Any((Zone z) => z.label == cand))
				{
					return cand;
				}
			}
			Log.Error("Ran out of zone names.", false);
			return "Zone X";
		}

		// Token: 0x060047E4 RID: 18404 RVA: 0x0025C698 File Offset: 0x0025AA98
		internal void Notify_NoZoneOverlapThingSpawned(Thing thing)
		{
			CellRect cellRect = thing.OccupiedRect();
			for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
			{
				for (int j = cellRect.minX; j <= cellRect.maxX; j++)
				{
					IntVec3 c = new IntVec3(j, 0, i);
					Zone zone = this.ZoneAt(c);
					if (zone != null)
					{
						zone.RemoveCell(c);
						zone.CheckContiguous();
					}
				}
			}
		}

		// Token: 0x040030B8 RID: 12472
		public Map map;

		// Token: 0x040030B9 RID: 12473
		private List<Zone> allZones = new List<Zone>();

		// Token: 0x040030BA RID: 12474
		private Zone[] zoneGrid;
	}
}
