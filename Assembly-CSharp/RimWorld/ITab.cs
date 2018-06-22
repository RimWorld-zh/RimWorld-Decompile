using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000845 RID: 2117
	public abstract class ITab : InspectTabBase
	{
		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06002FF6 RID: 12278 RVA: 0x001A15E0 File Offset: 0x0019F9E0
		protected object SelObject
		{
			get
			{
				return Find.Selector.SingleSelectedObject;
			}
		}

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06002FF7 RID: 12279 RVA: 0x001A1600 File Offset: 0x0019FA00
		protected Thing SelThing
		{
			get
			{
				return Find.Selector.SingleSelectedThing;
			}
		}

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06002FF8 RID: 12280 RVA: 0x001A1620 File Offset: 0x0019FA20
		protected Pawn SelPawn
		{
			get
			{
				return this.SelThing as Pawn;
			}
		}

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06002FF9 RID: 12281 RVA: 0x001A1640 File Offset: 0x0019FA40
		private MainTabWindow_Inspect InspectPane
		{
			get
			{
				return (MainTabWindow_Inspect)MainButtonDefOf.Inspect.TabWindow;
			}
		}

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06002FFA RID: 12282 RVA: 0x001A1664 File Offset: 0x0019FA64
		protected override bool StillValid
		{
			get
			{
				return Find.MainTabsRoot.OpenTab == MainButtonDefOf.Inspect && ((MainTabWindow_Inspect)Find.MainTabsRoot.OpenTab.TabWindow).CurTabs.Contains(this);
			}
		}

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06002FFB RID: 12283 RVA: 0x001A16B0 File Offset: 0x0019FAB0
		protected override float PaneTopY
		{
			get
			{
				return this.InspectPane.PaneTopY;
			}
		}

		// Token: 0x06002FFC RID: 12284 RVA: 0x001A16D0 File Offset: 0x0019FAD0
		protected override void CloseTab()
		{
			this.InspectPane.CloseOpenTab();
		}
	}
}
