using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C9F RID: 3231
	public sealed class RoofGrid : IExposable, ICellBoolGiver
	{
		// Token: 0x06004733 RID: 18227 RVA: 0x00259679 File Offset: 0x00257A79
		public RoofGrid(Map map)
		{
			this.map = map;
			this.roofGrid = new RoofDef[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x06004734 RID: 18228 RVA: 0x002596A0 File Offset: 0x00257AA0
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

		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x06004735 RID: 18229 RVA: 0x00259700 File Offset: 0x00257B00
		public Color Color
		{
			get
			{
				return new Color(0.3f, 1f, 0.4f);
			}
		}

		// Token: 0x06004736 RID: 18230 RVA: 0x00259729 File Offset: 0x00257B29
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => (this.roofGrid[this.map.cellIndices.CellToIndex(c)] != null) ? this.roofGrid[this.map.cellIndices.CellToIndex(c)].shortHash : 0, delegate(IntVec3 c, ushort val)
			{
				this.SetRoof(c, DefDatabase<RoofDef>.GetByShortHash(val));
			}, "roofs");
		}

		// Token: 0x06004737 RID: 18231 RVA: 0x00259754 File Offset: 0x00257B54
		public bool GetCellBool(int index)
		{
			return this.roofGrid[index] != null && !this.map.fogGrid.IsFogged(index);
		}

		// Token: 0x06004738 RID: 18232 RVA: 0x00259790 File Offset: 0x00257B90
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

		// Token: 0x06004739 RID: 18233 RVA: 0x002597D4 File Offset: 0x00257BD4
		public bool Roofed(int index)
		{
			return this.roofGrid[index] != null;
		}

		// Token: 0x0600473A RID: 18234 RVA: 0x002597F8 File Offset: 0x00257BF8
		public bool Roofed(int x, int z)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(x, z)] != null;
		}

		// Token: 0x0600473B RID: 18235 RVA: 0x0025982C File Offset: 0x00257C2C
		public bool Roofed(IntVec3 c)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(c)] != null;
		}

		// Token: 0x0600473C RID: 18236 RVA: 0x00259860 File Offset: 0x00257C60
		public RoofDef RoofAt(int index)
		{
			return this.roofGrid[index];
		}

		// Token: 0x0600473D RID: 18237 RVA: 0x00259880 File Offset: 0x00257C80
		public RoofDef RoofAt(IntVec3 c)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x0600473E RID: 18238 RVA: 0x002598B0 File Offset: 0x00257CB0
		public RoofDef RoofAt(int x, int z)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(x, z)];
		}

		// Token: 0x0600473F RID: 18239 RVA: 0x002598E0 File Offset: 0x00257CE0
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

		// Token: 0x06004740 RID: 18240 RVA: 0x00259987 File Offset: 0x00257D87
		public void RoofGridUpdate()
		{
			if (Find.PlaySettings.showRoofOverlay)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x04003061 RID: 12385
		private Map map;

		// Token: 0x04003062 RID: 12386
		private RoofDef[] roofGrid;

		// Token: 0x04003063 RID: 12387
		private CellBoolDrawer drawerInt;
	}
}
