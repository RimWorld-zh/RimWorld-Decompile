using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020007B2 RID: 1970
	public static class AreaAllowedGUI
	{
		// Token: 0x06002B79 RID: 11129 RVA: 0x0016FD54 File Offset: 0x0016E154
		public static void DoAllowedAreaSelectors(Rect rect, Pawn p)
		{
			if (Find.CurrentMap != null)
			{
				List<Area> allAreas = Find.CurrentMap.areaManager.AllAreas;
				int num = 1;
				for (int i = 0; i < allAreas.Count; i++)
				{
					if (allAreas[i].AssignableAsAllowed())
					{
						num++;
					}
				}
				float num2 = rect.width / (float)num;
				Text.WordWrap = false;
				Text.Font = GameFont.Tiny;
				Rect rect2 = new Rect(rect.x, rect.y, num2, rect.height);
				AreaAllowedGUI.DoAreaSelector(rect2, p, null);
				int num3 = 1;
				for (int j = 0; j < allAreas.Count; j++)
				{
					if (allAreas[j].AssignableAsAllowed())
					{
						float num4 = (float)num3 * num2;
						Rect rect3 = new Rect(rect.x + num4, rect.y, num2, rect.height);
						AreaAllowedGUI.DoAreaSelector(rect3, p, allAreas[j]);
						num3++;
					}
				}
				Text.WordWrap = true;
				Text.Font = GameFont.Small;
			}
		}

		// Token: 0x06002B7A RID: 11130 RVA: 0x0016FE70 File Offset: 0x0016E270
		private static void DoAreaSelector(Rect rect, Pawn p, Area area)
		{
			rect = rect.ContractedBy(1f);
			GUI.DrawTexture(rect, (area == null) ? BaseContent.GreyTex : area.ColorTexture);
			Text.Anchor = TextAnchor.MiddleLeft;
			string text = AreaUtility.AreaAllowedLabel_Area(area);
			Rect rect2 = rect;
			rect2.xMin += 3f;
			rect2.yMin += 2f;
			Widgets.Label(rect2, text);
			if (p.playerSettings.AreaRestriction == area)
			{
				Widgets.DrawBox(rect, 2);
			}
			if (Event.current.rawType == EventType.MouseUp && Event.current.button == 0)
			{
				AreaAllowedGUI.dragging = false;
			}
			if (!Input.GetMouseButton(0) && Event.current.type != EventType.MouseDown)
			{
				AreaAllowedGUI.dragging = false;
			}
			if (Mouse.IsOver(rect))
			{
				if (area != null)
				{
					area.MarkForDraw();
				}
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
				{
					AreaAllowedGUI.dragging = true;
				}
				if (AreaAllowedGUI.dragging && p.playerSettings.AreaRestriction != area)
				{
					p.playerSettings.AreaRestriction = area;
					SoundDefOf.Designate_DragStandard_Changed.PlayOneShotOnCamera(null);
				}
			}
			Text.Anchor = TextAnchor.UpperLeft;
			TooltipHandler.TipRegion(rect, text);
		}

		// Token: 0x0400174A RID: 5962
		private static bool dragging;
	}
}
