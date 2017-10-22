using System.Collections.Generic;

namespace Verse
{
	public sealed class ThingGrid
	{
		private Map map;

		private List<Thing>[] thingGrid;

		private static readonly List<Thing> EmptyThingList = new List<Thing>();

		public ThingGrid(Map map)
		{
			this.map = map;
			CellIndices cellIndices = map.cellIndices;
			this.thingGrid = new List<Thing>[cellIndices.NumGridCells];
			for (int i = 0; i < cellIndices.NumGridCells; i++)
			{
				this.thingGrid[i] = new List<Thing>(4);
			}
		}

		public void Register(Thing t)
		{
			if (t.def.size.x == 1 && t.def.size.z == 1)
			{
				this.RegisterInCell(t, t.Position);
			}
			else
			{
				CellRect cellRect = t.OccupiedRect();
				for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
				{
					for (int j = cellRect.minX; j <= cellRect.maxX; j++)
					{
						this.RegisterInCell(t, new IntVec3(j, 0, i));
					}
				}
			}
		}

		private void RegisterInCell(Thing t, IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				Log.Warning(t + " tried to register out of bounds at " + c + ". Destroying.");
				t.Destroy(DestroyMode.Vanish);
			}
			else
			{
				this.thingGrid[this.map.cellIndices.CellToIndex(c)].Add(t);
			}
		}

		public void Deregister(Thing t, bool doEvenIfDespawned = false)
		{
			if (!t.Spawned && !doEvenIfDespawned)
				return;
			if (t.def.size.x == 1 && t.def.size.z == 1)
			{
				this.DeregisterInCell(t, t.Position);
			}
			else
			{
				CellRect cellRect = t.OccupiedRect();
				for (int i = cellRect.minZ; i <= cellRect.maxZ; i++)
				{
					for (int j = cellRect.minX; j <= cellRect.maxX; j++)
					{
						this.DeregisterInCell(t, new IntVec3(j, 0, i));
					}
				}
			}
		}

		private void DeregisterInCell(Thing t, IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				Log.Error(t + " tried to de-register out of bounds at " + c);
			}
			else
			{
				int num = this.map.cellIndices.CellToIndex(c);
				if (this.thingGrid[num].Contains(t))
				{
					this.thingGrid[num].Remove(t);
				}
			}
		}

		public IEnumerable<Thing> ThingsAt(IntVec3 c)
		{
			if (c.InBounds(this.map))
			{
				List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
				int i = 0;
				if (i < list.Count)
				{
					yield return list[i];
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
		}

		public List<Thing> ThingsListAt(IntVec3 c)
		{
			List<Thing> result;
			if (!c.InBounds(this.map))
			{
				Log.ErrorOnce("Got ThingsListAt out of bounds: " + c, 495287);
				result = ThingGrid.EmptyThingList;
			}
			else
			{
				result = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
			}
			return result;
		}

		public List<Thing> ThingsListAtFast(IntVec3 c)
		{
			return this.thingGrid[this.map.cellIndices.CellToIndex(c)];
		}

		public List<Thing> ThingsListAtFast(int index)
		{
			return this.thingGrid[index];
		}

		public bool CellContains(IntVec3 c, ThingCategory cat)
		{
			return this.ThingAt(c, cat) != null;
		}

		public Thing ThingAt(IntVec3 c, ThingCategory cat)
		{
			Thing result;
			List<Thing> list;
			int i;
			if (!c.InBounds(this.map))
			{
				result = null;
			}
			else
			{
				list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
				for (i = 0; i < list.Count; i++)
				{
					if (list[i].def.category == cat)
						goto IL_0051;
				}
				result = null;
			}
			goto IL_0076;
			IL_0051:
			result = list[i];
			goto IL_0076;
			IL_0076:
			return result;
		}

		public bool CellContains(IntVec3 c, ThingDef def)
		{
			return this.ThingAt(c, def) != null;
		}

		public Thing ThingAt(IntVec3 c, ThingDef def)
		{
			Thing result;
			List<Thing> list;
			int i;
			if (!c.InBounds(this.map))
			{
				result = null;
			}
			else
			{
				list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
				for (i = 0; i < list.Count; i++)
				{
					if (list[i].def == def)
						goto IL_004c;
				}
				result = null;
			}
			goto IL_0071;
			IL_004c:
			result = list[i];
			goto IL_0071;
			IL_0071:
			return result;
		}

		public T ThingAt<T>(IntVec3 c) where T : Thing
		{
			T result;
			T val;
			if (!c.InBounds(this.map))
			{
				result = (T)null;
			}
			else
			{
				List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
				for (int i = 0; i < list.Count; i++)
				{
					val = (T)(list[i] as T);
					if (val != null)
						goto IL_005c;
				}
				result = (T)null;
			}
			goto IL_0080;
			IL_005c:
			result = val;
			goto IL_0080;
			IL_0080:
			return result;
		}
	}
}
