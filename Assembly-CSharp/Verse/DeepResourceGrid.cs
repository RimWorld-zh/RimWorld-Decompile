using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C1F RID: 3103
	public sealed class DeepResourceGrid : ICellBoolGiver, IExposable
	{
		// Token: 0x04002E59 RID: 11865
		private Map map;

		// Token: 0x04002E5A RID: 11866
		private CellBoolDrawer drawer;

		// Token: 0x04002E5B RID: 11867
		private ushort[] defGrid;

		// Token: 0x04002E5C RID: 11868
		private ushort[] countGrid;

		// Token: 0x060043E7 RID: 17383 RVA: 0x0023D404 File Offset: 0x0023B804
		public DeepResourceGrid(Map map)
		{
			this.map = map;
			this.defGrid = new ushort[map.cellIndices.NumGridCells];
			this.countGrid = new ushort[map.cellIndices.NumGridCells];
			this.drawer = new CellBoolDrawer(this, map.Size.x, map.Size.z, 1f);
		}

		// Token: 0x17000AA6 RID: 2726
		// (get) Token: 0x060043E8 RID: 17384 RVA: 0x0023D478 File Offset: 0x0023B878
		public Color Color
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x060043E9 RID: 17385 RVA: 0x0023D494 File Offset: 0x0023B894
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

		// Token: 0x060043EA RID: 17386 RVA: 0x0023D4F4 File Offset: 0x0023B8F4
		public ThingDef ThingDefAt(IntVec3 c)
		{
			return DefDatabase<ThingDef>.GetByShortHash(this.defGrid[this.map.cellIndices.CellToIndex(c)]);
		}

		// Token: 0x060043EB RID: 17387 RVA: 0x0023D528 File Offset: 0x0023B928
		public int CountAt(IntVec3 c)
		{
			return (int)this.countGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x060043EC RID: 17388 RVA: 0x0023D558 File Offset: 0x0023B958
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

		// Token: 0x060043ED RID: 17389 RVA: 0x0023D62A File Offset: 0x0023BA2A
		public void DeepResourceGridUpdate()
		{
			this.drawer.CellBoolDrawerUpdate();
			if (DebugViewSettings.drawDeepResources)
			{
				this.MarkForDraw();
			}
		}

		// Token: 0x060043EE RID: 17390 RVA: 0x0023D648 File Offset: 0x0023BA48
		public void MarkForDraw()
		{
			if (this.map == Find.CurrentMap)
			{
				this.drawer.MarkForDraw();
			}
		}

		// Token: 0x060043EF RID: 17391 RVA: 0x0023D668 File Offset: 0x0023BA68
		public bool GetCellBool(int index)
		{
			return this.CountAt(this.map.cellIndices.IndexToCell(index)) > 0;
		}

		// Token: 0x060043F0 RID: 17392 RVA: 0x0023D698 File Offset: 0x0023BA98
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
