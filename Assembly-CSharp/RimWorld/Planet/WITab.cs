using System;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008E1 RID: 2273
	public abstract class WITab : InspectTabBase
	{
		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06003420 RID: 13344 RVA: 0x001BE07C File Offset: 0x001BC47C
		protected WorldObject SelObject
		{
			get
			{
				return Find.WorldSelector.SingleSelectedObject;
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06003421 RID: 13345 RVA: 0x001BE09C File Offset: 0x001BC49C
		protected int SelTileID
		{
			get
			{
				return Find.WorldSelector.selectedTile;
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06003422 RID: 13346 RVA: 0x001BE0BC File Offset: 0x001BC4BC
		protected Tile SelTile
		{
			get
			{
				return Find.WorldGrid[this.SelTileID];
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06003423 RID: 13347 RVA: 0x001BE0E4 File Offset: 0x001BC4E4
		protected Caravan SelCaravan
		{
			get
			{
				return this.SelObject as Caravan;
			}
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06003424 RID: 13348 RVA: 0x001BE104 File Offset: 0x001BC504
		private WorldInspectPane InspectPane
		{
			get
			{
				return Find.World.UI.inspectPane;
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06003425 RID: 13349 RVA: 0x001BE128 File Offset: 0x001BC528
		protected override bool StillValid
		{
			get
			{
				return WorldRendererUtility.WorldRenderedNow && Find.WindowStack.IsOpen<WorldInspectPane>() && this.InspectPane.CurTabs.Contains(this);
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06003426 RID: 13350 RVA: 0x001BE178 File Offset: 0x001BC578
		protected override float PaneTopY
		{
			get
			{
				return this.InspectPane.PaneTopY;
			}
		}

		// Token: 0x06003427 RID: 13351 RVA: 0x001BE198 File Offset: 0x001BC598
		protected override void CloseTab()
		{
			this.InspectPane.CloseOpenTab();
		}
	}
}
