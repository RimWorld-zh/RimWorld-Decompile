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
			base.DoBottomButtons(rect, null, null, null, true);
		}

		protected override bool CanDoNext()
		{
			bool result;
			if (!base.CanDoNext())
			{
				result = false;
			}
			else
			{
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
				Current.Game.storyteller = new Storyteller(this.storyteller, this.difficulty);
				result = true;
			}
			return result;
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
	}
}
