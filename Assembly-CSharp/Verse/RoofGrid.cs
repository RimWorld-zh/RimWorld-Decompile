using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CA1 RID: 3233
	public sealed class RoofGrid : IExposable, ICellBoolGiver
	{
		// Token: 0x04003061 RID: 12385
		private Map map;

		// Token: 0x04003062 RID: 12386
		private RoofDef[] roofGrid;

		// Token: 0x04003063 RID: 12387
		private CellBoolDrawer drawerInt;

		// Token: 0x06004736 RID: 18230 RVA: 0x00259755 File Offset: 0x00257B55
		public RoofGrid(Map map)
		{
			this.map = map;
			this.roofGrid = new RoofDef[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x06004737 RID: 18231 RVA: 0x0025977C File Offset: 0x00257B7C
		public CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawerInt == null)
				{
					this.drawerInt = new CellBoolDrawer(this, this.map.Size.x, this.map.Size.z, 0.33f);
				}
				return this.drawerInt;
			}
		}

		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x06004738 RID: 18232 RVA: 0x002597DC File Offset: 0x00257BDC
		public Color Color
		{
			get
			{
				return new Color(0.3f, 1f, 0.4f);
			}
		}

		// Token: 0x06004739 RID: 18233 RVA: 0x00259805 File Offset: 0x00257C05
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => (this.roofGrid[this.map.cellIndices.CellToIndex(c)] != null) ? this.roofGrid[this.map.cellIndices.CellToIndex(c)].shortHash : 0, delegate(IntVec3 c, ushort val)
			{
				this.SetRoof(c, DefDatabase<RoofDef>.GetByShortHash(val));
			}, "roofs");
		}

		// Token: 0x0600473A RID: 18234 RVA: 0x00259830 File Offset: 0x00257C30
		public bool GetCellBool(int index)
		{
			return this.roofGrid[index] != null && !this.map.fogGrid.IsFogged(index);
		}

		// Token: 0x0600473B RID: 18235 RVA: 0x0025986C File Offset: 0x00257C6C
		public Color GetCellExtraColor(int index)
		{
			Color result;
			if (RoofDefOf.RoofRockThick != null && this.roofGrid[index] == RoofDefOf.RoofRockThick)
			{
				result = Color.gray;
			}
			else
			{
				result = Color.white;
			}
			return result;
		}

		// Token: 0x0600473C RID: 18236 RVA: 0x002598B0 File Offset: 0x00257CB0
		public bool Roofed(int index)
		{
			return this.roofGrid[index] != null;
		}

		// Token: 0x0600473D RID: 18237 RVA: 0x002598D4 File Offset: 0x00257CD4
		public bool Roofed(int x, int z)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(x, z)] != null;
		}

		// Token: 0x0600473E RID: 18238 RVA: 0x00259908 File Offset: 0x00257D08
		public bool Roofed(IntVec3 c)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(c)] != null;
		}

		// Token: 0x0600473F RID: 18239 RVA: 0x0025993C File Offset: 0x00257D3C
		public RoofDef RoofAt(int index)
		{
			return this.roofGrid[index];
		}

		// Token: 0x06004740 RID: 18240 RVA: 0x0025995C File Offset: 0x00257D5C
		public RoofDef RoofAt(IntVec3 c)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06004741 RID: 18241 RVA: 0x0025998C File Offset: 0x00257D8C
		public RoofDef RoofAt(int x, int z)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(x, z)];
		}

		// Token: 0x06004742 RID: 18242 RVA: 0x002599BC File Offset: 0x00257DBC
		public void SetRoof(IntVec3 c, RoofDef def)
		{
			if (this.roofGrid[this.map.cellIndices.CellToIndex(c)] != def)
			{
				this.roofGrid[this.map.cellIndices.CellToIndex(c)] = def;
				this.map.glowGrid.MarkGlowGridDirty(c);
				Region validRegionAt_NoRebuild = this.map.regionGrid.GetValidRegionAt_NoRebuild(c);
				if (validRegionAt_NoRebuild != null)
				{
					validRegionAt_NoRebuild.Room.Notify_RoofChanged();
				}
				if (this.drawerInt != null)
				{
					this.drawerInt.SetDirty();
				}
				this.map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Roofs);
			}
		}

		// Token: 0x06004743 RID: 18243 RVA: 0x00259A63 File Offset: 0x00257E63
		public void RoofGridUpdate()
		{
			if (Find.PlaySettings.showRoofOverlay)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}
	}
}
