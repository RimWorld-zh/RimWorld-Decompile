using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000872 RID: 2162
	[StaticConstructorOnStartup]
	public class MainTabWindow_Inspect : MainTabWindow, IInspectPane
	{
		// Token: 0x04001A9F RID: 6815
		private Type openTabType;

		// Token: 0x04001AA0 RID: 6816
		private float recentHeight;

		// Token: 0x04001AA1 RID: 6817
		private static IntVec3 lastSelectCell;

		// Token: 0x04001AA2 RID: 6818
		private Gizmo mouseoverGizmo;

		// Token: 0x06003123 RID: 12579 RVA: 0x001AB93C File Offset: 0x001A9D3C
		public MainTabWindow_Inspect()
		{
			this.closeOnAccept = false;
			this.closeOnCancel = false;
		}

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06003124 RID: 12580 RVA: 0x001AB954 File Offset: 0x001A9D54
		// (set) Token: 0x06003125 RID: 12581 RVA: 0x001AB96F File Offset: 0x001A9D6F
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

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06003126 RID: 12582 RVA: 0x001AB97C File Offset: 0x001A9D7C
		// (set) Token: 0x06003127 RID: 12583 RVA: 0x001AB997 File Offset: 0x001A9D97
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

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06003128 RID: 12584 RVA: 0x001AB9A4 File Offset: 0x001A9DA4
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06003129 RID: 12585 RVA: 0x001AB9C0 File Offset: 0x001A9DC0
		public override Vector2 RequestedTabSize
		{
			get
			{
				return InspectPaneUtility.PaneSizeFor(this);
			}
		}

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x0600312A RID: 12586 RVA: 0x001AB9DC File Offset: 0x001A9DDC
		private IEnumerable<object> Selected
		{
			get
			{
				return Find.Selector.SelectedObjects;
			}
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x0600312B RID: 12587 RVA: 0x001AB9FC File Offset: 0x001A9DFC
		private Thing SelThing
		{
			get
			{
				return Find.Selector.SingleSelectedThing;
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x0600312C RID: 12588 RVA: 0x001ABA1C File Offset: 0x001A9E1C
		private Zone SelZone
		{
			get
			{
				return Find.Selector.SelectedZone;
			}
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x0600312D RID: 12589 RVA: 0x001ABA3C File Offset: 0x001A9E3C
		private int NumSelected
		{
			get
			{
				return Find.Selector.NumSelected;
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x0600312E RID: 12590 RVA: 0x001ABA5C File Offset: 0x001A9E5C
		public float PaneTopY
		{
			get
			{
				return (float)UI.screenHeight - 165f - 35f;
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x0600312F RID: 12591 RVA: 0x001ABA84 File Offset: 0x001A9E84
		public bool AnythingSelected
		{
			get
			{
				return this.NumSelected > 0;
			}
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06003130 RID: 12592 RVA: 0x001ABAA4 File Offset: 0x001A9EA4
		public bool ShouldShowSelectNextInCellButton
		{
			get
			{
				return this.NumSelected == 1 && (Find.Selector.SelectedZone == null || Find.Selector.SelectedZone.ContainsCell(MainTabWindow_Inspect.lastSelectCell));
			}
		}

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x06003131 RID: 12593 RVA: 0x001ABAF0 File Offset: 0x001A9EF0
		public bool ShouldShowPaneContents
		{
			get
			{
				return this.NumSelected == 1;
			}
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06003132 RID: 12594 RVA: 0x001ABB10 File Offset: 0x001A9F10
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

		// Token: 0x06003133 RID: 12595 RVA: 0x001ABB88 File Offset: 0x001A9F88
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

		// Token: 0x06003134 RID: 12596 RVA: 0x001ABBD8 File Offset: 0x001A9FD8
		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			InspectPaneUtility.InspectPaneOnGUI(inRect, this);
		}

		// Token: 0x06003135 RID: 12597 RVA: 0x001ABBEC File Offset: 0x001A9FEC
		public string GetLabel(Rect rect)
		{
			return InspectPaneUtility.AdjustedLabelFor(this.Selected, rect);
		}

		// Token: 0x06003136 RID: 12598 RVA: 0x001ABC0D File Offset: 0x001AA00D
		public void DrawInspectGizmos()
		{
			InspectGizmoGrid.DrawInspectGizmoGridFor(this.Selected, out this.mouseoverGizmo);
		}

		// Token: 0x06003137 RID: 12599 RVA: 0x001ABC21 File Offset: 0x001AA021
		public void DoPaneContents(Rect rect)
		{
			InspectPaneFiller.DoPaneContentsFor((ISelectable)Find.Selector.FirstSelectedObject, rect);
		}

		// Token: 0x06003138 RID: 12600 RVA: 0x001ABC3C File Offset: 0x001AA03C
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

		// Token: 0x06003139 RID: 12601 RVA: 0x001ABCF4 File Offset: 0x001AA0F4
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

		// Token: 0x0600313A RID: 12602 RVA: 0x001ABD89 File Offset: 0x001AA189
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			InspectPaneUtility.UpdateTabs(this);
			if (this.mouseoverGizmo != null)
			{
				this.mouseoverGizmo.GizmoUpdateOnMouseover();
			}
		}

		// Token: 0x0600313B RID: 12603 RVA: 0x001ABDAE File Offset: 0x001AA1AE
		public void CloseOpenTab()
		{
			this.openTabType = null;
		}

		// Token: 0x0600313C RID: 12604 RVA: 0x001ABDB8 File Offset: 0x001AA1B8
		public void Reset()
		{
			this.openTabType = null;
		}
	}
}
