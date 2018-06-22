using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000837 RID: 2103
	[StaticConstructorOnStartup]
	public static class StorytellerUI
	{
		// Token: 0x06002FA0 RID: 12192 RVA: 0x00197CD8 File Offset: 0x001960D8
		internal static void DrawStorytellerSelectionInterface(Rect rect, ref StorytellerDef chosenStoryteller, ref DifficultyDef difficulty, Listing_Standard selectedStorytellerInfoListing)
		{
			GUI.BeginGroup(rect);
			if (chosenStoryteller != null && chosenStoryteller.listVisible)
			{
				Rect position = new Rect(390f, rect.height - Storyteller.PortraitSizeLarge.y - 1f, Storyteller.PortraitSizeLarge.x, Storyteller.PortraitSizeLarge.y);
				GUI.DrawTexture(position, chosenStoryteller.portraitLargeTex);
				Widgets.DrawLineHorizontal(0f, rect.height, rect.width);
			}
			Rect outRect = new Rect(0f, 0f, Storyteller.PortraitSizeTiny.x + 16f, rect.height);
			Rect viewRect = new Rect(0f, 0f, Storyteller.PortraitSizeTiny.x, (float)DefDatabase<StorytellerDef>.AllDefs.Count<StorytellerDef>() * (Storyteller.PortraitSizeTiny.y + 10f));
			Widgets.BeginScrollView(outRect, ref StorytellerUI.scrollPosition, viewRect, true);
			Rect rect2 = new Rect(0f, 0f, Storyteller.PortraitSizeTiny.x, Storyteller.PortraitSizeTiny.y);
			foreach (StorytellerDef storytellerDef in from tel in DefDatabase<StorytellerDef>.AllDefs
			orderby tel.listOrder
			select tel)
			{
				if (storytellerDef.listVisible)
				{
					if (Widgets.ButtonImage(rect2, storytellerDef.portraitTinyTex))
					{
						TutorSystem.Notify_Event("ChooseStoryteller");
						chosenStoryteller = storytellerDef;
					}
					if (chosenStoryteller == storytellerDef)
					{
						GUI.DrawTexture(rect2, StorytellerUI.StorytellerHighlightTex);
					}
					rect2.y += rect2.height + 8f;
				}
			}
			Widgets.EndScrollView();
			Text.Font = GameFont.Small;
			Rect rect3 = new Rect(outRect.xMax + 8f, 0f, 240f, 999f);
			Widgets.Label(rect3, "HowStorytellersWork".Translate());
			if (chosenStoryteller != null && chosenStoryteller.listVisible)
			{
				Rect rect4 = new Rect(outRect.xMax + 8f, outRect.yMin + 200f, 290f, 0f);
				rect4.height = rect.height - rect4.y;
				Text.Font = GameFont.Medium;
				Rect rect5 = new Rect(rect4.x + 15f, rect4.y - 40f, 9999f, 40f);
				Widgets.Label(rect5, chosenStoryteller.label);
				Text.Anchor = TextAnchor.UpperLeft;
				Text.Font = GameFont.Small;
				selectedStorytellerInfoListing.Begin(rect4);
				selectedStorytellerInfoListing.Label(chosenStoryteller.description, 120f, null);
				selectedStorytellerInfoListing.Gap(6f);
				foreach (DifficultyDef difficultyDef in DefDatabase<DifficultyDef>.AllDefs)
				{
					Rect rect6 = selectedStorytellerInfoListing.GetRect(30f);
					if (Mouse.IsOver(rect6))
					{
						Widgets.DrawHighlight(rect6);
					}
					TooltipHandler.TipRegion(rect6, difficultyDef.description);
					if (Widgets.RadioButtonLabeled(rect6, difficultyDef.LabelCap, difficulty == difficultyDef))
					{
						difficulty = difficultyDef;
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

		// Token: 0x040019B9 RID: 6585
		private static Vector2 scrollPosition = default(Vector2);

		// Token: 0x040019BA RID: 6586
		private static readonly Texture2D StorytellerHighlightTex = ContentFinder<Texture2D>.Get("UI/HeroArt/Storytellers/Highlight", true);
	}
}
