using System;
using UnityEngine;

namespace Verse
{
	public sealed class DeepResourceGrid : ICellBoolGiver, IExposable
	{
		private Map map;

		private CellBoolDrawer drawer;

		private ushort[] defGrid;

		private ushort[] countGrid;

		public Color Color
		{
			get
			{
				return Color.white;
			}
		}

		public DeepResourceGrid(Map map)
		{
			this.map = map;
			this.defGrid = new ushort[map.cellIndices.NumGridCells];
			this.countGrid = new ushort[map.cellIndices.NumGridCells];
			IntVec3 size = map.Size;
			int x = size.x;
			IntVec3 size2 = map.Size;
			this.drawer = new CellBoolDrawer(this, x, size2.z, 1f);
		}

		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, (Func<IntVec3, ushort>)((IntVec3 c) => this.defGrid[this.map.cellIndices.CellToIndex(c)]), (Action<IntVec3, ushort>)delegate(IntVec3 c, ushort val)
			{
				this.defGrid[this.map.cellIndices.CellToIndex(c)] = val;
			}, "defGrid");
			MapExposeUtility.ExposeUshort(this.map, (Func<IntVec3, ushort>)((IntVec3 c) => this.countGrid[this.map.cellIndices.CellToIndex(c)]), (Action<IntVec3, ushort>)delegate(IntVec3 c, ushort val)
			{
				this.countGrid[this.map.cellIndices.CellToIndex(c)] = val;
			}, "countGrid");
		}

		public ThingDef ThingDefAt(IntVec3 c)
		{
			return DefDatabase<ThingDef>.GetByShortHash(this.defGrid[this.map.cellIndices.CellToIndex(c)]);
		}

		public int CountAt(IntVec3 c)
		{
			return this.countGrid[this.map.cellIndices.CellToIndex(c)];
		}

		public void SetAt(IntVec3 c, ThingDef def, int count)
		{
			if (count == 0)
			{
				def = null;
			}
			ushort num = (ushort)((def != null) ? def.shortHash : 0);
			ushort num2 = (ushort)count;
			if (count > 65535)
			{
				Log.Error("Cannot store count " + count + " in DeepResourceGrid: out of ushort range.");
				num2 = (ushort)65535;
			}
			if (count < 0)
			{
				Log.Error("Cannot store count " + count + " in DeepResourceGrid: out of ushort range.");
				num2 = (ushort)0;
			}
			int num3 = this.map.cellIndices.CellToIndex(c);
			if (this.defGrid[num3] == num && this.countGrid[num3] == num2)
				return;
			this.defGrid[num3] = num;
			this.countGrid[num3] = num2;
			this.drawer.SetDirty();
		}

		public void DeepResourceGridUpdate()
		{
			this.drawer.CellBoolDrawerUpdate();
			if (DebugViewSettings.drawDeepResources)
			{
				this.MarkForDraw();
			}
		}

		public void MarkForDraw()
		{
			if (this.map == Find.VisibleMap)
			{
				this.drawer.MarkForDraw();
			}
		}

		public bool GetCellBool(int index)
		{
			return this.CountAt(this.map.cellIndices.IndexToCell(index)) > 0;
		}

		public Color GetCellExtraColor(int index)
		{
			IntVec3 c = this.map.cellIndices.IndexToCell(index);
			int num = this.CountAt(c);
			ThingDef thingDef = this.ThingDefAt(c);
			float num2 = (float)((float)num / (float)thingDef.deepCountPerCell / 2.0);
			int num3 = Mathf.RoundToInt((float)(num2 * 100.0));
			num3 %= 100;
			return DebugMatsSpectrum.Mat(num3, true).color;
		}
	}
}
