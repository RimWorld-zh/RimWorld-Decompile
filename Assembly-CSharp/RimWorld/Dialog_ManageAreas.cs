using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000802 RID: 2050
	public class Dialog_ManageAreas : Window
	{
		// Token: 0x06002DB6 RID: 11702 RVA: 0x0018080A File Offset: 0x0017EC0A
		public Dialog_ManageAreas(Map map)
		{
			this.map = map;
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06002DB7 RID: 11703 RVA: 0x00180840 File Offset: 0x0017EC40
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(450f, 400f);
			}
		}

		// Token: 0x06002DB8 RID: 11704 RVA: 0x00180864 File Offset: 0x0017EC64
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

		// Token: 0x06002DB9 RID: 11705 RVA: 0x00180960 File Offset: 0x0017ED60
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

		// Token: 0x06002DBA RID: 11706 RVA: 0x00180A5C File Offset: 0x0017EE5C
		public static void DoNameInputRect(Rect rect, ref string name, int maxLength)
		{
			string text = Widgets.TextField(rect, name);
			if (text.Length <= maxLength && Dialog_ManageAreas.validNameRegex.IsMatch(text))
			{
				name = text;
			}
		}

		// Token: 0x04001837 RID: 6199
		private Map map;

		// Token: 0x04001838 RID: 6200
		private static Regex validNameRegex = new Regex("^[a-zA-Z0-9 '\\-]*$");
	}
}
