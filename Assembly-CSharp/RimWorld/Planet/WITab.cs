using System;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008E3 RID: 2275
	public abstract class WITab : InspectTabBase
	{
		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06003423 RID: 13347 RVA: 0x001BDD54 File Offset: 0x001BC154
		protected WorldObject SelObject
		{
			get
			{
				return Find.WorldSelector.SingleSelectedObject;
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06003424 RID: 13348 RVA: 0x001BDD74 File Offset: 0x001BC174
		protected int SelTileID
		{
			get
			{
				return Find.WorldSelector.selectedTile;
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06003425 RID: 13349 RVA: 0x001BDD94 File Offset: 0x001BC194
		protected Tile SelTile
		{
			get
			{
				return Find.WorldGrid[this.SelTileID];
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06003426 RID: 13350 RVA: 0x001BDDBC File Offset: 0x001BC1BC
		protected Caravan SelCaravan
		{
			get
			{
				return this.SelObject as Caravan;
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06003427 RID: 13351 RVA: 0x001BDDDC File Offset: 0x001BC1DC
		private WorldInspectPane InspectPane
		{
			get
			{
				return Find.World.UI.inspectPane;
			}
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06003428 RID: 13352 RVA: 0x001BDE00 File Offset: 0x001BC200
		protected override bool StillValid
		{
			get
			{
				return WorldRendererUtility.WorldRenderedNow && Find.WindowStack.IsOpen<WorldInspectPane>() && this.InspectPane.CurTabs.Contains(this);
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06003429 RID: 13353 RVA: 0x001BDE50 File Offset: 0x001BC250
		protected override float PaneTopY
		{
			get
			{
				return this.InspectPane.PaneTopY;
			}
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x001BDE70 File Offset: 0x001BC270
		protected override void CloseTab()
		{
			this.InspectPane.CloseOpenTab();
		}
	}
}
