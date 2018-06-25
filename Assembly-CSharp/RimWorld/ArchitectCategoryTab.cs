using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ArchitectCategoryTab
	{
		public DesignationCategoryDef def;

		public const float InfoRectHeight = 270f;

		public ArchitectCategoryTab(DesignationCategoryDef def)
		{
			this.def = def;
		}

		public static Rect InfoRect
		{
			get
			{
				return new Rect(0f, (float)(UI.screenHeight - 35) - ((MainTabWindow_Architect)MainButtonDefOf.Architect.TabWindow).WinHeight - 270f, 200f, 270f);
			}
		}

		public void DesignationTabOnGUI()
		{
			if (Find.DesignatorManager.SelectedDesignator != null)
			{
				Find.DesignatorManager.SelectedDesignator.DoExtraGuiControls(0f, (float)(UI.screenHeight - 35) - ((MainTabWindow_Architect)MainButtonDefOf.Architect.TabWindow).WinHeight - 270f);
			}
			float startX = 210f;
			Gizmo selectedDesignator;
			GizmoGridDrawer.DrawGizmoGrid(this.def.ResolvedAllowedDesignators.Cast<Gizmo>(), startX, out selectedDesignator);
			if (selectedDesignator == null && Find.DesignatorManager.SelectedDesignator != null)
			{
				selectedDesignator = Find.DesignatorManager.SelectedDesignator;
			}
			this.DoInfoBox(ArchitectCategoryTab.InfoRect, (Designator)selectedDesignator);
		}

		protected void DoInfoBox(Rect infoRect, Designator designator)
		{
			Find.WindowStack.ImmediateWindow(32520, infoRect, WindowLayer.GameUI, delegate
			{
				if (designator != null)
				{
					Rect position = infoRect.AtZero().ContractedBy(7f);
					GUI.BeginGroup(position);
					Rect rect = new Rect(0f, 0f, position.width - designator.PanelReadoutTitleExtraRightMargin, 999f);
					Text.Font = GameFont.Small;
					Widgets.Label(rect, designator.LabelCap);
					float num = Mathf.Max(24f, Text.CalcHeight(designator.LabelCap, rect.width));
					designator.DrawPanelReadout(ref num, position.width);
					Rect rect2 = new Rect(0f, num, position.width, position.height - num);
					string desc = designator.Desc;
					GenText.SetTextSizeToFit(desc, rect2);
					Widgets.Label(rect2, desc);
					GUI.EndGroup();
				}
			}, true, false, 1f);
		}

		[CompilerGenerated]
		private sealed class <DoInfoBox>c__AnonStorey0
		{
			internal Designator designator;

			internal Rect infoRect;

			public <DoInfoBox>c__AnonStorey0()
			{
			}

			internal void <>m__0()
			{
				if (this.designator != null)
				{
					Rect position = this.infoRect.AtZero().ContractedBy(7f);
					GUI.BeginGroup(position);
					Rect rect = new Rect(0f, 0f, position.width - this.designator.PanelReadoutTitleExtraRightMargin, 999f);
					Text.Font = GameFont.Small;
					Widgets.Label(rect, this.designator.LabelCap);
					float num = Mathf.Max(24f, Text.CalcHeight(this.designator.LabelCap, rect.width));
					this.designator.DrawPanelReadout(ref num, position.width);
					Rect rect2 = new Rect(0f, num, position.width, position.height - num);
					string desc = this.designator.Desc;
					GenText.SetTextSizeToFit(desc, rect2);
					Widgets.Label(rect2, desc);
					GUI.EndGroup();
				}
			}
		}
	}
}
