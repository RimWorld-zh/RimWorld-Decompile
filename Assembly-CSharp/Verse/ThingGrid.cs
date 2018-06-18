using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C28 RID: 3112
	public sealed class ThingGrid
	{
		// Token: 0x0600444A RID: 17482 RVA: 0x0023E034 File Offset: 0x0023C434
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

		// Token: 0x0600444B RID: 17483 RVA: 0x0023E090 File Offset: 0x0023C490
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

		// Token: 0x0600444C RID: 17484 RVA: 0x0023E138 File Offset: 0x0023C538
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

		// Token: 0x0600444D RID: 17485 RVA: 0x0023E1B0 File Offset: 0x0023C5B0
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

		// Token: 0x0600444E RID: 17486 RVA: 0x0023E26C File Offset: 0x0023C66C
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

		// Token: 0x0600444F RID: 17487 RVA: 0x0023E2DC File Offset: 0x0023C6DC
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

		// Token: 0x06004450 RID: 17488 RVA: 0x0023E310 File Offset: 0x0023C710
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

		// Token: 0x06004451 RID: 17489 RVA: 0x0023E378 File Offset: 0x0023C778
		public List<Thing> ThingsListAtFast(IntVec3 c)
		{
			return this.thingGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06004452 RID: 17490 RVA: 0x0023E3A8 File Offset: 0x0023C7A8
		public List<Thing> ThingsListAtFast(int index)
		{
			return this.thingGrid[index];
		}

		// Token: 0x06004453 RID: 17491 RVA: 0x0023E3C8 File Offset: 0x0023C7C8
		public bool CellContains(IntVec3 c, ThingCategory cat)
		{
			return this.ThingAt(c, cat) != null;
		}

		// Token: 0x06004454 RID: 17492 RVA: 0x0023E3EC File Offset: 0x0023C7EC
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

		// Token: 0x06004455 RID: 17493 RVA: 0x0023E470 File Offset: 0x0023C870
		public bool CellContains(IntVec3 c, ThingDef def)
		{
			return this.ThingAt(c, def) != null;
		}

		// Token: 0x06004456 RID: 17494 RVA: 0x0023E494 File Offset: 0x0023C894
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

		// Token: 0x06004457 RID: 17495 RVA: 0x0023E514 File Offset: 0x0023C914
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

		// Token: 0x04002E64 RID: 11876
		private Map map;

		// Token: 0x04002E65 RID: 11877
		private List<Thing>[] thingGrid;

		// Token: 0x04002E66 RID: 11878
		private static readonly List<Thing> EmptyThingList = new List<Thing>();
	}
}
