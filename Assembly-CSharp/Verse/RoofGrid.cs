using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CA2 RID: 3234
	public sealed class RoofGrid : IExposable, ICellBoolGiver
	{
		// Token: 0x0600472A RID: 18218 RVA: 0x00258289 File Offset: 0x00256689
		public RoofGrid(Map map)
		{
			this.map = map;
			this.roofGrid = new RoofDef[map.cellIndices.NumGridCells];
		}

		// Token: 0x17000B45 RID: 2885
		// (get) Token: 0x0600472B RID: 18219 RVA: 0x002582B0 File Offset: 0x002566B0
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

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x0600472C RID: 18220 RVA: 0x00258310 File Offset: 0x00256710
		public Color Color
		{
			get
			{
				return new Color(0.3f, 1f, 0.4f);
			}
		}

		// Token: 0x0600472D RID: 18221 RVA: 0x00258339 File Offset: 0x00256739
		public void ExposeData()
		{
			MapExposeUtility.ExposeUshort(this.map, (IntVec3 c) => (this.roofGrid[this.map.cellIndices.CellToIndex(c)] != null) ? this.roofGrid[this.map.cellIndices.CellToIndex(c)].shortHash : 0, delegate(IntVec3 c, ushort val)
			{
				this.SetRoof(c, DefDatabase<RoofDef>.GetByShortHash(val));
			}, "roofs");
		}

		// Token: 0x0600472E RID: 18222 RVA: 0x00258364 File Offset: 0x00256764
		public bool GetCellBool(int index)
		{
			return this.roofGrid[index] != null && !this.map.fogGrid.IsFogged(index);
		}

		// Token: 0x0600472F RID: 18223 RVA: 0x002583A0 File Offset: 0x002567A0
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

		// Token: 0x06004730 RID: 18224 RVA: 0x002583E4 File Offset: 0x002567E4
		public bool Roofed(int index)
		{
			return this.roofGrid[index] != null;
		}

		// Token: 0x06004731 RID: 18225 RVA: 0x00258408 File Offset: 0x00256808
		public bool Roofed(int x, int z)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(x, z)] != null;
		}

		// Token: 0x06004732 RID: 18226 RVA: 0x0025843C File Offset: 0x0025683C
		public bool Roofed(IntVec3 c)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(c)] != null;
		}

		// Token: 0x06004733 RID: 18227 RVA: 0x00258470 File Offset: 0x00256870
		public RoofDef RoofAt(int index)
		{
			return this.roofGrid[index];
		}

		// Token: 0x06004734 RID: 18228 RVA: 0x00258490 File Offset: 0x00256890
		public RoofDef RoofAt(IntVec3 c)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x06004735 RID: 18229 RVA: 0x002584C0 File Offset: 0x002568C0
		public RoofDef RoofAt(int x, int z)
		{
			return this.roofGrid[this.map.cellIndices.CellToIndex(x, z)];
		}

		// Token: 0x06004736 RID: 18230 RVA: 0x002584F0 File Offset: 0x002568F0
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

		// Token: 0x06004737 RID: 18231 RVA: 0x00258597 File Offset: 0x00256997
		public void RoofGridUpdate()
		{
			if (Find.PlaySettings.showRoofOverlay)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x04003056 RID: 12374
		private Map map;

		// Token: 0x04003057 RID: 12375
		private RoofDef[] roofGrid;

		// Token: 0x04003058 RID: 12376
		private CellBoolDrawer drawerInt;
	}
}
