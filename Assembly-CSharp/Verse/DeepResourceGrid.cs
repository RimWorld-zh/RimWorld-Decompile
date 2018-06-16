using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C21 RID: 3105
	public sealed class DeepResourceGrid : ICellBoolGiver, IExposable
	{
		// Token: 0x060043DD RID: 17373 RVA: 0x0023BF88 File Offset: 0x0023A388
		public DeepResourceGrid(Map map)
		{
			this.map = map;
			this.defGrid = new ushort[map.cellIndices.NumGridCells];
			this.countGrid = new ushort[map.cellIndices.NumGridCells];
			this.drawer = new CellBoolDrawer(this, map.Size.x, map.Size.z, 1f);
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x060043DE RID: 17374 RVA: 0x0023BFFC File Offset: 0x0023A3FC
		public Color Color
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x060043DF RID: 17375 RVA: 0x0023C018 File Offset: 0x0023A418
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

		// Token: 0x060043E0 RID: 17376 RVA: 0x0023C078 File Offset: 0x0023A478
		public ThingDef ThingDefAt(IntVec3 c)
		{
			return DefDatabase<ThingDef>.GetByShortHash(this.defGrid[this.map.cellIndices.CellToIndex(c)]);
		}

		// Token: 0x060043E1 RID: 17377 RVA: 0x0023C0AC File Offset: 0x0023A4AC
		public int CountAt(IntVec3 c)
		{
			return (int)this.countGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x060043E2 RID: 17378 RVA: 0x0023C0DC File Offset: 0x0023A4DC
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

		// Token: 0x060043E3 RID: 17379 RVA: 0x0023C1AE File Offset: 0x0023A5AE
		public void DeepResourceGridUpdate()
		{
			this.drawer.CellBoolDrawerUpdate();
			if (DebugViewSettings.drawDeepResources)
			{
				this.MarkForDraw();
			}
		}

		// Token: 0x060043E4 RID: 17380 RVA: 0x0023C1CC File Offset: 0x0023A5CC
		public void MarkForDraw()
		{
			if (this.map == Find.CurrentMap)
			{
				this.drawer.MarkForDraw();
			}
		}

		// Token: 0x060043E5 RID: 17381 RVA: 0x0023C1EC File Offset: 0x0023A5EC
		public bool GetCellBool(int index)
		{
			return this.CountAt(this.map.cellIndices.IndexToCell(index)) > 0;
		}

		// Token: 0x060043E6 RID: 17382 RVA: 0x0023C21C File Offset: 0x0023A61C
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

		// Token: 0x04002E51 RID: 11857
		private Map map;

		// Token: 0x04002E52 RID: 11858
		private CellBoolDrawer drawer;

		// Token: 0x04002E53 RID: 11859
		private ushort[] defGrid;

		// Token: 0x04002E54 RID: 11860
		private ushort[] countGrid;
	}
}
