using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C20 RID: 3104
	public sealed class DeepResourceGrid : ICellBoolGiver, IExposable
	{
		// Token: 0x04002E60 RID: 11872
		private Map map;

		// Token: 0x04002E61 RID: 11873
		private CellBoolDrawer drawer;

		// Token: 0x04002E62 RID: 11874
		private ushort[] defGrid;

		// Token: 0x04002E63 RID: 11875
		private ushort[] countGrid;

		// Token: 0x060043E7 RID: 17383 RVA: 0x0023D6E4 File Offset: 0x0023BAE4
		public DeepResourceGrid(Map map)
		{
			this.map = map;
			this.defGrid = new ushort[map.cellIndices.NumGridCells];
			this.countGrid = new ushort[map.cellIndices.NumGridCells];
			this.drawer = new CellBoolDrawer(this, map.Size.x, map.Size.z, 1f);
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x060043E8 RID: 17384 RVA: 0x0023D758 File Offset: 0x0023BB58
		public Color Color
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x060043E9 RID: 17385 RVA: 0x0023D774 File Offset: 0x0023BB74
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

		// Token: 0x060043EA RID: 17386 RVA: 0x0023D7D4 File Offset: 0x0023BBD4
		public ThingDef ThingDefAt(IntVec3 c)
		{
			return DefDatabase<ThingDef>.GetByShortHash(this.defGrid[this.map.cellIndices.CellToIndex(c)]);
		}

		// Token: 0x060043EB RID: 17387 RVA: 0x0023D808 File Offset: 0x0023BC08
		public int CountAt(IntVec3 c)
		{
			return (int)this.countGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x060043EC RID: 17388 RVA: 0x0023D838 File Offset: 0x0023BC38
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

		// Token: 0x060043ED RID: 17389 RVA: 0x0023D90A File Offset: 0x0023BD0A
		public void DeepResourceGridUpdate()
		{
			this.drawer.CellBoolDrawerUpdate();
			if (DebugViewSettings.drawDeepResources)
			{
				this.MarkForDraw();
			}
		}

		// Token: 0x060043EE RID: 17390 RVA: 0x0023D928 File Offset: 0x0023BD28
		public void MarkForDraw()
		{
			if (this.map == Find.CurrentMap)
			{
				this.drawer.MarkForDraw();
			}
		}

		// Token: 0x060043EF RID: 17391 RVA: 0x0023D948 File Offset: 0x0023BD48
		public bool GetCellBool(int index)
		{
			return this.CountAt(this.map.cellIndices.IndexToCell(index)) > 0;
		}

		// Token: 0x060043F0 RID: 17392 RVA: 0x0023D978 File Offset: 0x0023BD78
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
	}
}
