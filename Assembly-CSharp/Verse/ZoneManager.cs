using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CBA RID: 3258
	public sealed class ZoneManager : IExposable
	{
		// Token: 0x040030C3 RID: 12483
		public Map map;

		// Token: 0x040030C4 RID: 12484
		private List<Zone> allZones = new List<Zone>();

		// Token: 0x040030C5 RID: 12485
		private Zone[] zoneGrid;

		// Token: 0x060047E2 RID: 18402 RVA: 0x0025D7E0 File Offset: 0x0025BBE0
		public ZoneManager(Map map)
		{
			this.map = map;
			this.zoneGrid = new Zone[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x060047E3 RID: 18403 RVA: 0x0025D814 File Offset: 0x0025BC14
		public List<Zone> AllZones
		{
			get
			{
				return this.allZones;
			}
		}

		// Token: 0x060047E4 RID: 18404 RVA: 0x0025D82F File Offset: 0x0025BC2F
		public void ExposeData()
		{
			Scribe_Collections.Look<Zone>(ref this.allZones, "allZones", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateZoneManagerLinks();
				this.RebuildZoneGrid();
			}
		}

		// Token: 0x060047E5 RID: 18405 RVA: 0x0025D864 File Offset: 0x0025BC64
		private void UpdateZoneManagerLinks()
		{
			for (int i = 0; i < this.allZones.Count; i++)
			{
				this.allZones[i].zoneManager = this;
			}
		}

		// Token: 0x060047E6 RID: 18406 RVA: 0x0025D8A4 File Offset: 0x0025BCA4
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

		// Token: 0x060047E7 RID: 18407 RVA: 0x0025D970 File Offset: 0x0025BD70
		public void RegisterZone(Zone newZone)
		{
			this.allZones.Add(newZone);
			newZone.PostRegister();
		}

		// Token: 0x060047E8 RID: 18408 RVA: 0x0025D985 File Offset: 0x0025BD85
		public void DeregisterZone(Zone oldZone)
		{
			this.allZones.Remove(oldZone);
			oldZone.PostDeregister();
		}

		// Token: 0x060047E9 RID: 18409 RVA: 0x0025D99B File Offset: 0x0025BD9B
		internal void AddZoneGridCell(Zone zone, IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = zone;
		}

		// Token: 0x060047EA RID: 18410 RVA: 0x0025D9B7 File Offset: 0x0025BDB7
		internal void ClearZoneGridCell(IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = null;
		}

		// Token: 0x060047EB RID: 18411 RVA: 0x0025D9D4 File Offset: 0x0025BDD4
		public Zone ZoneAt(IntVec3 c)
		{
			return this.zoneGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x060047EC RID: 18412 RVA: 0x0025DA04 File Offset: 0x0025BE04
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

		// Token: 0x060047ED RID: 18413 RVA: 0x0025DA88 File Offset: 0x0025BE88
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
	}
}
