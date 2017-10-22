using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Page_SelectStoryteller : Page
	{
		private StorytellerDef storyteller;

		private DifficultyDef difficulty;

		private Listing_Standard selectedStorytellerInfoListing = new Listing_Standard();

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
			select d).First();
		}

		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			StorytellerUI.DrawStorytellerSelectionInterface(mainRect, ref this.storyteller, ref this.difficulty, this.selectedStorytellerInfoListing);
			base.DoBottomButtons(rect, (string)null, (string)null, null, true);
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
						Messages.Message("MustChooseDifficulty".Translate(), MessageTypeDefOf.RejectInput);
						result = false;
						goto IL_0089;
					}
					Messages.Message("Difficulty has been automatically selected (debug mode only)", MessageTypeDefOf.SilentInput);
					this.difficulty = DifficultyDefOf.Hard;
				}
				Current.Game.storyteller = new Storyteller(this.storyteller, this.difficulty);
				result = true;
			}
			goto IL_0089;
			IL_0089:
			return result;
		}
	}
}
