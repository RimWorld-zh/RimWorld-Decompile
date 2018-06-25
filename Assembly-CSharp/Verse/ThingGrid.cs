using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

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

		public bool CellContains(IntVec3 c, ThingDef def)
		{
			return this.ThingAt(c, def) != null;
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static ThingGrid()
		{
		}

		[CompilerGenerated]
		private sealed class <ThingsAt>c__Iterator0 : IEnumerable, IEnumerable<Thing>, IEnumerator, IDisposable, IEnumerator<Thing>
		{
			internal IntVec3 c;

			internal List<Thing> <list>__0;

			internal int <i>__1;

			internal ThingGrid $this;

			internal Thing $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <ThingsAt>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (!c.InBounds(this.map))
					{
						return false;
					}
					list = this.thingGrid[this.map.cellIndices.CellToIndex(c)];
					i = 0;
					break;
				case 1u:
					i++;
					break;
				default:
					return false;
				}
				if (i < list.Count)
				{
					this.$current = list[i];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			Thing IEnumerator<Thing>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Thing>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ThingGrid.<ThingsAt>c__Iterator0 <ThingsAt>c__Iterator = new ThingGrid.<ThingsAt>c__Iterator0();
				<ThingsAt>c__Iterator.$this = this;
				<ThingsAt>c__Iterator.c = c;
				return <ThingsAt>c__Iterator;
			}
		}
	}
}
