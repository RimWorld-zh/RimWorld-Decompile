using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000849 RID: 2121
	public abstract class ITab : InspectTabBase
	{
		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06002FFB RID: 12283 RVA: 0x001A1338 File Offset: 0x0019F738
		protected object SelObject
		{
			get
			{
				return Find.Selector.SingleSelectedObject;
			}
		}

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06002FFC RID: 12284 RVA: 0x001A1358 File Offset: 0x0019F758
		protected Thing SelThing
		{
			get
			{
				return Find.Selector.SingleSelectedThing;
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06002FFD RID: 12285 RVA: 0x001A1378 File Offset: 0x0019F778
		protected Pawn SelPawn
		{
			get
			{
				return this.SelThing as Pawn;
			}
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06002FFE RID: 12286 RVA: 0x001A1398 File Offset: 0x0019F798
		private MainTabWindow_Inspect InspectPane
		{
			get
			{
				return (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
			}
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06002FFF RID: 12287 RVA: 0x001A13BC File Offset: 0x0019F7BC
		protected override bool StillValid
		{
			get
			{
				return Find.MainTabsRoot.OpenTab == MainButtonDefOf.Inspect && ((MainTabWindow_Inspect)Find.MainTabsRoot.OpenTab.TabWindow).CurTabs.Contains(this);
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06003000 RID: 12288 RVA: 0x001A1408 File Offset: 0x0019F808
		protected override float PaneTopY
		{
			get
			{
				return this.InspectPane.PaneTopY;
			}
		}

		// Token: 0x06003001 RID: 12289 RVA: 0x001A1428 File Offset: 0x0019F828
		protected override void CloseTab()
		{
			this.InspectPane.CloseOpenTab();
		}
	}
}
