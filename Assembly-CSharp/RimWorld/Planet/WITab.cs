using System;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008E3 RID: 2275
	public abstract class WITab : InspectTabBase
	{
		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06003421 RID: 13345 RVA: 0x001BDC8C File Offset: 0x001BC08C
		protected WorldObject SelObject
		{
			get
			{
				return Find.WorldSelector.SingleSelectedObject;
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06003422 RID: 13346 RVA: 0x001BDCAC File Offset: 0x001BC0AC
		protected int SelTileID
		{
			get
			{
				return Find.WorldSelector.selectedTile;
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06003423 RID: 13347 RVA: 0x001BDCCC File Offset: 0x001BC0CC
		protected Tile SelTile
		{
			get
			{
				return Find.WorldGrid[this.SelTileID];
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06003424 RID: 13348 RVA: 0x001BDCF4 File Offset: 0x001BC0F4
		protected Caravan SelCaravan
		{
			get
			{
				return this.SelObject as Caravan;
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06003425 RID: 13349 RVA: 0x001BDD14 File Offset: 0x001BC114
		private WorldInspectPane InspectPane
		{
			get
			{
				return Find.World.UI.inspectPane;
			}
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06003426 RID: 13350 RVA: 0x001BDD38 File Offset: 0x001BC138
		protected override bool StillValid
		{
			get
			{
				return WorldRendererUtility.WorldRenderedNow && Find.WindowStack.IsOpen<WorldInspectPane>() && this.InspectPane.CurTabs.Contains(this);
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06003427 RID: 13351 RVA: 0x001BDD88 File Offset: 0x001BC188
		protected override float PaneTopY
		{
			get
			{
				return this.InspectPane.PaneTopY;
			}
		}

		// Token: 0x06003428 RID: 13352 RVA: 0x001BDDA8 File Offset: 0x001BC1A8
		protected override void CloseTab()
		{
			this.InspectPane.CloseOpenTab();
		}
	}
}
