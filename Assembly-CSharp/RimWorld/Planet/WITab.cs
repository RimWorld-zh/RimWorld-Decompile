using System;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008DF RID: 2271
	public abstract class WITab : InspectTabBase
	{
		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x0600341C RID: 13340 RVA: 0x001BDF3C File Offset: 0x001BC33C
		protected WorldObject SelObject
		{
			get
			{
				return Find.WorldSelector.SingleSelectedObject;
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x0600341D RID: 13341 RVA: 0x001BDF5C File Offset: 0x001BC35C
		protected int SelTileID
		{
			get
			{
				return Find.WorldSelector.selectedTile;
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x0600341E RID: 13342 RVA: 0x001BDF7C File Offset: 0x001BC37C
		protected Tile SelTile
		{
			get
			{
				return Find.WorldGrid[this.SelTileID];
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x0600341F RID: 13343 RVA: 0x001BDFA4 File Offset: 0x001BC3A4
		protected Caravan SelCaravan
		{
			get
			{
				return this.SelObject as Caravan;
			}
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06003420 RID: 13344 RVA: 0x001BDFC4 File Offset: 0x001BC3C4
		private WorldInspectPane InspectPane
		{
			get
			{
				return Find.World.UI.inspectPane;
			}
		}

		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06003421 RID: 13345 RVA: 0x001BDFE8 File Offset: 0x001BC3E8
		protected override bool StillValid
		{
			get
			{
				return WorldRendererUtility.WorldRenderedNow && Find.WindowStack.IsOpen<WorldInspectPane>() && this.InspectPane.CurTabs.Contains(this);
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06003422 RID: 13346 RVA: 0x001BE038 File Offset: 0x001BC438
		protected override float PaneTopY
		{
			get
			{
				return this.InspectPane.PaneTopY;
			}
		}

		// Token: 0x06003423 RID: 13347 RVA: 0x001BE058 File Offset: 0x001BC458
		protected override void CloseTab()
		{
			this.InspectPane.CloseOpenTab();
		}
	}
}
