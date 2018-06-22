using System;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000E7E RID: 3710
	public class Listing_ScenEdit : Listing_Standard
	{
		// Token: 0x0600577B RID: 22395 RVA: 0x002CFCC9 File Offset: 0x002CE0C9
		public Listing_ScenEdit(Scenario scen)
		{
			this.scen = scen;
		}

		// Token: 0x0600577C RID: 22396 RVA: 0x002CFCDC File Offset: 0x002CE0DC
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

		// Token: 0x040039EB RID: 14827
		private Scenario scen;
	}
}
