using System;
using System.Collections.Generic;

namespace Verse
{
	public sealed class ZoneManager : IExposable
	{
		public Map map;

		private List<Zone> allZones = new List<Zone>();

		private Zone[] zoneGrid;

		public List<Zone> AllZones
		{
			get
			{
				return this.allZones;
			}
		}

		public ZoneManager(Map map)
		{
			this.map = map;
			this.zoneGrid = new Zone[map.cellIndices.NumGridCells];
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<Zone>(ref this.allZones, "allZones", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.UpdateZoneManagerLinks();
				this.RebuildZoneGrid();
			}
		}

		private void UpdateZoneManagerLinks()
		{
			for (int i = 0; i < this.allZones.Count; i++)
			{
				this.allZones[i].zoneManager = this;
			}
		}

		private void RebuildZoneGrid()
		{
			CellIndices cellIndices = this.map.cellIndices;
			this.zoneGrid = new Zone[cellIndices.NumGridCells];
			foreach (Zone allZone in this.allZones)
			{
				foreach (IntVec3 item in allZone)
				{
					this.zoneGrid[cellIndices.CellToIndex(item)] = allZone;
				}
			}
		}

		public void RegisterZone(Zone newZone)
		{
			this.allZones.Add(newZone);
		}

		public void DeregisterZone(Zone oldZone)
		{
			this.allZones.Remove(oldZone);
		}

		internal void AddZoneGridCell(Zone zone, IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = zone;
		}

		internal void ClearZoneGridCell(IntVec3 c)
		{
			this.zoneGrid[this.map.cellIndices.CellToIndex(c)] = null;
		}

		public Zone ZoneAt(IntVec3 c)
		{
			return this.zoneGrid[this.map.cellIndices.CellToIndex(c)];
		}

		public string NewZoneName(string nameBase)
		{
			int num = 1;
			string result;
			while (true)
			{
				if (num <= 1000)
				{
					string cand = nameBase + " " + num;
					if (!this.allZones.Any((Predicate<Zone>)((Zone z) => z.label == cand)))
					{
						result = cand;
						break;
					}
					num++;
					continue;
				}
				Log.Error("Ran out of zone names.");
				result = "Zone X";
				break;
			}
			return result;
		}

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
