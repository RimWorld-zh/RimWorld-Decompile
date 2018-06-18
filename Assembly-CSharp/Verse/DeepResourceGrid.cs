using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C20 RID: 3104
	public sealed class DeepResourceGrid : ICellBoolGiver, IExposable
	{
		// Token: 0x060043DB RID: 17371 RVA: 0x0023BF60 File Offset: 0x0023A360
		public DeepResourceGrid(Map map)
		{
			this.map = map;
			this.defGrid = new ushort[map.cellIndices.NumGridCells];
			this.countGrid = new ushort[map.cellIndices.NumGridCells];
			this.drawer = new CellBoolDrawer(this, map.Size.x, map.Size.z, 1f);
		}

		// Token: 0x17000AA5 RID: 2725
		// (get) Token: 0x060043DC RID: 17372 RVA: 0x0023BFD4 File Offset: 0x0023A3D4
		public Color Color
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x060043DD RID: 17373 RVA: 0x0023BFF0 File Offset: 0x0023A3F0
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => this.defGrid[this.map.cellIndices.CellToIndex(c)], delegate(IntVec3 c, ushort val)
			{
				this.defGrid[this.map.cellIndices.CellToIndex(c)] = val;
			}, "defGrid");
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => this.countGrid[this.map.cellIndices.CellToIndex(c)], delegate(IntVec3 c, ushort val)
			{
				this.countGrid[this.map.cellIndices.CellToIndex(c)] = val;
			}, "countGrid");
		}

		// Token: 0x060043DE RID: 17374 RVA: 0x0023C050 File Offset: 0x0023A450
		public ThingDef ThingDefAt(IntVec3 c)
		{
			return DefDatabase<ThingDef>.GetByShortHash(this.defGrid[this.map.cellIndices.CellToIndex(c)]);
		}

		// Token: 0x060043DF RID: 17375 RVA: 0x0023C084 File Offset: 0x0023A484
		public int CountAt(IntVec3 c)
		{
			return (int)this.countGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x060043E0 RID: 17376 RVA: 0x0023C0B4 File Offset: 0x0023A4B4
		public void SetAt(IntVec3 c, ThingDef def, int count)
		{
			if (count == 0)
			{
				def = null;
			}
			ushort num;
			if (def == null)
			{
				num = 0;
			}
			else
			{
				num = def.shortHash;
			}
			ushort num2 = (ushort)count;
			if (count > 65535)
			{
				Log.Error("Cannot store count " + count + " in DeepResourceGrid: out of ushort range.", false);
				num2 = ushort.MaxValue;
			}
			if (count < 0)
			{
				Log.Error("Cannot store count " + count + " in DeepResourceGrid: out of ushort range.", false);
				num2 = 0;
			}
			int num3 = this.map.cellIndices.CellToIndex(c);
			if (this.defGrid[num3] != num || this.countGrid[num3] != num2)
			{
				this.defGrid[num3] = num;
				this.countGrid[num3] = num2;
				this.drawer.SetDirty();
			}
		}

		// Token: 0x060043E1 RID: 17377 RVA: 0x0023C186 File Offset: 0x0023A586
		public void DeepResourceGridUpdate()
		{
			this.drawer.CellBoolDrawerUpdate();
			if (DebugViewSettings.drawDeepResources)
			{
				this.MarkForDraw();
			}
		}

		// Token: 0x060043E2 RID: 17378 RVA: 0x0023C1A4 File Offset: 0x0023A5A4
		public void MarkForDraw()
		{
			if (this.map == Find.CurrentMap)
			{
				this.drawer.MarkForDraw();
			}
		}

		// Token: 0x060043E3 RID: 17379 RVA: 0x0023C1C4 File Offset: 0x0023A5C4
		public bool GetCellBool(int index)
		{
			return this.CountAt(this.map.cellIndices.IndexToCell(index)) > 0;
		}

		// Token: 0x060043E4 RID: 17380 RVA: 0x0023C1F4 File Offset: 0x0023A5F4
		public Color GetCellExtraColor(int index)
		{
			IntVec3 c = this.map.cellIndices.IndexToCell(index);
			int num = this.CountAt(c);
			ThingDef thingDef = this.ThingDefAt(c);
			float num2 = (float)num / (float)thingDef.deepCountPerCell / 2f;
			int num3 = Mathf.RoundToInt(num2 * 100f);
			num3 %= 100;
			return DebugMatsSpectrum.Mat(num3, true).color;
		}

		// Token: 0x04002E4F RID: 11855
		private Map map;

		// Token: 0x04002E50 RID: 11856
		private CellBoolDrawer drawer;

		// Token: 0x04002E51 RID: 11857
		private ushort[] defGrid;

		// Token: 0x04002E52 RID: 11858
		private ushort[] countGrid;
	}
}
