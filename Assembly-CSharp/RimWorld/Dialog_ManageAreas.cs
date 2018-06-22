using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020007FE RID: 2046
	public class Dialog_ManageAreas : Window
	{
		// Token: 0x06002DAF RID: 11695 RVA: 0x001809E2 File Offset: 0x0017EDE2
		public Dialog_ManageAreas(Map map)
		{
			this.map = map;
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06002DB0 RID: 11696 RVA: 0x00180A18 File Offset: 0x0017EE18
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(450f, 400f);
			}
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x00180A3C File Offset: 0x0017EE3C
		public override void DoWindowContents(Rect inRect)
		{
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.ColumnWidth = inRect.width;
			listing_Standard.Begin(inRect);
			List<Area> allAreas = this.map.areaManager.AllAreas;
			int i = 0;
			for (int j = 0; j < allAreas.Count; j++)
			{
				if (allAreas[j].Mutable)
				{
					Rect rect = listing_Standard.GetRect(24f);
					Dialog_ManageAreas.DoAreaRow(rect, allAreas[j]);
					listing_Standard.Gap(6f);
					i++;
				}
			}
			if (this.map.areaManager.CanMakeNewAllowed())
			{
				while (i < 9)
				{
					listing_Standard.Gap(30f);
					i++;
				}
				if (listing_Standard.ButtonText("NewArea".Translate(), null))
				{
					Area_Allowed area_Allowed;
					this.map.areaManager.TryMakeNewAllowed(out area_Allowed);
				}
			}
			listing_Standard.End();
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x00180B38 File Offset: 0x0017EF38
		private static void DoAreaRow(Rect rect, Area area)
		{
			if (Mouse.IsOver(rect))
			{
				area.MarkForDraw();
				GUI.color = area.Color;
				Widgets.DrawHighlight(rect);
				GUI.color = Color.white;
			}
			GUI.BeginGroup(rect);
			WidgetRow widgetRow = new WidgetRow(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			widgetRow.Icon(area.ColorTexture, null);
			widgetRow.Gap(4f);
			widgetRow.Label(area.Label, 220f);
			if (widgetRow.ButtonText("Rename".Translate(), null, true, false))
			{
				Find.WindowStack.Add(new Dialog_RenameArea(area));
			}
			if (widgetRow.ButtonText("InvertArea".Translate(), null, true, false))
			{
				area.Invert();
			}
			WidgetRow widgetRow2 = widgetRow;
			Texture2D deleteX = TexButton.DeleteX;
			Color? mouseoverColor = new Color?(GenUI.SubtleMouseoverColor);
			if (widgetRow2.ButtonIcon(deleteX, null, mouseoverColor))
			{
				area.Delete();
			}
			GUI.EndGroup();
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x00180C34 File Offset: 0x0017F034
		public static void DoNameInputRect(Rect rect, ref string name, int maxLength)
		{
			string text = Widgets.TextField(rect, name);
			if (text.Length <= maxLength && Dialog_ManageAreas.validNameRegex.IsMatch(text))
			{
				name = text;
			}
		}

		// Token: 0x04001835 RID: 6197
		private Map map;

		// Token: 0x04001836 RID: 6198
		private static Regex validNameRegex = new Regex("^[a-zA-Z0-9 '\\-]*$");
	}
}
