using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public static class StorytellerUI
	{
		private static Vector2 scrollPosition = default(Vector2);

		private static readonly Texture2D StorytellerHighlightTex = ContentFinder<Texture2D>.Get("UI/HeroArt/Storytellers/Highlight", true);

		internal static void DrawStorytellerSelectionInterface(Rect rect, ref StorytellerDef chosenStoryteller, ref DifficultyDef difficulty, Listing_Standard selectedStorytellerInfoListing)
		{
			GUI.BeginGroup(rect);
			if (chosenStoryteller != null && chosenStoryteller.listVisible)
			{
				float height = rect.height;
				Vector2 portraitSizeLarge = Storyteller.PortraitSizeLarge;
				double y = height - portraitSizeLarge.y - 1.0;
				Vector2 portraitSizeLarge2 = Storyteller.PortraitSizeLarge;
				float x = portraitSizeLarge2.x;
				Vector2 portraitSizeLarge3 = Storyteller.PortraitSizeLarge;
				Rect position = new Rect(390f, (float)y, x, portraitSizeLarge3.y);
				GUI.DrawTexture(position, chosenStoryteller.portraitLargeTex);
				Widgets.DrawLineHorizontal(0f, rect.height, rect.width);
			}
			Vector2 portraitSizeTiny = Storyteller.PortraitSizeTiny;
			Rect outRect = new Rect(0f, 0f, (float)(portraitSizeTiny.x + 16.0), rect.height);
			Vector2 portraitSizeTiny2 = Storyteller.PortraitSizeTiny;
			float x2 = portraitSizeTiny2.x;
			float num = (float)DefDatabase<StorytellerDef>.AllDefs.Count();
			Vector2 portraitSizeTiny3 = Storyteller.PortraitSizeTiny;
			Rect viewRect = new Rect(0f, 0f, x2, (float)(num * (portraitSizeTiny3.y + 10.0)));
			Widgets.BeginScrollView(outRect, ref StorytellerUI.scrollPosition, viewRect, true);
			Vector2 portraitSizeTiny4 = Storyteller.PortraitSizeTiny;
			float x3 = portraitSizeTiny4.x;
			Vector2 portraitSizeTiny5 = Storyteller.PortraitSizeTiny;
			Rect rect2 = new Rect(0f, 0f, x3, portraitSizeTiny5.y);
			foreach (StorytellerDef item in from tel in DefDatabase<StorytellerDef>.AllDefs
			orderby tel.listOrder
			select tel)
			{
				if (item.listVisible)
				{
					if (Widgets.ButtonImage(rect2, item.portraitTinyTex))
					{
						TutorSystem.Notify_Event("ChooseStoryteller");
						chosenStoryteller = item;
					}
					if (chosenStoryteller == item)
					{
						GUI.DrawTexture(rect2, StorytellerUI.StorytellerHighlightTex);
					}
					rect2.y += (float)(rect2.height + 8.0);
				}
			}
			Widgets.EndScrollView();
			Text.Font = GameFont.Small;
			Rect rect3 = new Rect((float)(outRect.xMax + 8.0), 0f, 240f, 999f);
			Widgets.Label(rect3, "HowStorytellersWork".Translate());
			if (chosenStoryteller != null && chosenStoryteller.listVisible)
			{
				Rect rect4 = new Rect((float)(outRect.xMax + 8.0), (float)(outRect.yMin + 200.0), 290f, 0f);
				rect4.height = rect.height - rect4.y;
				Text.Font = GameFont.Medium;
				Rect rect5 = new Rect((float)(rect4.x + 15.0), (float)(rect4.y - 40.0), 9999f, 40f);
				Widgets.Label(rect5, chosenStoryteller.label);
				Text.Anchor = TextAnchor.UpperLeft;
				Text.Font = GameFont.Small;
				selectedStorytellerInfoListing.Begin(rect4);
				selectedStorytellerInfoListing.Label(chosenStoryteller.description, 120f);
				selectedStorytellerInfoListing.Gap(6f);
				foreach (DifficultyDef allDef in DefDatabase<DifficultyDef>.AllDefs)
				{
					Rect rect6 = selectedStorytellerInfoListing.GetRect(30f);
					if (Mouse.IsOver(rect6))
					{
						Widgets.DrawHighlight(rect6);
					}
					TooltipHandler.TipRegion(rect6, allDef.description);
					if (Widgets.RadioButtonLabeled(rect6, allDef.LabelCap, difficulty == allDef))
					{
						difficulty = allDef;
					}
				}
				selectedStorytellerInfoListing.Gap(30f);
				if (Current.ProgramState == ProgramState.Entry)
				{
					selectedStorytellerInfoListing.CheckboxLabeled("PermadeathMode".Translate(), ref Find.GameInitData.permadeath, "PermadeathModeInfo".Translate());
				}
				selectedStorytellerInfoListing.End();
			}
			GUI.EndGroup();
		}
	}
}
