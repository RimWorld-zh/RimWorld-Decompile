using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class ScenarioUI
	{
		private static float editViewHeight;

		public static void DrawScenarioInfo(Rect rect, Scenario scen, ref Vector2 infoScrollPosition)
		{
			Widgets.DrawMenuSection(rect);
			rect = rect.GetInnerRect();
			if (scen != null)
			{
				string fullInformationText = scen.GetFullInformationText();
				float width = (float)(rect.width - 16.0);
				float height = (float)(30.0 + Text.CalcHeight(fullInformationText, width) + 100.0);
				Rect viewRect = new Rect(0f, 0f, width, height);
				Widgets.BeginScrollView(rect, ref infoScrollPosition, viewRect, true);
				Text.Font = GameFont.Medium;
				Rect rect2 = new Rect(0f, 0f, viewRect.width, 30f);
				Widgets.Label(rect2, scen.name);
				Text.Font = GameFont.Small;
				Rect rect3 = new Rect(0f, 30f, viewRect.width, (float)(viewRect.height - 30.0));
				Widgets.Label(rect3, fullInformationText);
				Widgets.EndScrollView();
			}
		}

		public static void DrawScenarioEditInterface(Rect rect, Scenario scen, ref Vector2 infoScrollPosition)
		{
			Widgets.DrawMenuSection(rect);
			rect = rect.GetInnerRect();
			if (scen != null)
			{
				Rect viewRect = new Rect(0f, 0f, (float)(rect.width - 16.0), ScenarioUI.editViewHeight);
				Widgets.BeginScrollView(rect, ref infoScrollPosition, viewRect, true);
				Rect rect2 = new Rect(0f, 0f, viewRect.width, 99999f);
				Listing_ScenEdit listing_ScenEdit = new Listing_ScenEdit(scen);
				listing_ScenEdit.ColumnWidth = rect2.width;
				listing_ScenEdit.Begin(rect2);
				listing_ScenEdit.Label("Title".Translate(), -1f);
				scen.name = listing_ScenEdit.TextEntry(scen.name, 1).TrimmedToLength(55);
				listing_ScenEdit.Label("Summary".Translate(), -1f);
				scen.summary = listing_ScenEdit.TextEntry(scen.summary, 2).TrimmedToLength(300);
				listing_ScenEdit.Label("Description".Translate(), -1f);
				scen.description = listing_ScenEdit.TextEntry(scen.description, 4).TrimmedToLength(1000);
				listing_ScenEdit.Gap(12f);
				foreach (ScenPart allPart in scen.AllParts)
				{
					allPart.DoEditInterface(listing_ScenEdit);
				}
				listing_ScenEdit.End();
				ScenarioUI.editViewHeight = (float)(listing_ScenEdit.CurHeight + 100.0);
				Widgets.EndScrollView();
			}
		}
	}
}
