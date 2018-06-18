using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BFE RID: 3070
	public abstract class Area : IExposable, ILoadReferenceable, ICellBoolGiver
	{
		// Token: 0x060042F6 RID: 17142 RVA: 0x00082CB5 File Offset: 0x000810B5
		public Area()
		{
		}

		// Token: 0x060042F7 RID: 17143 RVA: 0x00082CC5 File Offset: 0x000810C5
		public Area(AreaManager areaManager)
		{
			this.areaManager = areaManager;
			this.innerGrid = new BoolGrid(areaManager.map);
			this.ID = Find.UniqueIDsManager.GetNextAreaID();
		}

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x060042F8 RID: 17144 RVA: 0x00082D00 File Offset: 0x00081100
		public Map Map
		{
			get
			{
				return this.areaManager.map;
			}
		}

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x060042F9 RID: 17145 RVA: 0x00082D20 File Offset: 0x00081120
		public int TrueCount
		{
			get
			{
				return this.innerGrid.TrueCount;
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x060042FA RID: 17146
		public abstract string Label { get; }

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x060042FB RID: 17147
		public abstract Color Color { get; }

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x060042FC RID: 17148
		public abstract int ListPriority { get; }

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x060042FD RID: 17149 RVA: 0x00082D40 File Offset: 0x00081140
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

		// Token: 0x17000A85 RID: 2693
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

		// Token: 0x17000A86 RID: 2694
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

		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x06004302 RID: 17154 RVA: 0x00082DF8 File Offset: 0x000811F8
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

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06004303 RID: 17155 RVA: 0x00082E58 File Offset: 0x00081258
		public IEnumerable<IntVec3> ActiveCells
		{
			get
			{
				return this.innerGrid.ActiveCells;
			}
		}

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06004304 RID: 17156 RVA: 0x00082E78 File Offset: 0x00081278
		public virtual bool Mutable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004305 RID: 17157 RVA: 0x00082E8E File Offset: 0x0008128E
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Deep.Look<BoolGrid>(ref this.innerGrid, "innerGrid", new object[0]);
		}

		// Token: 0x06004306 RID: 17158 RVA: 0x00082EBC File Offset: 0x000812BC
		public bool GetCellBool(int index)
		{
			return this.innerGrid[index];
		}

		// Token: 0x06004307 RID: 17159 RVA: 0x00082EE0 File Offset: 0x000812E0
		public Color GetCellExtraColor(int index)
		{
			return Color.white;
		}

		// Token: 0x06004308 RID: 17160 RVA: 0x00082EFC File Offset: 0x000812FC
		public virtual bool AssignableAsAllowed()
		{
			return false;
		}

		// Token: 0x06004309 RID: 17161 RVA: 0x00082F12 File Offset: 0x00081312
		public virtual void SetLabel(string label)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600430A RID: 17162 RVA: 0x00082F1C File Offset: 0x0008131C
		protected virtual void Set(IntVec3 c, bool val)
		{
			int index = this.Map.cellIndices.CellToIndex(c);
			if (this.innerGrid[index] != val)
			{
				this.innerGrid[index] = val;
				this.MarkDirty(c);
			}
		}

		// Token: 0x0600430B RID: 17163 RVA: 0x00082F68 File Offset: 0x00081368
		private void MarkDirty(IntVec3 c)
		{
			this.Drawer.SetDirty();
			Region region = c.GetRegion(this.Map, RegionType.Set_All);
			if (region != null)
			{
				region.Notify_AreaChanged(this);
			}
		}

		// Token: 0x0600430C RID: 17164 RVA: 0x00082F9C File Offset: 0x0008139C
		public void Delete()
		{
			this.areaManager.Remove(this);
		}

		// Token: 0x0600430D RID: 17165 RVA: 0x00082FAB File Offset: 0x000813AB
		public void MarkForDraw()
		{
			if (this.Map == Find.CurrentMap)
			{
				this.Drawer.MarkForDraw();
			}
		}

		// Token: 0x0600430E RID: 17166 RVA: 0x00082FC9 File Offset: 0x000813C9
		public void AreaUpdate()
		{
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x0600430F RID: 17167 RVA: 0x00082FD7 File Offset: 0x000813D7
		public void Invert()
		{
			this.innerGrid.Invert();
			this.Drawer.SetDirty();
		}

		// Token: 0x06004310 RID: 17168
		public abstract string GetUniqueLoadID();

		// Token: 0x04002DD7 RID: 11735
		public AreaManager areaManager;

		// Token: 0x04002DD8 RID: 11736
		public int ID = -1;

		// Token: 0x04002DD9 RID: 11737
		private BoolGrid innerGrid;

		// Token: 0x04002DDA RID: 11738
		private CellBoolDrawer drawer;

		// Token: 0x04002DDB RID: 11739
		private Texture2D colorTextureInt;
	}
}
