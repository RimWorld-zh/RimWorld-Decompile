using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000847 RID: 2119
	public class ArchitectCategoryTab
	{
		// Token: 0x06002FEB RID: 12267 RVA: 0x001A0BB1 File Offset: 0x0019EFB1
		public ArchitectCategoryTab(DesignationCategoryDef def)
		{
			this.def = def;
		}

		// Token: 0x17000797 RID: 1943
		// (get) Token: 0x06002FEC RID: 12268 RVA: 0x001A0BC4 File Offset: 0x0019EFC4
		public static Rect InfoRect
		{
			get
			{
				return new Rect(0f, (float)(UI.screenHeight - 35) - ((MainTabWindow_Architect)MainButtonDefOf.Architect.TabWindow).WinHeight - 270f, 200f, 270f);
			}
		}

		// Token: 0x06002FED RID: 12269 RVA: 0x001A0C14 File Offset: 0x0019F014
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

		// Token: 0x06002FEE RID: 12270 RVA: 0x001A0CB8 File Offset: 0x0019F0B8
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

		// Token: 0x040019EE RID: 6638
		public DesignationCategoryDef def;

		// Token: 0x040019EF RID: 6639
		public const float InfoRectHeight = 270f;
	}
}
