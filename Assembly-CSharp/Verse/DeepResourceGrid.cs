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
			string compressedString = string.Empty;
			string compressedString2 = string.Empty;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				compressedString = GridSaveUtility.CompressedStringForShortGrid((Func<IntVec3, ushort>)((IntVec3 c) => this.defGrid[this.map.cellIndices.CellToIndex(c)]), this.map);
				compressedString2 = GridSaveUtility.CompressedStringForShortGrid((Func<IntVec3, ushort>)((IntVec3 c) => this.countGrid[this.map.cellIndices.CellToIndex(c)]), this.map);
			}
			Scribe_Values.Look(ref compressedString, "defGrid", (string)null, false);
			Scribe_Values.Look(ref compressedString2, "countGrid", (string)null, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				foreach (GridSaveUtility.LoadedGridShort item in GridSaveUtility.LoadedUShortGrid(compressedString, this.map))
				{
					GridSaveUtility.LoadedGridShort current = item;
					this.defGrid[this.map.cellIndices.CellToIndex(current.cell)] = current.val;
				}
				foreach (GridSaveUtility.LoadedGridShort item2 in GridSaveUtility.LoadedUShortGrid(compressedString2, this.map))
				{
					GridSaveUtility.LoadedGridShort current2 = item2;
					this.countGrid[this.map.cellIndices.CellToIndex(current2.cell)] = current2.val;
				}
			}
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
