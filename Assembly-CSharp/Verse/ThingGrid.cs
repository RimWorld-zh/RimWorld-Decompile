using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C29 RID: 3113
	public sealed class ThingGrid
	{
		// Token: 0x0600444C RID: 17484 RVA: 0x0023E05C File Offset: 0x0023C45C
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

		// Token: 0x0600444D RID: 17485 RVA: 0x0023E0B8 File Offset: 0x0023C4B8
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

		// Token: 0x0600444E RID: 17486 RVA: 0x0023E160 File Offset: 0x0023C560
		private void RegisterInCell(Thing t, IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				Log.Warning(string.Concat(new object[]
				{
					t,
					" tried to register out of bounds at ",
					c,
					". Destroying."
				}), false);
				t.Destroy(DestroyMode.Vanish);
			}
			else
			{
				this.thingGrid[this.map.cellIndices.CellToIndex(c)].Add(t);
			}
		}

		// Token: 0x0600444F RID: 17487 RVA: 0x0023E1D8 File Offset: 0x0023C5D8
		public void Deregister(Thing t, bool doEvenIfDespawned = false)
		{
			if (t.Spawned || doEvenIfDespawned)
			{
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
		}

		// Token: 0x06004450 RID: 17488 RVA: 0x0023E294 File Offset: 0x0023C694
		private void DeregisterInCell(Thing t, IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				Log.Error(t + " tried to de-register out of bounds at " + c, false);
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

		// Token: 0x06004451 RID: 17489 RVA: 0x0023E304 File Offset: 0x0023C704
		public IEnumerable<Thing> ThingsAt(IntVec3 c)
		{
			if (!c.InBounds(this.map))
			{
				yield break;
			}
			List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
			for (int i = 0; i < list.Count; i++)
			{
				yield return list[i];
			}
			yield break;
		}

		// Token: 0x06004452 RID: 17490 RVA: 0x0023E338 File Offset: 0x0023C738
		public List<Thing> ThingsListAt(IntVec3 c)
		{
			List<Thing> result;
			if (!c.InBounds(this.map))
			{
				Log.ErrorOnce("Got ThingsListAt out of bounds: " + c, 495287, false);
				result = ThingGrid.EmptyThingList;
			}
			else
			{
				result = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
			}
			return result;
		}

		// Token: 0x06004453 RID: 17491 RVA: 0x0023E3A0 File Offset: 0x0023C7A0
		public List<Thing> ThingsListAtFast(IntVec3 c)
		{
			return this.thingGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06004454 RID: 17492 RVA: 0x0023E3D0 File Offset: 0x0023C7D0
		public List<Thing> ThingsListAtFast(int index)
		{
			return this.thingGrid[index];
		}

		// Token: 0x06004455 RID: 17493 RVA: 0x0023E3F0 File Offset: 0x0023C7F0
		public bool CellContains(IntVec3 c, ThingCategory cat)
		{
			return this.ThingAt(c, cat) != null;
		}

		// Token: 0x06004456 RID: 17494 RVA: 0x0023E414 File Offset: 0x0023C814
		public Thing ThingAt(IntVec3 c, ThingCategory cat)
		{
			Thing result;
			if (!c.InBounds(this.map))
			{
				result = null;
			}
			else
			{
				List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].def.category == cat)
					{
						return list[i];
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06004457 RID: 17495 RVA: 0x0023E498 File Offset: 0x0023C898
		public bool CellContains(IntVec3 c, ThingDef def)
		{
			return this.ThingAt(c, def) != null;
		}

		// Token: 0x06004458 RID: 17496 RVA: 0x0023E4BC File Offset: 0x0023C8BC
		public Thing ThingAt(IntVec3 c, ThingDef def)
		{
			Thing result;
			if (!c.InBounds(this.map))
			{
				result = null;
			}
			else
			{
				List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].def == def)
					{
						return list[i];
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06004459 RID: 17497 RVA: 0x0023E53C File Offset: 0x0023C93C
		public T ThingAt<T>(IntVec3 c) where T : Thing
		{
			T result;
			if (!c.InBounds(this.map))
			{
				result = (T)((object)null);
			}
			else
			{
				List<Thing> list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
				for (int i = 0; i < list.Count; i++)
				{
					T t = list[i] as T;
					if (t != null)
					{
						return t;
					}
				}
				result = (T)((object)null);
			}
			return result;
		}

		// Token: 0x04002E66 RID: 11878
		private Map map;

		// Token: 0x04002E67 RID: 11879
		private List<Thing>[] thingGrid;

		// Token: 0x04002E68 RID: 11880
		private static readonly List<Thing> EmptyThingList = new List<Thing>();
	}
}
