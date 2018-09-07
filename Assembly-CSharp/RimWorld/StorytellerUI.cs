using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	[StaticConstructorOnStartup]
	public static class StorytellerUI
	{
		private static Vector2 scrollPosition = default(Vector2);

		private static readonly Texture2D StorytellerHighlightTex = ContentFinder<Texture2D>.Get("UI/HeroArt/Storytellers/Highlight", true);

		[CompilerGenerated]
		private static Func<StorytellerDef, int> <>f__am$cache0;

		public static void DrawStorytellerSelectionInterface(Rect rect, ref StorytellerDef chosenStoryteller, ref DifficultyDef difficulty, Listing_Standard infoListing)
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
			Rect rect3 = new Rect(outRect.xMax + 8f, 0f, 300f, 999f);
			Widgets.Label(rect3, "HowStorytellersWork".Translate());
			if (chosenStoryteller != null && chosenStoryteller.listVisible)
			{
				Rect rect4 = new Rect(outRect.xMax + 8f, outRect.yMin + 160f, 290f, 0f);
				rect4.height = rect.height - rect4.y;
				Text.Font = GameFont.Medium;
				Rect rect5 = new Rect(rect4.x + 15f, rect4.y - 40f, 9999f, 40f);
				Widgets.Label(rect5, chosenStoryteller.label);
				Text.Anchor = TextAnchor.UpperLeft;
				Text.Font = GameFont.Small;
				infoListing.Begin(rect4);
				infoListing.Label(chosenStoryteller.description, 160f, null);
				infoListing.Gap(6f);
				foreach (DifficultyDef difficultyDef in DefDatabase<DifficultyDef>.AllDefs)
				{
					if (!difficultyDef.isExtreme || Prefs.ExtremeDifficultyUnlocked)
					{
						GUI.color = difficultyDef.drawColor;
						string text = difficultyDef.LabelCap;
						bool active = difficulty == difficultyDef;
						string text2 = difficultyDef.description;
						if (infoListing.RadioButton(text, active, 0f, text2))
						{
							difficulty = difficultyDef;
						}
						infoListing.Gap(3f);
					}
				}
				GUI.color = Color.white;
				if (Current.ProgramState == ProgramState.Entry)
				{
					infoListing.Gap(25f);
					bool flag = Find.GameInitData.permadeathChosen && Find.GameInitData.permadeath;
					bool flag2 = Find.GameInitData.permadeathChosen && !Find.GameInitData.permadeath;
					string text2 = "ReloadAnytimeMode".Translate();
					bool active = flag2;
					string text = "ReloadAnytimeModeInfo".Translate();
					if (infoListing.RadioButton(text2, active, 0f, text))
					{
						Find.GameInitData.permadeathChosen = true;
						Find.GameInitData.permadeath = false;
					}
					infoListing.Gap(3f);
					text = "CommitmentMode".TranslateWithBackup("PermadeathMode");
					active = flag;
					text2 = "PermadeathModeInfo".Translate();
					if (infoListing.RadioButton(text, active, 0f, text2))
					{
						Find.GameInitData.permadeathChosen = true;
						Find.GameInitData.permadeath = true;
					}
				}
				infoListing.End();
			}
			GUI.EndGroup();
		}

		// Note: this type is marked as 'beforefieldinit'.
		static StorytellerUI()
		{
		}

		[CompilerGenerated]
		private static int <DrawStorytellerSelectionInterface>m__0(StorytellerDef tel)
		{
			return tel.listOrder;
		}
	}
}
