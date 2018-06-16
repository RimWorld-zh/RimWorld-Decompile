using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CA3 RID: 3235
	public sealed class RoofGrid : IExposable, ICellBoolGiver
	{
		// Token: 0x0600472C RID: 18220 RVA: 0x002582B1 File Offset: 0x002566B1
		public RoofGrid(Map map)
		{
			this.map = map;
			this.roofGrid = new RoofDef[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x0600472D RID: 18221 RVA: 0x002582D8 File Offset: 0x002566D8
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
		// (get) Token: 0x0600472E RID: 18222 RVA: 0x00258338 File Offset: 0x00256738
		public Color Color
		{
			get
			{
				return new Color(0.3f, 1f, 0.4f);
			}
		}

		// Token: 0x0600472F RID: 18223 RVA: 0x00258361 File Offset: 0x00256761
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => (this.roofGrid[this.map.cellIndices.CellToIndex(c)] != null) ? this.roofGrid[this.map.cellIndices.CellToIndex(c)].shortHash : 0, delegate(IntVec3 c, ushort val)
			{
				this.SetRoof(c, DefDatabase<RoofDef>.GetByShortHash(val));
			}, "roofs");
		}

		// Token: 0x06004730 RID: 18224 RVA: 0x0025838C File Offset: 0x0025678C
		public bool GetCellBool(int index)
		{
			return this.roofGrid[index] != null && !this.map.fogGrid.IsFogged(index);
		}

		// Token: 0x06004731 RID: 18225 RVA: 0x002583C8 File Offset: 0x002567C8
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

		// Token: 0x06004732 RID: 18226 RVA: 0x0025840C File Offset: 0x0025680C
		public bool Roofed(int index)
		{
			return this.roofGrid[index] != null;
		}

		// Token: 0x06004733 RID: 18227 RVA: 0x00258430 File Offset: 0x00256830
		public bool Roofed(int x, int z)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(x, z)] != null;
		}

		// Token: 0x06004734 RID: 18228 RVA: 0x00258464 File Offset: 0x00256864
		public bool Roofed(IntVec3 c)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(c)] != null;
		}

		// Token: 0x06004735 RID: 18229 RVA: 0x00258498 File Offset: 0x00256898
		public RoofDef RoofAt(int index)
		{
			return this.roofGrid[index];
		}

		// Token: 0x06004736 RID: 18230 RVA: 0x002584B8 File Offset: 0x002568B8
		public RoofDef RoofAt(IntVec3 c)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06004737 RID: 18231 RVA: 0x002584E8 File Offset: 0x002568E8
		public RoofDef RoofAt(int x, int z)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(x, z)];
		}

		// Token: 0x06004738 RID: 18232 RVA: 0x00258518 File Offset: 0x00256918
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

		// Token: 0x06004739 RID: 18233 RVA: 0x002585BF File Offset: 0x002569BF
		public void RoofGridUpdate()
		{
			if (Find.PlaySettings.showRoofOverlay)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x04003058 RID: 12376
		private Map map;

		// Token: 0x04003059 RID: 12377
		private RoofDef[] roofGrid;

		// Token: 0x0400305A RID: 12378
		private CellBoolDrawer drawerInt;
	}
}
