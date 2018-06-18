using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000874 RID: 2164
	[StaticConstructorOnStartup]
	public class MainTabWindow_Inspect : MainTabWindow, IInspectPane
	{
		// Token: 0x06003127 RID: 12583 RVA: 0x001AB39C File Offset: 0x001A979C
		public MainTabWindow_Inspect()
		{
			this.closeOnAccept = false;
			this.closeOnCancel = false;
		}

		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06003128 RID: 12584 RVA: 0x001AB3B4 File Offset: 0x001A97B4
		// (set) Token: 0x06003129 RID: 12585 RVA: 0x001AB3CF File Offset: 0x001A97CF
		public Type OpenTabType
		{
			get
			{
				return this.openTabType;
			}
			set
			{
				this.openTabType = value;
			}
		}

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x0600312A RID: 12586 RVA: 0x001AB3DC File Offset: 0x001A97DC
		// (set) Token: 0x0600312B RID: 12587 RVA: 0x001AB3F7 File Offset: 0x001A97F7
		public float RecentHeight
		{
			get
			{
				return this.recentHeight;
			}
			set
			{
				this.recentHeight = value;
			}
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x0600312C RID: 12588 RVA: 0x001AB404 File Offset: 0x001A9804
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x0600312D RID: 12589 RVA: 0x001AB420 File Offset: 0x001A9820
		public override Vector2 RequestedTabSize
		{
			get
			{
				return InspectPaneUtility.PaneSizeFor(this);
			}
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x0600312E RID: 12590 RVA: 0x001AB43C File Offset: 0x001A983C
		private IEnumerable<object> Selected
		{
			get
			{
				return Find.Selector.SelectedObjects;
			}
		}

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x0600312F RID: 12591 RVA: 0x001AB45C File Offset: 0x001A985C
		private Thing SelThing
		{
			get
			{
				return Find.Selector.SingleSelectedThing;
			}
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06003130 RID: 12592 RVA: 0x001AB47C File Offset: 0x001A987C
		private Zone SelZone
		{
			get
			{
				return Find.Selector.SelectedZone;
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x06003131 RID: 12593 RVA: 0x001AB49C File Offset: 0x001A989C
		private int NumSelected
		{
			get
			{
				return Find.Selector.NumSelected;
			}
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x06003132 RID: 12594 RVA: 0x001AB4BC File Offset: 0x001A98BC
		public float PaneTopY
		{
			get
			{
				return (float)UI.screenHeight - 165f - 35f;
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x06003133 RID: 12595 RVA: 0x001AB4E4 File Offset: 0x001A98E4
		public bool AnythingSelected
		{
			get
			{
				return this.NumSelected > 0;
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06003134 RID: 12596 RVA: 0x001AB504 File Offset: 0x001A9904
		public bool ShouldShowSelectNextInCellButton
		{
			get
			{
				return this.NumSelected == 1 && (Find.Selector.SelectedZone == null || Find.Selector.SelectedZone.ContainsCell(MainTabWindow_Inspect.lastSelectCell));
			}
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06003135 RID: 12597 RVA: 0x001AB550 File Offset: 0x001A9950
		public bool ShouldShowPaneContents
		{
			get
			{
				return this.NumSelected == 1;
			}
		}

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x06003136 RID: 12598 RVA: 0x001AB570 File Offset: 0x001A9970
		public IEnumerable<InspectTabBase> CurTabs
		{
			get
			{
				if (this.NumSelected == 1)
				{
					if (this.SelThing != null && this.SelThing.def.inspectorTabsResolved != null)
					{
						return this.SelThing.GetInspectTabs();
					}
					if (this.SelZone != null)
					{
						return this.SelZone.GetInspectTabs();
					}
				}
				return Enumerable.Empty<InspectTabBase>();
			}
		}

		// Token: 0x06003137 RID: 12599 RVA: 0x001AB5E8 File Offset: 0x001A99E8
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			InspectPaneUtility.ExtraOnGUI(this);
			if (this.AnythingSelected)
			{
				if (Find.DesignatorManager.SelectedDesignator != null)
				{
					Find.DesignatorManager.SelectedDesignator.DoExtraGuiControls(0f, this.PaneTopY);
				}
			}
		}

		// Token: 0x06003138 RID: 12600 RVA: 0x001AB638 File Offset: 0x001A9A38
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			InspectPaneUtility.InspectPaneOnGUI(inRect, this);
		}

		// Token: 0x06003139 RID: 12601 RVA: 0x001AB64C File Offset: 0x001A9A4C
		public string GetLabel(Rect rect)
		{
			return InspectPaneUtility.AdjustedLabelFor(this.Selected, rect);
		}

		// Token: 0x0600313A RID: 12602 RVA: 0x001AB66D File Offset: 0x001A9A6D
		public void DrawInspectGizmos()
		{
			InspectGizmoGrid.DrawInspectGizmoGridFor(this.Selected, out this.mouseoverGizmo);
		}

		// Token: 0x0600313B RID: 12603 RVA: 0x001AB681 File Offset: 0x001A9A81
		public void DoPaneContents(Rect rect)
		{
			InspectPaneFiller.DoPaneContentsFor((ISelectable)Find.Selector.FirstSelectedObject, rect);
		}

		// Token: 0x0600313C RID: 12604 RVA: 0x001AB69C File Offset: 0x001A9A9C
		public void DoInspectPaneButtons(Rect rect, ref float lineEndWidth)
		{
			if (this.NumSelected == 1)
			{
				Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
				if (singleSelectedThing != null)
				{
					Widgets.InfoCardButton(rect.width - 48f, 0f, Find.Selector.SingleSelectedThing);
					lineEndWidth += 24f;
					Pawn pawn = singleSelectedThing as Pawn;
					if (pawn != null && pawn.playerSettings != null && pawn.playerSettings.UsesConfigurableHostilityResponse)
					{
						HostilityResponseModeUtility.DrawResponseButton(new Rect(rect.width - 72f, 0f, 24f, 24f), pawn, false);
						lineEndWidth += 24f;
					}
				}
			}
		}

		// Token: 0x0600313D RID: 12605 RVA: 0x001AB754 File Offset: 0x001A9B54
		public void SelectNextInCell()
		{
			if (this.NumSelected != 0)
			{
				Selector selector = Find.Selector;
				if (selector.SelectedZone == null || selector.SelectedZone.ContainsCell(MainTabWindow_Inspect.lastSelectCell))
				{
					if (selector.SelectedZone == null)
					{
						MainTabWindow_Inspect.lastSelectCell = selector.SingleSelectedThing.Position;
					}
					Map map;
					if (selector.SingleSelectedThing != null)
					{
						map = selector.SingleSelectedThing.Map;
					}
					else
					{
						map = selector.SelectedZone.Map;
					}
					selector.SelectNextAt(MainTabWindow_Inspect.lastSelectCell, map);
				}
			}
		}

		// Token: 0x0600313E RID: 12606 RVA: 0x001AB7E9 File Offset: 0x001A9BE9
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			InspectPaneUtility.UpdateTabs(this);
			if (this.mouseoverGizmo != null)
			{
				this.mouseoverGizmo.GizmoUpdateOnMouseover();
			}
		}

		// Token: 0x0600313F RID: 12607 RVA: 0x001AB80E File Offset: 0x001A9C0E
		public void CloseOpenTab()
		{
			this.openTabType = null;
		}

		// Token: 0x06003140 RID: 12608 RVA: 0x001AB818 File Offset: 0x001A9C18
		public void Reset()
		{
			this.openTabType = null;
		}

		// Token: 0x04001A9D RID: 6813
		private Type openTabType;

		// Token: 0x04001A9E RID: 6814
		private float recentHeight;

		// Token: 0x04001A9F RID: 6815
		private static IntVec3 lastSelectCell;

		// Token: 0x04001AA0 RID: 6816
		private Gizmo mouseoverGizmo;
	}
}
