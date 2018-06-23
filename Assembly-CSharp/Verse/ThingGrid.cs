using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C25 RID: 3109
	public sealed class ThingGrid
	{
		// Token: 0x04002E6E RID: 11886
		private Map map;

		// Token: 0x04002E6F RID: 11887
		private List<Thing>[] thingGrid;

		// Token: 0x04002E70 RID: 11888
		private static readonly List<Thing> EmptyThingList = new List<Thing>();

		// Token: 0x06004453 RID: 17491 RVA: 0x0023F3FC File Offset: 0x0023D7FC
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

		// Token: 0x06004454 RID: 17492 RVA: 0x0023F458 File Offset: 0x0023D858
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

		// Token: 0x06004455 RID: 17493 RVA: 0x0023F500 File Offset: 0x0023D900
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

		// Token: 0x06004456 RID: 17494 RVA: 0x0023F578 File Offset: 0x0023D978
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

		// Token: 0x06004457 RID: 17495 RVA: 0x0023F634 File Offset: 0x0023DA34
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

		// Token: 0x06004458 RID: 17496 RVA: 0x0023F6A4 File Offset: 0x0023DAA4
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

		// Token: 0x06004459 RID: 17497 RVA: 0x0023F6D8 File Offset: 0x0023DAD8
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

		// Token: 0x0600445A RID: 17498 RVA: 0x0023F740 File Offset: 0x0023DB40
		public List<Thing> ThingsListAtFast(IntVec3 c)
		{
			return this.thingGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x0600445B RID: 17499 RVA: 0x0023F770 File Offset: 0x0023DB70
		public List<Thing> ThingsListAtFast(int index)
		{
			return this.thingGrid[index];
		}

		// Token: 0x0600445C RID: 17500 RVA: 0x0023F790 File Offset: 0x0023DB90
		public bool CellContains(IntVec3 c, ThingCategory cat)
		{
			return this.ThingAt(c, cat) != null;
		}

		// Token: 0x0600445D RID: 17501 RVA: 0x0023F7B4 File Offset: 0x0023DBB4
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

		// Token: 0x0600445E RID: 17502 RVA: 0x0023F838 File Offset: 0x0023DC38
		public bool CellContains(IntVec3 c, ThingDef def)
		{
			return this.ThingAt(c, def) != null;
		}

		// Token: 0x0600445F RID: 17503 RVA: 0x0023F85C File Offset: 0x0023DC5C
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

		// Token: 0x06004460 RID: 17504 RVA: 0x0023F8DC File Offset: 0x0023DCDC
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
	}
}
