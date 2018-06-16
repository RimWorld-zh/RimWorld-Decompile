using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E80 RID: 3712
	public class Listing_ScenEdit : Listing_Standard
	{
		// Token: 0x0600575D RID: 22365 RVA: 0x002CE0B9 File Offset: 0x002CC4B9
		public Listing_ScenEdit(Scenario scen)
		{
			this.scen = scen;
		}

		// Token: 0x0600575E RID: 22366 RVA: 0x002CE0CC File Offset: 0x002CC4CC
		public Rect GetScenPartRect(ScenPart part, float height)
		{
			string label = part.Label;
			Rect rect = base.GetRect(height);
			Widgets.DrawBoxSolid(rect, new Color(1f, 1f, 1f, 0.08f));
			WidgetRow widgetRow = new WidgetRow(rect.x, rect.y, UIDirection.RightThenDown, 72f, 0f);
			if (part.def.PlayerAddRemovable)
			{
				WidgetRow widgetRow2 = widgetRow;
				Texture2D deleteX = TexButton.DeleteX;
				Color? mouseoverColor = new Color?(GenUI.SubtleMouseoverColor);
				if (widgetRow2.ButtonIcon(deleteX, null, mouseoverColor))
				{
					this.scen.RemovePart(part);
					SoundDefOf.Click.PlayOneShotOnCamera(null);
				}
			}
			if (this.scen.CanReorder(part, ReorderDirection.Up))
			{
				if (widgetRow.ButtonIcon(TexButton.ReorderUp, null, null))
				{
					this.scen.Reorder(part, ReorderDirection.Up);
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
			}
			if (this.scen.CanReorder(part, ReorderDirection.Down))
			{
				if (widgetRow.ButtonIcon(TexButton.ReorderDown, null, null))
				{
					this.scen.Reorder(part, ReorderDirection.Down);
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
			}
			Text.Anchor = TextAnchor.UpperRight;
			Rect rect2 = rect.LeftPart(0.5f).Rounded();
			rect2.xMax -= 4f;
			Widgets.Label(rect2, label);
			Text.Anchor = TextAnchor.UpperLeft;
			base.Gap(4f);
			return rect.RightPart(0.5f).Rounded();
		}

		// Token: 0x040039DD RID: 14813
		private Scenario scen;
	}
}
