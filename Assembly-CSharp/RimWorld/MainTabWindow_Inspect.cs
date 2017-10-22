using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public class MainTabWindow_Inspect : MainTabWindow, IInspectPane
	{
		private Type openTabType;

		private float recentHeight;

		private static IntVec3 lastSelectCell;

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

		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		public override Vector2 RequestedTabSize
		{
			get
			{
				return InspectPaneUtility.PaneSizeFor(this);
			}
		}

		private IEnumerable<object> Selected
		{
			get
			{
				return Find.Selector.SelectedObjects;
			}
		}

		private Thing SelThing
		{
			get
			{
				return Find.Selector.SingleSelectedThing;
			}
		}

		private Zone SelZone
		{
			get
			{
				return Find.Selector.SelectedZone;
			}
		}

		private int NumSelected
		{
			get
			{
				return Find.Selector.NumSelected;
			}
		}

		public float PaneTopY
		{
			get
			{
				return (float)((float)UI.screenHeight - 165.0 - 35.0);
			}
		}

		public bool AnythingSelected
		{
			get
			{
				return this.NumSelected > 0;
			}
		}

		public bool ShouldShowSelectNextInCellButton
		{
			get
			{
				return this.NumSelected == 1 && (Find.Selector.SelectedZone == null || Find.Selector.SelectedZone.ContainsCell(MainTabWindow_Inspect.lastSelectCell));
			}
		}

		public bool ShouldShowPaneContents
		{
			get
			{
				return this.NumSelected == 1;
			}
		}

		public IEnumerable<InspectTabBase> CurTabs
		{
			get
			{
				IEnumerable<InspectTabBase> result;
				if (this.NumSelected == 1)
				{
					if (this.SelThing != null && this.SelThing.def.inspectorTabsResolved != null)
					{
						result = this.SelThing.GetInspectTabs();
						goto IL_0067;
					}
					if (this.SelZone != null)
					{
						result = this.SelZone.GetInspectTabs();
						goto IL_0067;
					}
				}
				result = Enumerable.Empty<InspectTabBase>();
				goto IL_0067;
				IL_0067:
				return result;
			}
		}

		public MainTabWindow_Inspect()
		{
			base.closeOnEscapeKey = false;
		}

		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			InspectPaneUtility.ExtraOnGUI(this);
			if (this.AnythingSelected && Find.DesignatorManager.SelectedDesignator != null)
			{
				Find.DesignatorManager.SelectedDesignator.DoExtraGuiControls(0f, this.PaneTopY);
			}
		}

		public override void DoWindowContents(Rect inRect)
		{
			base.DoWindowContents(inRect);
			InspectPaneUtility.InspectPaneOnGUI(inRect, this);
		}

		public string GetLabel(Rect rect)
		{
			return InspectPaneUtility.AdjustedLabelFor(this.Selected, rect);
		}

		public void DrawInspectGizmos()
		{
			InspectGizmoGrid.DrawInspectGizmoGridFor(this.Selected);
		}

		public void DoPaneContents(Rect rect)
		{
			InspectPaneFiller.DoPaneContentsFor((ISelectable)Find.Selector.FirstSelectedObject, rect);
		}

		public void DoInspectPaneButtons(Rect rect, ref float lineEndWidth)
		{
			if (this.NumSelected == 1)
			{
				Thing singleSelectedThing = Find.Selector.SingleSelectedThing;
				if (singleSelectedThing != null)
				{
					Widgets.InfoCardButton((float)(rect.width - 48.0), 0f, Find.Selector.SingleSelectedThing);
					lineEndWidth += 24f;
					Pawn pawn = singleSelectedThing as Pawn;
					if (pawn != null && pawn.playerSettings != null && pawn.playerSettings.UsesConfigurableHostilityResponse)
					{
						HostilityResponseModeUtility.DrawResponseButton(new Vector2((float)(rect.width - 72.0), 0f), pawn);
						lineEndWidth += 24f;
					}
				}
			}
		}

		public void SelectNextInCell()
		{
			if (this.NumSelected != 0)
			{
				Selector selector = Find.Selector;
				if (selector.SelectedZone != null && !selector.SelectedZone.ContainsCell(MainTabWindow_Inspect.lastSelectCell))
					return;
				if (selector.SelectedZone == null)
				{
					MainTabWindow_Inspect.lastSelectCell = selector.SingleSelectedThing.Position;
				}
				Map map = (selector.SingleSelectedThing == null) ? selector.SelectedZone.Map : selector.SingleSelectedThing.Map;
				selector.SelectNextAt(MainTabWindow_Inspect.lastSelectCell, map);
			}
		}

		public override void WindowUpdate()
		{
			base.WindowUpdate();
			InspectPaneUtility.UpdateTabs(this);
		}

		public void CloseOpenTab()
		{
			this.openTabType = null;
		}

		public void Reset()
		{
			this.openTabType = null;
		}
	}
}
