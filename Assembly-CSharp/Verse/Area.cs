using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BFB RID: 3067
	public abstract class Area : IExposable, ILoadReferenceable, ICellBoolGiver
	{
		// Token: 0x04002DE1 RID: 11745
		public AreaManager areaManager;

		// Token: 0x04002DE2 RID: 11746
		public int ID = -1;

		// Token: 0x04002DE3 RID: 11747
		private BoolGrid innerGrid;

		// Token: 0x04002DE4 RID: 11748
		private CellBoolDrawer drawer;

		// Token: 0x04002DE5 RID: 11749
		private Texture2D colorTextureInt;

		// Token: 0x060042FF RID: 17151 RVA: 0x00082EA1 File Offset: 0x000812A1
		public Area()
		{
		}

		// Token: 0x06004300 RID: 17152 RVA: 0x00082EB1 File Offset: 0x000812B1
		public Area(AreaManager areaManager)
		{
			this.areaManager = areaManager;
			this.innerGrid = new BoolGrid(areaManager.map);
			this.ID = Find.UniqueIDsManager.GetNextAreaID();
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06004301 RID: 17153 RVA: 0x00082EEC File Offset: 0x000812EC
		public Map Map
		{
			get
			{
				return this.areaManager.map;
			}
		}

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06004302 RID: 17154 RVA: 0x00082F0C File Offset: 0x0008130C
		public int TrueCount
		{
			get
			{
				return this.innerGrid.TrueCount;
			}
		}

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06004303 RID: 17155
		public abstract string Label { get; }

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06004304 RID: 17156
		public abstract Color Color { get; }

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06004305 RID: 17157
		public abstract int ListPriority { get; }

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x06004306 RID: 17158 RVA: 0x00082F2C File Offset: 0x0008132C
		public Texture2D ColorTexture
		{
			get
			{
				if (this.colorTextureInt == null)
				{
					this.colorTextureInt = SolidColorMaterials.NewSolidColorTexture(this.Color);
				}
				return this.colorTextureInt;
			}
		}

		// Token: 0x17000A87 RID: 2695
		public bool this[int index]
		{
			get
			{
				return this.innerGrid[index];
			}
			set
			{
				this.Set(this.Map.cellIndices.IndexToCell(index), value);
			}
		}

		// Token: 0x17000A88 RID: 2696
		public bool this[IntVec3 c]
		{
			get
			{
				return this.innerGrid[this.Map.cellIndices.CellToIndex(c)];
			}
			set
			{
				this.Set(c, value);
			}
		}

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x0600430B RID: 17163 RVA: 0x00082FE4 File Offset: 0x000813E4
		private CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawer == null)
				{
					this.drawer = new CellBoolDrawer(this, this.Map.Size.x, this.Map.Size.z, 0.33f);
				}
				return this.drawer;
			}
		}

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x0600430C RID: 17164 RVA: 0x00083044 File Offset: 0x00081444
		public IEnumerable<IntVec3> ActiveCells
		{
			get
			{
				return this.innerGrid.ActiveCells;
			}
		}

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x0600430D RID: 17165 RVA: 0x00083064 File Offset: 0x00081464
		public virtual bool Mutable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600430E RID: 17166 RVA: 0x0008307A File Offset: 0x0008147A
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Deep.Look<BoolGrid>(ref this.innerGrid, "innerGrid", new object[0]);
		}

		// Token: 0x0600430F RID: 17167 RVA: 0x000830A8 File Offset: 0x000814A8
		public bool GetCellBool(int index)
		{
			return this.innerGrid[index];
		}

		// Token: 0x06004310 RID: 17168 RVA: 0x000830CC File Offset: 0x000814CC
		public Color GetCellExtraColor(int index)
		{
			return Color.white;
		}

		// Token: 0x06004311 RID: 17169 RVA: 0x000830E8 File Offset: 0x000814E8
		public virtual bool AssignableAsAllowed()
		{
			return false;
		}

		// Token: 0x06004312 RID: 17170 RVA: 0x000830FE File Offset: 0x000814FE
		public virtual void SetLabel(string label)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004313 RID: 17171 RVA: 0x00083108 File Offset: 0x00081508
		protected virtual void Set(IntVec3 c, bool val)
		{
			int index = this.Map.cellIndices.CellToIndex(c);
			if (this.innerGrid[index] != val)
			{
				this.innerGrid[index] = val;
				this.MarkDirty(c);
			}
		}

		// Token: 0x06004314 RID: 17172 RVA: 0x00083154 File Offset: 0x00081554
		private void MarkDirty(IntVec3 c)
		{
			this.Drawer.SetDirty();
			Region region = c.GetRegion(this.Map, RegionType.Set_All);
			if (region != null)
			{
				region.Notify_AreaChanged(this);
			}
		}

		// Token: 0x06004315 RID: 17173 RVA: 0x00083188 File Offset: 0x00081588
		public void Delete()
		{
			this.areaManager.Remove(this);
		}

		// Token: 0x06004316 RID: 17174 RVA: 0x00083197 File Offset: 0x00081597
		public void MarkForDraw()
		{
			if (this.Map == Find.CurrentMap)
			{
				this.Drawer.MarkForDraw();
			}
		}

		// Token: 0x06004317 RID: 17175 RVA: 0x000831B5 File Offset: 0x000815B5
		public void AreaUpdate()
		{
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x06004318 RID: 17176 RVA: 0x000831C3 File Offset: 0x000815C3
		public void Invert()
		{
			this.innerGrid.Invert();
			this.Drawer.SetDirty();
		}

		// Token: 0x06004319 RID: 17177
		public abstract string GetUniqueLoadID();
	}
}
