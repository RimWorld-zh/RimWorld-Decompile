using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BFE RID: 3070
	public abstract class Area : IExposable, ILoadReferenceable, ICellBoolGiver
	{
		// Token: 0x04002DE8 RID: 11752
		public AreaManager areaManager;

		// Token: 0x04002DE9 RID: 11753
		public int ID = -1;

		// Token: 0x04002DEA RID: 11754
		private BoolGrid innerGrid;

		// Token: 0x04002DEB RID: 11755
		private CellBoolDrawer drawer;

		// Token: 0x04002DEC RID: 11756
		private Texture2D colorTextureInt;

		// Token: 0x06004302 RID: 17154 RVA: 0x00083001 File Offset: 0x00081401
		public Area()
		{
		}

		// Token: 0x06004303 RID: 17155 RVA: 0x00083011 File Offset: 0x00081411
		public Area(AreaManager areaManager)
		{
			this.areaManager = areaManager;
			this.innerGrid = new BoolGrid(areaManager.map);
			this.ID = Find.UniqueIDsManager.GetNextAreaID();
		}

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06004304 RID: 17156 RVA: 0x0008304C File Offset: 0x0008144C
		public Map Map
		{
			get
			{
				return this.areaManager.map;
			}
		}

		// Token: 0x17000A81 RID: 2689
		// (get) Token: 0x06004305 RID: 17157 RVA: 0x0008306C File Offset: 0x0008146C
		public int TrueCount
		{
			get
			{
				return this.innerGrid.TrueCount;
			}
		}

		// Token: 0x17000A82 RID: 2690
		// (get) Token: 0x06004306 RID: 17158
		public abstract string Label { get; }

		// Token: 0x17000A83 RID: 2691
		// (get) Token: 0x06004307 RID: 17159
		public abstract Color Color { get; }

		// Token: 0x17000A84 RID: 2692
		// (get) Token: 0x06004308 RID: 17160
		public abstract int ListPriority { get; }

		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06004309 RID: 17161 RVA: 0x0008308C File Offset: 0x0008148C
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

		// Token: 0x17000A86 RID: 2694
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

		// Token: 0x17000A87 RID: 2695
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

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x0600430E RID: 17166 RVA: 0x00083144 File Offset: 0x00081544
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

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x0600430F RID: 17167 RVA: 0x000831A4 File Offset: 0x000815A4
		public IEnumerable<IntVec3> ActiveCells
		{
			get
			{
				return this.innerGrid.ActiveCells;
			}
		}

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06004310 RID: 17168 RVA: 0x000831C4 File Offset: 0x000815C4
		public virtual bool Mutable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004311 RID: 17169 RVA: 0x000831DA File Offset: 0x000815DA
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Deep.Look<BoolGrid>(ref this.innerGrid, "innerGrid", new object[0]);
		}

		// Token: 0x06004312 RID: 17170 RVA: 0x00083208 File Offset: 0x00081608
		public bool GetCellBool(int index)
		{
			return this.innerGrid[index];
		}

		// Token: 0x06004313 RID: 17171 RVA: 0x0008322C File Offset: 0x0008162C
		public Color GetCellExtraColor(int index)
		{
			return Color.white;
		}

		// Token: 0x06004314 RID: 17172 RVA: 0x00083248 File Offset: 0x00081648
		public virtual bool AssignableAsAllowed()
		{
			return false;
		}

		// Token: 0x06004315 RID: 17173 RVA: 0x0008325E File Offset: 0x0008165E
		public virtual void SetLabel(string label)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004316 RID: 17174 RVA: 0x00083268 File Offset: 0x00081668
		protected virtual void Set(IntVec3 c, bool val)
		{
			int index = this.Map.cellIndices.CellToIndex(c);
			if (this.innerGrid[index] != val)
			{
				this.innerGrid[index] = val;
				this.MarkDirty(c);
			}
		}

		// Token: 0x06004317 RID: 17175 RVA: 0x000832B4 File Offset: 0x000816B4
		private void MarkDirty(IntVec3 c)
		{
			this.Drawer.SetDirty();
			Region region = c.GetRegion(this.Map, RegionType.Set_All);
			if (region != null)
			{
				region.Notify_AreaChanged(this);
			}
		}

		// Token: 0x06004318 RID: 17176 RVA: 0x000832E8 File Offset: 0x000816E8
		public void Delete()
		{
			this.areaManager.Remove(this);
		}

		// Token: 0x06004319 RID: 17177 RVA: 0x000832F7 File Offset: 0x000816F7
		public void MarkForDraw()
		{
			if (this.Map == Find.CurrentMap)
			{
				this.Drawer.MarkForDraw();
			}
		}

		// Token: 0x0600431A RID: 17178 RVA: 0x00083315 File Offset: 0x00081715
		public void AreaUpdate()
		{
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x0600431B RID: 17179 RVA: 0x00083323 File Offset: 0x00081723
		public void Invert()
		{
			this.innerGrid.Invert();
			this.Drawer.SetDirty();
		}

		// Token: 0x0600431C RID: 17180
		public abstract string GetUniqueLoadID();
	}
}
