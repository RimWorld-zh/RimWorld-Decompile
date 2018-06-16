using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CBE RID: 3262
	public sealed class ZoneManager : IExposable
	{
		// Token: 0x060047DB RID: 18395 RVA: 0x0025C418 File Offset: 0x0025A818
		public ZoneManager(Map map)
		{
			this.map = map;
			this.zoneGrid = new Zone[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x060047DC RID: 18396 RVA: 0x0025C44C File Offset: 0x0025A84C
		public List<Zone> AllZones
		{
			get
			{
				return this.allZones;
			}
		}

		// Token: 0x060047DD RID: 18397 RVA: 0x0025C467 File Offset: 0x0025A867
		public void ExposeData()
		{
			Scribe_Collections.Look<Zone>(ref this.allZones, "allZones", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateZoneManagerLinks();
				this.RebuildZoneGrid();
			}
		}

		// Token: 0x060047DE RID: 18398 RVA: 0x0025C49C File Offset: 0x0025A89C
		private void UpdateZoneManagerLinks()
		{
			for (int i = 0; i < this.allZones.Count; i++)
			{
				this.allZones[i].zoneManager = this;
			}
		}

		// Token: 0x060047DF RID: 18399 RVA: 0x0025C4DC File Offset: 0x0025A8DC
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

		// Token: 0x060047E0 RID: 18400 RVA: 0x0025C5A8 File Offset: 0x0025A9A8
		public void RegisterZone(Zone newZone)
		{
			this.allZones.Add(newZone);
			newZone.PostRegister();
		}

		// Token: 0x060047E1 RID: 18401 RVA: 0x0025C5BD File Offset: 0x0025A9BD
		public void DeregisterZone(Zone oldZone)
		{
			this.allZones.Remove(oldZone);
			oldZone.PostDeregister();
		}

		// Token: 0x060047E2 RID: 18402 RVA: 0x0025C5D3 File Offset: 0x0025A9D3
		internal void AddZoneGridCell(Zone zone, IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = zone;
		}

		// Token: 0x060047E3 RID: 18403 RVA: 0x0025C5EF File Offset: 0x0025A9EF
		internal void ClearZoneGridCell(IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = null;
		}

		// Token: 0x060047E4 RID: 18404 RVA: 0x0025C60C File Offset: 0x0025AA0C
		public Zone ZoneAt(IntVec3 c)
		{
			return this.zoneGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x060047E5 RID: 18405 RVA: 0x0025C63C File Offset: 0x0025AA3C
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

		// Token: 0x060047E6 RID: 18406 RVA: 0x0025C6C0 File Offset: 0x0025AAC0
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

		// Token: 0x040030BA RID: 12474
		public Map map;

		// Token: 0x040030BB RID: 12475
		private List<Zone> allZones = new List<Zone>();

		// Token: 0x040030BC RID: 12476
		private Zone[] zoneGrid;
	}
}
