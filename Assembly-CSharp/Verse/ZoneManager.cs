using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000CBD RID: 3261
	public sealed class ZoneManager : IExposable
	{
		// Token: 0x040030CA RID: 12490
		public Map map;

		// Token: 0x040030CB RID: 12491
		private List<Zone> allZones = new List<Zone>();

		// Token: 0x040030CC RID: 12492
		private Zone[] zoneGrid;

		// Token: 0x060047E5 RID: 18405 RVA: 0x0025DB9C File Offset: 0x0025BF9C
		public ZoneManager(Map map)
		{
			this.map = map;
			this.zoneGrid = new Zone[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x060047E6 RID: 18406 RVA: 0x0025DBD0 File Offset: 0x0025BFD0
		public List<Zone> AllZones
		{
			get
			{
				return this.allZones;
			}
		}

		// Token: 0x060047E7 RID: 18407 RVA: 0x0025DBEB File Offset: 0x0025BFEB
		public void ExposeData()
		{
			Scribe_Collections.Look<Zone>(ref this.allZones, "allZones", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateZoneManagerLinks();
				this.RebuildZoneGrid();
			}
		}

		// Token: 0x060047E8 RID: 18408 RVA: 0x0025DC20 File Offset: 0x0025C020
		private void UpdateZoneManagerLinks()
		{
			for (int i = 0; i < this.allZones.Count; i++)
			{
				this.allZones[i].zoneManager = this;
			}
		}

		// Token: 0x060047E9 RID: 18409 RVA: 0x0025DC60 File Offset: 0x0025C060
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

		// Token: 0x060047EA RID: 18410 RVA: 0x0025DD2C File Offset: 0x0025C12C
		public void RegisterZone(Zone newZone)
		{
			this.allZones.Add(newZone);
			newZone.PostRegister();
		}

		// Token: 0x060047EB RID: 18411 RVA: 0x0025DD41 File Offset: 0x0025C141
		public void DeregisterZone(Zone oldZone)
		{
			this.allZones.Remove(oldZone);
			oldZone.PostDeregister();
		}

		// Token: 0x060047EC RID: 18412 RVA: 0x0025DD57 File Offset: 0x0025C157
		internal void AddZoneGridCell(Zone zone, IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = zone;
		}

		// Token: 0x060047ED RID: 18413 RVA: 0x0025DD73 File Offset: 0x0025C173
		internal void ClearZoneGridCell(IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = null;
		}

		// Token: 0x060047EE RID: 18414 RVA: 0x0025DD90 File Offset: 0x0025C190
		public Zone ZoneAt(IntVec3 c)
		{
			return this.zoneGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x060047EF RID: 18415 RVA: 0x0025DDC0 File Offset: 0x0025C1C0
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

		// Token: 0x060047F0 RID: 18416 RVA: 0x0025DE44 File Offset: 0x0025C244
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
