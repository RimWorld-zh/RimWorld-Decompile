using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Page_SelectStoryteller : Page
	{
		private StorytellerDef storyteller;

		private DifficultyDef difficulty;

		private Listing_Standard selectedStorytellerInfoListing = new Listing_Standard();

		[CompilerGenerated]
		private static Func<StorytellerDef, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<StorytellerDef, int> <>f__am$cache1;

		[CompilerGenerated]
		private static Action <>f__am$cache2;

		public Page_SelectStoryteller()
		{
		}

		public override string PageTitle
		{
			get
			{
				return "ChooseAIStoryteller".Translate();
			}
		}

		public override void PreOpen()
		{
			base.PreOpen();
			this.storyteller = (from d in DefDatabase<StorytellerDef>.AllDefs
			where d.listVisible
			orderby d.listOrder
			select d).First<StorytellerDef>();
		}

		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			StorytellerUI.DrawStorytellerSelectionInterface(mainRect, ref this.storyteller, ref this.difficulty, this.selectedStorytellerInfoListing);
			string text = null;
			Action midAct = null;
			if (!Prefs.ExtremeDifficultyUnlocked)
			{
				text = "UnlockExtremeDifficulty".Translate();
				midAct = delegate()
				{
					this.OpenDifficultyUnlockConfirmation();
				};
			}
			Rect rect2 = rect;
			string midLabel = text;
			base.DoBottomButtons(rect2, null, midLabel, midAct, true);
			Rect rect3 = new Rect(rect.xMax - Page.BottomButSize.x - 200f - 6f, rect.yMax - Page.BottomButSize.y, 200f, Page.BottomButSize.y);
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect3, "CanChangeStorytellerSettingsDuringPlay".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
		}

		private void OpenDifficultyUnlockConfirmation()
		{
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmUnlockExtremeDifficulty".Translate(), delegate
			{
				Prefs.ExtremeDifficultyUnlocked = true;
				Prefs.Save();
			}, true, null));
		}

		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			if (this.difficulty == null)
			{
				if (!Prefs.DevMode)
				{
					Messages.Message("MustChooseDifficulty".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				Messages.Message("Difficulty has been automatically selected (debug mode only)", MessageTypeDefOf.SilentInput, false);
				this.difficulty = DifficultyDefOf.Rough;
			}
			if (!Find.GameInitData.permadeathChosen)
			{
				if (!Prefs.DevMode)
				{
					Messages.Message("MustChoosePermadeath".Translate(), MessageTypeDefOf.RejectInput, false);
					return false;
				}
				Messages.Message("Reload anytime mode has been automatically selected (debug mode only)", MessageTypeDefOf.SilentInput, false);
				Find.GameInitData.permadeathChosen = true;
				Find.GameInitData.permadeath = false;
			}
			Current.Game.storyteller = new Storyteller(this.storyteller, this.difficulty);
			return true;
		}

		[CompilerGenerated]
		private static bool <PreOpen>m__0(StorytellerDef d)
		{
			return d.listVisible;
		}

		[CompilerGenerated]
		private static int <PreOpen>m__1(StorytellerDef d)
		{
			return d.listOrder;
		}

		[CompilerGenerated]
		private void <DoWindowContents>m__2()
		{
			this.OpenDifficultyUnlockConfirmation();
		}

		[CompilerGenerated]
		private static void <OpenDifficultyUnlockConfirmation>m__3()
		{
			Prefs.ExtremeDifficultyUnlocked = true;
			Prefs.Save();
		}
	}
}
